using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Socket
{
    public delegate void SendComplete();//定义委托，用来在全部数据传输完成的时候有用户指定一些操作
    public delegate void SendOneComplete(String filePath);//单个文件传输完成时执行的操作
    public delegate void SendDirComplete(String dirPath);//一个目录传输完成时执行的操作
    public delegate void SendFailed();//数据传输失败
    /// <summary>
    /// tcp通信客户端
    /// </summary>
    public class Client
    {
        private TcpClient tcpClient;
        private String toIp;
        private int toPort;

        public List<string> directoryPath { get; set; }

        /// <summary>
        /// 链接到指定的服务器上去
        /// </summary>
        /// <param name="toIp"></param>
        /// <param name="toPort"></param>
        public Client(String toIp, int toPort)
        {
            this.toIp = toIp;
            this.toPort = toPort;
            // tcpClient = new TcpClient();
            //send();
        }


        /// <summary>
        /// 开始进行通信
        /// <param name="sendComplete">数据全部传输完毕需要执行的操作</param>
        /// <param name="sendFailed">数据传输失败需要执行的操作</param>
        /// <param name="sendOneComplete">一个文件传输完毕时执行的操作</param>
        /// <param name="sendDirComplete">一个目录传输完毕时执行的操作</param>
        /// </summary>
        public void send(SendComplete sendComplete, SendDirComplete sendDirComplete, SendOneComplete sendOneComplete, SendFailed sendFailed)
        {
            // if(this.directoryPath.Count<0)
            try
            {
                using (tcpClient = new TcpClient(toIp, toPort))
                {
                    int count = 0;
                    while (!tcpClient.Connected)
                    {
                        if (count++ == 5)
                        {
                            //sendFailed();
                            break;
                        }
                        // Thread.Sleep(5000);
                        tcpClient.Connect(toIp, toPort);

                    }
                    //发送指定目录下的全部文件
                    using (NetworkStream ns = tcpClient.GetStream())
                    {
                        for (int i = 0; i < directoryPath.Count - 1; i++)
                        {
                            FileUtil.sendFiles(ns, sendOneComplete, directoryPath.ElementAt(i), false);

                            sendDirComplete(directoryPath.ElementAt(i));
                        }
                        FileUtil.sendFiles(ns, sendOneComplete, directoryPath.ElementAt(directoryPath.Count - 1), true);//最后一个关闭流 
                        sendDirComplete(directoryPath.ElementAt(directoryPath.Count - 1));
                    }
                    sendComplete();
                }
                // tcpClient.Close();//发送完毕，则关闭连接
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                sendFailed();
            }
            finally
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    tcpClient.Close();
                }
            }
        }
        /// <summary>
        /// 开始进行通信
        /// <param name="sendComplete">数据传输完毕需要执行的操作</param>
        /// <param name="sendFailed">数据传输失败需要执行的操作</param>
        /// </summary>
        public void send(SendComplete sendComplete, SendOneComplete sendOneComplete, SendFailed sendFailed)
        {
            try
            {
                using (tcpClient = new TcpClient(toIp, toPort))
                {
                    int count = 0;
                    while (!tcpClient.Connected)
                    {
                        if (count++ == 5)
                        {
                            //sendFailed();
                            break;
                        }
                        // Thread.Sleep(5000);
                        tcpClient.Connect(toIp, toPort);
                    }
                    //发送指定目录下的全部文件
                    using (NetworkStream ns = tcpClient.GetStream())
                    {
                        for (int i = 0; i < directoryPath.Count - 1; i++)
                        {
                            FileUtil.sendFiles(ns, sendOneComplete, directoryPath.ElementAt(i), false);
                            //   sendOneComplete(directoryPath.ElementAt(i));
                        }
                        FileUtil.sendFiles(ns, sendOneComplete, directoryPath.ElementAt(directoryPath.Count - 1), true);//最后一个关闭流 
                                                                                                                        // sendOneComplete(directoryPath.ElementAt(directoryPath.Count - 1));
                    }
                    sendComplete();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                sendFailed();
            }
        }
        /// <summary>
        /// 开始进行通信
        /// <param name="sendComplete">数据传输完毕需要执行的操作</param>
        /// <param name="sendFailed">数据传输失败需要执行的操作</param>
        /// </summary>
        public void send(SendComplete sendComplete, SendFailed sendFailed)
        {
            try
            {
                using (tcpClient = new TcpClient(toIp, toPort))
                {
                    int count = 0;
                    while (!tcpClient.Connected)
                    {
                        if (count++ == 5)
                        {
                            //sendFailed();
                            break;
                        }
                        // Thread.Sleep(5000);
                        tcpClient.Connect(toIp, toPort);
                    }
                    //发送指定目录下的全部文件
                    using (NetworkStream ns = tcpClient.GetStream())
                    {
                        for (int i = 0; i < directoryPath.Count - 1; i++)
                        {
                            FileUtil.sendFiles(ns, directoryPath.ElementAt(i), false);
                            // sendDirComplete(directoryPath.ElementAt(i));
                        }
                        FileUtil.sendFiles(ns, directoryPath.ElementAt(directoryPath.Count - 1), true);//最后一个关闭流 
                                                                                                       //  sendDirComplete(directoryPath.ElementAt(directoryPath.Count - 1));
                    }
                    sendComplete();
                }
                // tcpClient.Close();//发送完毕，则关闭连接
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                sendFailed();
            }
            finally
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    tcpClient.Close();
                }
            }
        }

        public string getUserNameOfServerComputer()
        {
            string userName = "";
            try
            {
                using (tcpClient = new TcpClient(toIp, toPort))
                {
                    int count = 0;
                    while (!tcpClient.Connected)
                    {
                        if (count++ == 5)
                        {
                            break;
                        }
                        tcpClient.Connect(toIp, toPort);
                    }
                    //发送指令
                    using (NetworkStream ns = tcpClient.GetStream())
                    {
                        FileUtil.sendMsgOfGetUserNameComputer(ns);
                        int msgLength = 0;
                        msgLength = FileUtil.getMsgLength(ns);
                        userName = FileUtil.getMsg(ns, msgLength);
                        Console.WriteLine("客户端:用户名消息长度{0},用户名{1}", msgLength, userName);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    tcpClient.Close();
                }
            }
            return userName;

        }

        /// <summary>
        /// 通过通信取得服务器的地址
        /// </summary>
        /// <returns></returns>
        public string getAppDir()
        {
            string dir = "";
            try
            {
                using (tcpClient = new TcpClient(toIp, toPort))
                {
                    int count = 0;
                    while (!tcpClient.Connected)
                    {
                        if (count++ == 5)
                        {
                            break;
                        }
                        tcpClient.Connect(toIp, toPort);
                    }
                    //发送指令
                    using (NetworkStream ns = tcpClient.GetStream())
                    {
                        FileUtil.sendMsgOfGetAppDir(ns);
                        int msgLength = 0;
                        msgLength = FileUtil.getMsgLength(ns);
                        dir = FileUtil.getMsg(ns, msgLength);
                        Console.WriteLine("客户端:应用程序路径消息长度{0},应用程序路径{1}", msgLength, dir);
                    }
                }
                // tcpClient.Close();//发送完毕，则关闭连接
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    tcpClient.Close();
                }
            }
            return dir;
        }
    }
}
