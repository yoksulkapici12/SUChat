namespace chat_client
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.screen = new System.Windows.Forms.RichTextBox();
            this.ip = new System.Windows.Forms.TextBox();
            this.port = new System.Windows.Forms.TextBox();
            this.user_name = new System.Windows.Forms.TextBox();
            this.msg = new System.Windows.Forms.TextBox();
            this.send = new System.Windows.Forms.Button();
            this.connect = new System.Windows.Forms.Button();
            this.IF100 = new System.Windows.Forms.Button();
            this.SPS101 = new System.Windows.Forms.Button();
            this.Leave_it = new System.Windows.Forms.Button();
            this.participants = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.disconnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // screen
            // 
            this.screen.Location = new System.Drawing.Point(474, 43);
            this.screen.Name = "screen";
            this.screen.ReadOnly = true;
            this.screen.Size = new System.Drawing.Size(241, 353);
            this.screen.TabIndex = 0;
            this.screen.Text = "";
            // 
            // ip
            // 
            this.ip.Location = new System.Drawing.Point(121, 62);
            this.ip.Name = "ip";
            this.ip.Size = new System.Drawing.Size(181, 22);
            this.ip.TabIndex = 1;
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(121, 90);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(181, 22);
            this.port.TabIndex = 2;
            // 
            // user_name
            // 
            this.user_name.Location = new System.Drawing.Point(121, 118);
            this.user_name.Name = "user_name";
            this.user_name.Size = new System.Drawing.Size(181, 22);
            this.user_name.TabIndex = 3;
            // 
            // msg
            // 
            this.msg.Location = new System.Drawing.Point(110, 325);
            this.msg.Name = "msg";
            this.msg.Size = new System.Drawing.Size(294, 22);
            this.msg.TabIndex = 4;
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(303, 363);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(75, 23);
            this.send.TabIndex = 5;
            this.send.Text = "send";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // connect
            // 
            this.connect.Location = new System.Drawing.Point(68, 172);
            this.connect.Name = "connect";
            this.connect.Size = new System.Drawing.Size(75, 23);
            this.connect.TabIndex = 6;
            this.connect.Text = "connect";
            this.connect.UseVisualStyleBackColor = true;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // IF100
            // 
            this.IF100.Location = new System.Drawing.Point(276, 229);
            this.IF100.Name = "IF100";
            this.IF100.Size = new System.Drawing.Size(102, 34);
            this.IF100.TabIndex = 7;
            this.IF100.Text = "IF100";
            this.IF100.UseVisualStyleBackColor = true;
            this.IF100.Click += new System.EventHandler(this.IF100_Click);
            // 
            // SPS101
            // 
            this.SPS101.Location = new System.Drawing.Point(68, 229);
            this.SPS101.Name = "SPS101";
            this.SPS101.Size = new System.Drawing.Size(102, 34);
            this.SPS101.TabIndex = 8;
            this.SPS101.Text = "SPS101";
            this.SPS101.UseVisualStyleBackColor = true;
            this.SPS101.Click += new System.EventHandler(this.SPS101_Click);
            // 
            // Leave_it
            // 
            this.Leave_it.Location = new System.Drawing.Point(167, 269);
            this.Leave_it.Name = "Leave_it";
            this.Leave_it.Size = new System.Drawing.Size(102, 34);
            this.Leave_it.TabIndex = 9;
            this.Leave_it.Text = "Leave";
            this.Leave_it.UseVisualStyleBackColor = true;
            this.Leave_it.Click += new System.EventHandler(this.Leave_it_Click);
            // 
            // participants
            // 
            this.participants.Location = new System.Drawing.Point(335, 43);
            this.participants.Name = "participants";
            this.participants.ReadOnly = true;
            this.participants.Size = new System.Drawing.Size(133, 164);
            this.participants.TabIndex = 10;
            this.participants.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(365, 24);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(73, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Participant ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 65);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(19, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 93);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(31, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 124);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(73, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "UserName";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 331);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(64, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "Message";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(671, 24);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(44, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "Room";
            // 
            // disconnect
            // 
            this.disconnect.Location = new System.Drawing.Point(208, 171);
            this.disconnect.Name = "disconnect";
            this.disconnect.Size = new System.Drawing.Size(85, 24);
            this.disconnect.TabIndex = 17;
            this.disconnect.Text = "disconnect";
            this.disconnect.UseVisualStyleBackColor = true;
            this.disconnect.Click += new System.EventHandler(this.disconnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.disconnect);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.participants);
            this.Controls.Add(this.Leave_it);
            this.Controls.Add(this.SPS101);
            this.Controls.Add(this.IF100);
            this.Controls.Add(this.connect);
            this.Controls.Add(this.send);
            this.Controls.Add(this.msg);
            this.Controls.Add(this.user_name);
            this.Controls.Add(this.port);
            this.Controls.Add(this.ip);
            this.Controls.Add(this.screen);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox screen;
        private System.Windows.Forms.TextBox ip;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.TextBox user_name;
        private System.Windows.Forms.TextBox msg;
        private System.Windows.Forms.Button send;
        private System.Windows.Forms.Button connect;
        private System.Windows.Forms.Button IF100;
        private System.Windows.Forms.Button SPS101;
        private System.Windows.Forms.Button Leave_it;
        private System.Windows.Forms.RichTextBox participants;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button disconnect;
    }
}

