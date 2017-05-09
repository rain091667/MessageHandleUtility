using MessageHandleUtility.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageHandleUtility
{
    class MessageSocketHandler : IMessageSendHandler
    {
        private class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 4096;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }

        private Socket _socket = null;
        private int _PollTime = 3000;
        private bool Task_CheckPollFlag = false;
        private MessageSocketInformation _MessageSocketInformation = null;
        private static ManualResetEvent _ReceiveFlag = new ManualResetEvent(false);
        private MessageSessionContract _MessageSessionContract = null;
        private ConcurrentDictionary<string, IMessageHandler> _MessageInterfaceTable = null;
        private string _TargetID = string.Empty;
        private IMessageSocketDisconnectEvent _DisconnectEventInterface = null;

        public bool IsConnected { get { return SocketIsConnected(_socket); } }
        public void SendMessage(string msg)
        {
            string temp = msg;
            Task.Factory.StartNew(() => SendMessageTask(temp), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void SendMessageTask(string msg)
        {
            MessageSessionContract obj = new MessageSessionContract();
            obj.TimeTick_String = DateTime.Now.Ticks.ToString();
            obj.MessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(msg));
            string data = Convert.ToBase64String(Encoding.UTF8.GetBytes(SerializeUtility.Serialize(obj)));
            byte[] SendBytes = Encoding.UTF8.GetBytes(data + "\n");
            try
            {
                _socket.Send(SendBytes, SendBytes.Length, SocketFlags.None);
            }
            catch (Exception e)
            {
                // skip
            }
        }

        public MessageSocketHandler(string TargetID, MessageSocketInformation MessageSocketInfo, Socket socket, ConcurrentDictionary<string, IMessageHandler> MessageTable, IMessageSocketDisconnectEvent DisconnectEventInterface)
        {
            if (socket == null) throw new NullReferenceException("socket is null.");

            _socket = socket;
            _MessageSocketInformation = MessageSocketInfo;
            _MessageInterfaceTable = MessageTable;
            _TargetID = TargetID;
            _DisconnectEventInterface = DisconnectEventInterface;

            if (_MessageInterfaceTable.ContainsKey(_TargetID))
            {
                _MessageInterfaceTable[_TargetID].MessageSendHandler = this;
            }

            setKeepAlive();
            Task_CheckPollFlag = true;
            Task.Factory.StartNew(() => CheckSocketIsConnected(), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            Task.Factory.StartNew(() => ExecutingSocketTask(_socket), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void ExecutingSocketTask(Socket socket)
        {
            StateObject state = new StateObject();
            state.workSocket = socket;
            while (socket.Connected)
            {
                try
                {
                    socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(MessageReadCallback), state);
                }
                catch(Exception)
                {
                    // skip
                }
                _ReceiveFlag.WaitOne();
            }
        }

        private void MessageReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;
            int SplitIndex = 0;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            string parseString = string.Empty;
            byte[] data;

            try
            {
                int databytesRead = handler.EndReceive(ar);
                if (databytesRead > 0)
                {
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, databytesRead));

                    content = state.sb.ToString();
                    if (content.Contains("\n"))
                    {
                        do
                        {
                            SplitIndex = content.IndexOf("\n");
                            parseString = content.Substring(0, SplitIndex);
                            state.sb.Remove(0, SplitIndex + 1);
                            data = Convert.FromBase64String(parseString);
                            parseString = Encoding.UTF8.GetString(data);

                            _MessageSessionContract = SerializeUtility.DeSerialize<MessageSessionContract>(parseString);
                            if (_MessageSessionContract != null)
                            {
                                _MessageSessionContract.MessageContent = Encoding.UTF8.GetString(Convert.FromBase64String(_MessageSessionContract.MessageContent));

                                if (_MessageInterfaceTable.ContainsKey(_TargetID))
                                    _MessageInterfaceTable[_TargetID].OnMessageReceive((IMessageContent)_MessageSessionContract);
                            }
                        } while (state.sb.ToString().Contains("\n"));
                    }
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(MessageReadCallback), state);
                }
                else
                {
                    _ReceiveFlag.Set();
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private void CheckSocketIsConnected()
        {
            while (Task_CheckPollFlag)
            {
                if (!SocketIsConnected(this._socket))
                {
                    Task_CheckPollFlag = false;
                }
                Task.Delay(1000);
            }
            this._socket.Close();
            _DisconnectEventInterface.OnConnectionDisconnected(_TargetID, _MessageSocketInformation);
        }

        private void setKeepAlive()
        {
            // Keep Alive Setting
            int size = sizeof(uint);
            uint on = 1;
            uint keepAliveInterval = 10000;
            uint retryInterval = 1000;
            byte[] inArray = new byte[size * 3];
            Array.Copy(BitConverter.GetBytes(on), 0, inArray, 0, size);
            Array.Copy(BitConverter.GetBytes(keepAliveInterval), 0, inArray, size, size);
            Array.Copy(BitConverter.GetBytes(retryInterval), 0, inArray, size * 2, size);
            _socket.IOControl(IOControlCode.KeepAliveValues, inArray, null);
        }

        private bool SocketIsConnected(Socket s)
        {
            bool part1 = true;
            bool part2 = true;
            try
            {
                part1 = this._socket.Poll(_PollTime, SelectMode.SelectRead);
                part2 = (this._socket.Available == 0);
            }
            catch (Exception)
            {
                // skip
            }
            return !(part1 && part2);
        }
    }
}
