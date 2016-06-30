using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformDev
{
    public class Util
    {
        /// <summary>
        /// 统计目录之下的文件数目
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static int countFileNum(string dir) {
            int count = 0;
            //递归遍历目录
            List<string> dirFullPathLocal = new List<string>();
            dirFullPathLocal.Add(dir);
            string currentLocalDir = "";
            while (dirFullPathLocal.Count > 0)
            {
                currentLocalDir = dirFullPathLocal.ElementAt(0);
                dirFullPathLocal.RemoveAt(0);
                dirFullPathLocal.AddRange(Directory.GetDirectories(currentLocalDir));
                count += Directory.GetFiles(currentLocalDir).Length;              
            }
            return count;
        }
        /// <summary>
        /// 取得data下面所有的文件
        /// </summary>
        /// <returns></returns>
        public static List<String> getAllFiles(string dir)
        {
            List<String> files = new List<string>();
            List<string> dirFullPathLocal = new List<string>();
            dirFullPathLocal.Add(dir);
            string currentLocalDir = "";
            while (dirFullPathLocal.Count > 0)
            {
                currentLocalDir = dirFullPathLocal.ElementAt(0);
                dirFullPathLocal.RemoveAt(0);
                dirFullPathLocal.AddRange(Directory.GetDirectories(currentLocalDir));
                files.AddRange(Directory.GetFiles(currentLocalDir));
            }
            return files;
        }
    }
}
