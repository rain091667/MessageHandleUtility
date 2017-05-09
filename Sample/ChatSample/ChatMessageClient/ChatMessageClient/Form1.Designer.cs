namespace ChatMessageClient
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_Login = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_SendMessage = new System.Windows.Forms.Button();
            this.textBox_SendMessage = new System.Windows.Forms.TextBox();
            this.listBox_MessageChat = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox_UserOnlineLists = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listBox_ServerStatus = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Login
            // 
            this.button_Login.Location = new System.Drawing.Point(350, 24);
            this.button_Login.Name = "button_Login";
            this.button_Login.Size = new System.Drawing.Size(126, 33);
            this.button_Login.TabIndex = 0;
            this.button_Login.Text = "Login";
            this.button_Login.UseVisualStyleBackColor = true;
            this.button_Login.Click += new System.EventHandler(this.button_Login_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_Name);
            this.groupBox1.Controls.Add(this.button_Login);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(491, 73);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Setting";
            // 
            // textBox_Name
            // 
            this.textBox_Name.Font = new System.Drawing.Font("Book Antiqua", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Name.Location = new System.Drawing.Point(146, 24);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(188, 33);
            this.textBox_Name.TabIndex = 2;
            this.textBox_Name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Book Antiqua", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "User Name:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_SendMessage);
            this.groupBox2.Controls.Add(this.textBox_SendMessage);
            this.groupBox2.Controls.Add(this.listBox_MessageChat);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(12, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(491, 462);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Chart Message";
            // 
            // button_SendMessage
            // 
            this.button_SendMessage.Font = new System.Drawing.Font("Book Antiqua", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_SendMessage.Location = new System.Drawing.Point(365, 409);
            this.button_SendMessage.Name = "button_SendMessage";
            this.button_SendMessage.Size = new System.Drawing.Size(120, 42);
            this.button_SendMessage.TabIndex = 3;
            this.button_SendMessage.Text = "Send";
            this.button_SendMessage.UseVisualStyleBackColor = true;
            this.button_SendMessage.Click += new System.EventHandler(this.button_SendMessage_Click);
            // 
            // textBox_SendMessage
            // 
            this.textBox_SendMessage.Font = new System.Drawing.Font("Book Antiqua", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_SendMessage.Location = new System.Drawing.Point(6, 421);
            this.textBox_SendMessage.Name = "textBox_SendMessage";
            this.textBox_SendMessage.Size = new System.Drawing.Size(353, 30);
            this.textBox_SendMessage.TabIndex = 2;
            this.textBox_SendMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_SendMessage_KeyPress);
            // 
            // listBox_MessageChat
            // 
            this.listBox_MessageChat.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox_MessageChat.FormattingEnabled = true;
            this.listBox_MessageChat.HorizontalScrollbar = true;
            this.listBox_MessageChat.ItemHeight = 21;
            this.listBox_MessageChat.Location = new System.Drawing.Point(6, 21);
            this.listBox_MessageChat.Name = "listBox_MessageChat";
            this.listBox_MessageChat.Size = new System.Drawing.Size(479, 382);
            this.listBox_MessageChat.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBox_UserOnlineLists);
            this.groupBox3.Location = new System.Drawing.Point(509, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(207, 327);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Online User Lists";
            // 
            // listBox_UserOnlineLists
            // 
            this.listBox_UserOnlineLists.Font = new System.Drawing.Font("Book Antiqua", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox_UserOnlineLists.FormattingEnabled = true;
            this.listBox_UserOnlineLists.HorizontalScrollbar = true;
            this.listBox_UserOnlineLists.ItemHeight = 18;
            this.listBox_UserOnlineLists.Location = new System.Drawing.Point(6, 21);
            this.listBox_UserOnlineLists.Name = "listBox_UserOnlineLists";
            this.listBox_UserOnlineLists.Size = new System.Drawing.Size(195, 292);
            this.listBox_UserOnlineLists.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listBox_ServerStatus);
            this.groupBox4.Location = new System.Drawing.Point(509, 345);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(207, 209);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "ServerStatus";
            // 
            // listBox_ServerStatus
            // 
            this.listBox_ServerStatus.Font = new System.Drawing.Font("Book Antiqua", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox_ServerStatus.FormattingEnabled = true;
            this.listBox_ServerStatus.HorizontalScrollbar = true;
            this.listBox_ServerStatus.ItemHeight = 17;
            this.listBox_ServerStatus.Location = new System.Drawing.Point(6, 18);
            this.listBox_ServerStatus.Name = "listBox_ServerStatus";
            this.listBox_ServerStatus.Size = new System.Drawing.Size(195, 157);
            this.listBox_ServerStatus.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(728, 564);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Name = "Form1";
            this.Text = "Message Handle Utility Beta Ver.1.0.2.1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Login;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_Name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_SendMessage;
        private System.Windows.Forms.ListBox listBox_MessageChat;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBox_UserOnlineLists;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox listBox_ServerStatus;
        private System.Windows.Forms.Button button_SendMessage;
    }
}

