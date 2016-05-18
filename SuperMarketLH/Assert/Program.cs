using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            while (true)
            {
                try
                {
                    Process[] processes = System.Diagnostics.Process.GetProcesses();
                    Process process = null;
                    if (processes != null)
                    {
                        for (int i = 0; i < processes.Length; i++)
                        {
                            if (processes[i].ProcessName.Equals("SuperMarketLH"))
                            {
                                process = processes[i];
                                break;
                            }
                        }
                        if (process != null)
                        {
                            process.WaitForExit();
                            process.Close();
                            process.StartInfo.FileName = System.IO.Path.Combine(appName, "SuperMarketLH.exe");
                            process.Start();
                        }
                        else
                        {
                            process = new Process();
                            process.StartInfo.FileName = System.IO.Path.Combine(appName, "SuperMarketLH.exe");
                            process.Start();
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception e) { Console.WriteLine(e.Message); }


            }


        }
    }
}
