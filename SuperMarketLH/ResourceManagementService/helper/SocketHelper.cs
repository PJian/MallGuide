using PJ.ConTcp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ResourceManagementService.helper
{
    public class SocketHelper
    {
        //private delegate void SendComplete();
        /// <summary>
        /// 连接到指定的服务器去
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static Client connectToServer(string ip)
        {
            return new Client(ip, 9999);
        }
        /// <summary>
        /// 发送指定目录下的数据
        /// </summary>
        /// <param name="directory"></param>
        public static void sendData(Client client, string directory, SendComplete sendComplete,SendFailed sendFailed)
        {
            client.directoryPath = new string[1] { directory }.ToList();
            client.send(sendComplete,  sendFailed);
        }
        /// <summary>
        /// 发送指定目录下的数据
        /// </summary>
        /// <param name="directory"></param>
        public static void sendHeartData(Client client, string directory, SendComplete sendComplete, SendFailed sendFailed)
        {
            client.directoryPath = new string[1] { directory }.ToList();
            client.send(sendComplete, sendFailed);
        }

        /// <summary>
        /// 取得客户端的安装目录
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sendComplete"></param>
        /// <param name="sendFailed"></param>
        public static string  getAppDir(Client client, SendComplete sendComplete, SendFailed sendFailed)
        {
            return client.getAppDir();
        }

        /// <summary>
        /// 取得客户端的用户名
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sendComplete"></param>
        /// <param name="sendFailed"></param>
        /// <returns></returns>
        public static string getUserNameOfComputer(Client client, SendComplete sendComplete, SendFailed sendFailed) {
            return client.getUserNameOfServerComputer();
        }
        /// <summary>
        /// 监听本机的端口号
        /// </summary>
        /// <returns></returns>
        public static Server createServer()
        {
            Server.isAlive = true;
            Server server = new Server(GetLocalIp(), 9999);
            return server;
        }


        static string GetLocalIp()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch (Exception e)
            {
               // MessageBox.Show("获取本机IP出错:" + ex.Message);
                return "127.0.0.1";
            }


        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="server"></param>
        public static void recvData(Server server)
        {
            server.acceptAlways(delegate() {
                if (File.Exists(@"data\1.zip")) {
                    new ZipHelper().UnZip(@"data\1.zip", "data");
                }
            });
        }
        /// <summary>
        /// 接受客户端消息
        /// </summary>
        /// <param name="server"></param>
        public static void receMsg(Server server) {
            server.acceptAlways();
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="server"></param>
        public static void recvData(Server server,string dataFileName,string testFileName,string unZipDir)
        {
            server.acceptAlways(delegate()
            {
                if (File.Exists(testFileName))
                {
                    new ZipHelper().UnZip(testFileName, unZipDir);
                    //ZipHelperHaoZip.unZip(testFileName, unZipDir);
                    File.Delete(testFileName);
                }
                if (File.Exists(dataFileName))
                {
                    //ZipHelperHaoZip.unZip(dataFileName, unZipDir);
                    new ZipHelper().UnZip(dataFileName, unZipDir);
                    File.Delete(dataFileName);
                }
            });
        }
    }
}
