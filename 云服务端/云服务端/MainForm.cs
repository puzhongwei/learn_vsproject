using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;    
using System.Threading;
using System.IO;


namespace 云服务端
{
    public partial class 云服务端 : Form
    {
        public 云服务端()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            txtIp.Text = "127.0.0.1";
            txtPort.Text = "9000";
        }

        private void btnBeginListen_Click(object sender, EventArgs e)
        {
            try
            {
                
                Files.BeginListening(txtIp.Text, txtPort.Text, lstbxMsgView, listbOnline);
                btnBeginListen.Enabled = false;
                //btnCancel.Enabled = true;
            }
            catch (Exception ex)
            {
                ShwMsgForView.ShwMsgforView(lstbxMsgView, "监听服务器出现了错误:" + ex.Message);
            }
        }
 
    }
}
