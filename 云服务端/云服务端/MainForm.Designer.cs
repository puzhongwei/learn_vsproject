namespace 云服务端
{
    partial class 云服务端
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
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listbOnline = new System.Windows.Forms.ListBox();
            this.btnBeginListen = new System.Windows.Forms.Button();
            this.lstbxMsgView = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(524, 12);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(161, 21);
            this.txtIp.TabIndex = 2;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(524, 39);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(161, 21);
            this.txtPort.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(491, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(491, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "port";
            // 
            // listbOnline
            // 
            this.listbOnline.FormattingEnabled = true;
            this.listbOnline.ItemHeight = 12;
            this.listbOnline.Location = new System.Drawing.Point(524, 67);
            this.listbOnline.Name = "listbOnline";
            this.listbOnline.Size = new System.Drawing.Size(161, 184);
            this.listbOnline.TabIndex = 6;
            // 
            // btnBeginListen
            // 
            this.btnBeginListen.Location = new System.Drawing.Point(524, 270);
            this.btnBeginListen.Name = "btnBeginListen";
            this.btnBeginListen.Size = new System.Drawing.Size(104, 31);
            this.btnBeginListen.TabIndex = 7;
            this.btnBeginListen.Text = "启动服务";
            this.btnBeginListen.UseVisualStyleBackColor = true;
            this.btnBeginListen.Click += new System.EventHandler(this.btnBeginListen_Click);
            // 
            // lstbxMsgView
            // 
            this.lstbxMsgView.FormattingEnabled = true;
            this.lstbxMsgView.ItemHeight = 12;
            this.lstbxMsgView.Location = new System.Drawing.Point(12, 12);
            this.lstbxMsgView.Name = "lstbxMsgView";
            this.lstbxMsgView.Size = new System.Drawing.Size(473, 304);
            this.lstbxMsgView.TabIndex = 8;
            // 
            // 云服务端
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 373);
            this.Controls.Add(this.lstbxMsgView);
            this.Controls.Add(this.btnBeginListen);
            this.Controls.Add(this.listbOnline);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIp);
            this.Name = "云服务端";
            this.Text = "云服务端";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listbOnline;
        private System.Windows.Forms.Button btnBeginListen;
        private System.Windows.Forms.ListBox lstbxMsgView;
    }
}

