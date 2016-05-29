using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socket
{
    public class FileUtil
    {
        //这里就是标识文件开始和结束的几个 字段
        public static string FILE_START = "FS";
        public static string FILE_END = "FE";
        public static string SEND_END = "SE";
        public static string FILE_NAME = "FN";
        public static string MSG_LENGTH = "ML";
        public static string FILE_RECV = "FC";

        public static string APP_DIR = "AP";
        public static string USER_NAME = "UN";

        //保存数据的路径
        public static string DATA_DIRECTORY = Path.Combine("signData", DateTime.Now.ToString("yyyy-MM-dd"));
        //发送数据所在的文件夹
        public static string SEND_DATA_DIRECTORY = "";
        /// <summary>
        /// 接收文件
        ///     分为4步：
        ///         1，接收消息标志
        ///         2，接收消息
        ///         3，接收文件的长度
        ///         4，接收文件内容
        ///         5，接收完毕，发送FILE_RECV，表示文件已经接受完毕，可以进行下一个文件的发送
        ///  发送应用程序目录
        ///     2步：
        ///         1，发送消息长度
        ///         2，发送目录字符串
        /// </summary>
        /// <param name="receiveStream"> NetworkStream 类型的流，从这个流中接收数据</param>
        public static void receive(Object receiveStream)
        {
            NetworkStream rStream = receiveStream as NetworkStream;
            if (rStream != null)
            {
                string flag = "";
                //若流上没有数据，则等待1秒钟
                //  using (rStream)
                //  {
                while (!rStream.DataAvailable)
                {
                    Thread.Sleep(100);
                }
                while (!SEND_END.Equals(flag))
                {
                    flag = getFlag(rStream);
                    //  Console.WriteLine(flag);
                    if (FILE_START.Equals(flag))
                    {
                        receiveFile(rStream);
                    }
                    //如果是请求应用程序路径，则发送路径
                    if (APP_DIR.Equals(flag))
                    {
                        string appDir = AppDomain.CurrentDomain.BaseDirectory;
                        sendMsgLength(rStream, Encoding.UTF8.GetBytes(appDir).Length);//是字节长度，中应为不同的
                        sendMsg(rStream, appDir);
                        Console.WriteLine("应用程序路径:{0},长度{1}", appDir, appDir.Length);
                        break;
                    }
                    //如果是请求主机用户名，则发送用户名
                    if (USER_NAME.Equals(flag))
                    {
                        string userName = Environment.UserName;
                        sendMsgLength(rStream, Encoding.UTF8.GetBytes(userName).Length);
                        sendMsg(rStream, userName);
                        Console.WriteLine("用户名:{0},长度{1}", userName, userName.Length);
                        break;
                    }
                }

                //  }
            }
        }

        public static void receiveAndSaveByFileName(Object receiveStream)
        {
            NetworkStream rStream = receiveStream as NetworkStream;
            if (rStream != null)
            {
                string flag = "";
                //若流上没有数据，则等待1秒钟
                //  using (rStream)
                //  {
                while (!rStream.DataAvailable)
                {
                    Thread.Sleep(100);
                }
                while (!SEND_END.Equals(flag))
                {
                    flag = getFlag(rStream);
                    //  Console.WriteLine(flag);
                    if (FILE_START.Equals(flag))
                    {
                        receiveFileAndSaveByFileName(rStream);
                    }
                    //如果是请求应用程序路径，则发送路径
                    //if (APP_DIR.Equals(flag))
                    //{
                    //    string appDir = AppDomain.CurrentDomain.BaseDirectory;
                    //    sendMsgLength(rStream, Encoding.UTF8.GetBytes(appDir).Length);//是字节长度，中应为不同的
                    //    sendMsg(rStream, appDir);
                    //    Console.WriteLine("应用程序路径:{0},长度{1}", appDir, appDir.Length);
                    //    break;
                    //}
                    ////如果是请求主机用户名，则发送用户名
                    //if (USER_NAME.Equals(flag))
                    //{
                    //    string userName = Environment.UserName;
                    //    sendMsgLength(rStream, Encoding.UTF8.GetBytes(userName).Length);
                    //    sendMsg(rStream, userName);
                    //    Console.WriteLine("用户名:{0},长度{1}", userName, userName.Length);
                    //    break;
                    //}
                }

                //  }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveStream"></param>
        /// <param name="recvComplete"></param>
        public static void receiveZipFile(Object receiveStream, ReceiveComplete recvComplete)
        {
            NetworkStream rStream = receiveStream as NetworkStream;
            if (rStream != null)
            {
                string flag = "";
                //若流上没有数据，则等待1秒钟
                //  using (rStream)
                //  {
                while (!rStream.DataAvailable)
                {
                    Thread.Sleep(100);
                }



                //  receiveZipFile(rStream);


                int msgLength = 0;

                String fileName = "";
                int r = 0;
                byte[] buffer = new byte[1024 * 5];
                while (!FILE_START.Equals(flag))
                {
                    flag = getFlag(rStream);
                    //  Console.WriteLine(flag);
                    if (FILE_START.Equals(flag))
                    {
                        //接收文件名和文件内容
                        msgLength = getMsgLength(rStream);
                        // Console.WriteLine("****msgLength***" + msgLength);

                        r = rStream.Read(buffer, 0, msgLength);

                        fileName = Encoding.UTF8.GetString(buffer, 0, r);

                        Console.WriteLine("初始的文件名：" + fileName);
                        fileName = System.IO.Path.Combine("data", Path.GetFileName(fileName));//保存文件时用到的文件名

                        //Console.WriteLine("文件存在" + File.Exists(fileName));

                        //while (File.Exists(fileName))
                        //{
                        //    File.Delete(fileName);
                        //}
                        //创建文件
                        Console.WriteLine("最终的名字：" + fileName);
                        // File.Create(fileName);
                        //写入文件内容
                        msgLength = getMsgLength(rStream);//文件长度
                        Console.WriteLine("文件长度:" + msgLength);
                    }

                }

                int rlength = 1024 * 5;//每次读取的数据量
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    while (msgLength > 0)
                    {
                        if (msgLength <= rlength)
                        {
                            r = rStream.Read(buffer, 0, msgLength);
                            msgLength = 0;
                        }
                        else
                        {
                            r = rStream.Read(buffer, 0, rlength);
                            msgLength -= r;
                        }
                        fs.Write(buffer, 0, r);
                    }
                }
                //发送标志，文件接收完毕
                // Console.WriteLine("服务器端发送数据");
                sendFlag(rStream, 6);
                Console.WriteLine("文件接受完毕");
                //接受完毕之后，解压到data目录下
                recvComplete();
                // recvComplete();//接受完毕之后要做什么
                //  }
            }
        }




        /// <summary>
        /// 接收一个文件，并且保存在本地
        ///      消息长度 + 消息内容的格式
        /// </summary>
        /// <param name="stream"></param>
        public static void receiveFile(NetworkStream stream)
        {

            //使用当前日期创建文件夹，当天接受的签名全部保存在这个文件夹下
            String currentDir = DateTime.Now.ToString("yyyymmdd");

            //接收文件名和文件内容
            int msgLength = getMsgLength(stream);
            // Console.WriteLine("****msgLength***" + msgLength);
            byte[] buffer = new byte[1024 * 5];
            int r = stream.Read(buffer, 0, msgLength);

            String fileName = Encoding.UTF8.GetString(buffer, 0, r);

            //  Console.WriteLine("初始的文件名：" + fileName);
            fileName = getSaveFilePath(DATA_DIRECTORY, fileName);//保存文件时用到的文件名
            //创建保存数据的文件夹
            if (!Directory.Exists(DATA_DIRECTORY))
            {
                Directory.CreateDirectory(DATA_DIRECTORY);
            }
            // Console.WriteLine("文件存在" + File.Exists(fileName));
            while (File.Exists(fileName))
            {
                //  Console.WriteLine("新的"+fileName);
                fileName = createNewFileName(fileName);
            }
            //创建文件
            // Console.WriteLine("最终的名字："+fileName);
            // File.Create(fileName);
            //写入文件内容
            msgLength = getMsgLength(stream);//文件长度
            // int i = 0;//指示已经读取的数据
            int rlength = 1024 * 5;//每次读取的数据量
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                while (msgLength > 0)
                {
                    if (msgLength <= rlength)
                    {
                        r = stream.Read(buffer, 0, msgLength);
                        msgLength = 0;
                    }
                    else
                    {
                        r = stream.Read(buffer, 0, rlength);
                        msgLength -= r;
                    }
                    fs.Write(buffer, 0, r);
                }
            }
            //发送标志，文件接收完毕
            Console.WriteLine("服务器端发送数据");
            sendFlag(stream, 6);
        }

        public static void receiveFileAndSaveByFileName(NetworkStream stream)
        {

            //使用当前日期创建文件夹，当天接受的签名全部保存在这个文件夹下
            //String currentDir = DateTime.Now.ToString("yyyymmdd");

            //接收文件名和文件内容
            int msgLength = getMsgLength(stream);
            // Console.WriteLine("****msgLength***" + msgLength);
            byte[] buffer = new byte[1024 * 5];
            int r = stream.Read(buffer, 0, msgLength);

            String fileName = Encoding.UTF8.GetString(buffer, 0, r);
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,fileName);

            String dir = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            //创建文件
            // Console.WriteLine("最终的名字："+fileName);
            // File.Create(fileName);
            //写入文件内容
            msgLength = getMsgLength(stream);//文件长度
            // int i = 0;//指示已经读取的数据
            int rlength = 1024 * 5;//每次读取的数据量
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                while (msgLength > 0)
                {
                    if (msgLength <= rlength)
                    {
                        r = stream.Read(buffer, 0, msgLength);
                        msgLength = 0;
                    }
                    else
                    {
                        r = stream.Read(buffer, 0, rlength);
                        msgLength -= r;
                    }
                    fs.Write(buffer, 0, r);
                    fs.Flush();
                    fs.Flush();
                    fs.Flush();
                }
            }
            //发送标志，文件接收完毕
            Console.WriteLine("服务器端发送数据");
            sendFlag(stream, 6);
        }
        /// <summary>
        /// 过滤掉fileName中卷分割符之前的内容fileName.Substring(fileName.IndexOf(Path.VolumeSeparatorChar))
        /// </summary>
        /// <returns></returns>
        public static string getSaveFilePath(string prePath, string fileName)
        {
            return Path.Combine(prePath, Path.GetFileName(fileName));
        }
        /// <summary>
        /// 创建一个新的文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string createNewFileName(string fileName)
        {
            return Path.Combine(DATA_DIRECTORY, Path.GetFileNameWithoutExtension(fileName) + "_1" + Path.GetExtension(fileName));
        }
        /// <summary>
        /// 从流中读取标志
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static String getFlag(NetworkStream stream)
        {
            byte[] buffer = new byte[2];
            stream.Read(buffer, 0, 2);
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// 从流中取得特定长度的消息
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string getMsg(NetworkStream stream, int length)
        {
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// 取得消息的长度
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static int getMsgLength(NetworkStream stream)
        {
            byte[] buffer = new byte[16];
            stream.Read(buffer, 0, 16);
            String msg = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            return int.Parse(msg);
        }

        /// <summary>
        /// 发送文件
        ///    分为4步：
        ///     1,发送消息标志
        ///         FNAME
        ///         FLENGTH
        ///         FCONTENT
        ///     2，发送文件的名字
        ///     3，发送文件的长度
        ///     4，发送文件内容
        /// </summary>
        /// <param name="stream">通过这个网络流发送数据</param>
        public static void send(NetworkStream stream, string fileName)
        {
            //测试发送D://test.txt
            if (stream.CanWrite)
            {
                //发送文件名
                //  Console.WriteLine("开始发送文件" + fileName);
                sendFileAndSaveRelative(stream, @fileName);//发送文件
                //  Console.WriteLine("文件发送完毕");
                //sendFlag(stream, 3);//发送流结束标志
            }
        }


        /// <summary>
        /// 发送文件
        ///    分为4步：
        ///     1,发送消息标志
        ///         FNAME
        ///         FLENGTH
        ///         FCONTENT
        ///     2，发送文件的名字
        ///     3，发送文件的长度
        ///     4，发送文件内容
        /// </summary>
        /// <param name="stream">通过这个网络流发送数据</param>
        /// <param name="fileName"></param>
        /// <param name="sendOneComplete">发送完毕之后，需要执行的操作</param>
        public static void send(NetworkStream stream, SendOneComplete sendOneComplete, string fileName)
        {
            //测试发送D://test.txt
            if (stream.CanWrite)
            {
                //发送文件名
                // Console.WriteLine("开始发送文件" + fileName);
                sendFile(stream, sendOneComplete, @fileName);//发送文件
                //  Console.WriteLine("文件发送完毕");
                //sendFlag(stream, 3);//发送流结束标志
            }
        }
        /// <summary>
        /// 发送两字节的标志位
        /// </summary>
        /// <param name="type">
        /// 标志类型
        ///     1，文件开始
        ///     2，文件结束
        ///     3，流结束
        ///     4，文件名字
        ///     5，消息长度  长度固定为 8 字节大小
        ///     6，文件接收完毕
        /// </param>
        private static void sendFlag(NetworkStream stream, int type)
        {
            byte[] buffer = new byte[2];
            switch (type)
            {
                case 1: buffer = Encoding.UTF8.GetBytes(FILE_START); break;
                case 2: buffer = Encoding.UTF8.GetBytes(FILE_END); break;
                case 3: buffer = Encoding.UTF8.GetBytes(SEND_END); break;
                case 4: buffer = Encoding.UTF8.GetBytes(FILE_NAME); break;
                case 5: buffer = Encoding.UTF8.GetBytes(MSG_LENGTH); break;
                case 6: buffer = Encoding.UTF8.GetBytes(FILE_RECV); break;
            }
            // Console.WriteLine(buffer.ToString());
            stream.Write(buffer, 0, 2);
            stream.Flush();
        }
        /// <summary>
        /// 发送标志取得客户端程序安装目录
        /// </summary>
        /// <param name="stream"></param>
        public static void sendMsgOfGetAppDir(NetworkStream stream)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(APP_DIR);
            stream.Write(buffer, 0, 2);
            stream.Flush();
        }
        /// <summary>
        /// 发送标志取得客户端程序安装目录
        /// </summary>
        /// <param name="stream"></param>
        public static void sendMsgOfGetUserNameComputer(NetworkStream stream)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(USER_NAME); ;
            stream.Write(buffer, 0, 2);
            stream.Flush();
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="msg"></param>
        private static void sendMsg(NetworkStream stream, String msg)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }
        /// <summary>
        /// 发送8字节的文件长度
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        private static void sendMsgLength(NetworkStream stream, long length)
        {
            byte[] buffer = new byte[16];
            string strLength = length.ToString();
            int i = strLength.Length;
            while (i < 16)
            {
                i++;
                strLength += " ";
            }
            buffer = Encoding.UTF8.GetBytes(strLength);
            stream.Write(buffer, 0, 16);
        }

        /// <summary>
        /// 读取并发送文件内容
        ///     消息长度 + 消息内容的格式
        ///     文件开始标志（2）+文件名长度(8)+文件名+文件长度（8）+文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        private static void sendFile(NetworkStream stream, String fileName)
        {
            //1,发送文件开始标志
            sendFlag(stream, 1);
            // 发送文件名
            byte[] fileNameBuffer = UTF8Encoding.UTF8.GetBytes(fileName);
            sendMsgLength(stream, fileNameBuffer.Length);
            stream.Write(fileNameBuffer, 0, fileNameBuffer.Length);
            // sendMsg(stream, fileNameBuffer);
            // 发送文件长度
            FileInfo fileInfo = new FileInfo(fileName);
            long fileLength = fileInfo.Length;
            sendMsgLength(stream, fileLength);
            // 发送文件内容
            using (FileStream fStream = new FileStream(fileName, FileMode.Open))
            {
                if (fStream.Length > 200000)
                {
                    byte[] fileData = new byte[1024 * 100];
                    int count = 0;
                    while ((count = fStream.Read(fileData, 0, fileData.Length)) != 0)
                    {
                        //  Console.WriteLine("count-->"+count);
                        stream.Write(fileData, 0, count);
                    }
                    Console.WriteLine("count-->" + count);
                }
                else
                {
                    byte[] fileData = new byte[fStream.Length];
                    fStream.Read(fileData, 0, fileData.Length);
                    stream.Write(fileData, 0, fileData.Length);
                }

            }
            //2，发送文件结束标志
            //   sendFlag(stream, 2);
        }


        /// <summary>
        /// 读取并发送文件内容
        ///     消息长度 + 消息内容的格式
        ///     文件开始标志（2）+文件名长度(8)+文件名+文件长度（8）+文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        private static void sendFileAndSaveRelative(NetworkStream stream, String fileName)
        {
            //1,发送文件开始标志
            sendFlag(stream, 1);
            // 发送文件名
            byte[] fileNameBuffer = UTF8Encoding.UTF8.GetBytes(getNewFilePath(fileName));
            sendMsgLength(stream, fileNameBuffer.Length);
            stream.Write(fileNameBuffer, 0, fileNameBuffer.Length);
            // sendMsg(stream, fileNameBuffer);
            // 发送文件长度
            FileInfo fileInfo = new FileInfo(fileName);
            long fileLength = fileInfo.Length;
            sendMsgLength(stream, fileLength);
            // 发送文件内容
            using (FileStream fStream = new FileStream(fileName, FileMode.Open))
            {
                if (fStream.Length > 200000)
                {
                    byte[] fileData = new byte[1024 * 100];
                    int count = 0;
                    while ((count = fStream.Read(fileData, 0, fileData.Length)) != 0)
                    {
                        //  Console.WriteLine("count-->"+count);
                        stream.Write(fileData, 0, count);
                    }
                    Console.WriteLine("count-->" + count);
                }
                else
                {
                    byte[] fileData = new byte[fStream.Length];
                    fStream.Read(fileData, 0, fileData.Length);
                    stream.Write(fileData, 0, fileData.Length);
                }

            }
            //2，发送文件结束标志
            //   sendFlag(stream, 2);
        }

        /// <summary>
        /// 读取并发送文件内容
        ///     消息长度 + 消息内容的格式
        ///     文件开始标志（2）+文件名长度(8)+文件名+文件长度（8）+文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <param name="sendOneComplete">文件发送完毕时，需要执行的操作</param>
        private static void sendFile(NetworkStream stream, SendOneComplete sendOneComplete, String fileName)
        {
            //1,发送文件开始标志
            sendFlag(stream, 1);
            // 发送文件名
            byte[] fileNameBuffer = UTF8Encoding.UTF8.GetBytes(fileName);
            sendMsgLength(stream, fileNameBuffer.Length);
            stream.Write(fileNameBuffer, 0, fileNameBuffer.Length);
            // sendMsg(stream, fileNameBuffer);
            // 发送文件长度
            FileInfo fileInfo = new FileInfo(fileName);
            long fileLength = fileInfo.Length;
            sendMsgLength(stream, fileLength);
            // 发送文件内容
            using (FileStream fStream = new FileStream(fileName, FileMode.Open))
            {
                byte[] fileData = new byte[fStream.Length];
                fStream.Read(fileData, 0, fileData.Length);
                stream.Write(fileData, 0, fileData.Length);
            }
            sendOneComplete(fileName);
            //2，发送文件结束标志
            //   sendFlag(stream, 2);
        }
        /// <summary>
        /// 批量发送指定文件夹下面的所有文件
        /// </summary>
        public static void sendFiles(NetworkStream stream, string directoryName)
        {
            sendManyFile(stream, directoryName);
            sendFlag(stream, 3);//发送流结束标志
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="directoryName"></param>
        /// <param name="isLast">指示是否为最后一个数据，如果是就在发送结束之后关闭流</param>
        public static void sendFiles(NetworkStream stream, string directoryName, bool isLast)
        {
            sendManyFile(stream, directoryName);
            if (isLast)
                sendFlag(stream, 3);//发送流结束标志
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="directoryName"></param>
        /// <param name="sendOneComplete">文件传输完毕需要执行的操作</param>
        /// <param name="isLast">指示是否为最后一个数据，如果是就在发送结束之后关闭流</param>
        public static void sendFiles(NetworkStream stream, SendOneComplete sendOneComplete, string directoryName, bool isLast)
        {
            sendManyFile(stream, sendOneComplete, directoryName);
            if (isLast)
                sendFlag(stream, 3);//发送流结束标志
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataDir">保存数据的文件夹，将原始文件路径中该文件夹之前的路径都截断</param>
        /// <param name="stream"></param>
        /// <param name="directoryName"></param>
        /// <param name="sendOneComplete">文件传输完毕需要执行的操作</param>
        /// <param name="isLast">指示是否为最后一个数据，如果是就在发送结束之后关闭流</param>
        public static void sendFiles(String dataDir, NetworkStream stream, SendOneComplete sendOneComplete, string directoryName, bool isLast)
        {
            sendManyFile(dataDir,stream, sendOneComplete, directoryName);
            if (isLast)
                sendFlag(stream, 3);//发送流结束标志
        }
        /// <summary>
        /// 取得相对于当前应用程序的路径
        /// </summary>
        /// <param name="initialFileName"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string getNewFilePath(String initialFileName)
        {
          ///  int index = initialFileName.IndexOf(prefix);
           

            return initialFileName.Replace(AppDomain.CurrentDomain.BaseDirectory,"");
        }

        /// <summary>
        /// 传输多个文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="directoryName"></param>
        public static void sendManyFile(String dataDir, NetworkStream stream, SendOneComplete sendOneComplete, string directoryName)
        {
            // Console.WriteLine(directoryName+"的存在性："+Directory.Exists(directoryName));
            if (!Directory.Exists(directoryName)) return;
            string[] subdirectoryEntries = Directory.GetDirectories(directoryName);
            //遍历子目录
            foreach (string subDir in subdirectoryEntries)
            {
                sendManyFile(dataDir, stream, sendOneComplete, subDir);
            }
            string[] files = Directory.GetFiles(directoryName);
            Console.WriteLine(files.Length);
            foreach (string filePath in files)
            {
                send(stream, getNewFilePath(filePath));
                //发送完一个文件休息5秒钟，好让服务器端进行接收
                //Thread.Sleep(5000);
                //接收服务器端的信息，即文件接收完毕的信息
                while (true)
                {
                    if (!stream.DataAvailable)
                    {
                        Thread.Sleep(100);
                    }
                    else
                    {
                        string recv_flag = getFlag(stream);
                        Console.WriteLine("客户端接收数据" + recv_flag);
                        if (FILE_RECV.Equals(recv_flag))
                        {
                            break;//服务器接收完毕，则继续发送下一个文件
                        }
                    }
                }
                sendOneComplete(filePath);
            }
        }


        /// <summary>
        /// 传输多个文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="directoryName"></param>
        public static void sendManyFile(NetworkStream stream, SendOneComplete sendOneComplete, string directoryName)
        {
            // Console.WriteLine(directoryName+"的存在性："+Directory.Exists(directoryName));
            if (!Directory.Exists(directoryName)) return;
            string[] subdirectoryEntries = Directory.GetDirectories(directoryName);
            //遍历子目录
            foreach (string subDir in subdirectoryEntries)
            {
                sendManyFile(stream, sendOneComplete, subDir);
            }
            string[] files = Directory.GetFiles(directoryName);
            Console.WriteLine(files.Length);
            foreach (string filePath in files)
            {
               
                send(stream, filePath);
                Console.WriteLine("发送文件："+filePath);
                //发送完一个文件休息5秒钟，好让服务器端进行接收
                Thread.Sleep(100);
                //接收服务器端的信息，即文件接收完毕的信息
                while (true)
                {
                    if (!stream.DataAvailable)
                    {
                        Thread.Sleep(100);
                    }
                    else
                    {
                        string recv_flag = getFlag(stream);
                        Console.WriteLine("客户端接收数据" + recv_flag);
                        if (FILE_RECV.Equals(recv_flag))
                        {
                            break;//服务器接收完毕，则继续发送下一个文件
                        }
                    }
                }
                sendOneComplete(filePath);
            }
        }

        /// <summary>
        /// 传输多个文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="directoryName"></param>
        public static void sendManyFile(NetworkStream stream, string directoryName)
        {
            // Console.WriteLine(directoryName+"的存在性："+Directory.Exists(directoryName));
            if (!Directory.Exists(directoryName)) return;
            string[] subdirectoryEntries = Directory.GetDirectories(directoryName);
            //遍历子目录
            foreach (string subDir in subdirectoryEntries)
            {
                sendManyFile(stream, subDir);
            }
            string[] files = Directory.GetFiles(directoryName);
            Console.WriteLine(files.Length);
            foreach (string filePath in files)
            {
                send(stream, filePath);
                //发送完一个文件休息5秒钟，好让服务器端进行接收
                Thread.Sleep(5000);
            }
        }

    }
}
