using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Xml;






namespace 云服务端
{
    class Files
    {

        private static Thread threadWatch = null;
        private static Socket socketWatch = null;
        private static ListBox lstbxMsgView;//显示接受的文件等信息
        private static ListBox listbOnline;//显示用户连接列表
        

        private static Dictionary<string, Socket> dict = new Dictionary<string, Socket>();
        private static Dictionary<string, Thread> dictThread = new Dictionary<string, Thread>();
        public static long posission = 0;

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="localIp"></param>
        /// <param name="localPort"></param>
        public static void BeginListening(string localIp, string localPort, ListBox listbox, ListBox listboxOnline)
        {
            //基本参数初始化
            lstbxMsgView = listbox;
            listbOnline = listboxOnline;

            //创建服务端负责监听的套接字，参数（使用IPV4协议，使用流式连接，使用Tcp协议传输数据）
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //获取Ip地址对象
            IPAddress address = IPAddress.Parse(localIp);
            //创建包含Ip和port的网络节点对象
            IPEndPoint endpoint = new IPEndPoint(address, int.Parse(localPort));
            //将负责监听的套接字绑定到唯一的Ip和端口上
            socketWatch.Bind(endpoint);
            //设置监听队列的长度
            socketWatch.Listen(10);
            //创建负责监听的线程，并传入监听方法
            threadWatch = new Thread(WatchConnecting);
            threadWatch.IsBackground = true;//设置为后台线程
            threadWatch.Start();//开始线程
            //ShowMgs("服务器启动监听成功");
            ShwMsgForView.ShwMsgforView(lstbxMsgView, "服务器启动监听成功");
        }

        /// <summary>
        /// 连接客户端
        /// </summary>
        private static void WatchConnecting()
        {
            while (true)//持续不断的监听客户端的请求
            {
                //开始监听 客户端连接请求，注意：Accept方法，会阻断当前的线程
                Socket connection = socketWatch.Accept();
                if (connection.Connected)
                {
                    //向列表控件中添加一个客户端的Ip和端口，作为发送时客户的唯一标识
                    listbOnline.Items.Add(connection.RemoteEndPoint.ToString());
                    //将与客户端通信的套接字对象connection添加到键值对集合中，并以客户端Ip做为健
                    dict.Add(connection.RemoteEndPoint.ToString(), connection);

                    //创建通信线程
                    ParameterizedThreadStart pts = new ParameterizedThreadStart(RecMsg);
                    Thread thradRecMsg = new Thread(pts);
                    thradRecMsg.IsBackground = true;
                    thradRecMsg.Start(connection);
                    ShwMsgForView.ShwMsgforView(lstbxMsgView, "客户端连接成功" + connection.RemoteEndPoint.ToString());
                }
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="socketClientPara"></param>
        private static void RecMsg(object socketClientPara)
        {
            Socket socketClient = socketClientPara as Socket;
            byte[] buffer = new byte[1024*100];
            while (true)
            {
                string uname="zhangsan";//获取云盘路径，待设置
                try
                {
                    socketClient.Receive(buffer); // 接收数据，并返回数据的长度；
                    if (buffer[0] == 1)//客户端上传文件请求
                    {
                        
                        socketClient.Receive(buffer); // 接收数据，并返回数据的长度；
                        long receive = 0L, length = BitConverter.ToInt64(buffer, 0);
                        string fileName = Encoding.Default.GetString(buffer, 0, socketClient.Receive(buffer));
                        using (FileStream writer = new FileStream(Path.Combine("D://云管家//云服务端//receivedfile//"+uname+"//", fileName), FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            int received;

                            while (receive < length)
                            {
                                received = socketClient.Receive(buffer);

                                writer.Write(buffer, 0, received);
                                writer.Flush();
                                receive += (long)received;

                            }
                        }
                        ShwMsgForView.ShwMsgforView(lstbxMsgView, "文件保存成功:" + fileName);
                    }


                    else if (buffer[0] == 2)   //下载请求
                    {

                        string filename = Encoding.Default.GetString(buffer, 0, socketClient.Receive(buffer));
                        string name = filename;
                        filename = @"D:\云管家\云服务端\receivedfile\" + uname + @"\" + filename;
                        using (FileStream reader = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                        {


                            byte[] arrSendMsg = new byte[1];
                            arrSendMsg[0] = 1; // 用来表示发送的是文件 
                            socketClient.Send(arrSendMsg); // 发送消息； 
                            

                            long send = 0L, length = reader.Length;
                            System.Threading.Thread.Sleep(100);
                            socketClient.Send(BitConverter.GetBytes(length));
                            string fileName = Path.GetFileName(filename);
                      
                            socketClient.Send(Encoding.Default.GetBytes(fileName));
                            byte[] buffer1 = new byte[1024];
                            int read, sent;
                            
                            XmlHelper xml = new XmlHelper();
                            XmlNodeList nodelist = xml.GetXmlNodeListByXpath(@"D:\云管家\config\xuchuan.xml", "/root/file");
                            foreach (XmlNode xn in nodelist)
                            {
                                //遍历子节点

                                string name1 = (xn.SelectSingleNode("filename")).InnerText;
                                // string isxuchuan = (xn.SelectSingleNode("isxuchuan")).InnerText;
                                if (name1.Equals(name))
                                {
                                    reader.Position = long.Parse((xn.SelectSingleNode("posission")).InnerText.ToString());
                                   // p = writer.Position;
                                }
                            }
                            //断点发送 在这里判断设置reader.Position即可
                            while ((read = reader.Read(buffer1, 0, 1024)) != 0)
                            {
                                sent = 0;
                                while ((sent += socketClient.Send(buffer1, sent, read, SocketFlags.None)) < read)
                                {
                                    send += (long)sent;
                                   
                                    //Console.WriteLine("Sent " + send + "/" + length + ".");//进度
                                }
                            }
                            // Console.WriteLine("Send finish.");
                            //txtMsg.Text = "Send finish";
                            ShwMsgForView.ShwMsgforView(lstbxMsgView, "文件发送成功:" + fileName);
                        }
                    }
                    else if (buffer[0] == 3)//登陆请求
                    {
                        string info = Encoding.Default.GetString(buffer, 0, socketClient.Receive(buffer));



                        string[] s = info.Split(new char[] { ' ' });
                        byte[] arrSendMsg = new byte[1];
                        arrSendMsg[0] = 4; // 验证失败 
                        XmlHelper xml = new XmlHelper();
                        XmlNodeList nodelist = xml.GetXmlNodeListByXpath(@"D:\云管家\云服务端\userinfo.xml", "/root/user");
                        foreach (XmlNode xn in nodelist)
                        {

                            string name = (xn.SelectSingleNode("username")).InnerText;
                            string password = (xn.SelectSingleNode("password")).InnerText;
                            if (s[0].Equals(name) && s[1].Equals(password))
                            {
                                arrSendMsg[0] = 3;//验证成功
                                uname = name;
                            }
                            socketClient.Send(arrSendMsg); // 发送消息； 
                        }
                        

                    }
                    else if (buffer[0] == 6)//删除请求
                    {
                        string fname= Encoding.Default.GetString(buffer, 0, socketClient.Receive(buffer));
                        string fpath = @"D:\云管家\云服务端\receivedfile\" + uname + @"\" + fname;
                        File.Delete(fpath);
                        
                        ShwMsgForView.ShwMsgforView(lstbxMsgView, "文件删除成功:" + fname);
                    }
                    else if (buffer[0] == 7)//新建文件夹请求
                    {
                        string fname = Encoding.Default.GetString(buffer, 0, socketClient.Receive(buffer));
                        string fpath = @"D:\云管家\云服务端\receivedfile\" + uname + @"\" + fname;
                        Directory.CreateDirectory(fpath);
                    }
                    else if (buffer[0] == 8)//登陆请求
                    {
                        string info = Encoding.Default.GetString(buffer, 0, socketClient.Receive(buffer));



                        string[] s = info.Split(new char[] { ' ' });
                        byte[] arrSendMsg = new byte[1];
                        arrSendMsg[0] = 9; // 验证失败 
                        System.Collections.Hashtable ht = new System.Collections.Hashtable();
                        ht["username"] = s[0];
                        ht["password"] = s[1];
                        XmlHelper xml = new XmlHelper();

                        if (xml.InsertNode(@"D:\云管家\云服务端\userinfo.xml", "user", false, "/root", ht, ht))
                        {
                            arrSendMsg[0] = 8;//验证成功
                        }
                        socketClient.Send(arrSendMsg); // 发送消息； 

                    }
                }
                catch (SocketException se)
                {
                    //ShowMsg("异常：" + se.Message);
                    ShwMsgForView.ShwMsgforView(lstbxMsgView, "异常：" + se.Message);
                    // 从 通信套接字 集合中删除被中断连接的通信套接字；  
                    dict.Remove(socketClient.RemoteEndPoint.ToString());
                    // 从通信线程集合中删除被中断连接的通信线程对象；  
                    dictThread.Remove(socketClient.RemoteEndPoint.ToString());
                    // 从列表中移除被中断的连接IP  
                    listbOnline.Items.Remove(socketClient.RemoteEndPoint.ToString());
                    break;

                }
                catch (Exception e)
                {
                    ShwMsgForView.ShwMsgforView(lstbxMsgView, "异常：" + e.Message);
                    // 从 通信套接字 集合中删除被中断连接的通信套接字；  
                    dict.Remove(socketClient.RemoteEndPoint.ToString());
                    // 从通信线程集合中删除被中断连接的通信线程对象；  
                    dictThread.Remove(socketClient.RemoteEndPoint.ToString());
                    // 从列表中移除被中断的连接IP  
                    listbOnline.Items.Remove(socketClient.RemoteEndPoint.ToString());
                    break;
                }

            }

        }
       

        /// <summary>
        /// 关闭连接
        /// </summary>
        public static void CloseTcpSocket()
        {
            dict.Clear();
            listbOnline.Items.Clear();
            threadWatch.Abort();
            socketWatch.Close();
            ShwMsgForView.ShwMsgforView(lstbxMsgView, "服务器关闭监听");
        }

        /// <summary>
        /// 计算文件夹大小
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static long DirSize(DirectoryInfo d)
        {
            long Size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                Size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                Size += DirSize(di);
            }
            return (Size);
        }

        /// <summary>
        /// 从txt读取一行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string readlineintxt(string path)
        {
            try
            {
                //FileStream objFileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read); 
                StreamReader sr = new StreamReader(path,Encoding.UTF8);
                
                string r=sr.ReadLine();
                 sr.Close();
                 return r;
                //sr.Dispose();

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
    }

}
