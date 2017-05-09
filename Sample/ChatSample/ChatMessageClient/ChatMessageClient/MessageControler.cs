using MessageHandleUtility;
using MessageHandleUtility.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatMessageClient
{
    class MessageControler : IMessageHandler
    {
        private delegate void UpdateMsg(string msg);
        private delegate void UpdateOnlineLists(string[] list);
        private ChartMessageContract data = null;
        private ListBox _MessageListBox = null;
        private ListBox _OnlineListsListBox = null;

        public MessageControler(ListBox MessageListBox, ListBox OnlineListsListBox)
        {
            _MessageListBox = MessageListBox;
            _OnlineListsListBox = OnlineListsListBox;
        }

        public override void OnMessageReceive(IMessageContent msg)
        {
            data = SerializeUtility.DeSerialize<ChartMessageContract>(msg.Data_MessageContent);
            ListBoxMessageUpdate("[" + data.MessageTime() + "] " + data.RecvMessage);
            ListBoxOnLineListsUpdate(data.UserOnlineLists.ToArray());
        }

        private void ListBoxMessageUpdate(string msg)
        {
            if (_MessageListBox.InvokeRequired)
            {
                _MessageListBox.Invoke(new UpdateMsg(ListBoxMessageUpdate), new object[] { msg });
            }
            else
            {
                _MessageListBox.Items.Add(msg);
                int visibleItems = _MessageListBox.ClientSize.Height / _MessageListBox.ItemHeight;
                _MessageListBox.TopIndex = Math.Max(_MessageListBox.Items.Count - visibleItems + 1, 0);
            }
        }

        private void ListBoxOnLineListsUpdate(string[] list)
        {
            if (_OnlineListsListBox.InvokeRequired)
            {
                _OnlineListsListBox.Invoke(new UpdateOnlineLists(ListBoxOnLineListsUpdate), new object[] { list });
            }
            else
            {
                _OnlineListsListBox.Items.Clear();
                for (int i = 0; i < list.Length; i++)
                {
                    _OnlineListsListBox.Items.Add(list[i]);
                }
                int visibleItems = _OnlineListsListBox.ClientSize.Height / _OnlineListsListBox.ItemHeight;
                _OnlineListsListBox.TopIndex = Math.Max(_OnlineListsListBox.Items.Count - visibleItems + 1, 0);
            }
        }
    }
}
