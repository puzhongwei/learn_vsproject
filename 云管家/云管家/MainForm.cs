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
using System.Threading;
using System.IO;
using MyCharRoomClient;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
using System.Media;
using System.Security.Cryptography;

 
namespace 云管家
{
    public partial class MainForm : Form
    {
        /// <summary>接收用</summary>  
        private UdpClient receiveUdpClient;
        /// <summary>发送用</summary>  
        private UdpClient sendUdpClient;
        /// <summary>和本机绑定的端口号</summary>  
        private  int port = 18001;
        private int port1 = 18001;
        /// <summary>本机IP</summary>  
        IPAddress ip=IPAddress.Parse("127.0.0.1");
        /// <summary>远程主机IP</summary>  
        IPAddress remoteIp=IPAddress.Parse("127.0.0.1");  


       
        public MainForm()
        {
            InitializeComponent();
           // showinfo();//显示云端文件
            initlistview();
             buttonvisiable(true);
            TextBox.CheckForIllegalCrossThreadCalls = false;
            initMainframButton(false);

            //textBoxRemoteIP.Text = remoteIp.ToString();
            textBoxSend.Text = "你好！";  
           

            
            ;

        }
        void initMainframButton(bool r)
        {
            btdown.Enabled =r;
            btstop.Enabled = r;
            btshare.Enabled = r;
            btdelete.Enabled = r;
            btshareOK.Enabled = r;
        }
        Thread threadClient = null; // 创建用于接收服务端消息的 线程；
        //Thread threadrecfile = null; // 创建文件线程
        Socket sockClient = null;
        private string filename;
        public string downloadfile;
        public string sharefile;
        bool islogin = false;
        bool isstop = false;
        bool isregistersuccess = false;
        public static long posission = 0;
        bool isxuchuan = false;
        int index = 0;
        ListViewItem lvi = new ListViewItem();
        ListViewEx lve = new ListViewEx();

 
        /// <summary>
        /// 连接服务器
        /// </summary>
        bool connect()
        {
            //IPAddress ip = IPAddress.Parse(txtIp.Text.Trim());
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            //IPAddress ip = IPAddress.Parse("172.29.135.246");


            // IPEndPoint endPoint = new IPEndPoint(ip, in.Parse(txtPort.Text.Trim()));
            IPEndPoint endPoint = new IPEndPoint(ip, int.Parse("9000"));
            sockClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
           /* IPAddress address = IPAddress.Parse("127.0.0.1");
            //创建包含Ip和port的网络节点对象
            IPEndPoint endpoint = new IPEndPoint(address, int.Parse("9002"));
            //将负责监听的套接字绑定到唯一的Ip和端口上
            sockClient.Bind(endpoint);*/
            try
            {
                ShowMsg("与服务器连接中……");
                sockClient.Connect(endPoint);

            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
                return false;
                //this.Close();  
            }
            ShowMsg("与服务器连接成功！！！");
            threadClient = new Thread(RecMsg);
            threadClient.IsBackground = true;
            threadClient.Start();
            return true;
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        void RecMsg()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];

                try
                {
                    sockClient.Receive(buffer); // 接收数据，并返回数据的长度； 
                    if (buffer[0] == 1)//发送文件请求
                    {
                       recevefile();
                       
                       if (!isxuchuan)//完成提示音
                       {
                           try
                           {
                           System.Media.SoundPlayer sp = new SoundPlayer();
                           sp.SoundLocation = @"D:\云管家\resource\download-complete.wav";
                           
                               sp.Play();
                           }
                           catch (FileNotFoundException en)
                           {
                               MessageBox.Show("异常:" + en.Message);
                           }
                       }
                      // buffer[0] = 15;
                       /* threadrecfile = new Thread(recevefile);
                        threadrecfile.IsBackground = true;
                        threadrecfile.Start();*/
                      
                    }
                    else  if (buffer[0] == 3)//反馈登陆正确
                    {
                        islogin = true;
                        
                    }
                    else if (buffer[0] == 8)//反馈注册成功
                    {
                        isregistersuccess = true;

                    }
                    
                   
                }

                catch (SocketException se)
                {
                    ShowMsg("异常；" + se.Message);
                    return;
                }
                catch (Exception e)
                {
                    ShowMsg("异常：" + e.Message);
                    return;
                }
                /* if (arrMsgRec[0] == 0) // 表示接收到的是消息数据；  
                 {
                     string strMsg = System.Text.Encoding.UTF8.GetString(arrMsgRec, 1, length - 1);// 将接受到的字节数据转化成字符串；  
                     ShowMsg(strMsg);
                 }*/

            }

        }

        /// <summary>
        /// 接收文件
        /// </summary>
        void recevefile()
        {
            byte[] buffer = new byte[1024*100];
            sockClient.Receive(buffer); // 接收数据，并返回数据的长度；
            long receive = 0L, length = BitConverter.ToInt64(buffer, 0);
            string fileName = Encoding.Default.GetString(buffer, 0, sockClient.Receive(buffer));
            string savepath = Path.Combine("D://云管家//mydownload//", fileName);
            XmlHelper xml4 = new XmlHelper();
            XmlNodeList nodelist4 = xml4.GetXmlNodeListByXpath(@"D:\云管家\config\xuchuan.xml", "/root/file");
            foreach (XmlNode xn in nodelist4)
            {
                //遍历子节点
                string name = (xn.SelectSingleNode("filename")).InnerText;
                if (name.Equals(fileName))
                {
                    isxuchuan = true;
                }
            }
            if (!isxuchuan)
            {
                filesavedlg fdlg = new filesavedlg(savepath);
                if (fdlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (fdlg.filename != "")
                    {
                        savepath = fdlg.filename;
                    }
                    fdlg.Close();
                    if (File.Exists(savepath))
                    {
                        if (MessageBox.Show(this, "文件已经存在，是否替换？", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                            return;
                    }
                }
            }
            else
            {
                if (MessageBox.Show(this, "是否继续下载？", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    return;
            }
            this.Invoke((EventHandler)(delegate
            {

                progressBar1.Maximum = Convert.ToInt32(length);
                //tlblState.Text = "状态：你正接收到文件：" + filename + "，文件大小为：" + total + "字节.";

            }));
            try
            {

                //string strinfo = readlineintxt(@"D:\xuchuan.txt");
                // string[] s = strinfo.Split(new char[] { ' ' });
                XmlHelper xml = new XmlHelper();
                XmlNodeList nodelist1 = xml.GetXmlNodeListByXpath(@"D:\云管家\config\xuchuan.xml", "/root/file");
                foreach (XmlNode xn in nodelist1)
                {
                    //遍历子节点
                    string name = (xn.SelectSingleNode("filename")).InnerText;
                    if (name.Equals(fileName))
                    {
                        isxuchuan = true;
                    }
                }
                if (!isxuchuan)
                {
                    using (FileStream writer = new FileStream(savepath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        int received;
                        double percent = 0;
                        //断点接受 在这里判断设置writer.Position即可
                        while (receive < length)
                        {
                            if (isstop)
                            {
                                //posission = receive;
                                // posission = 1024;
                                // writlinfo(@"D:\xuchuan.txt", "true " + receive.ToString() + " "+fileName+"\r\n");
                                //MyVars.position = receive;
                                System.Collections.Hashtable ht = new System.Collections.Hashtable();
                                ht["filename"] = fileName;
                                ht["posission"] = receive.ToString();
                                ht["isxuchuan"] = "true";
                                ht["length"] = length.ToString();
                                XmlHelper xml2 = new XmlHelper();
                                xml2.InsertNode(@"D:\云管家\config\xuchuan.xml", "file", false, "/root", ht, ht);
                                //sockClient.
                                //sockClient.Close();

                               
                                return;
                            }
                            received = sockClient.Receive(buffer);
                            writer.Write(buffer, 0, received);
                            writer.Flush();
                            receive += (long)received;
                            percent = receive * 100.00 / length;
                            string strprecent = String.Format("{0:F}", percent);//默认为保留两位
                            strprecent = strprecent + "%";
                            this.Invoke((EventHandler)(delegate
                            {
                                label2.Text = strprecent;
                                progressBar1.Value = Convert.ToInt32(receive);
                            }));
                        }
                        ShowMsg("文件保存成功:");

                        this.Invoke((EventHandler)(delegate
                        {
                            //tlblState.Text = "状态：文件接收完成！";
                            progressBar1.Value = 0;
                            btstop.Enabled = false;
                            label2.Text = "";
                        }));

                    }
                }
                else if (isxuchuan)
                {
                    using (FileStream writer = new FileStream(savepath, FileMode.Open, FileAccess.Write, FileShare.None))
                    {
                        int received;
                        double percent = 0;
                        long p = 0;
                        //断点接受 在这里判断设置writer.Position即可
                        //  string info = readlineintxt(@"D:\xuchuan.txt");
                        // string[] s2 = info.Split(new char[] { ' ' });

                        XmlHelper xml3 = new XmlHelper();
                        XmlNodeList nodelist2 = xml3.GetXmlNodeListByXpath(@"D:\云管家\config\xuchuan.xml", "/root/file");
                        foreach (XmlNode xn in nodelist2)
                        {
                            //遍历子节点

                            string name = (xn.SelectSingleNode("filename")).InnerText;
                            // string isxuchuan = (xn.SelectSingleNode("isxuchuan")).InnerText;
                            if (name.Equals(fileName))
                            {
                                writer.Position = long.Parse((xn.SelectSingleNode("posission")).InnerText.ToString());
                                p = writer.Position;
                            }
                        }

                        double percent1 = p * 100.00 / length;
                        string strprecent1 = String.Format("{0:F}", percent);//默认为保留两位
                        //percent.ToString("#.00");
                        strprecent1 = strprecent1 + "%";
                        this.Invoke((EventHandler)(delegate
                        {
                            label2.Text = strprecent1;
                            progressBar1.Value = Convert.ToInt32(p);
                        }));
                        while (receive + p < length)
                        {
                            /*if (receive== 1024 * 1024)
                                return;*/
                            if (isstop)
                            {
                                //writlinfo(@"D:\xuchuan.txt", "true " + receive.ToString() + " "+fileName+"\r\n");
                                System.Collections.Hashtable ht = new System.Collections.Hashtable();
                                ht["filename"] = fileName;
                                ht["posission"] = receive.ToString();
                                ht["isxuchuan"] = "true";
                                XmlHelper xml1 = new XmlHelper();
                                // xml1.UpdateNode(@"D:\xuchuan.xml", "/root", ht, ht);//待重写
                                // xml.InsertNode(@"D:\xuchuan.xml", "file", false, "/root", ht, ht);
                                // return;
                            }
                            received = sockClient.Receive(buffer);
                            writer.Write(buffer, 0, received);
                            writer.Flush();
                            receive += (long)received;
                            //Console.WriteLine("Received " + receive + "/" + length + ".");//进度
                            percent = (Convert.ToInt32(p + receive) * 100.00) / length;
                            string strprecent = String.Format("{0:F}", percent);//默认为保留两位
                            //percent.ToString("#.00");
                            strprecent = strprecent + "%";
                            this.Invoke((EventHandler)(delegate
                            {
                                label2.Text = strprecent;
                               // lve.Items[0].SubItems[1].Text = strprecent;//////////////////
                               // System.Threading.Thread.Sleep(100);
                                progressBar1.Value = Convert.ToInt32(p + Convert.ToInt32(receive));
                            }));
                        }


                    }
                    ShowMsg("文件保存成功:");
                    isxuchuan = false;
                    delfromxml(@"D:\云管家\config\xuchuan.xml", fileName, "续传");
                    this.Invoke((EventHandler)(delegate
                    {
                        //tlblState.Text = "状态：文件接收完成！";
                        progressBar1.Value = 0;
                        btstop.Enabled = false;
                        label2.Text = "";
                    }));
                }

            }
            catch (ArgumentException ea)
            {
                MessageBox.Show("异常:" + ea.Message);
                return;
            }

        }
       


        public  string readlineintxt(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path, Encoding.UTF8);
                string r=sr.ReadLine();
                sr.Close();
                return r;

            }
            catch (FileLoadException efl)
            {
                MessageBox.Show("异常:" + efl.Message);
                return "";
            }
            catch (FieldAccessException efld)
            {
                MessageBox.Show("异常:" + efld.Message);
                return "";
            }
        }
        /// <summary>
        /// 显示提示消息
        /// </summary>
        /// <param name="str"></param>
        void ShowMsg(string str)
        {
           // txtMsg.AppendText(str + "\r\n");
        }

      
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btsendfile_Click(object sender, EventArgs e)
        {
            const int BufferSize = 1024;
            string fileinfo;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "D:\\";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = ofd.FileName;
            }
            if (filename.Equals(""))
            {
                MessageBox.Show("请选择要发送的文件！！！");
                return;
            }
            else
            {
                try
                {
                    
                    fileinfo = Path.GetFileName(filename);
                    string fname = Path.GetFileName(filename);
                    string size = GetFileSize(filename);
                    string data = DateTime.Now.ToString();
                  
                    fileinfo += "#" + size + "#" + data + "#" + "文件" + "#"+filename+"\r\n";
                    //string path = @"D:\用户目录\Documents\Visual Studio 2010\Projects\云管家\云管家\setting.xml";
                    System.Collections.Hashtable ht = new System.Collections.Hashtable();
                    ht["filename"] = fname;
                    ht["size"] = size;
                    ht["date"] = data;
                    ht["type"] = "文件";
                    ht["path"] = filename;
                  
                    using (FileStream reader = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    {


                        byte[] arrSendMsg = new byte[1];
                        arrSendMsg[0] = 1; // 用来表示发送的是文件 
                        sockClient.Send(arrSendMsg); // 发送消息；  


                        long send = 0L, length = reader.Length;
                        sockClient.Send(BitConverter.GetBytes(length));
                        string fileName = Path.GetFileName(filename);
                        sockClient.Send(Encoding.Default.GetBytes(fileName));
                        //Console.WriteLine("Sending file:" + fileName + ".Plz wait...");
                        byte[] buffer = new byte[BufferSize];
                        int read, sent;

                        //进度条

                      //this.Invoke((EventHandler)(delegate
                      //  {
                           
                            progressBar1.Maximum = Convert.ToInt32(length);
                            //tlblState.Text = "状态：你正接收到文件：" + filename + "，文件大小为：" + total + "字节.";

                       // })); 
                          

                       
                        //断点发送 在这里判断设置reader.Position即可

                        //double percent = 0;
                        //long temp = 0;
                          while ((read = reader.Read(buffer, 0, BufferSize)) != 0)
                          {
                              sent = 0;

                             
                              while ((sent += sockClient.Send(buffer, sent, read, SocketFlags.None)) < read)
                              {
                                  
                                  send += (long)sent;
                                  //Console.WriteLine("Sent " + send + "/" + length + ".");//进度

                              }
                             /* temp += sent;
                              percent =temp * 100.00 / length;
                              string strprecent = String.Format("{0:F}", percent);//默认为保留两位
                              //percent.ToString("#.00");
                              strprecent = strprecent + "%";*/
                           //   this.Invoke((EventHandler)(delegate
                           //   {
                                  //label2.Text = strprecent;
                                  progressBar1.Value += sent;
                          //    }));

                          }
                          

                        }
                     
                        // Console.WriteLine("Send finish.");
                        /*StreamWriter sw = File.AppendText("D:\\fileinfo.txt");
                        sw.Write(fileinfo);
                        sw.Close();*/
                         XmlHelper xml = new XmlHelper();
                         xml.InsertNode(@"D:\云管家\config\fileinfo.xml", "file", false, "fileroot/files", ht, ht);
                       // writeinfo("D:\\fileinfo.txt", fileinfo);
                        //txtMsg.Text = "上传成功!";
                        MessageBox.Show("上传成功！");
                     //  this.Invoke((EventHandler)(delegate
                     //   {
                            //tlblState.Text = "状态：文件接收完成！";
                            progressBar1.Value = 0;
                            //label2.Text = "";
                     //   }));
                       showinfo();
                    }
                
                catch (SocketException se)
                {
                    ShowMsg("异常；" + se.Message);
                    return;
                }
                catch (Exception e2)
                {
                    ShowMsg("异常：" + e2.Message);
                    return;
                }
                


            }
        }
      

       /// <summary>
       /// 下载文件按钮
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void btdownload_Click(object sender, EventArgs e)
        {
               
                byte[] arrSendMsg = new byte[1024];
                arrSendMsg[0] = 2; // 用来表示发送的是文件 
                sockClient.Send(arrSendMsg);

                sockClient.Send(Encoding.Default.GetBytes(downloadfile)); //申请下载文件名  
                btstop.Enabled = true;//暂停按钮可用
           
        }

        /// <summary>
        /// 初始化listview
        /// </summary>
        void initlistview()
        {
            this.listView1.Clear(); //清除所有项和列
            this.listView1.Height = 305;
            this.listView1.Width = 657;
            this.listView1.SmallImageList = this.imageList1;  //将listView的图标集与imageList1绑定
            this.listView1.Columns.Add("文件名", 300, HorizontalAlignment.Left);
            this.listView1.Columns.Add("大小", 80, HorizontalAlignment.Left);
            this.listView1.Columns.Add("修改时间", 120, HorizontalAlignment.Left);
            this.listView1.Columns.Add("文件类型", 120, HorizontalAlignment.Left);
        }
        /// <summary>
        /// 显示全部云端相关信息
        /// </summary>
        void showinfo()
        {
            textBox1.Text = "我的网盘";
            this.listView1.View = View.Details;
            this.listView1.CheckBoxes = true;
            this.listView1.Clear(); //清除所有项和列
            this.listView1.Height = 305;
            this.listView1.Width = 657;
            this.listView1.SmallImageList = this.imageList1;  //将listView的图标集与imageList1绑定
            this.listView1.Columns.Add("文件名", 300, HorizontalAlignment.Left);
            this.listView1.Columns.Add("大小", 80, HorizontalAlignment.Left);
            this.listView1.Columns.Add("修改时间", 120, HorizontalAlignment.Left);
            this.listView1.Columns.Add("文件类型", 120, HorizontalAlignment.Left);
            try
            {

                XmlHelper xml = new XmlHelper();
                /* string path1 = @"D:\用户目录\Documents\Visual Studio 2010\Projects\云管家\云管家\setting.xml";
                  XmlNode node = xml.GetXmlNodeByXpath(path1, "/root/headsculpture");
                  if (node != null)
                  {
                      string ppath = (node.SelectSingleNode("path")).InnerText;
                      btselphoto.Image = Image.FromFile(ppath);
                  }
                  else { return ; }*/

                XmlNodeList nodelist2 = xml.GetXmlNodeListByXpath(@"D:\云管家\config\xuchuan.xml", "/root/file");
                int si = 0;
                bool isshow = false;
                foreach (XmlNode xn in nodelist2)
                {
                    //遍历子节点

                    string name = (xn.SelectSingleNode("filename")).InnerText;
                    // string date = (xn.SelectSingleNode("date")).InnerText;
                    if (!name.Equals(""))
                    {
                        si++;
                        isshow = true;

                    }

                }
                if (isshow)
                {
                    this.bttransportlist.Enabled = true;
                    this.bttransportlist.BackColor = Color.FromArgb(102, 184, 44);
                    this.bttransportlist.Text = "传输列表(" + si.ToString() + ")";
                }


                XmlNodeList nodelist1 = xml.GetXmlNodeListByXpath(@"D:\云管家\config\fileinfo.xml", "/fileroot/folders/folder");
                this.listView1.BeginUpdate();

                foreach (XmlNode xn in nodelist1)
                {
                    //遍历子节点

                    string name = (xn.SelectSingleNode("filename")).InnerText;
                    string date = (xn.SelectSingleNode("date")).InnerText;
                    string size = (xn.SelectSingleNode("size")).InnerText;
                    string type = (xn.SelectSingleNode("type")).InnerText;
                    string path = (xn.SelectSingleNode("path")).InnerText;
                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = 0;     //文件夹
                    lvi.Text = name;
                    lvi.SubItems.Add(size);
                    lvi.SubItems.Add(date);
                    lvi.SubItems.Add(type);
                    this.listView1.Items.Add(lvi);
                }
                //this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。

                //this.listView1.BeginUpdate();
                XmlNodeList nodelist = xml.GetXmlNodeListByXpath(@"D:\云管家\config\fileinfo.xml", "/fileroot/files/file");
                foreach (XmlNode xn in nodelist)
                {
                    //遍历子节点

                    string name = (xn.SelectSingleNode("filename")).InnerText;
                    string date = (xn.SelectSingleNode("date")).InnerText;
                    string size = (xn.SelectSingleNode("size")).InnerText;
                    string type = (xn.SelectSingleNode("type")).InnerText;
                    string path = (xn.SelectSingleNode("path")).InnerText;

                    int i = 0;//动态加载图片下标
                    ListViewItem lvi = new ListViewItem();
                    if (type.Equals("文件") || type.Equals("压缩文件"))
                    {
                        string s1 = name.Substring(name.LastIndexOf("."));
                        if (s1.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 2;//doc文档

                        }
                        else if (s1.Equals(".txt"))
                        {
                            lvi.ImageIndex = 3;//txt

                        }
                        else if (s1.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jar", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".rar", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 1;//压缩文件
                        }
                        else if (s1.Equals(".png", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 4;//图片资源

                        }
                        else if (s1.Equals(".mp4", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 5;

                        }
                        else if (s1.Equals(".avi", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 6;

                        }
                        else if (s1.Equals(".rmvb", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 7;

                        }
                        else if (s1.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 8;

                        }
                        else if (s1.Equals(".pdf"))
                        {
                            lvi.ImageIndex = 9;
                        }
                        else if (s1.Equals(".cpp"))
                        {
                            lvi.ImageIndex = 10;
                        }
                        else if (s1.Equals(".exe"))
                        {
                            ImageList imgLst = new ImageList();
                            string filepath = path;
                            System.Drawing.Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(@filepath);
                            //imgLst.Images.Add(fileIcon);
                            imageList1.Images.Add(fileIcon);
                            lvi.ImageIndex = 12 + i;//可执行文件
                            i++;

                        }
                        else
                            lvi.ImageIndex = 11;
                    }
                    else
                    {
                        lvi.ImageIndex = 0;     //文件夹
                    }

                    lvi.Text = name;
                    lvi.SubItems.Add(size);
                    lvi.SubItems.Add(date);
                    lvi.SubItems.Add(type);
                    this.listView1.Items.Add(lvi);

                    //Console.WriteLine(nextLine);
                }
                this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。
               
                /*//分享文件显示
                XmlNodeList nodelist5 = xml.GetXmlNodeListByXpath(@"D:\云管家\config\shareinfo.xml", "/fileroot/files/file");
                foreach (XmlNode xn in nodelist5)
                {
                    string name = (xn.SelectSingleNode("filename")).InnerText;
                    string date = (xn.SelectSingleNode("date")).InnerText;
                    //string size = (xn.SelectSingleNode("size")).InnerText;
                    string type = (xn.SelectSingleNode("type")).InnerText;
                    // string path = (xn.SelectSingleNode("path")).InnerText;

                    ListViewItem lvi = new ListViewItem();
                    if (type.Equals("文件") || type.Equals("压缩文件"))
                    {
                        string s1 = name.Substring(name.LastIndexOf("."));
                        if (s1.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 2;//doc文档

                        }
                        else if (s1.Equals(".txt"))
                        {
                            lvi.ImageIndex = 3;//txt

                        }
                        else if (s1.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jar", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".rar", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 1;//压缩文件
                        }
                        else if (s1.Equals(".png", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 4;//图片资源

                        }
                        else if (s1.Equals(".mp4", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 5;

                        }
                        else if (s1.Equals(".avi", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 6;

                        }
                        else if (s1.Equals(".rmvb", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 7;

                        }
                        else if (s1.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 8;

                        }
                        else if (s1.Equals(".pdf"))
                        {
                            lvi.ImageIndex = 9;
                        }
                        else if (s1.Equals(".cpp"))
                        {
                            lvi.ImageIndex = 10;
                        }
                        else
                        {
                            lvi.ImageIndex = 11;
                        }
                        lvi.Text = name;
                        lvi.SubItems.Add("未知");
                        lvi.SubItems.Add(date);
                        lvi.SubItems.Add("(来自分享)" + type);
                        this.listView1.Items.Add(lvi);

                    }

                    //this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。



                }*/
            }
            catch (ArgumentException ea)
            {
                MessageBox.Show("异常:" + ea.Message);
                return;
            }
            catch (XmlException ex)
            {
                MessageBox.Show("异常:" + ex.Message);
                return;
            }
            catch (FileLoadException efl)
            {
                MessageBox.Show("异常:" + efl.Message);
                return;
            }
            catch (FieldAccessException efld)
            {
                MessageBox.Show("异常:" + efld.Message);
                return;
            }

        }
       
        /// <summary>
        /// 显示可以分享的文件信息
        /// </summary>
        void canshowshareinfo()
        {
            this.listView3.Clear(); //清除所有项和列
            this.listView3.Location = new Point(3,3);
            this.listView3.Height = 390;
            this.listView3.Width = 450;
            this.listView3.SmallImageList = this.imageList1;  //将listView的图标集与imageList1绑定
            this.listView3.Columns.Add("文件名", 190, HorizontalAlignment.Left);
            this.listView3.Columns.Add("大小", 80, HorizontalAlignment.Left);
            this.listView3.Columns.Add("修改时间", 120, HorizontalAlignment.Left);
            this.listView3.Columns.Add("文件类型", 80, HorizontalAlignment.Left);
            try
            {

                XmlHelper xml = new XmlHelper();
                XmlNodeList nodelist1 = xml.GetXmlNodeListByXpath(@"D:\云管家\config\fileinfo.xml", "/fileroot/folders/folder");
                this.listView3.BeginUpdate();

                foreach (XmlNode xn in nodelist1)
                {
                    //遍历子节点

                    string name = (xn.SelectSingleNode("filename")).InnerText;
                    string date = (xn.SelectSingleNode("date")).InnerText;
                    string size = (xn.SelectSingleNode("size")).InnerText;
                    string type = (xn.SelectSingleNode("type")).InnerText;
                    string path = (xn.SelectSingleNode("path")).InnerText;
                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = 0;     //文件夹
                    lvi.Text = name;
                    lvi.SubItems.Add(size);
                    lvi.SubItems.Add(date);
                    lvi.SubItems.Add(type);
                    this.listView1.Items.Add(lvi);
                }
                //this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。

                //this.listView1.BeginUpdate();
                XmlNodeList nodelist = xml.GetXmlNodeListByXpath(@"D:\云管家\config\fileinfo.xml", "/fileroot/files/file");
                foreach (XmlNode xn in nodelist)
                {
                    //遍历子节点

                    string name = (xn.SelectSingleNode("filename")).InnerText;
                    string date = (xn.SelectSingleNode("date")).InnerText;
                    string size = (xn.SelectSingleNode("size")).InnerText;
                    string type = (xn.SelectSingleNode("type")).InnerText;
                    string path = (xn.SelectSingleNode("path")).InnerText;

                    int i = 0;//动态加载图片下标
                    ListViewItem lvi = new ListViewItem();
                    if (type.Equals("文件") || type.Equals("压缩文件"))
                    {
                        string s1 = name.Substring(name.LastIndexOf("."));
                        if (s1.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 2;//doc文档

                        }
                        else if (s1.Equals(".txt"))
                        {
                            lvi.ImageIndex = 3;//txt

                        }
                        else if (s1.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jar", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".rar", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 1;//压缩文件
                        }
                        else if (s1.Equals(".png", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 4;//图片资源

                        }
                        else if (s1.Equals(".mp4", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 5;

                        }
                        else if (s1.Equals(".avi", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 6;

                        }
                        else if (s1.Equals(".rmvb", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 7;

                        }
                        else if (s1.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 8;

                        }
                        else if (s1.Equals(".pdf"))
                        {
                            lvi.ImageIndex = 9;
                        }
                        else if (s1.Equals(".cpp"))
                        {
                            lvi.ImageIndex = 10;
                        }
                        else if (s1.Equals(".exe"))
                        {
                            ImageList imgLst = new ImageList();
                            string filepath = path;
                            System.Drawing.Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(@filepath);
                            //imgLst.Images.Add(fileIcon);
                            imageList1.Images.Add(fileIcon);
                            lvi.ImageIndex = 11 + i;//可执行文件
                            i++;

                        }
                    }
                    else
                    {
                        lvi.ImageIndex = 0;     //文件夹
                    }

                    lvi.Text = name;
                    lvi.SubItems.Add(size);
                    lvi.SubItems.Add(date);
                    lvi.SubItems.Add(type);
                    this.listView3.Items.Add(lvi);

                    //Console.WriteLine(nextLine);
                }

                this.listView3.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            }
            catch (ArgumentException ea)
            {
                MessageBox.Show("异常:" + ea.Message);
                return;
            }
            catch (XmlException ex)
            {
                MessageBox.Show("异常:" + ex.Message);
                return;
            }
            catch (FileLoadException efl)
            {
                MessageBox.Show("异常:" + efl.Message);
                return;
            }
            catch (FieldAccessException efld)
            {
                MessageBox.Show("异常:" + efld.Message);
                return;
            }

        }
        /// <summary>
        /// 显示分享文件信息
        /// </summary>
        void showshareinfo()
        {
            this.listView3.Clear(); //清除所有项和列
            this.listView3.Location = new Point(3, 3);
            this.listView3.Height = 390;
            this.listView3.Width = 450;
            this.listView3.SmallImageList = this.imageList1;  //将listView的图标集与imageList1绑定
            this.listView3.Columns.Add("地址", 300, HorizontalAlignment.Left);
            this.listView3.Columns.Add("分享人", 80, HorizontalAlignment.Left);
            this.listView3.Columns.Add("时间", 120, HorizontalAlignment.Left);
           //this.listView3.Columns.Add("文件类型", 80, HorizontalAlignment.Left);
            try
            {

                XmlHelper xml = new XmlHelper();
                XmlNodeList nodelist5 = xml.GetXmlNodeListByXpath(@"D:\云管家\config\shareinfo.xml", "/fileroot/files/file");
                this.listView3.BeginUpdate();
                foreach (XmlNode xn in nodelist5)
                {
                    string path = (xn.SelectSingleNode("path")).InnerText;
                    string date = (xn.SelectSingleNode("date")).InnerText;
                    string user= (xn.SelectSingleNode("user")).InnerText;
                    string fname=(xn.SelectSingleNode("fname")).InnerText;
                    
                    ListViewItem lvi = new ListViewItem();
                    /* if (type.Equals("文件") || type.Equals("压缩文件"))
                     {
                         string s1 = name.Substring(name.LastIndexOf("."));
                         if (s1.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
                         {
                             lvi.ImageIndex = 2;//doc文档

                         }
                         else if (s1.Equals(".txt"))
                         {
                             lvi.ImageIndex = 3;//txt

                         }
                         else if (s1.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jar", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".rar", StringComparison.CurrentCultureIgnoreCase))
                         {
                             lvi.ImageIndex = 1;//压缩文件
                         }
                         else if (s1.Equals(".png", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                         {
                             lvi.ImageIndex = 4;//图片资源

                         }
                         else if (s1.Equals(".mp4", StringComparison.CurrentCultureIgnoreCase))
                         {
                             lvi.ImageIndex = 5;

                         }
                         else if (s1.Equals(".avi", StringComparison.CurrentCultureIgnoreCase))
                         {
                             lvi.ImageIndex = 6;

                         }
                         else if (s1.Equals(".rmvb", StringComparison.CurrentCultureIgnoreCase))
                         {
                             lvi.ImageIndex = 7;

                         }
                         else if (s1.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase))
                         {
                             lvi.ImageIndex = 8;

                         }
                         else if (s1.Equals(".pdf"))
                         {
                             lvi.ImageIndex = 9;
                         }
                         else if (s1.Equals(".cpp"))
                         {
                             lvi.ImageIndex = 10;
                         }
                         else
                         {
                             lvi.ImageIndex = 11;
                         }
                     }*/
                        lvi.Text = path+fname;
                        lvi.SubItems.Add(user);
                        lvi.SubItems.Add(date);
                        this.listView3.Items.Add(lvi);

                    

              
                }
                this.listView3.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            }
            catch (ArgumentException ea)
            {
                MessageBox.Show("异常:" + ea.Message);
                return;
            }
            catch (XmlException ex)
            {
                MessageBox.Show("异常:" + ex.Message);
                return;
            }
            catch (FileLoadException efl)
            {
                MessageBox.Show("异常:" + efl.Message);
                return;
            }
            catch (FieldAccessException efld)
            {
                MessageBox.Show("异常:" + efld.Message);
                return;
            }
        }
        /// <summary>
        /// 按类型显示云端相关信息
        /// </summary>
        void showinfo(string type)
        {
            this.listView1.View = View.Details;
            this.listView1.Clear(); //清除所有项和列
            this.listView1.Height = 305;
            this.listView1.Width = 657;
            this.listView1.SmallImageList = this.imageList1;  //将listView的图标集与imageList1绑定
            this.listView1.Columns.Add("文件名", 600, HorizontalAlignment.Left);
            this.listView1.Columns.Add("大小", 80, HorizontalAlignment.Left);
            this.listView1.Columns.Add("修改时间", 120, HorizontalAlignment.Left);
            this.listView1.Columns.Add("文件类型", 120, HorizontalAlignment.Left);




           try
            {

                XmlHelper xml = new XmlHelper();
                XmlNodeList nodelist = xml.GetXmlNodeListByXpath(@"D:\云管家\config\fileinfo.xml", "/fileroot/files/file");
                this.listView1.BeginUpdate();
                foreach (XmlNode xn in nodelist)
                {
                    //遍历子节点
                  
                   string name = (xn.SelectSingleNode("filename")).InnerText;
                   string date = (xn.SelectSingleNode("date")).InnerText;
                   string size = (xn.SelectSingleNode("size")).InnerText;
                   string type1 = (xn.SelectSingleNode("type")).InnerText;
                   string path = (xn.SelectSingleNode("path")).InnerText;


               
               
               
                    int flg = 0;
                    int i = 1;//动态加载图片下标
                     ListViewItem lvi = new ListViewItem();
                   
                    if(type1.Equals("文件") || type1.Equals("压缩文件"))
                    {
                       
                        string s1 = name.Substring(name.LastIndexOf("."));
                        if (type.Equals("document"))//显示文档
                        {
                            if (s1.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
                            {
                                lvi.ImageIndex = 2;//doc文档
                                flg = 1;
                            }
                            else if (s1.Equals(".txt"))
                            {
                                lvi.ImageIndex = 3;//txt
                                flg = 1;
                            }
                        }
                        else if (type.Equals("picture"))//显示图片
                        {
                            if (s1.Equals(".png", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                            {
                                lvi.ImageIndex = 4;//图片资源
                                flg = 1;
                            }
                        }
                        else if (type.Equals("video"))//显示视频
                        {
                            if (s1.Equals(".mp4", StringComparison.CurrentCultureIgnoreCase))
                            {
                                lvi.ImageIndex = 5;
                                flg = 1;
                            }
                            else if (s1.Equals(".avi", StringComparison.CurrentCultureIgnoreCase))
                            {
                                lvi.ImageIndex = 6;
                                flg = 1;
                            }
                            else if (s1.Equals(".rmvb", StringComparison.CurrentCultureIgnoreCase))
                            {
                                lvi.ImageIndex = 7;
                                flg = 1;
                            }
                        }
                        else if (type.Equals("music"))//音乐文件
                        {
                            if (s1.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase))
                            {
                                lvi.ImageIndex = 8;
                                flg = 1;
                            }
                        }
                        else if (s1.Equals(".pdf"))
                        {
                            lvi.ImageIndex = 9;
                        }
                        else if (type.Equals("application"))//应用
                        {
                            if (s1.Equals(".exe"))
                            {
                                flg = 1;
                                ImageList imgLst = new ImageList();
                                string filepath = path;
                                System.Drawing.Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(@filepath);
                                //imgLst.Images.Add(fileIcon);
                                imageList1.Images.Add(fileIcon);
                                lvi.ImageIndex = 11 + i;//可执行文件
                                i++;

                            }
                        }
                       else if (s1.Equals(".zip") || s1.Equals(".jar"))
                        {
                            flg = 1;
                            lvi.ImageIndex = 1;//压缩文件
                        }
                       
                    }
                    if (flg == 1)
                    {
                        lvi.Text = name;
                        lvi.SubItems.Add(size);
                        lvi.SubItems.Add(date);
                        lvi.SubItems.Add(type1);
                        this.listView1.Items.Add(lvi);
                    }

                    //Console.WriteLine(nextLine);
                }
               
                
                this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。

            }
            catch (FileLoadException efl)
            {
                MessageBox.Show("异常:" + efl.Message);
                return;
            }
            catch (FieldAccessException efld)
            {
                MessageBox.Show("异常:" + efld.Message);
                return;
            }
         

        }

        /// <summary>
        /// 网盘按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btwp_Click(object sender, EventArgs e)
        {
            buttonvisiable(true);
            showinfo();
        }

        private void btmin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        /// <summary>
        /// 选中下载项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_ItemCheck(object sender, ItemCheckedEventArgs e)
        {
           
            btdown.Enabled = false;//下载按钮不可用
            btshare.Enabled = false;
            btdelete.Enabled = false;
            for ( int i = 0; i < this.listView1.CheckedItems.Count; i++)
            {
                if (this.listView1.CheckedItems[i].Checked)
                {
                    string str = this.listView1.CheckedItems[i].Text;

                    downloadfile = this.listView1.CheckedItems[i].Text;
                    btdown.Enabled = true;
                    btshare.Enabled = true;
                    btdelete.Enabled = true;
                }
            }
            
        }
        

        /// <summary>
        ///获取文件大小 
        /// </summary>
        /// <param name="sFileFullName"></param>
        /// <returns></returns>
        public string GetFileSize(string sFileFullName)
        {
            FileInfo fiInput = new FileInfo(sFileFullName);
            double len = fiInput.Length;

            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            string filesize = String.Format("{0:0.##}{1}", len, sizes[order]);
            return filesize;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btlogin_Click(object sender, EventArgs e)
        {
            if (!connect())//连接服务器
            {
                return;
            }
           
           
            string username = "";
            string password;
            logindlg dlg = new logindlg();
            try
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    username = dlg.username;
                    string temp = username + ".txt";
                   // if (username.Equals("zhangsan"))
                   // {
                    string info = readlineintxt(@"D:\云管家\config\" + temp);
                        string[] s = info.Split(new char[] { ' ' });
                        port = int.Parse(s[0].ToString());
                        port1 = int.Parse(s[1].ToString());
                   /* }
                    else if (username.Equals("pzw"))
                    {
                        string info = readlineintxt(@"D:\用户目录\Documents\Visual Studio 2010\Projects\云管家\pzw.txt");
                        string[] s = info.Split(new char[] { ' ' });
                        port = int.Parse(s[0].ToString());
                        port1 = int.Parse(s[1].ToString());
                    }*/
                    //创建一个线程接收远程主机发来的信息  
                    Thread myThread = new Thread(ReceiveData);
                    //将线程设为后台运行  
                    myThread.IsBackground = true;
                    myThread.Start();
                    textBoxSend.Focus();  
                    password = dlg.newpassword;
                    string uinfo = username + " " + password;
                    byte[] arrSendMsg = new byte[1024];
                    arrSendMsg[0] = 3; // 登录请求 
                    sockClient.Send(arrSendMsg); // 发送消息；
                    System.Threading.Thread.Sleep(100);
                    sockClient.Send(Encoding.Default.GetBytes(uinfo));


                }
                dlg.Close();
                System.Threading.Thread.Sleep(1 * 100);
                if (islogin == true)
                {
                    MessageBox.Show("登陆成功");
                    
                    showinfo();//显示云端文件
                    btuname.Text = username;
                    btlogin.Text = "已登录";
                    btlogin.Enabled = false;
                    XmlHelper xml = new XmlHelper();
                    string path1 = @"D:\云管家\config\setting.xml";
                    XmlNode node = xml.GetXmlNodeByXpath(path1, "/root/headsculpture");
                    if (node != null)
                    {
                        string ppath = (node.SelectSingleNode("path")).InnerText;
                        btselphoto.Image = Image.FromFile(ppath);
                    }
                    else { return; }

                }
                else
                {
                    MessageBox.Show("登陆失败");
                }

            }
            catch (SocketException se)
            {
                ShowMsg("异常；" + se.Message);
                return;
            }
            catch (Exception e2)
            {
                ShowMsg("异常：" + e2.Message);
                return;
            }


        }


        /// <summary>
        /// 选择头像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btclephoto_Click(object sender, EventArgs e)
        {
            string filepath = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "D:\\"; //初始显示的目录
            ofd.RestoreDirectory = true; //操作完后恢复为原有的目录
            ofd.Filter = "标签|*.jpg;*.png;*.gif";
            //filterindex与上个属性filter关联对应；它是显示打开文件对话框时默认显示的文件类型下拉列表的子项内容
            ofd.FilterIndex = 2;//默认是1
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filepath = ofd.FileName;
            }
            if (filepath.Equals(""))
            {
                MessageBox.Show("未选择图片!");
                return;
            }
            else
            {
                try
                {
                   
                    XmlHelper xml = new XmlHelper();
                    string path = @"D:\云管家\config\setting.xml";
                    System.Collections.Hashtable ht = new System.Collections.Hashtable();
                    string lpath=@"D:\云管家\resource";
                    //MakeThumbnail(path, lpath, 58, 58, "Cut ");
                    ht["path"] = filepath;
                    ht["isset"] = "yes";
                    xml.UpdateNode(path,"root",ht,ht);
                    btselphoto.Image = Image.FromFile(filepath);
                       // .InsertNode(path, "headsculpture", false, "root", ht, ht);
                }
                catch (FileNotFoundException ep)
                {
                    MessageBox.Show("异常:" + ep.Message);
                }
                catch (OutOfMemoryException eo)
                {
                    MessageBox.Show("异常:" + eo.Message);
                }
            }
        }

        /// <summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param   name= "originalImagePath ">源图路径（物理路径） </param> 
        /// <param   name= "thumbnailPath "> 缩略图路径（物理路径） </param> 
        /// <param   name= "width "> 缩略图宽度 </param> 
        /// <param   name= "height "> 缩略图高度 </param> 
        /// <param   name= "mode "> 生成缩略图的方式 </param>         
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW "://指定高宽缩放（可能变形）                                 
                    break;
                case "W "://指定宽，高按比例                                         
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H "://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut "://指定高宽裁减（不变形）                                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                    new System.Drawing.Rectangle(x, y, ow, oh),
                    System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 检测图片类型
        /// </summary>
        /// <param name="_fileExt"></param>
        /// <returns>正确返回True</returns>
        private bool CheckFileExt(string _fileExt)
        {
            string[] allowExt = new string[] { ".gif", ".jpg", ".jpeg" };
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i] == _fileExt) { return true; }
            }
            return false;
        }
           
        private void btstop_Click(object sender, EventArgs e)
        {   
            
            if (index % 2 == 0)
            {
                isstop = true;
                isxuchuan = false;
                /*System.Threading.Thread.Sleep(10);
                if (posission > 0)
                {
                    writlinfo(@"D:\xuchuan.txt", "true " + posission.ToString() + "\r\n");
                }*/
                btstop.Text = "继续";
            }
            else
            {
                isstop = false;
                isxuchuan = true;
                btstop.Text = "暂停";
            }
            index++;
        }
        /// <summary>
        /// 删除云端文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btdelete_Click(object sender, EventArgs e)
        {
            string type;
            for (int i = 0; i < this.listView1.CheckedItems.Count; i++)
            {
                if (this.listView1.CheckedItems[i].Checked)
                {
                    string fname = this.listView1.CheckedItems[i].Text;
                    type = this.listView1.CheckedItems[i].SubItems[3].Text;
                   
                   // MessageBox.Show(type);
                    if (MessageBox.Show(this, "确定删除"+fname+"？", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        return;
                    if (type.Equals("(来自分享)文件"))
                    {
                        delfromxml(@"D:\云管家\config\shareinfo.xml", fname, "文件");
                        this.listView1.CheckedItems[i].Remove();//从列表删除
                        return;
                    }
                    try
                    {
                        byte[] arrSendMsg = new byte[1024];//告诉服务器删除对应文件
                        arrSendMsg[0] = 6; //删除请求 
                        sockClient.Send(arrSendMsg); // 发送消息；
                        System.Threading.Thread.Sleep(10);
                        sockClient.Send(Encoding.Default.GetBytes(fname));
                    }
                    catch (SocketException es)
                    {
                        MessageBox.Show("异常:" + es.Message);
                        return;
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("异常:" + ee.Message);
                        return;
                    }
                    //delinetxt(fname);//本地文件中删除
                    if (type.Equals("文件") || type.Equals("压缩文件"))
                    {
                        delfromxml(@"D:\云管家\config\fileinfo.xml", fname, "文件");
                    }
                    else
                    {
                        delfromxml(@"D:\云管家\config\fileinfo.xml", fname, "文件夹");
                    }
                    this.listView1.CheckedItems[i].Remove();//从列表删除
                    
                }
            }
        }

        /// <summary>
        /// 从XML文件删除一条文件信息
        /// </summary>
        /// <param name="xfilepath"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        void delfromxml(string xfilepath, string name, string type)
        {
            XmlHelper xml = new XmlHelper();
            if (type.Equals("文件") || type.Equals("压缩文件"))
            {


                bool r = xml.DeleteXmlNodeByXPath(@xfilepath, name, 1, "fileroot/files");
                if (r == false)
                {
                    MessageBox.Show("从文件删除失败!");
                    return;
                }
                
            }
            else if(type.Equals("文件夹"))
            {
                bool r = xml.DeleteXmlNodeByXPath(@xfilepath, name, 2, "fileroot/folders");
                if (r == false)
                {
                    MessageBox.Show("从文件删除失败!");
                    return;
                }
            }
            else if (type.Equals("续传"))
            {
                bool r = xml.DeleteXmlNodeByXPath(@"D:\云管家\config\xuchuan.xml", name, 3, "/root");
                if (r == false)
                {
                    MessageBox.Show("从文件删除失败!");
                    return;
                }
            }


        }
        /// <summary>
        /// 从文件删除一行
        /// </summary>
        /// <param name="finme"></param>
        void delinetxt(string fname)
        {

            try
            {
                StreamReader msw = new StreamReader(@"D:\fileinfo.txt", System.Text.Encoding.UTF8);
                List<string> lines = new List<string>(File.ReadAllLines(@"D:\fileinfo.txt"));
                string line;
                int index = 0;
                while ((line = msw.ReadLine()) != null)
                {
                    string[] str = line.Split('#');
                    if (str[0].ToString().Equals(fname))
                    {

                        lines.RemoveAt(index);

                    }
                    index++;
                }
                msw.Close();
                File.WriteAllLines(@"D:\fileinfo.txt", lines.ToArray());
            }
            catch (FileNotFoundException ef)
            {
                MessageBox.Show("异常:" + ef.Message);
                return;
            }
            catch (ArgumentException ea)
            {
                MessageBox.Show("异常:" + ea.Message);
                return;
            }
            catch (Exception e1)
            {
                MessageBox.Show("异常:" + e1.Message);
                return;
            }
        }

        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnewfolder_Click(object sender, EventArgs e)
        {
            int i =this.listView1.CheckedItems.Count;//获取当前条目数
            this.listView1.SmallImageList = this.imageList1;  //将listView的图标集与imageList1绑定
            lvi.ImageIndex = 0;//文件夹图标
            string fname = "新建文件夹";
            lvi.Text =fname;
            lvi.SubItems.Add("0KB");
            DateTime DT = System.DateTime.Now;
            string dt = System.DateTime.Now.ToString();
            lvi.SubItems.Add(dt);
            lvi.SubItems.Add("文件夹");
            this.listView1.Items.Add(lvi);
            listView1.LabelEdit = true;

            renamedlg dlg = new renamedlg(lvi.Text);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                lvi.Text = dlg.name;
            }
            dlg.Close();
            System.Collections.Hashtable ht = new System.Collections.Hashtable();
            ht["filename"] = dlg.name;
            ht["size"] = "0KB";
            ht["date"] = dt;
            ht["type"] = "文件夹";
            ht["path"] = " ";
            try
            {
                byte[] arrSendMsg = new byte[1024];//告诉服务器删除对应文件
                arrSendMsg[0] = 7; //删除请求 
                sockClient.Send(arrSendMsg); // 发送消息；
                //System.Threading.Thread.Sleep(10);
                string str = lvi.Text;
                sockClient.Send(Encoding.Default.GetBytes(lvi.Text));
            }
            catch (SocketException es)
            {
                MessageBox.Show("异常:" + es.Message);
                return;
            }
            catch (Exception ee)
            {
                MessageBox.Show("异常:" + ee.Message);
                return;
            }
            XmlHelper xml = new XmlHelper();
            xml.InsertNode(@"D:\云管家\config\fileinfo.xml", "folder", false, "fileroot/folders", ht, ht);
            string fileinfo = lvi.Text + "#" + "0KB" + "#" + dt + "#" + "文件夹" + "#" + "***" + "\r\n";
           // writeinfo("D:\\fileinfo.txt", fileinfo);
            

            


        }

     


       
        /// <summary>
        /// 往文件写入一行信息
        /// </summary>
        /// <param name="textfile"></param>
        /// <param name="content"></param>
        void writlinfo(string textfile, string content)
        {
            try
            {

                StreamWriter sw = new StreamWriter(textfile);
                sw.Write(content);
                sw.Close();
               
            }
            catch (FieldAccessException ea)
            {
                MessageBox.Show("异常:" + ea.Message);
            }
            catch (ArgumentException er)
            {
                MessageBox.Show("异常:" + er.Message);
            }
        }

       
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btupdate_Click(object sender, EventArgs e)
        {
            showinfo();
        }

        private void btphoto_Click(object sender, EventArgs e)
        {
            showinfo("picture");
        }

        private void btdocument_Click(object sender, EventArgs e)
        {
            showinfo("document");
        }

        private void btfilm_Click(object sender, EventArgs e)
        {
            showinfo("video");
        }

        private void btmusic_Click(object sender, EventArgs e)
        {
            showinfo("music");
        }

        private void btapp_Click(object sender, EventArgs e)
        {
            showinfo("application");
        }

        private void showall_Click(object sender, EventArgs e)
        {
            showinfo();
        }

        private void openfolder_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.listView1.CheckedItems.Count; i++)
            {
                if (this.listView1.CheckedItems[i].Checked)
                {
                    string fname = this.listView1.CheckedItems[i].Text;
                    string type = this.listView1.CheckedItems[i].SubItems[3].Text;

                    if (type.Equals("文件夹"))
                    {
                        XmlHelper xml = new XmlHelper();
                        XmlNodeList nodelist1 = xml.GetXmlNodeListByXpath(@"D:\云管家\config\fileinfo.xml", "/fileroot/folders/folder");
                        foreach (XmlNode xn in nodelist1)
                        {
                            //遍历子节点
                           
                            string name = (xn.SelectSingleNode("filename")).InnerText;
                            string date = (xn.SelectSingleNode("date")).InnerText;
                            string size = (xn.SelectSingleNode("size")).InnerText;
                            string type1 = (xn.SelectSingleNode("type")).InnerText;
                            string path = (xn.SelectSingleNode("path")).InnerText;

                            /* if (name.Equals(fname))
                            {
                                 System.Collections.Hashtable ht = new System.Collections.Hashtable();
                                 ht["filename"] = name;
                                 ht["size"] =size;
                                 ht["date"] = date;
                                 ht["type"] = type1;
                                 ht["path"] = path;
                                 ht["isshow"]="true";
                                 xml.UpdateNode(@"D:\fileinfo.xml", "/fileroot/folders", ht, ht);
                            }*/
                        }
                        
                    }
                }
            }
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btregister_Click(object sender, EventArgs e)
        {
            if (!connect())//连接服务器
            {
                return;
            }
            string username = "";
            string password;
            registerdlg dlg = new registerdlg();
            try
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    username = dlg.username;
                    password = dlg.password;
                    string uinfo = username + " " + password;
                    byte[] arrSendMsg = new byte[1024];
                    arrSendMsg[0] = 8; //注册请求 
                    sockClient.Send(arrSendMsg); // 发送消息；
                    System.Threading.Thread.Sleep(100);
                    sockClient.Send(Encoding.Default.GetBytes(uinfo));


                }
                dlg.Close();
                System.Threading.Thread.Sleep(1 * 100);
                if (isregistersuccess == true)
                {
                    MessageBox.Show("注册成功!");
                    /*btuname.Text = username;
                    btlogin.Text = "已登录";
                    XmlHelper xml = new XmlHelper();
                    string path1 = @"D:\用户目录\Documents\Visual Studio 2010\Projects\云管家\云管家\setting.xml";
                    XmlNode node = xml.GetXmlNodeByXpath(path1, "/root/headsculpture");
                    if (node != null)
                    {
                        string ppath = (node.SelectSingleNode("path")).InnerText;
                        btselphoto.Image = Image.FromFile(ppath);
                    }
                    else { return; }
                    */
                }
                else
                {
                    MessageBox.Show("注册失败!");
                    return;
                }

            }
            catch (SocketException se)
            {
                ShowMsg("异常；" + se.Message);
                return;
            }
            catch (Exception e2)
            {
                ShowMsg("异常：" + e2.Message);
                return;
            }

        }

       
        /// <summary>
        /// 显示传输列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bttransportlist_Click(object sender, EventArgs e)
        {
            this.listView1.Clear(); //清除所有项和列
            this.listView1.Height = 0;
            this.listView1.Width = 0;
            lve.Clear();
            lve.Parent = listView1.Parent;
            lve.Location = new Point(96, 175);
            lve.Width= 655;
            lve.Height = 300;
            lve.CheckBoxes = true;
            lve.Columns.Add("文件名",300, HorizontalAlignment.Left);
            lve.Columns.Add("进度",200, HorizontalAlignment.Left);
            //lve.Columns.Add("修改时间", 120, HorizontalAlignment.Left);
            
           

            XmlHelper xml =new XmlHelper();
            XmlNodeList nodelist2 = xml.GetXmlNodeListByXpath(@"D:\云管家\config\xuchuan.xml", "/root/file");
             int i = 0;
                foreach (XmlNode xn in nodelist2)
                {
                    //遍历子节点
                    ListViewItem lviUserName = new ListViewItem();
                    ListViewItem.ListViewSubItem lvsinc = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem lvsihostname = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem lvsiip = new ListViewItem.ListViewSubItem();
                    string name = (xn.SelectSingleNode("filename")).InnerText;
                    // string date = (xn.SelectSingleNode("date")).InnerText;
                    if (!name.Equals(""))
                    {
                        lviUserName.Text =name;
                        long p = long.Parse((xn.SelectSingleNode("posission")).InnerText.ToString());
                        long l = long.Parse((xn.SelectSingleNode("length")).InnerText.ToString());
                        double percent = (Convert.ToInt32(p) * 100.00) / l;
                        string strprecent = String.Format("{0:F}", percent);//默认为保留两位
                        lvsinc.Text = "0";
                        lviUserName.SubItems.Add(lvsinc);
                        lve.Items.Add(lviUserName);
                        lve.ProgressTextColor = Color.Red;
                        lve.ProgressColor = Color.YellowGreen;
                        lve.ProgressColumIndex = 1;

                        lve.Items[i].SubItems[1].Text = strprecent;
                        i++;

                    }
                }

            
        }

       
        
public partial class ListViewEx : System.Windows.Forms.ListView
    {
        public ListViewEx()
        {
            InitializeComponent();
        }
        //C# listview进度条显示
        private Color mProgressColor = Color.Red;
        public Color ProgressColor
        {
            get
            {
                return this.mProgressColor;
            }
            set
            {
                this.mProgressColor = value;
            }
        }
        private Color mProgressTextColor = Color.Black;
        public Color ProgressTextColor
        {
            get
            {
                return mProgressTextColor;
            }
            set
            {
                mProgressTextColor = value;
            }
        }
        public int ProgressColumIndex
        {
            set
            {
                progressIndex = value;
            }
            get
            {
                return progressIndex;
            }
        }
        int progressIndex = -1;
        const string numberstring = "0123456789.";
        private bool CheckIsFloat(String s)
        {
            //C# listview进度条显示
            foreach (char c in s)
            {
                if (numberstring.IndexOf(c) > -1)
                { continue; }
                else return false;
            }
            return true;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        //C# listview进度条显示
        private void InitializeComponent()
        {
            this.OwnerDraw = true;
            this.View = View.Details;
        }
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawColumnHeader(e);
        }
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {

            if (e.ColumnIndex != this.progressIndex)
            {
                e.DrawDefault = true; base.OnDrawSubItem(e);
            }
            else
            {
                if (CheckIsFloat(e.Item.SubItems[e.ColumnIndex].Text))
                //判断当前subitem文本是否可以转为浮点数
                {
                   
                    //string strprecent = String.Format("{0:F}", percent);//默认为保留两位
                    float per = float.Parse(e.Item.SubItems[e.ColumnIndex].Text);
                    if (per >= 1.00f) { per = per / 100.00f; }
                    Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    DrawProgress(rect, per, e.Graphics);
                }
            }
        }
        //C# listview进度条显示 ///绘制进度条列的subitem 
        private void DrawProgress(Rectangle rect, float percent, Graphics g)
        {
            if (rect.Height > 2 && rect.Width > 2)
            {
                if ((rect.Top > 0 && rect.Top < this.Height) && (rect.Left > this.Left && rect.Left < this.Width))
                {
                    //绘制进度  
                    int width = (int)(rect.Width * percent);
                    Rectangle newRect = new Rectangle(rect.Left + 1, rect.Top + 1, width - 2, rect.Height - 2);
                    using (Brush tmpb = new SolidBrush(this.mProgressColor))
                    { g.FillRectangle(tmpb, newRect); }
                    newRect = new Rectangle(rect.Left + 1, rect.Top + 1, rect.Width - 2, rect.Height - 2);
                    g.DrawRectangle(Pens.RoyalBlue, newRect);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    newRect = new Rectangle(rect.Left + 1, rect.Top + 1, rect.Width - 2, rect.Height - 2);
                    using (Brush b = new SolidBrush(mProgressTextColor))
                    {
                        g.DrawString(percent.ToString("p1"), this.Font, b, newRect, sf);
                    }
                }
            }
            //C# listview进度条显示
            else
            {
                return;
            }
        } 
    }
/// <summary>
/// 分享
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void btshare_Click(object sender, EventArgs e)
{
    buttonvisiable(false);
    panel7.Visible = false;
    panel10.Visible = false;

    //创建一个线程接收远程主机发来的信息  
    //Thread myThread = new Thread(ReceiveData);
    //将线程设为后台运行  
   // myThread.IsBackground = true;
   // myThread.Start();
    //textBoxSend.Focus();  
}
void buttonvisiable(bool r)
{
    panel2.Visible = r;
    panel3.Visible = r;
    panel4.Visible = r;
    listView1.Visible = r;
    progressBar1.Visible = r;
    lve.Visible = r;
    btstop.Visible = r;
    panel5.Visible = !r;
}

/// <summary>
/// 发起聊天
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void btchat_Click(object sender, EventArgs e)
{
    panel10.Visible = true;
    panel11.Visible = true;
    listView3.Visible = false;
    
}
/// <summary>
/// 分享文件给好友
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void btsharefile_Click(object sender, EventArgs e)
{
    
    panel10.Visible = true;
    panel11.Visible = false;
    listView3.Visible = true;
    btshareOK.Text = "分享";
    canshowshareinfo();
   
}



string getUserName()
{
    string info = readlineintxt(@"D:\云管家\config\zhangsan.txt");
    string[] s = info.Split(new char[] { ' ' });
    if (s[2].ToString().Equals(btuname.Text) && s[0].ToString().Equals("9002"))
    {
        return "pzw";
    }
    else
    {
        return"zhangsan";
    }
}
private void ReceiveData()
{
    
    IPEndPoint local = new IPEndPoint(ip, port);
    receiveUdpClient = new UdpClient(local);
    IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);//待改进
    while (true)
    {
        try
        {
            //关闭udpClient时此句会产生异常  
            byte[] receiveBytes = receiveUdpClient.Receive(ref remote);
            string Message = Encoding.Unicode.GetString(
                receiveBytes, 0, receiveBytes.Length);
            Code cd=new Code();
            string receiveMessage = cd.Decode(Message);
            string[] s1 = receiveMessage.Split(new char[] { '*' });
            if (s1[0].ToString().Equals("##"))
            {
                MessageBox.Show("来自分享" + s1[1].ToString());
                System.Collections.Hashtable ht = new System.Collections.Hashtable();
                string st1 = "wp/yun/--" + Message;
                string name = getUserName();
                ht["path"] = st1;
                ht["date"] = DateTime.Now.ToString();
                ht["user"] = name;
                ht["fname"] = s1[1].ToString();
               // ht["path"] = filename;
                XmlHelper xml = new XmlHelper();
                xml.InsertNode(@"D:\云管家\config\shareinfo.xml", "file", false, "fileroot/files", ht, ht);
                AddItem(listBoxMsg, string.Format("{0}:  {1}",name, st1 + "     (分享)" + currentTime()));
                AddItem(listBoxMsg, string.Format("{0}{1}", "", ""));//空行
            }
            else
            {
                string name = getUserName();
                /*string info = readlineintxt(@"D:\云管家\config\zhangsan.txt");
                string[] s = info.Split(new char[] { ' ' });
               
                if (s[2].ToString().Equals(btuname.Text) && s[0].ToString().Equals("9002"))
                {
                    name = "pzw";
                }
                else
                {
                    name = "zhangsan";
                }*/
                //AddItem(listBoxReceive, string.Format("来自{0}：{1}", remote, receiveMessage));
                AddItem(listBoxMsg, string.Format("{0}:  {1}", name, receiveMessage+"     "+currentTime()));
                AddItem(listBoxMsg, string.Format("{0}{1}", "", ""));//空行
            }
        }
        catch
        {
            break;
        }
    }
}

string currentTime()
{
    DateTime dt = DateTime.Now;
    return dt.ToString();
}
/// <summary>发送数据到远程主机</summary>  
private void SendMessage(object obj)
{
    string message = (string)obj;
    sendUdpClient = new UdpClient(0);
    Code cd = new Code();
    byte[] bytes = System.Text.Encoding.Unicode.GetBytes(cd.Encode(message));//加密
    IPEndPoint iep = new IPEndPoint(remoteIp, port1);
    try
    {
         string name = btuname.Text;
        sendUdpClient.Send(bytes, bytes.Length, iep);
        //AddItem(listBoxMsg, string.Format("向{0}发送：{1}", name, message));
        string[] s1 = message.Split(new char[] { '*' });
        if (s1[0].ToString().Equals("##"))
        {
            message = "分享文件   " + s1[1].ToString() + "     " + currentTime();
        }
        else
        {
            message = message + "     " + currentTime();
        }
        AddItem(listBoxMsg, string.Format("{0}： {1}", name, message));
        AddItem(listBoxMsg, string.Format("{0}{1}", "", ""));//空行
        ClearTextBox();
    }
    catch (Exception ex)
    {
        AddItem(listBoxMsg, "发送出错:" + ex.Message);
    }
}
delegate void AddListBoxItemDelegate(ListBox listbox, string text);
private void AddItem(ListBox listbox, string text)
{
    if (listbox.InvokeRequired)
    {
        AddListBoxItemDelegate d = AddItem;
        listbox.Invoke(d, new object[] { listbox, text });
    }
    else
    {
        listbox.Items.Add(text);
        listbox.SelectedIndex = listbox.Items.Count - 1;
        listbox.ClearSelected();
    }
}
delegate void ClearTextBoxDelegate();
private void ClearTextBox()
{
    if (textBoxSend.InvokeRequired)
    {
        ClearTextBoxDelegate d = ClearTextBox;
        textBoxSend.Invoke(d);
    }
    else
    {
        textBoxSend.Clear();
        textBoxSend.Focus();
    }
}
 /// <summary>
        /// 发送消息按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
private void btsendmessage_Click(object sender, EventArgs e)
{
    Thread t = new Thread(SendMessage);
    t.IsBackground = true;
    t.Start(textBoxSend.Text);  
}


/// <summary>
/// 选中条目
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void ListView3Checked(object sender, ItemCheckedEventArgs e)
{
    btshareOK.Enabled = false;
    for (int i = 0; i < this.listView3.CheckedItems.Count; i++)
    {
        if (this.listView3.CheckedItems[i].Checked)
        {
            sharefile = this.listView3.CheckedItems[i].Text;
            btshareOK.Enabled = true;
          
        }
    }
}

/// <summary>
/// 分享文件
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void btshareOK_Click(object sender, EventArgs e)
{
    if (this.listView3.Columns[0].Text == "地址")
    {
        byte[] arrSendMsg = new byte[1024];
        arrSendMsg[0] = 2; // 用来表示发送的是文件 
        sockClient.Send(arrSendMsg);


        //有待改进
        sockClient.Send(Encoding.Default.GetBytes("1.rmvb")); //申请下载文件名  
        //MessageBox.Show("yao xiaz 成功!");
    }
    else
    {
        Thread t = new Thread(SendMessage);
        t.IsBackground = true;
        string info = "##*" + sharefile;
        //Code cd = new Code();
        //string temp = cd.Encode(info);//加密
        t.Start(info);
        MessageBox.Show("分享成功!");
    }
  
   
}

private void btfriend_Click(object sender, EventArgs e)
{
    panel7.Visible =true;
    panel10.Visible = true;//9.26改
    panel11.Visible = true;//
    listView3.Visible = false;//
    //添加好友信息
    this.listView2.Clear(); //清除所有项和列
    this.listView2.SmallImageList = this.imageList3;
    //this.listView2.SmallImageList = this.imageList1;  //将listView的图标集与imageList1绑定
    this.listView2.Columns.Add("好友列表", 300, HorizontalAlignment.Left);
     ListViewItem lvi = new ListViewItem();
     lvi.ImageIndex = 0;
    lvi.Text=getUserName();
    this.listView2.Items.Add(lvi);
    
   
}
/// <summary>
/// 隐私空间
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void button4_Click(object sender, EventArgs e)
{
    
}
/// <summary>
/// 大图标方式显示
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void btBigImg(object sender, EventArgs e)
{

    this.listView1.View = View.LargeIcon;
    this.listView1.Clear();
    this.listView1.CheckBoxes = false;
    this.listView1.LargeImageList = this.imageList2;
    try
    {

        XmlHelper xml = new XmlHelper();




        XmlNodeList nodelist1 = xml.GetXmlNodeListByXpath(@"D:\云管家\config\fileinfo.xml", "/fileroot/folders/folder");
        this.listView1.BeginUpdate();

        foreach (XmlNode xn in nodelist1)
        {
            //遍历子节点

            string name = (xn.SelectSingleNode("filename")).InnerText;
            string date = (xn.SelectSingleNode("date")).InnerText;
            string size = (xn.SelectSingleNode("size")).InnerText;
            string type = (xn.SelectSingleNode("type")).InnerText;
            string path = (xn.SelectSingleNode("path")).InnerText;
            ListViewItem lvi = new ListViewItem();
            lvi.ImageIndex = 0;     //文件夹
            lvi.Text = name;
            /*lvi.SubItems.Add(size);
            lvi.SubItems.Add(date);
            lvi.SubItems.Add(type);*/
            this.listView1.Items.Add(lvi);
        }

        XmlNodeList nodelist = xml.GetXmlNodeListByXpath(@"D:\云管家\config\fileinfo.xml", "/fileroot/files/file");
        foreach (XmlNode xn in nodelist)
        {
            //遍历子节点

            string name = (xn.SelectSingleNode("filename")).InnerText;
            string date = (xn.SelectSingleNode("date")).InnerText;
            string size = (xn.SelectSingleNode("size")).InnerText;
            string type = (xn.SelectSingleNode("type")).InnerText;
            string path = (xn.SelectSingleNode("path")).InnerText;

            int i = 0;//动态加载图片下标
            ListViewItem lvi = new ListViewItem();
            if (type.Equals("文件") || type.Equals("压缩文件"))
            {
                string s1 = name.Substring(name.LastIndexOf("."));
                if (s1.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 3;//doc文档

                }
                else if (s1.Equals(".txt"))
                {
                    lvi.ImageIndex = 2;//txt

                }
                else if (s1.Equals(".xml"))
                {
                    lvi.ImageIndex = 5;//txt

                }
                else if (s1.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jar", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".rar", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 1;//压缩文件
                }
                else if (s1.Equals(".png", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 6;//图片资源

                }
                else if (s1.Equals(".mp4", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 4;

                }
                else if (s1.Equals(".avi", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 4;

                }
                else if (s1.Equals(".rmvb", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 4;

                }
                else if (s1.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 7;

                }
                else if (s1.Equals(".pdf"))
                {
                    lvi.ImageIndex = 8;
                }
                else if (s1.Equals(".cpp"))
                {
                    lvi.ImageIndex = 9;
                }
                else if (s1.Equals(".exe"))
                {
                    ImageList imgLst = new ImageList();
                    string filepath = path;
                    System.Drawing.Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(@filepath);
                    //imgLst.Images.Add(fileIcon);
                    imageList2.Images.Add(fileIcon);
                    lvi.ImageIndex = 11 + i;//可执行文件
                    i++;

                }
                else
                {
                    lvi.ImageIndex = 10;
 
                }
            }
            else
            {
                lvi.ImageIndex = 0;     //文件夹
            }

            lvi.Text = name;
            /*lvi.SubItems.Add(size);
            lvi.SubItems.Add(date);
            lvi.SubItems.Add(type);*/
            this.listView1.Items.Add(lvi);

        }

        //分享文件显示
        XmlNodeList nodelist5 = xml.GetXmlNodeListByXpath(@"D:\云管家\config\shareinfo.xml", "/fileroot/files/file");
        foreach (XmlNode xn in nodelist5)
        {
            string name = (xn.SelectSingleNode("filename")).InnerText;
            string date = (xn.SelectSingleNode("date")).InnerText;
            //string size = (xn.SelectSingleNode("size")).InnerText;
            string type = (xn.SelectSingleNode("type")).InnerText;
            // string path = (xn.SelectSingleNode("path")).InnerText;

            ListViewItem lvi = new ListViewItem();
            if (type.Equals("文件") || type.Equals("压缩文件"))
            {
                string s1 = name.Substring(name.LastIndexOf("."));
                if (s1.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 3;//doc文档

                }
                else if (s1.Equals(".txt"))
                {
                    lvi.ImageIndex = 2;//txt

                }
                else if (s1.Equals(".xml"))
                {
                    lvi.ImageIndex = 5;//txt

                }
                else if (s1.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jar", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".rar", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 1;//压缩文件
                }
                else if (s1.Equals(".png", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 6;//图片资源

                }
                else if (s1.Equals(".mp4", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 4;

                }
                else if (s1.Equals(".avi", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 4;

                }
                else if (s1.Equals(".rmvb", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 4;

                }
                else if (s1.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase))
                {
                    lvi.ImageIndex = 7;

                }
                else if (s1.Equals(".pdf"))
                {
                    lvi.ImageIndex = 8;
                }
                lvi.Text = name + "(来自分享)";
                //lvi.SubItems.Add("未知");
                //lvi.SubItems.Add(date);
                //lvi.SubItems.Add("(来自分享)"+type);
                this.listView1.Items.Add(lvi);
            }
        }

        this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。

    }
    catch (ArgumentException ea)
    {
        MessageBox.Show("异常:" + ea.Message);
        return;
    }
    catch (XmlException ex)
    {
        MessageBox.Show("异常:" + ex.Message);
        return;
    }
    catch (FileLoadException efl)
    {
        MessageBox.Show("异常:" + efl.Message);
        return;
    }
    catch (FieldAccessException efld)
    {
        MessageBox.Show("异常:" + efld.Message);
        return;
    }
    /*this.listView1.BeginUpdate();

    for (int i = 0; i < 10; i++)
    {
        ListViewItem lvi = new ListViewItem();

        lvi.ImageIndex = i;

        lvi.Text = "item" + i;

        this.listView1.Items.Add(lvi);
    }

    this.listView1.EndUpdate();*/

}
/// <summary>
/// 显示子文件夹
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void showchild(object sender, MouseEventArgs e)
{
    string name = listView1.FocusedItem.Text;
    //listView1.FocusedItem.SubItems[3].Text.Equals("文件夹") || 
    if (listView1.FocusedItem.ImageIndex.Equals(0))
    {
        textBox1.Text = "我的网盘" + " ▶ " + name;
        showchilds(name);
    }
    //MessageBox.Show(name);
}
/// <summary>
/// 显示子文件
/// </summary>
/// <param name="name"></param>
void showchilds(string foldername)
{
    if (this.listView1.View.Equals(View.Details))
    {
        this.listView1.CheckBoxes = true;
        this.listView1.Clear(); //清除所有项和列
        this.listView1.Height = 305;
        this.listView1.Width = 657;
        this.listView1.SmallImageList = this.imageList1;  //将listView的图标集与imageList1绑定
        this.listView1.Columns.Add("文件名", 300, HorizontalAlignment.Left);
        this.listView1.Columns.Add("大小", 80, HorizontalAlignment.Left);
        this.listView1.Columns.Add("修改时间", 120, HorizontalAlignment.Left);
        this.listView1.Columns.Add("文件类型", 120, HorizontalAlignment.Left);
        try
        {

            XmlHelper xml = new XmlHelper();

            this.listView1.BeginUpdate();
            XmlNodeList nodelist = xml.GetFolderChild(foldername);
            if (!(nodelist == null))//存在子文件
            {

                foreach (XmlNode xn in nodelist)
                {
                    //遍历子节点

                    string name = (xn.SelectSingleNode("filename")).InnerText;
                    string date = (xn.SelectSingleNode("date")).InnerText;
                    string size = (xn.SelectSingleNode("size")).InnerText;
                    string type = (xn.SelectSingleNode("type")).InnerText;
                    string path = (xn.SelectSingleNode("path")).InnerText;

                    int i = 0;//动态加载图片下标
                    ListViewItem lvi = new ListViewItem();
                    if (type.Equals("文件") || type.Equals("压缩文件"))
                    {
                        string s1 = name.Substring(name.LastIndexOf("."));
                        if (s1.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 2;//doc文档

                        }
                        else if (s1.Equals(".txt"))
                        {
                            lvi.ImageIndex = 3;//txt

                        }
                        else if (s1.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jar", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".rar", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 1;//压缩文件
                        }
                        else if (s1.Equals(".png", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 4;//图片资源

                        }
                        else if (s1.Equals(".mp4", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 5;

                        }
                        else if (s1.Equals(".avi", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 6;

                        }
                        else if (s1.Equals(".rmvb", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 7;

                        }
                        else if (s1.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 8;

                        }
                        else if (s1.Equals(".pdf"))
                        {
                            lvi.ImageIndex = 9;
                        }
                        else if (s1.Equals(".cpp"))
                        {
                            lvi.ImageIndex = 10;
                        }
                        else if (s1.Equals(".exe"))
                        {
                            ImageList imgLst = new ImageList();
                            string filepath = path;
                            System.Drawing.Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(@filepath);
                            //imgLst.Images.Add(fileIcon);
                            imageList1.Images.Add(fileIcon);
                            lvi.ImageIndex = 11 + i;//可执行文件
                            i++;

                        }
                    }
                    else
                    {
                        lvi.ImageIndex = 0;     //文件夹
                    }

                    lvi.Text = name;
                    lvi.SubItems.Add(size);
                    lvi.SubItems.Add(date);
                    lvi.SubItems.Add(type);
                    this.listView1.Items.Add(lvi);

                }




            }
            this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。

        }

        catch (ArgumentException ea)
        {
            MessageBox.Show("异常:" + ea.Message);
            return;
        }
        catch (XmlException ex)
        {
            MessageBox.Show("异常:" + ex.Message);
            return;
        }
        catch (FileLoadException efl)
        {
            MessageBox.Show("异常:" + efl.Message);
            return;
        }
        catch (FieldAccessException efld)
        {
            MessageBox.Show("异常:" + efld.Message);
            return;
        }
    }
    else if (this.listView1.View.Equals(View.LargeIcon))
    {
        this.listView1.Clear();
        this.listView1.CheckBoxes = false;
        this.listView1.LargeImageList = this.imageList2;
        try
        {

            XmlHelper xml = new XmlHelper();
            XmlNodeList nodelist = xml.GetFolderChild(foldername);
            if (!(nodelist == null))//存在子文件
            {
                this.listView1.BeginUpdate();
                foreach (XmlNode xn in nodelist)
                {
                    //遍历子节点

                    string name = (xn.SelectSingleNode("filename")).InnerText;
                    string date = (xn.SelectSingleNode("date")).InnerText;
                    string size = (xn.SelectSingleNode("size")).InnerText;
                    string type = (xn.SelectSingleNode("type")).InnerText;
                    string path = (xn.SelectSingleNode("path")).InnerText;

                    int i = 0;//动态加载图片下标
                    ListViewItem lvi = new ListViewItem();
                    if (type.Equals("文件") || type.Equals("压缩文件"))
                    {
                        string s1 = name.Substring(name.LastIndexOf("."));
                        if (s1.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 3;//doc文档

                        }
                        else if (s1.Equals(".txt"))
                        {
                            lvi.ImageIndex = 2;//txt

                        }
                        else if (s1.Equals(".xml"))
                        {
                            lvi.ImageIndex = 5;//txt

                        }
                        else if (s1.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jar", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".rar", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 1;//压缩文件
                        }
                        else if (s1.Equals(".png", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 6;//图片资源

                        }
                        else if (s1.Equals(".mp4", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 4;

                        }
                        else if (s1.Equals(".avi", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 4;

                        }
                        else if (s1.Equals(".rmvb", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 4;

                        }
                        else if (s1.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase))
                        {
                            lvi.ImageIndex = 7;

                        }
                        else if (s1.Equals(".pdf"))
                        {
                            lvi.ImageIndex = 8;
                        }
                        else if (s1.Equals(".cpp"))
                        {
                            lvi.ImageIndex = 9;
                        }
                        else if (s1.Equals(".exe"))
                        {
                            ImageList imgLst = new ImageList();
                            string filepath = path;
                            System.Drawing.Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(@filepath);
                            //imgLst.Images.Add(fileIcon);
                            imageList2.Images.Add(fileIcon);
                            lvi.ImageIndex = 11 + i;//可执行文件
                            i++;

                        }
                        else
                        {
                            lvi.ImageIndex = 10;
                        }
                    }
                    else
                    {
                        lvi.ImageIndex = 0;     //文件夹
                    }

                    lvi.Text = name;
                    /*lvi.SubItems.Add(size);
                    lvi.SubItems.Add(date);
                    lvi.SubItems.Add(type);*/
                    this.listView1.Items.Add(lvi);

                }



                this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。

            }
        }
        catch (ArgumentException ea)
        {
            MessageBox.Show("异常:" + ea.Message);
            return;
        }
        catch (XmlException ex)
        {
            MessageBox.Show("异常:" + ex.Message);
            return;
        }
        catch (FileLoadException efl)
        {
            MessageBox.Show("异常:" + efl.Message);
            return;
        }
        catch (FieldAccessException efld)
        {
            MessageBox.Show("异常:" + efld.Message);
            return;
        }
    }


}

/// <summary>
/// 搜索功能
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void btsearch(object sender, EventArgs e)
{
    string s = textBox2.Text;
    showsearchres(s);

}

/// <summary>
/// 显示搜索结果
/// 遍历listview，匹配正则表达式
/// </summary>
/// <param name="s"></param>
void showsearchres(string s)
{
    this.listView1.View = View.Details;
    this.listView1.CheckBoxes = true;
    this.listView1.Clear(); //清除所有项和列
    this.listView1.Height = 305;
    this.listView1.Width = 657;
    this.listView1.SmallImageList = this.imageList1;  //将listView的图标集与imageList1绑定
    this.listView1.Columns.Add("文件名", 300, HorizontalAlignment.Left);
    this.listView1.Columns.Add("大小", 80, HorizontalAlignment.Left);
    this.listView1.Columns.Add("修改时间", 120, HorizontalAlignment.Left);
    this.listView1.Columns.Add("文件类型", 120, HorizontalAlignment.Left);

    XmlHelper xml = new XmlHelper();
    XmlNodeList nodelist = xml.GetXmlNodeListByXpath(@"D:\云管家\config\fileinfo.xml", "/fileroot/files/file");
    this.listView1.BeginUpdate();
    foreach (XmlNode xn in nodelist)
    {
        //遍历子节点

        string name = (xn.SelectSingleNode("filename")).InnerText;
        string date = (xn.SelectSingleNode("date")).InnerText;
        string size = (xn.SelectSingleNode("size")).InnerText;
        string type = (xn.SelectSingleNode("type")).InnerText;
        string path = (xn.SelectSingleNode("path")).InnerText;
          //string path = listView1.Items[j].SubItems[4].Text;
          if (name.Contains(s))
          {
              int i = 0;//动态加载图片下标
              ListViewItem lvi = new ListViewItem();
              if (type.Equals("文件") || type.Equals("压缩文件"))
              {
                  string s1 = name.Substring(name.LastIndexOf("."));
                  if (s1.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
                  {
                      lvi.ImageIndex = 2;//doc文档

                  }
                  else if (s1.Equals(".txt"))
                  {
                      lvi.ImageIndex = 3;//txt

                  }
                  else if (s1.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jar", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".rar", StringComparison.CurrentCultureIgnoreCase))
                  {
                      lvi.ImageIndex = 1;//压缩文件
                  }
                  else if (s1.Equals(".png", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || s1.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                  {
                      lvi.ImageIndex = 4;//图片资源

                  }
                  else if (s1.Equals(".mp4", StringComparison.CurrentCultureIgnoreCase))
                  {
                      lvi.ImageIndex = 5;

                  }
                  else if (s1.Equals(".avi", StringComparison.CurrentCultureIgnoreCase))
                  {
                      lvi.ImageIndex = 6;

                  }
                  else if (s1.Equals(".rmvb", StringComparison.CurrentCultureIgnoreCase))
                  {
                      lvi.ImageIndex = 7;

                  }
                  else if (s1.Equals(".mp3", StringComparison.CurrentCultureIgnoreCase))
                  {
                      lvi.ImageIndex = 8;

                  }
                  else if (s1.Equals(".pdf"))
                  {
                      lvi.ImageIndex = 9;
                  }
                  else if (s1.Equals(".cpp"))
                  {
                      lvi.ImageIndex = 10;
                  }
                  else if (s1.Equals(".exe"))
                  {
                      ImageList imgLst = new ImageList();
                      string filepath = path;
                      System.Drawing.Icon fileIcon = System.Drawing.Icon.ExtractAssociatedIcon(@filepath);
                      //imgLst.Images.Add(fileIcon);
                      imageList1.Images.Add(fileIcon);
                      lvi.ImageIndex = 11 + i;//可执行文件
                      i++;
                     

                  }
              }
              else
              {
                  lvi.ImageIndex = 0;     //文件夹
              }

              lvi.Text = name;
              lvi.SubItems.Add(size);
              lvi.SubItems.Add(date);
              lvi.SubItems.Add(type);
              this.listView1.Items.Add(lvi);
          }    

    }
    this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。
}


/// <summary>
/// 显示分享信息
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void btshareres_Click(object sender, EventArgs e)
{
    panel10.Visible = true;
    panel11.Visible = false;
    listView3.Visible = true;
    btshareOK.Text = "下载";
    showshareinfo();
}



    }
        
  }
 

