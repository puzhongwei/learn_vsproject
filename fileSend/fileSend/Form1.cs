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

namespace fileSend
{
    public partial class Form1 : Form
    {
        private Thread worker;//这是一个工作线程
        private string hostname;
        private int port;
        private string filename;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (txtPort.Text.Trim() == "")
            {
                MessageBox.Show("ip地址不能为空！");
                txtPort.Focus();
                return;
            }
            if (int.Parse(txtPort.Text.Trim()) < 1 || int.Parse(txtPort.Text.Trim()) > 65535)
            {
                MessageBox.Show("端口范围无效！");
                txtPort.Focus();
                return;
            }
            hostname = txtPort.Text;
            port = int.Parse(txtPort.Text);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "c:/";
            ofd.Filter = "所有文件|*.*";
            ofd.FilterIndex = 1;
            if (ofd.ShowDialog(this) == DialogResult.OK)
                filename = ofd.FileName;
            if (!File.Exists(filename))
            {
                MessageBox.Show("文件不存在！");
                return;
            }
            btnStart.Enabled = false;
            btnCancel.Enabled = true;
            worker = new Thread(new ThreadStart(Start));
            worker.IsBackground = true;
            worker.Start();
        }

        private void Start()
        {
            try
            {
                this.Invoke((EventHandler)(delegate
                {
                    tlblState.Text = "状态：正在连接...";
                    progressBar1.Minimum = 0;
                }));
                TcpClient tcpClient = new TcpClient();
                tcpClient.NoDelay = true;
                tcpClient.SendTimeout = 30000;
                try
                {
                    tcpClient.Connect(hostname, port);
                }
                catch (Exception ex1)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        tlblState.Text = "状态：连接出错...";
                        btnCancel.Enabled = false;
                        btnStart.Enabled = true;
                    }));
                    worker = null;
                    return;
                }
                BinaryWriter writer = new BinaryWriter(tcpClient.GetStream());
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        tlblState.Text = "状态：正在发送文件信息...";
                    }));
                    FileInfo fi = new FileInfo(filename);
                    writer.Write(fi.Name);
                    writer.Write(fi.Length);
                    this.Invoke((EventHandler)(delegate
                    {
                        progressBar1.Maximum = Convert.ToInt32(fi.Length);
                        tlblState.Text = "状态：开始发送文件内容...";
                    }));
                    FileStream fs = fi.OpenRead();
                    try
                    {
                        byte[] buffer = new byte[8192];
                        int len;
                        while ((len = fs.Read(buffer, 0, 8192)) != 0)
                        {
                            writer.Write(buffer, 0, len);
                            this.Invoke((EventHandler)(delegate
                            {
                                progressBar1.Value = len;
                            }));
                        }
                        this.Invoke((EventHandler)(delegate
                        {
                            progressBar1.Value = 0;
                            tlblState.Text = "状态：文件内容发送完成!";
                            btnCancel.Enabled = false;
                            btnStart.Enabled = true;
                        }));
                    }
                    finally
                    {
                        fs.Close();
                    }
                }
                catch (Exception ex2)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        progressBar1.Value = 0;
                        tlblState.Text = "状态：文件发送过程出现错误!";
                        btnCancel.Enabled = false;
                        btnStart.Enabled = true;
                    }));
                }
            }
            catch (Exception ex3)
            {
                this.Invoke((EventHandler)(delegate
                {
                    progressBar1.Value = 0;
                    tlblState.Text = "状态：用户中断!";
                    btnCancel.Enabled = false;
                    btnStart.Enabled = true;
                }));
            }
            finally
            {
                worker = null;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnStart.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            worker.Abort();
            btnCancel.Enabled = false;
            btnStart.Enabled = true;
        }

       

       
    }
}