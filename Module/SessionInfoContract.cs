using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandleUtility
{
    public interface IMessageSocketInformation
    {
        string Info_PrimaryID { get; }
        string Info_ServerID { get; }
        string Info_SessionPassCode { get; }
        List<string> Info_Description { get; }
        bool Info_AcceptFlag { get; }
    }

    [DataContract]
    class MessageSocketInformation : IMessageSocketInformation
    {
        [DataMember(Name = "PrimaryID")]
        public string PrimaryID { get; set; }

        [DataMember(Name = "ServerID")]
        public string ServerID { get; set; }

        [DataMember(Name = "SessionPassCode")]
        public string SessionPassCode { get; set; }

        [DataMember(Name = "Description")]
        public List<string> Description { get; set; }

        [DataMember(Name = "AcceptFlag")]
        public bool AcceptFlag { get; set; }

        public string Info_PrimaryID { get { return PrimaryID; } }

        public string Info_ServerID { get { return ServerID; } }

        public string Info_SessionPassCode { get { return SessionPassCode; } }

        public List<string> Info_Description { get { return Description; } }

        public bool Info_AcceptFlag { get { return AcceptFlag; } }

        public MessageSocketInformation(string mPrimaryID)
        {
            this.PrimaryID = mPrimaryID;
            this.ServerID = string.Empty;
            this.Description = new List<string>();
            this.AcceptFlag = false;
            this.SessionPassCode = string.Empty;
        }
    }

    public interface IMessageContent
    {
        string Data_TimeTick_String { get; }

        string Data_MessageContent { get; }

        TimeSpan? Data_TimeSpanInstance { get; }
    }

    [DataContract]
    class MessageSessionContract : IMessageContent
    {
        [DataMember(Name = "TimeTick")]
        public string TimeTick_String { get; set; }

        [DataMember(Name = "MessageContent")]
        public string MessageContent { get; set; }

        public MessageSessionContract()
        {
            this.MessageContent = string.Empty;
            this.TimeTick_String = string.Empty;
        }

        public TimeSpan? TimeSpanInstance
        {
            get
            {
                long long_TimeTickString;
                if (long.TryParse(TimeTick_String, out long_TimeTickString))
                    return TimeSpan.FromTicks(long_TimeTickString);
                return null;
            }
        }

        public string Data_TimeTick_String { get { return TimeTick_String; } }

        public string Data_MessageContent { get { return MessageContent; } }

        public TimeSpan? Data_TimeSpanInstance { get { return TimeSpanInstance; } }
    }
}
