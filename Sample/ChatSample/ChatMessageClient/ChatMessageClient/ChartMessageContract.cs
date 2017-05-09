using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChatMessageClient
{
    [DataContract]
    class ChartMessageContract
    {
        [DataMember(Name = "UserOnlineLists")]
        public List<string> UserOnlineLists { get; set; }

        [DataMember(Name = "RecvMessage")]
        public string RecvMessage { get; set; }

        [DataMember(Name = "ServerMessageTime")]
        public string ServerMessageTime { get; set; }

        public ChartMessageContract()
        {
            UserOnlineLists = new List<string>();
            ServerMessageTime = RecvMessage = string.Empty;
        }

        public string MessageTime()
        {
            DateTime temp;
            if (DateTime.TryParse(ServerMessageTime, out temp))
            {
                return temp.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return null;
        }
    }
}
