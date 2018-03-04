using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ServT
{
    public partial class Form1 : Form
    {
        private Socket s;                                    //定义Socket对象
        private Thread th;                                   //客户端连接服务器的线程
        public Socket cSocket;                              //单个客户端连接的Socket对象
        public NetworkStream ns;                            //网络流
        public StreamReader sr;                        //流读取
        public StreamWriter sw;                        //流写入
        private delegate void SetTextCallback();         //用于操作主线程控件
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket对象
            IPAddress serverIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint server = new IPEndPoint(serverIP, 9000);    //实例化服务器的IP和端口
            s.Bind(server);
            s.Listen(10);
            try
            {
                th = new Thread(new ThreadStart(Communication));     //创建线程
                th.Start();                                                //启动线程
                label.Text = "服务器启动成功！";
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器启动失败！" + ex.Message);
            }
        }
        public void Communication()
        {
            while (true)
            {
                try
                {
                    cSocket = s.Accept();                   //用cSocket来代表该客户端连接
                    if (cSocket.Connected)                  //测试是否连接成功
                    {
                        ns = new NetworkStream(cSocket);  //建立网络流，便于数据的读取
                        sr = new StreamReader(ns);         //实例化流读取对象
                        sw = new StreamWriter(ns);         //实例化写入流对象
                        test();                               //从流中读取
                        sw.WriteLine("收到请求，允许连接"); //向流中写入数据
                        sw.Flush();                           //清理缓冲区
                    }
                    else
                    {
                        MessageBox.Show("连接失败");
                    }
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message);           //捕获Socket异常
                }
                catch (Exception es)
                {
                    MessageBox.Show("其他异常" + es.Message);      //捕获其他异常
                }
            }
        }
        //以下代码的用法在第16章有关线程的用法时曾提到过，主要用于从当前线程操作主线程中的控件，这里就不在赘//述
        public void send()
        {
            lbInfo.Items.Add(sr.ReadLine() + "\n");
        }
        public void test()
        {
            SetTextCallback stcb = new SetTextCallback(send);
            Invoke(stcb);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

  }
}
