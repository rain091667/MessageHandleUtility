using MessageHandleUtility.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageHandleUtility
{
    public class MessageSocketManager : IMessageSocketDisconnectEvent
    {
        private IPHostEndPoint _LocalhostEndPoint;
        private MessageSocketInformation _InformationProperty = null;
        private ConcurrentDictionary<string, MessageSocketHandler> _ConnectionTable = null;
        private ConcurrentDictionary<string, IMessageHandler> _MessageInterfaceTable = null;
        private IMessageSocketSession _IMessageSocketSession = null;

        // recv Listen
        private TcpListener _RecvMessageSocketListener = null;
        private Task _RecvSessionTask = null;
        private bool _RecvSessionTask_Flag = false;

        // utility
        private NetworkInfoUtility _NetworkInfoUtility = null;

        public MessageSocketManager(string PrimaryID, IMessageSocketSession mIMessageSocketSession)
        {
            if (string.IsNullOrEmpty(PrimaryID))
            {
                throw new ArgumentNullException("PrimaryID is null or empty.");
            }
            if (mIMessageSocketSession == null)
            {
                throw new ArgumentNullException("IMessageSocketSession is null.");
            }

            this._InformationProperty = new MessageSocketInformation(PrimaryID);
            this._IMessageSocketSession = mIMessageSocketSession;

            this._ConnectionTable = new ConcurrentDictionary<string, MessageSocketHandler>();
            this._MessageInterfaceTable = new ConcurrentDictionary<string, IMessageHandler>();
            this._NetworkInfoUtility = new NetworkInfoUtility();
        }

        public void StartListenMessageSocket(IPHostEndPoint hostEndPoint)
        {
            if (hostEndPoint.IPaddress == null || !hostEndPoint.Port.HasValue)
            {
                throw new ArgumentNullException("hostEndPoint is null.");
            }

            if (_NetworkInfoUtility.IsUsedIPEndPoint((int)hostEndPoint.Port))
            {
                throw new ArgumentException("Port: " + hostEndPoint.Port + " is used.");
            }

            if (_RecvSessionTask_Flag) return;

            this._LocalhostEndPoint = hostEndPoint;

            _RecvSessionTask_Flag = false;
            _RecvMessageSocketListener = new TcpListener(_LocalhostEndPoint.IPaddress, (int)_LocalhostEndPoint.Port);
            _RecvSessionTask = Task.Factory.StartNew(() => ExecutingRecvSessionTask(_RecvMessageSocketListener), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public bool StopListenMessageSocket()
        {
            bool result = false;
            if (_RecvSessionTask == null)
            {
                throw new ArgumentException("RecvSessionTask is null");
            }
            if (!_RecvSessionTask.IsCompleted && _RecvSessionTask_Flag)
            {
                _RecvSessionTask_Flag = false;
                _RecvMessageSocketListener.Stop();
                result = true;
            }
            return result;
        }

        private void ExecutingRecvSessionTask(TcpListener RecvSessionListener)
        {
            _RecvSessionTask_Flag = true;
            RecvSessionListener.Start();
            Socket tempSocket = null;
            while (_RecvSessionTask_Flag)
            {
                try
                {
                    tempSocket = RecvSessionListener.AcceptSocket();
                    Check_ComingIn_Connection(tempSocket);
                }
                catch(Exception e)
                {
                    RecvSessionListener.Stop();
                    _RecvSessionTask_Flag = false;
                    // skip
                }
            }
        }

        private async void Check_ComingIn_Connection(Socket socket)
        {
            bool AcceptFlag = false;
            MessageSocketInformation value = await Task.Run(() =>
            {
                const int BufferSize = 4096;
                byte[] buffer = new byte[BufferSize];
                StringBuilder sb = new StringBuilder();
                int databytesRead = 0;
                string content = string.Empty;
                int SplitIndex;
                string parseString = string.Empty;
                byte[] data;

                try
                {
                    do
                    {
                        databytesRead = socket.Receive(buffer);
                        sb.Append(Encoding.UTF8.GetString(buffer, 0, databytesRead));
                        content = sb.ToString();
                        if (content.Contains("\n"))
                        {
                            SplitIndex = content.IndexOf("\n");
                            parseString = content.Substring(0, SplitIndex);
                            data = Convert.FromBase64String(parseString);
                            parseString = Encoding.UTF8.GetString(data);
                            break;
                        }
                    } while (databytesRead > 0);
                }
                catch (Exception)
                {
                    socket.Close();
                    return null;
                }

                return SerializeUtility.DeSerialize<MessageSocketInformation>(parseString);
            });

            if (value != null)
            {
                value.ServerID = _InformationProperty.PrimaryID;
                // check is in table // reject
                if (_ConnectionTable.ContainsKey(value.PrimaryID))
                {
                    value.AcceptFlag = false;
                    value.Description.Add("This PrimaryID is in connection table.");
                    string data = string.Empty;
                    data = Convert.ToBase64String(Encoding.UTF8.GetBytes(SerializeUtility.Serialize(value)));
                    byte[] SendDataBytes = Encoding.UTF8.GetBytes(data + "\n");
                    socket.Send(SendDataBytes, SendDataBytes.Length, SocketFlags.None);
                }
                else
                {
                    _IMessageSocketSession.OnNewConnection(value.Info_PrimaryID, value, out AcceptFlag);
                    if (AcceptFlag)
                    {
                        value.AcceptFlag = true;
                        string data = string.Empty;
                        data = Convert.ToBase64String(Encoding.UTF8.GetBytes(SerializeUtility.Serialize(value)));
                        byte[] SendDataBytes = Encoding.UTF8.GetBytes(data + "\n");
                        socket.Send(SendDataBytes, SendDataBytes.Length, SocketFlags.None);
                        MessageSocketHandler tempHandler = new MessageSocketHandler(value.Info_PrimaryID, value, socket, _MessageInterfaceTable, this); // 之後去除宣告
                        _ConnectionTable.TryAdd(value.Info_PrimaryID, tempHandler);
                        if (_MessageInterfaceTable.ContainsKey(value.Info_PrimaryID))
                        {
                            _MessageInterfaceTable[value.Info_PrimaryID].MessageSendHandler = tempHandler;
                        }
                    }
                    else
                    {
                        value.AcceptFlag = false;
                        value.Description.Add("Reject by Control Manager.");
                        string data = string.Empty;
                        data = Convert.ToBase64String(Encoding.UTF8.GetBytes(SerializeUtility.Serialize(value)));
                        byte[] SendDataBytes = Encoding.UTF8.GetBytes(data + "\n");
                        socket.Send(SendDataBytes, SendDataBytes.Length, SocketFlags.None);
                    }
                }
            }
            else
            {
                socket.Close();
            }
        }

        public void Connect(IPHostEndPoint remoteEndPoint, string[] Descriptions)
        {
            Socket SessionSocket = new Socket(remoteEndPoint.IPaddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            if (!remoteEndPoint.Port.HasValue)
            {
                throw new NullReferenceException("Port is null");
            }

            try
            {
                SessionSocket.Connect(new IPEndPoint(remoteEndPoint.IPaddress, (int)remoteEndPoint.Port));
                Check_ConnectOut_Connection(SessionSocket, Descriptions);
            }
            catch (Exception)
            {
                MessageSocketInformation temp = new MessageSocketInformation(this._InformationProperty.PrimaryID);
                temp.Description.Add("Connect Failed. Please check remote host.");
                _IMessageSocketSession.OnConnectFailed(temp.Info_ServerID, temp);
            }
        }

        private async void Check_ConnectOut_Connection(Socket socket, string[] Descriptions)
        {
            bool AcceptFlag = false;
            MessageSocketInformation info = new MessageSocketInformation(this._InformationProperty.PrimaryID);
            info.AcceptFlag = false;
            if (Descriptions != null)
            {
                for (int i = 0; i < Descriptions.Length; i++)
                    info.Description.Add(Descriptions[i]);
            }
            string data = string.Empty;
            data = Convert.ToBase64String(Encoding.UTF8.GetBytes(SerializeUtility.Serialize(info)));
            byte[] SendDataBytes = Encoding.UTF8.GetBytes(data + "\n");
            socket.Send(SendDataBytes, SendDataBytes.Length, SocketFlags.None);

            info = await Task.Run(() =>
            {
                const int BufferSize = 4096;
                byte[] buffer = new byte[BufferSize];
                StringBuilder sb = new StringBuilder();
                int databytesRead = 0;
                string content = string.Empty;
                int SplitIndex;
                string parseString = string.Empty;
                byte[] recvBytes;

                try
                {
                    do
                    {
                        databytesRead = socket.Receive(buffer);
                        sb.Append(Encoding.UTF8.GetString(buffer, 0, databytesRead));
                        content = sb.ToString();
                        if (content.Contains("\n"))
                        {
                            SplitIndex = content.IndexOf("\n");
                            parseString = content.Substring(0, SplitIndex);
                            recvBytes = Convert.FromBase64String(parseString);
                            parseString = Encoding.UTF8.GetString(recvBytes);
                            break;
                        }
                    } while (databytesRead > 0);
                }
                catch (Exception)
                {
                    socket.Close();
                    return null;
                }

                return SerializeUtility.DeSerialize<MessageSocketInformation>(parseString);
            });

            if (info != null)
            {
                if (info.Info_AcceptFlag)
                {
                    if (this._ConnectionTable.ContainsKey(info.Info_ServerID))
                    {
                        socket.Close();
                        info.Description.Add("This connection is exist.");
                        _IMessageSocketSession.OnConnectFailed(info.Info_ServerID, info);
                    }
                    else
                    {
                        _IMessageSocketSession.OnNewConnection(info.Info_ServerID, info, out AcceptFlag);
                        if (AcceptFlag)
                        {
                            MessageSocketHandler tempHandler = new MessageSocketHandler(info.Info_ServerID, info, socket, _MessageInterfaceTable, this);
                            _ConnectionTable.TryAdd(info.Info_ServerID, tempHandler);
                            if (_MessageInterfaceTable.ContainsKey(info.Info_ServerID))
                            {
                                _MessageInterfaceTable[info.Info_ServerID].MessageSendHandler = tempHandler;
                            }
                        }
                        else
                        {
                            socket.Close();
                        }
                    }
                }
                else
                {
                    socket.Close();
                    _IMessageSocketSession.OnConnectFailed(info.Info_ServerID, info);
                }
            }
            else
            {
                socket.Close();
            }
        }

        public bool addMessageHandler(string ListenPrimaryID, IMessageHandler handler)
        {
            bool result = false;
            if (!this._MessageInterfaceTable.ContainsKey(ListenPrimaryID))
            {
                result = this._MessageInterfaceTable.TryAdd(ListenPrimaryID, handler);
                if (result)
                {
                    if (_ConnectionTable.ContainsKey(ListenPrimaryID))
                    {
                        this._MessageInterfaceTable[ListenPrimaryID].MessageSendHandler = _ConnectionTable[ListenPrimaryID];
                    }
                }
            }
            return result;
        }

        public bool removeMessageHandler(string ListenPrimaryID)
        {
            bool result = false;
            IMessageHandler handler = null;
            if (this._MessageInterfaceTable.ContainsKey(ListenPrimaryID))
            {
                result = this._MessageInterfaceTable.TryRemove(ListenPrimaryID, out handler);
                if (result)
                {
                    handler.MessageSendHandler = null;
                }
            }
            return result;
        }

        public bool IsHaveMessageHandler(string ListenPrimaryID)
        {
            return this._MessageInterfaceTable.ContainsKey(ListenPrimaryID);
        }

        void IMessageSocketDisconnectEvent.OnConnectionDisconnected(string TargetID, IMessageSocketInformation info)
        {
            MessageSocketHandler temp = null;
            _ConnectionTable.TryRemove(TargetID, out temp);
            _IMessageSocketSession.OnConnectionDisconnected(TargetID, info);
        }

        public IMessageSocketInformation InformationProperty { get { return _InformationProperty; } }
        public bool IsListenMessageSocket { get { return _RecvSessionTask_Flag; } }
    }
}
