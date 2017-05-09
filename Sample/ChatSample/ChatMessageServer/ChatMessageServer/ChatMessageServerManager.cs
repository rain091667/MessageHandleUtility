using MessageHandleUtility.Utility;
using MessageHandleUtility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChatMessageServer
{
    class ChatMessageServerManager : IMessageSocketSession
    {
        private string Server_IP = "server ip";
        private ushort Server_Port = 38500;

        private ConcurrentDictionary<string, MessageControler> _OnLineTable = null;
        private MessageSocketManager _ChatMessageManager = null;
        private string tempTimeString = string.Empty;
        private MessageControler tempMessageControler = null;

        public ChatMessageServerManager()
        {
            _OnLineTable = new ConcurrentDictionary<string, MessageControler>();
            _ChatMessageManager = new MessageSocketManager("MessageServer", this);
        }

        public void Start()
        {
            _ChatMessageManager.StartListenMessageSocket(new IPHostEndPoint(Server_IP, Server_Port));
        }

        public void OnConnectFailed(string TargetID, IMessageSocketInformation info)
        {
            // skip
        }

        public void OnConnectionDisconnected(string TargetID, IMessageSocketInformation info)
        {
            MessageControler temp = null;
            _ChatMessageManager.removeMessageHandler(TargetID);
            _OnLineTable.TryRemove(TargetID, out temp);

            Console.WriteLine("=============================================================");
            Console.WriteLine("Client Disconnected ID: " + TargetID);
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine("OnLine List:");
            foreach (KeyValuePair<string, MessageControler> controler in _OnLineTable)
            {
                Console.WriteLine(controler.Key);
            }
            Task.Run(() =>
            {
                ChartMessageContract tempContract = new ChartMessageContract();
                string tempTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                List<string> onLineList = new List<string>();
                foreach (KeyValuePair<string, MessageControler> controler in _OnLineTable)
                {
                    onLineList.Add(controler.Key);
                }

                foreach (KeyValuePair<string, MessageControler> controler in _OnLineTable)
                {
                    if (controler.Value.IsSendAvailable)
                    {
                        tempContract.UserOnlineLists = onLineList;
                        tempContract.ServerMessageTime = tempTime;
                        tempContract.RecvMessage = "Server: \"" + TargetID + "\" is offline.";
                        controler.Value.SendMessage(SerializeUtility.Serialize(tempContract));
                    }
                }
            });
            Console.WriteLine("=============================================================");
        }

        public void OnNewConnection(string TargetID, IMessageSocketInformation info, out bool AcceptFlag)
        {
            AcceptFlag = true;
            tempTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            info.Info_Description.Add("C.S. Ver.1.0.3.1, RegTime: " + tempTimeString);
            tempMessageControler = new MessageControler(_OnLineTable);
            _ChatMessageManager.addMessageHandler(TargetID, tempMessageControler);
            _OnLineTable.TryAdd(TargetID, tempMessageControler);

            Console.WriteLine("=============================================================");
            Console.WriteLine("Client NewConnection ID: " + TargetID);
            Console.WriteLine("Register Time: " + tempTimeString);
            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine("OnLine List:");
            foreach (KeyValuePair<string, MessageControler> controler in _OnLineTable)
            {
                Console.WriteLine(controler.Key);
            }
            Task.Run(() =>
            {
                ChartMessageContract tempContract = new ChartMessageContract();
                string tempTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                List<string> onLineList = new List<string>();
                foreach (KeyValuePair<string, MessageControler> controler in _OnLineTable)
                {
                    onLineList.Add(controler.Key);
                }

                foreach (KeyValuePair<string, MessageControler> controler in _OnLineTable)
                {
                    if (controler.Value.IsSendAvailable)
                    {
                        tempContract.UserOnlineLists = onLineList;
                        tempContract.ServerMessageTime = tempTime;
                        tempContract.RecvMessage = "Server: \"" + TargetID + "\" is online.";
                        controler.Value.SendMessage(SerializeUtility.Serialize(tempContract));
                    }
                }
            });
            Console.WriteLine("=============================================================");
        }
    }

    class MessageControler : IMessageHandler
    {
        ConcurrentDictionary<string, MessageControler> _OnLineTable = null;
        public MessageControler(ConcurrentDictionary<string, MessageControler> table)
        {
            _OnLineTable = table;
        }

        public override void OnMessageReceive(IMessageContent msg)
        {
            Task.Run(() =>
            {
                ChartMessageContract tempContract = new ChartMessageContract();
                string tempTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                List<string> onLineList = new List<string>();
                foreach (KeyValuePair<string, MessageControler> controler in _OnLineTable)
                {
                    onLineList.Add(controler.Key);
                }

                foreach (KeyValuePair<string, MessageControler> controler in _OnLineTable)
                {
                    if (controler.Value.IsSendAvailable)
                    {
                        tempContract.UserOnlineLists = onLineList;
                        tempContract.ServerMessageTime = tempTime;
                        tempContract.RecvMessage = msg.Data_MessageContent;
                        controler.Value.SendMessage(SerializeUtility.Serialize(tempContract));
                    }
                }
            });
        }
    }
}
