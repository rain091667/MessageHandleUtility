using MessageHandleUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatMessageClient
{
    public partial class Form1 : Form, IMessageSocketSession
    {
        private string Server_IP = "server ip";
        private ushort Server_Port = 38500;

        private delegate void UpdateUI();
        private delegate void UpdateServerStatus(string msg);
        public const string ServerID = "MessageServer";
        private MessageSocketManager manager = null;
        private MessageControler ClientMessageControler = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientMessageControler = new MessageControler(listBox_MessageChat, listBox_UserOnlineLists);
        }

        private void button_SendMessage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_SendMessage.Text))
            {
                MessageBox.Show("no message!");
                return;
            }

            if (ClientMessageControler.IsSendAvailable)
            {
                ClientMessageControler.SendMessage(textBox_Name.Text + ": " + textBox_SendMessage.Text);
                textBox_SendMessage.Text = "";
            }
            else
            {
                MessageBox.Show("message send is unavailable!");
            }
        }

        private void textBox_SendMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (string.IsNullOrEmpty(textBox_SendMessage.Text))
                {
                    MessageBox.Show("no message!");
                    return;
                }

                if (ClientMessageControler.IsSendAvailable)
                {
                    ClientMessageControler.SendMessage(textBox_Name.Text + ": " + textBox_SendMessage.Text);
                    textBox_SendMessage.Text = "";
                }
                else
                {
                    MessageBox.Show("message send is unavailable!");
                }
            }
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_Name.Text))
            {
                MessageBox.Show("Input your name!");
                return;
            }

            manager = new MessageSocketManager(textBox_Name.Text, this);
            manager.addMessageHandler(ServerID, ClientMessageControler);
            manager.Connect(new IPHostEndPoint(Server_IP, Server_Port), null);            
        }

        private void ListBox_UpdateServerStatus(string msg)
        {
            if (listBox_ServerStatus.InvokeRequired)
            {
                listBox_ServerStatus.Invoke(new UpdateServerStatus(ListBox_UpdateServerStatus), new object[] { msg });
            }
            else
            {
                listBox_ServerStatus.Items.Add(msg);
            }
        }

        private void UpdateUI_GroupBox()
        {
            if (groupBox2.InvokeRequired)
            {
                groupBox2.Invoke(new UpdateUI(UpdateUI_GroupBox));
            }
            else
            {
                groupBox2.Enabled = true;
                groupBox1.Enabled = false;
            }
        }

        public void OnNewConnection(string TargetID, IMessageSocketInformation info, out bool AcceptFlag)
        {
            AcceptFlag = false;
            if (TargetID == ServerID)
            {
                AcceptFlag = true;
                ListBox_UpdateServerStatus("Connect success.");
                ListBox_UpdateServerStatus("Server: " + info.Info_Description[0]);
                UpdateUI_GroupBox();
            }
        }

        public void OnConnectionDisconnected(string TargetID, IMessageSocketInformation info)
        {
            ListBox_UpdateServerStatus("Disconnected...");
            ListBox_UpdateServerStatus("Reconnect in 5 seconds.");
            Task.Run(() => ReConnectTask());
        }

        public void OnConnectFailed(string TargetID, IMessageSocketInformation info)
        {
            ListBox_UpdateServerStatus("ConnectFailed...");
            ListBox_UpdateServerStatus("Reconnect in 5 seconds.");
            Task.Run(() => ReConnectTask());
        }

        private async void ReConnectTask()
        {
            int count = 5;
            while (count > 0)
            {
                await Task.Delay(1000);
                count--;
                ListBox_UpdateServerStatus("Reconnect in " + count + " seconds.");
            }
            manager.Connect(new IPHostEndPoint(Server_IP, Server_Port), null);
        }
    }
}
