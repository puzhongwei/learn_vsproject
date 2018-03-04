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
    public partial class logindlg : Form
    {
        public string username="";
        public string password="";
        public string newpassword = "";
        public logindlg()
        {
            InitializeComponent();
            texUname.Text = "zhangsan";
            texPassword.Text = "123";
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            byte[] result = Encoding.Default.GetBytes(this.texPassword.Text.Trim());
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            newpassword= BitConverter.ToString(output).Replace("-", ""); 
            username = texUname.Text;
            password = texPassword.Text;
           
            if (username.Equals("") || newpassword.Equals(""))
            {
                MessageBox.Show(this, "用户名或密码为空!", "提示");
                return;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btshowpassword_Click(object sender, EventArgs e)
        {
            
            texPassword.UseSystemPasswordChar = false;
            texPassword.Text = password;
        }
       
        private void texPassword_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                this.texPassword.PasswordChar = Convert.ToChar(0);
            }
            else
            {
                this.texPassword.PasswordChar = '●';
            }
        }

        private void checkBox1_Checked(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                this.texPassword.PasswordChar = Convert.ToChar(0);
            }
            else
            {
                this.texPassword.PasswordChar = '●';
            }
        }
    }
}
