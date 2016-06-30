using PlatformDev;
using ResourceManagementService.helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Assert
{
    class Program
    {
        //应用程序路径
        static void Main(string[] args)
        {
            //监视文件是否有变化

            //  string appName = args[0];
            string appName = @"E:\项目\MallGuide\SuperMarketLH\SuperMarketLH\bin\Debug";
            Environment.SetEnvironmentVariable("SUPERMARKETLH_HOME", appName, EnvironmentVariableTarget.User);
            Console.WriteLine(Environment.GetEnvironmentVariable("SUPERMARKETLH_HOME"),EnvironmentVariableTarget.User);
            Console.WriteLine(1);
            // string appName = AppDomain.CurrentDomain.BaseDirectory;
            //   string tomcatHome = Path.Combine(appName,@"tomcat");
            //   string batHome = Path.Combine(appName, @"tomcat\bin\startup.bat");
            //Thread.Sleep(20000);
            //  ;
            //Thread t1 = new Thread(new ThreadStart(()=> { }));
            //t1.IsBackground = true;
            //t1.Start();

            //Thread t2 = new Thread(new ThreadStart(() =>
            //{
            //    while (true)
            //    {
            //        Thread.Sleep(1000);
            //        string appName1 = AppDomain.CurrentDomain.BaseDirectory;
            //        //throw new NotImplementedException();
            //        //检查当前目录下是否有指定文件，有，则表示需要重启
            //        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "updateConfig", "updateComplete");
            //        string path1 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sConfig", "updateStart");
            //        if (File.Exists(path))
            //        {
            //            File.Delete(path);
            //            File.Delete(path1);
            //            Process processLH = new Process();
            //            processLH.StartInfo.FileName = System.IO.Path.Combine(appName1, "SuperMarketLH.exe");
            //            processLH.Start();
            //        }
            //        else if (!File.Exists(path1))
            //        {
            //            Process[] pp = Process.GetProcessesByName("SuperMarketLH");
            //            if (pp.Length <= 0)
            //            {
            //                Process processLH = new Process();
            //                processLH.StartInfo.FileName = System.IO.Path.Combine(appName1, "SuperMarketLH.exe");
            //                processLH.Start();
            //            }
            //        }
            //        //此时进程应该存在，如果不存在，则重启



            //    }
            //}));
            //t2.IsBackground = true;
            //t2.Start();

            //Process process = new Process();

            //process.StartInfo.FileName = System.IO.Path.Combine(appName, "SuperMarketLH.exe");
            //process.Start();

            // SocketHelper.receMsg(SocketHelper.createServer());

            // Process process_tomcat = new Process();
            // process_tomcat.StartInfo.FileName = batHome;

            //// processStartInfo.Arguments = tomcatHome;  process_tomcat.StartInfo = processStartInfo;
            // bool flag = process_tomcat.Start();
            // RunBat(batHome);

        }

        private static void RunBat(string batPath)
        {
            Process pro = new Process();

            FileInfo file = new FileInfo(batPath);
            pro.StartInfo.WorkingDirectory = file.Directory.FullName;
            pro.StartInfo.FileName = batPath;
            pro.StartInfo.CreateNoWindow = false;
            pro.Start();
            pro.WaitForExit();
        }



    }
}
