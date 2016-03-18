using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamir.SharpSsh;
using Tamir.SharpSsh.jsch.examples;

namespace PlatformDev.ssh
{
    public delegate void SSHClientTransferEvent(int curIndex, int totalNum);
    /// <summary>
    /// SSH 协议客户端
    /// </summary>
    public class SSHClient
    {

        public event SSHClientTransferEvent OnTransferEnd;
        public event SSHClientTransferEvent OnTransferProgress;
        public event SSHClientTransferEvent OnTransferStart;

        /// <summary>
        /// 查看指定的主机是否可以联通
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="identifyFile"></param>
        /// <returns></returns>
        public bool isConnected(string host, string user, string identifyFile)
        {
            bool isConnected = false;
            SshConnectionInfo input = new SshConnectionInfo(host, user, "", identifyFile);
            //使用scp协议
            SshTransferProtocolBase sshCp = new Scp(input.Host, input.User);
            try
            {
                if (input.Pass != null && !input.Pass.Equals("")) sshCp.Password = input.Pass;
                else if (input.IdentityFile != null && !input.IdentityFile.Equals("")) sshCp.AddIdentityFile(input.IdentityFile);
                else return false;
                sshCp.Connect();
                isConnected = sshCp.Connected;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally {
                sshCp.Close();
            }
            return isConnected;
        }
        /// <summary>
        /// 使用scp协议
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user">使用那个用户名登陆，如果使用公私密钥验证的话，将会去对应目录下寻找公钥文件</param>
        /// <param name="identifyFile">私钥文件</param>
        /// <param name="sourceDir">要发送的文件目录</param>
        /// <param name="destDir">要保存的文件目录</param>
        public Boolean ScpTo(string host, string user, string identifyFile, string sourceDir, string destDir)
        {
            SshConnectionInfo input = new SshConnectionInfo(host, user, "", identifyFile);
            //使用scp协议
            SshTransferProtocolBase sshCp = new Scp(input.Host, input.User);
            try
            {
                if (input.Pass != null && !input.Pass.Equals("")) sshCp.Password = input.Pass;
                else if (input.IdentityFile != null && !input.IdentityFile.Equals("")) sshCp.AddIdentityFile(input.IdentityFile);
                else return false;
                sshCp.OnTransferStart += new FileTransferEvent(sshCp_OnTransferStart);
                sshCp.OnTransferProgress += new FileTransferEvent(sshCp_OnTransferProgress);
                sshCp.OnTransferEnd += new FileTransferEvent(sshCp_OnTransferEnd);

                Console.Write("Connecting...");
                sshCp.Connect();
                Console.WriteLine("OK");

                int transferFileNum = Util.countFileNum(sourceDir);

                if (OnTransferStart != null) OnTransferStart(1, transferFileNum);

                int currentTransferFileIndex = 1;
                //递归遍历目录
                List<string> dirFullPathLocal = new List<string>();
                dirFullPathLocal.Add(sourceDir);
                string currentLocalDir = "";

                while (dirFullPathLocal.Count > 0)
                {
                    currentLocalDir = dirFullPathLocal.ElementAt(0);
                    dirFullPathLocal.RemoveAt(0);
                    string[] fileNames = Directory.GetFiles(currentLocalDir);
                    dirFullPathLocal.AddRange(Directory.GetDirectories(currentLocalDir));
                    string entFlag = "";
                    if (!sourceDir.EndsWith("\\")) entFlag = "\\";
                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        if (File.Exists(fileNames[i]))
                        {
                            string destName = Path.Combine(destDir, fileNames[i].Replace(sourceDir + entFlag, ""));
                            string destDirName = Path.GetDirectoryName(destName);
                            if (!Directory.Exists(destDirName))
                            {
                                sshCp.Mkdir(destDirName);
                            }
                            if (OnTransferProgress != null) OnTransferProgress(currentTransferFileIndex, transferFileNum);
                            sshCp.Put(fileNames[i], destName);
                            currentTransferFileIndex++;
                        }
                    }
                }
                Console.Write("Disconnecting...");
                sshCp.Close();
                if (OnTransferEnd != null) OnTransferEnd(transferFileNum, transferFileNum);
                Console.WriteLine("OK");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally {
                sshCp.Close();
            }
            return true;
        }

        static ConsoleProgressBar progressBar;
        // static int transferFileNum = 0;
        private static void sshCp_OnTransferStart(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            Console.WriteLine();
            progressBar = new ConsoleProgressBar();
            progressBar.Update(transferredBytes, totalBytes, message);
        }

        private static void sshCp_OnTransferProgress(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            if (progressBar != null)
            {
                progressBar.Update(transferredBytes, totalBytes, message);
            }
        }

        private static void sshCp_OnTransferEnd(string src, string dst, int transferredBytes, int totalBytes, string message)
        {
            if (progressBar != null)
            {
                progressBar.Update(transferredBytes, totalBytes, message);
                progressBar = null;
            }
            // transferFileNum--;
        }


    }
}
