using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography; 

namespace 云管家
{
    public partial class registerdlg : Form
    {
        public registerdlg()
        {
            InitializeComponent();
            this.texPassword.PasswordChar = '●';
            this.texpasswords.PasswordChar = '●';
        }

        public string username;
        public string password;
        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.texPassword.Text.Equals(this.texpasswords.Text))
            {
                MessageBox.Show("两次输入的密码不一致!");
                return;
            }
            byte[] result = Encoding.Default.GetBytes(this.texPassword.Text.Trim());
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            password = BitConverter.ToString(output).Replace("-", "");  //tbMd5pass为输出加密文本的文本框
            username = texUname.Text;
           // password = texPassword.Text;
           
            if (username.Equals("") || password.Equals(""))
            {
                MessageBox.Show(this, "用户名或密码为空!", "提示");
                return;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
