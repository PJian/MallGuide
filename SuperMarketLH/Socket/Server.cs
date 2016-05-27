using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socket
{
    public delegate void ReceiveComplete();//定义委托，用来在全部接收完成的时候有用户指定一些操作
    /// <summary>
    /// tcp通信服务端
    /// </summary>
    public class Server
    {
        private TcpListener listener;
        private int port;// = int.Parse(ConfigurationManager.ConnectionStrings["listenerPort"].ConnectionString);
        private String ip;//= ConfigurationManager.ConnectionStrings["listenerIp"].ConnectionString;
        private int listenerCount = 10;

        public static bool isAlive = false;

        /// <summary>
        /// 监听指定的IP地址和端口号
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public Server(String ip, int port)
        {
            this.ip = ip;
            this.port = port;
            listener = new TcpListener(IPAddress.Parse(ip), port);
            //   if(listener.)
            //if (!isAlive) {
            ///服务器未启动，才可以进行监听
            ///
            try
            {
                // Console.WriteLine("启动监听器");
                listener.Start();//启动监听
            }
            catch (Exception e)
            {
                throw e;
            }

            // }
        }
        /// <summary>
        /// 关闭侦听器
        /// </summary>
        public void stop()
        {
            Console.WriteLine("关闭监听器");
            this.listener.Stop();
        }
        /// <summary>
        /// 监听指定的IP地址和端口号，同时限定监听数
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="count"></param>
        public Server(String ip, int port, int count)
        {
            this.ip = ip;
            this.port = port;
            this.listenerCount = count;
            listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();//启动监听
        }
        /// <summary>
        /// 一直接受客户端的链接
        /// </summary>
        public void acceptAlways()
        {
            while (isAlive)
            {
                acceptOnce();
            }
        }
        /// <summary>
        /// 保存文件位置根据指定的文件路径
        /// </summary>
        public void acceptAlwaysByDefault()
        {
            while (isAlive)
            {
                acceptOnceByDefault();
            }
        }

        /// <summary>
        /// 一直接受客户端的链接
        /// </summary>
        public void acceptAlways(ReceiveComplete recvComplete)
        {
            while (isAlive)
            {
                acceptOnce(recvComplete);
            }
        }

        /// <summary>
        /// 接收一个客户端
        /// </summary>
        public void acceptOnceByDefault()
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("有人链接" + client.ToString());
                //启动一个线程与client进行通信
                Thread clientT = new Thread(FileUtil.receiveAndSaveByFileName);
                clientT.IsBackground = true;
                clientT.Start(client.GetStream());
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 接收一个客户端
        /// </summary>
        public void acceptOnce()
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("有人链接" + client.ToString());
                //启动一个线程与client进行通信
                Thread clientT = new Thread(FileUtil.receive);
                clientT.IsBackground = true;
                clientT.Start(client.GetStream());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 接收一个客户端
        /// </summary>
        public void acceptOnce(ReceiveComplete recvComplete)
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("有人链接" + client.ToString());
                //启动一个线程与client进行通信
                Thread clientT = new Thread(delegate () {
                    FileUtil.receiveZipFile(client.GetStream(), recvComplete);
                });
                clientT.IsBackground = true;
                clientT.Start();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
