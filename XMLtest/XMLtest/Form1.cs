using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography; 

namespace XMLtest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string fpath = @"D:\用户目录\Documents\Visual Studio 2010\Projects\云服务端\userinfo.xml";
            //XmlHelper xml = new XmlHelper();
           /* System.Collections.Hashtable ht = new System.Collections.Hashtable();
            ht["filename"] = "1.cpp";
            ht["size"] = "2KB";
            ht["date"] = "2016/7/23 21:32:31";
            ht["type"] = "文件";
            ht["path"] = " ";*/
            //xml.InsertNode(@"D:\fileinfo.xml", "file", false, "fileroot/folders/folder", ht, ht);
           
            //xml.CreateXmlDocument(fpath, "root", "utf-8");
           // xml.InsertNode(fpath, "", false, "froot", null, null);
           // xml.InsertNode(fpath, "files", false, "fileroot", null, null);
            /*System.Collections.Hashtable ht = new System.Collections.Hashtable();
            ht["UserName"] = "csharpxml";
            ht["Password"] = "123456789"; 
            xml.InsertNode(fpath, "folder2", false, "fileroot/folder1",ht,ht);*/

            byte[] result = Encoding.Default.GetBytes(this.textBox1.Text.Trim());    //tbPass为输入密码的文本框
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            this.textBox2.Text = BitConverter.ToString(output).Replace("-", "");  //tbMd5pass为输出加密文本的文本框
             
           
        }
    }
}
