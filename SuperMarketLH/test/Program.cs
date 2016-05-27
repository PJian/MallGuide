using PlatformDev.ssh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            SSHClient client = new SSHClient();
            
            client.ScpTo("127.0.0.1","peijian",@"C:\Users\peijian\.ssh\id_rsa",@"E:\json",@"D:\testSSh");
            while (true) ;
        }

        static void client_OnTransferProgress(int curIndex, int totalNum)
        {
            Console.WriteLine("共{0}文件，当前第{1}文件", totalNum, curIndex);
        }
        static void client_OnTransferEnd(int curIndex, int totalNum)
        {
            Console.WriteLine("发送结束");
        }
        static void client_OnTransferStart(int curIndex, int totalNum)
        {
            Console.WriteLine("发送开始");
        }
    }
}
