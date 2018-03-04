namespace 服务端
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbOnline = new System.Windows.Forms.ListBox();
            this.btnBeginListen = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lable2 = new System.Windows.Forms.Label();
            this.txtSelectFile = new System.Windows.Forms.TextBox();
            this.txtMsg = new System.Windows.Forms.RichTextBox();
            this.lstbxMsgView = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbOnline
            // 
            this.lbOnline.FormattingEnabled = true;
            this.lbOnline.ItemHeight = 12;
            this.lbOnline.Location = new System.Drawing.Point(487, 89);
            this.lbOnline.Name = "lbOnline";
            this.lbOnline.Size = new System.Drawing.Size(217, 196);
            this.lbOnline.TabIndex = 1;
            // 
            // btnBeginListen
            // 
            this.btnBeginListen.Location = new System.Drawing.Point(584, 301);
            this.btnBeginListen.Name = "btnBeginListen";
            this.btnBeginListen.Size = new System.Drawing.Size(88, 23);
            this.btnBeginListen.TabIndex = 2;
            this.btnBeginListen.Text = "启动服务";
            this.btnBeginListen.UseVisualStyleBackColor = true;
            this.btnBeginListen.Click += new System.EventHandler(this.btnBeginListen_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(584, 341);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(88, 23);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "发送消息";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(584, 370);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(88, 23);
            this.btnSendFile.TabIndex = 5;
            this.btnSendFile.Text = "发送文件";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(487, 388);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(91, 23);
            this.btnSelectFile.TabIndex = 6;
            this.btnSelectFile.Text = "选择文件";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(554, 12);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(136, 21);
            this.txtIp.TabIndex = 7;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(554, 49);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(136, 21);
            this.txtPort.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(525, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "IP:";
            // 
            // lable2
            // 
            this.lable2.AutoSize = true;
            this.lable2.Location = new System.Drawing.Point(513, 52);
            this.lable2.Name = "lable2";
            this.lable2.Size = new System.Drawing.Size(35, 12);
            this.lable2.TabIndex = 10;
            this.lable2.Text = "port:";
            // 
            // txtSelectFile
            // 
            this.txtSelectFile.Location = new System.Drawing.Point(12, 392);
            this.txtSelectFile.Name = "txtSelectFile";
            this.txtSelectFile.Size = new System.Drawing.Size(454, 21);
            this.txtSelectFile.TabIndex = 11;
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(12, 12);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(431, 160);
            this.txtMsg.TabIndex = 13;
            this.txtMsg.Text = "";
            // 
            // lstbxMsgView
            // 
            this.lstbxMsgView.FormattingEnabled = true;
            this.lstbxMsgView.ItemHeight = 12;
            this.lstbxMsgView.Location = new System.Drawing.Point(12, 213);
            this.lstbxMsgView.Name = "lstbxMsgView";
            this.lstbxMsgView.Size = new System.Drawing.Size(440, 136);
            this.lstbxMsgView.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 425);
            this.Controls.Add(this.lstbxMsgView);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.txtSelectFile);
            this.Controls.Add(this.lable2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnBeginListen);
            this.Controls.Add(this.lbOnline);
            this.Name = "Form1";
            this.Text = "服务端";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbOnline;
        private System.Windows.Forms.Button btnBeginListen;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lable2;
        private System.Windows.Forms.TextBox txtSelectFile;
        private System.Windows.Forms.RichTextBox txtMsg;
        private System.Windows.Forms.ListBox lstbxMsgView;
    }
}

