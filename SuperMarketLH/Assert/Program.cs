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
            // string appName = @"E:\项目\MallGuide\SuperMarketLH\SuperMarketLH\bin\Debug";
            string appName = AppDomain.CurrentDomain.BaseDirectory;
            //Thread.Sleep(20000);
         //  ;
            //Thread t1 = new Thread(new ThreadStart(()=> { }));
            //t1.IsBackground = true;
            //t1.Start();

            Thread t2 = new Thread(new ThreadStart(() => {
                while (true)
                {
                    Thread.Sleep(1000);
                    string appName1 = AppDomain.CurrentDomain.BaseDirectory;
                    //throw new NotImplementedException();
                    //检查当前目录下是否有指定文件，有，则表示需要重启
                    string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "updateConfig", "updateComplete");

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        Process processLH = new Process();
                        processLH.StartInfo.FileName = System.IO.Path.Combine(appName1, "SuperMarketLH.exe");
                        processLH.Start();
                    }
                }
            }));
            t2.IsBackground = true;
            t2.Start();

            Process process = new Process();
            process.StartInfo.FileName = System.IO.Path.Combine(appName, "SuperMarketLH.exe");
            process.Start();

            SocketHelper.receMsg(SocketHelper.createServer());

        }

        
       
    }
}
