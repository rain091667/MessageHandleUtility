using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandleUtility
{
    public interface IMessageSocketSession
    {
        void OnNewConnection(string TargetID, IMessageSocketInformation info, out bool AcceptFlag);
        void OnConnectionDisconnected(string TargetID, IMessageSocketInformation info);
        void OnConnectFailed(string TargetID, IMessageSocketInformation info);
    }

    public interface IMessageSendHandler
    {
        void SendMessage(string msg);
        bool IsConnected { get; }
    }

    public abstract class IMessageHandler
    {
        public IMessageSendHandler MessageSendHandler = null;

        public bool IsSendAvailable { get { return MessageSendHandler != null; } }

        public void SendMessage(string msg)
        {
            if (MessageSendHandler == null) return;
            MessageSendHandler.SendMessage(msg);
        }

        bool IsConnected
        {
            get
            {
                if (MessageSendHandler == null) return false;
                return MessageSendHandler.IsConnected;
            }
        }

        public abstract void OnMessageReceive(IMessageContent msg);
    }

    interface IMessageSocketDisconnectEvent
    {
        void OnConnectionDisconnected(string TargetID, IMessageSocketInformation info);
    }
}
