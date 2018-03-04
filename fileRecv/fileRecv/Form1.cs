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
 
namespace fileRecv
{
    public partial class Form1 : Form
    {
        Thread worker;
        int port;
        public Form1()
        {
            InitializeComponent();
        }
 
 
        private void Start()
        {
            try
            {
                this.Invoke((EventHandler)(delegate
                {
                    tlblState.Text = "状态：开始监听中...";
                }));
                TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
                try
                {
                    tcpListener.Start();
                }
                catch (Exception ex1)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        tlblState.Text = "状态：监听端口出错！";
                        btnCancel.Enabled = false;
                        btnStartListen.Enabled = true;
                    }));
                    worker.Abort();
                    return;
                }
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        progressBar1.Minimum = 0;
                        tlblState.Text = "状态：开始接收文件信息！";
                    }));
                    while (true)
                    {
                        //下面的循环作用是等待新的连接请求到来
                        while (!tcpListener.Pending())
                            Thread.Sleep(100);
                        TcpClient tcpClient = tcpListener.AcceptTcpClient();
                        tcpClient.NoDelay = true;
                        tcpClient.ReceiveTimeout = 30000;
                        BinaryReader reader = new BinaryReader(tcpClient.GetStream());
                        try
                        {
                            string filename = reader.ReadString();
                            long total = reader.ReadInt64();
                            this.Invoke((EventHandler)(delegate
                            {
                                progressBar1.Maximum = Convert.ToInt32(total);
                                tlblState.Text = "状态：你正接收到文件："  + filename+   "，文件大小为：" +  total +  "字节.";
 
                            })); 
                            string saveAs = null;
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.FileName = filename;
                            this.Invoke((EventHandler)(delegate
                            {
                                if (sfd.ShowDialog(this) == DialogResult.OK)
                                    saveAs = sfd.FileName;
                                else
                                    saveAs = null;
                            }));
                            if (saveAs == null)
                            {
                                this.Invoke((EventHandler)(delegate
                                {
                                    tlblState.Text = "状态：你拒绝了该文件！";
                                }));
                            }
                            else
                            {
                                this.Invoke((EventHandler)(delegate
                                {
                                    tlblState.Text = "状态：文件接收中...";
                                }));
                                FileStream fs = File.Create(saveAs);
                                try
                                {
                                    byte[] buffer = new byte[8192];
                                    int len;
                                    while (total > 0)
                                    {
                                        len = reader.Read(buffer, 0, 8192);
 
                                        if (len == 0)
                                        {
                                            this.Invoke((EventHandler)(delegate
                                            {
                                                tlblState.Text = "状态：对方终止连接！";
                                                btnCancel.Enabled = false;
                                                btnStartListen.Enabled = true;
                                                worker.Abort();
                                                return;
                                            }));
                                        }
                                        fs.Write(buffer, 0, len);
                                        this.Invoke((EventHandler)(delegate
                                        {
                                            progressBar1.Value  = len;
                                        }));
                                        total -= len;
                                    }
                                    this.Invoke((EventHandler)(delegate
                                    {
                                        tlblState.Text = "状态：文件接收完成！";
                                        progressBar1.Value = 0;
                                    }));
                                }
                                finally
                                {
                                    fs.Close();
                                }
                            }
                        }
                        catch (Exception ex2)
                        {
                            this.Invoke((EventHandler)(delegate
                            {
                                tlblState.Text = "状态：文件接收中途出错！";
                                btnCancel.Enabled = false;
                                btnStartListen.Enabled = true;
                            }));
                        }
                        finally
                        {
                            reader.Close();
                            tcpClient.Close();
                        }
                    }
                }
                finally
                {
                    tcpListener.Stop();
                }
            }
            catch (Exception ex3)
            {
                this.Invoke((EventHandler)(delegate
                {
                    tlblState.Text = "状态：用户中断！";
                    btnCancel.Enabled = false;
                    btnStartListen.Enabled = true;
                }));
            }
            finally
            {
                worker = null;
            }
        }

        private void btnStartListen_Click_1(object sender, EventArgs e)
        {
            port = int.Parse(txtPort.Text);
            btnStartListen.Enabled = false;
            btnCancel.Enabled = true;
            worker = new Thread(new ThreadStart(Start));
            worker.IsBackground = true;
            worker.Start();
        }
    }
}