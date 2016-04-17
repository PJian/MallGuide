using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SuperMarketLHS.comm
{
   
    public class WinUtil
    {
        public static bool isDataBaseInitiallized = false;

        public static void chengToSelectBtn(Button btn,Style selectStyle,List<Button> others,Style unSelectStyle) {
            btn.Style = selectStyle;
            for(int i=0;i<others.Count;i++){
                others.ElementAt(i).Style = unSelectStyle;
            }
        }

        /// <summary>
        /// 打开选择对话框让用户选择图片
        /// </summary>
        /// <returns>返回选中的图片</returns>
        public static string chooseImg() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "全部|*.*|JPEG|*.JPEG|PNG|*.PNG|JPG|*.JPG";
            ofd.Title = "请选择图片";
            if ((bool)ofd.ShowDialog()) {
                return ofd.FileName;
            }
            return null;
        }
        /// <summary>
        /// 选择多张图片
        /// </summary>
        /// <returns></returns>
        public static string[] chooseMultiImg()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "全部|*.*|JPEG|*.JPEG|PNG|*.PNG|JPG|*.JPG";
            ofd.Title = "请选择图片";
            ofd.Multiselect = true;
            if ((bool)ofd.ShowDialog())
            {
                return ofd.FileNames;
            }
            return null;
        }

        /// <summary>
        /// 选择影片
        /// </summary>
        /// <returns></returns>
        public static string chooseMovie() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "全部|*.*|MP4|*.MP4|AVI|*.AVI|WMV|*.WMV";
            ofd.Title = "请选择影片";
            if ((bool)ofd.ShowDialog())
            {
                return ofd.FileName;
            }
            return null;
        }
        /// <summary>
        /// 图片的复制
        /// </summary>
        /// <param name="sourceImgs"></param>
        /// <param name="destFloder"></param>
        public static List<string> copyImg(List<String> sourceImgs, String destFloder)
        {
           // destFloderPull = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destFloder);
            List<string> str = new List<string>();
            if (!Directory.Exists(destFloder)) {
                Directory.CreateDirectory(destFloder);
            }
            for (int i = 0; i < sourceImgs.Count; i++) {
                if (sourceImgs.ElementAt(i) == null || "".Equals(sourceImgs.ElementAt(i))) continue;
                string newFileName = getNewFileName(sourceImgs.ElementAt(i));
                str.Add(Path.Combine(destFloder , newFileName));
                if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destFloder, newFileName)))
                {
                    File.Copy(sourceImgs.ElementAt(i), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destFloder, newFileName));
                }
               
            }
            return str;
        }


        /// <summary>
        /// 拷贝一个文件
        /// </summary>
        /// <param name="sourceImgs"></param>
        /// <param name="destFloder"></param>
        /// <returns></returns>
        public static string copyOne(String sourceImgs, String destFloder)
        {
            if (sourceImgs == null || "".Equals(sourceImgs.Trim())) {
                return "";
            }
           // destFloder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destFloder);
            string str = "";
            if (!Directory.Exists(destFloder))
            {
                Directory.CreateDirectory(destFloder);
            }
            string newFileName = getNewFileName(sourceImgs);
            str = Path.Combine(destFloder, newFileName);
            File.Copy(sourceImgs, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destFloder, newFileName));
            return str;
        }
        /// <summary>
        /// 删除指定文件夹下面的文件
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName">不删除的文件</param>
        public static void delFile(String folder,List<string> fileName) {
            folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder);
            if (!Directory.Exists(folder)) {
                return;
            }
            String[] fileNames = Directory.EnumerateFiles(folder).ToArray();
            foreach (string file in fileNames)
	        {
                bool isDelete = true;
                for (int i = 0; i < fileName.Count; i++) {
                    if (Path.GetFileName(fileName.ElementAt(i)).Equals(Path.GetFileName(file))) {
                        isDelete = false;
                        break;
                    }
                }
                if (isDelete) {
                    File.Delete(Path.Combine(folder,file));
                }

	        }
        }

        /// <summary>
        /// 生成新的文件名字
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <returns></returns>
        private static string getNewFileName(string oldFileName) {
            string fileExtension = Path.GetExtension(oldFileName);
            string fileName = DateTime.Now.ToFileTime().ToString();
            return fileName + fileExtension;
        }
        /// <summary>
        /// 返回绝对地址
        /// </summary>
        /// <param name="relativePathes"></param>
        /// <returns></returns>
        public static List<String> getPathesFull(List<string> relativePathes) {
            List<string> pathes = new List<String>();
            if (relativePathes != null) {
                for (int i = 0; i < relativePathes.Count; i++)
                {
                    string temp = getMoviePathFull(relativePathes.ElementAt(i));
                    if (temp != null && !temp.Equals(""))
                    {
                        pathes.Add(temp);
                    }
                }
            }
            return pathes;
        }

        /// <summary>
        /// 根据相对地址，返回绝对地址，如果文件不存在，则返回空字符串
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string getMoviePathFull(string relativePath) {
            if (relativePath == null|| relativePath.Equals("")) {
                return "";
            }
            string str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,relativePath);
            if (!File.Exists(str)) {
                return "";
            }
            return str;
        }
        /// <summary>
        /// 得到相对的地址，指定了前缀路径
        /// </summary>
        /// <param name="pathFull"></param>
        /// <param name="prefixFolder"></param>
        /// <returns></returns>
        public static string getRelativePath(string pathFull, string prefixFolder) {
            if (pathFull == null || pathFull.Trim().Equals("")) {
                return "";
            }
            return Path.Combine(prefixFolder,Path.GetFileName(pathFull));
        }
        public static List<string> getRelativePath(List<string> pathPull,string prefixFolder) {
            List<string> str = new List<string>();
            for (int i = 0; i < pathPull.Count; i++) {
                str.Add(getRelativePath(pathPull.ElementAt(i),prefixFolder));
            }
            return str;
        }


        /// <summary>
        /// 电影拷贝
        /// </summary>
        /// <param name="moviePath"></param>
        /// <param name="folder"></param>
        public static void movieCopy(string moviePath, string folder, RunWorkerCompletedEventHandler runWorkerCompleted_Handler)
        {

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += DoWork_Handler;
            bw.RunWorkerAsync(new string[] { moviePath, folder });
            bw.RunWorkerCompleted += runWorkerCompleted_Handler;
        }
       
        private static void DoWork_Handler(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            string[] a = args.Argument as string[];
            args.Result = WinUtil.copyOne(a[0], a[1]);
        }
    }
}
