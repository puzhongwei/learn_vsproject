using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace clientT
{
    public partial class Form1 : Form
    {
        private Socket s;                                         //定义Socket对象                  
        public NetworkStream ns;                                //网络流
        public StreamReader sr;                                 //流读取 
        public StreamWriter sw;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress serverIP = IPAddress.Parse("127.0.0.1");   //服务器IP
            try
            {
                s.Connect(serverIP, 9000);                        //连接服务器，端口号用13
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                ns = new NetworkStream(s);                     //实例化网络流
                sr = new StreamReader(ns);                     //实例化流读取对象
                sw = new StreamWriter(ns);                     //实例化写入流对象
                sw.WriteLine(textBox1.Text);                  //将textBox1.Text的数据写入流
                sw.Flush();                                       //清理缓冲区
                lbInfo.Items.Add(sr.ReadLine());              //将从流中读取的数据写入lbInfo28  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                  //捕获异常
            }

        }
    }
}
