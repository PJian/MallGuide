using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ResourceManagementService.helper
{
    /// <summary>
    /// 调用好压命令行
    /// </summary>
    public class ZipHelperHaoZip
    {
        /// <summary>
        /// 调用haozip进行压缩
        /// </summary>
        /// <param name="dirName">要压缩的文件目录</param>
        /// <param name="zipFileName">压缩后的文件名</param>
        public static void zipDir(string dirName, string zipFileName)
        {
            string arg = " a -tzip " + zipFileName + " " + dirName + "\\*" + " -r";
            using (Process haozip = new Process())
            {
                haozip.StartInfo.FileName = "HaoZipC.exe";
                haozip.StartInfo.Arguments = arg;
                //隐藏rar本身的窗口
                haozip.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                haozip.Start();
                //等待解压完成
                haozip.WaitForExit();
                haozip.Close();
            }
        }
        /// <summary>
        /// 调用haozip进行解压
        /// </summary>
        /// <param name="zipFileName">压缩文件名</param>
        /// <param name="unzipDir">需要解压到的目录</param>
        public static void unZip(string zipFileName, string unzipDir)
        {
            string arg = " x " + zipFileName + " -o" + unzipDir + " -ao -y" + " -r";
            using (Process haozip = new Process())
            {
                haozip.StartInfo.FileName = "HaoZipC.exe";
                haozip.StartInfo.Arguments = arg;
                //隐藏rar本身的窗口
                haozip.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                haozip.Start();
                //等待解压完成
                haozip.WaitForExit(1000*60);
                haozip.Close();
            }
        }
    }
}
