using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperMarketLH.util
{
    public class WinUtil
    {



        public const int PAGE_SIZE = 21;   //每页店铺展示数目
        /// <summary>
        /// 要跳转的页面索引
        /// </summary>
        /// 
        public const int PAGE_NULL = -1;

        public const int PAGE_HOME_INDEX = 0; //首页

        public const int PAGE_INTRODUCTION_INDEX = 1;//商场简介
        public const int PAGE_NEWSQURE = 11;//新联华广场
        public const int PAGE_HOTEL_INDEX = 12;//酒店
        public const int PAGE_SHOPPING_CENTER_INDEX = 13;//购物中心
        public const int PAGE_GLOBAL_PROJECT = 14;//全国项目简介


        public const int PAGE_FIRE_EMERGENCY = 5;//消防通道
        public const int PAGE_MAGAZINE = 6;//电子杂志
        public const int PAGE_AD = 7;//招商广告
        public const int PAGE_EMPLOYEE = 8;//招聘信息
        public const int PAGE_SURROUND = 9;//周边
        public const int PAGE_MEMBER_ACTIVITIES = 41;//会员活动
        public const int PAGE_NORMAL_ACTIVITIES = 42;//非会员活动
        public const int PAGE_SHOP = 3;//商铺信息
        public const int PAGE_FLOOR = 2;//楼层指引

        public const int PAGE_QUESTION = 10;//问卷调查



        public const int GRID_BTN_CONTAINER_ALL_INDEX = 0;//全部的按钮
        public const int GRID_BTN_MALL_INTRODUCTION_INDEX = 1;//商场简介
        public const int GRID_BTN_FLOOR_INDEX = 2;//楼层指引
        public const int GRID_BTN_SHOP_INDEX = 3;//店铺信息
        public const int GRID_BTN_ACTIVITIES_INDEX = 4;//活动信息
        public const int GRID_BTN_FIRE_EMERGENCY_INDEX = 5;//消防通道
        public const int GRID_BTN_MAGAZINE_INDEX = 6;//电子杂志
        public const int GRID_BTN_AD_INDEX = 7;//招商广告
        public const int GRID_BTN_EMPLOOYEE_INDEX = 8;//招聘信息
        public const int GRID_BTN_SURROUND_INFO_INDEX = 9;//周边信息
        public const int GRID_BTN_QUESTION_INDEX = 10;


        //商铺导航
        public const int FRAME_SHOP_CAN_YIN = 0;
        public const int FRAME_SHOP_SHOPPING = 1;
        public const int FRAME_SHOP_CHILDREN = 2;
        public const int FRAME_SHOP_SUPER_MARKET_DARUNFA = 3;
        public const int FRAME_SHOP_SUNINGYIGOU = 4;
        public const int FRAME_SHOP_DABAIJINGSHIJIEERTONGLEYUAN = 5;
        public const int FRAME_SHOP_KTV = 6;
        public const int FRAME_SHOP_JIANSHENG = 7;
        public const int FRAME_SHOP_MOVIE = 8;
        public const int FRAME_SHOP_ALL = -1;//显示全部的商铺




        /// <summary>
        /// 返回绝对地址
        /// </summary>
        /// <param name="relativePathes"></param>
        /// <returns></returns>
        public static List<String> getPathesFull(List<string> relativePathes)
        {
            List<string> pathes = new List<String>();
            if (relativePathes != null)
            {
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
        public static string getMoviePathFull(string relativePath)
        {
            if (relativePath == null || relativePath.Equals(""))
            {
                return "";
            }
            string str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            if (!File.Exists(str))
            {
                return "";
            }
            return str;
        }

        static bool processIsRunning(string process)
        {
            //return (System.Diagnostics.Process.GetProcessesByName(process).Length != 0);
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(process);

            return (processes.Length != 0);
        }
        /// <summary>
        /// 启动触摸键盘
        /// </summary>
        public static void startTabTip()
        {
            if (processIsRunning("TabTip") == true)
            {
                //System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("TabTip");
                //foreach (System.Diagnostics.Process proc in processes)
                //{
                //    proc.Kill();
                //}
                //前端显示
            }
            else
            {
                try {
                    //Process process = new Process();
                    //process.StartInfo.FileName = "cmd.exe";   //调用系统的CMD
                    //process.StartInfo.UseShellExecute = false;
                    //process.StartInfo.RedirectStandardInput = true;  //输入重定向
                    //process.StartInfo.RedirectStandardOutput = true;  //输出重定向
                    //process.StartInfo.CreateNoWindow = true;   //不显示CMD窗口
                    //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //process.StartInfo.WorkingDirectory =@"C:\Program Files\Common Files\microsoft shared\ink";    //CMD命令行运行的初始目录，根据需要指定或不指定
                    //process.Start();
                    //process.StandardInput.WriteLine("TabTip.exe");  //你要执行的命令
                    //process.StandardInput.WriteLine("exit");
                    //process.WaitForExit();
                    Process.Start(@"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe");
                   //
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
              
            }
            Process.Start(@"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe");
        }
        /// <summary>
        /// 关闭触摸软键盘
        /// </summary>
        public static void killTabTip()
        {
            if (processIsRunning("TabTip") == true)
            {
                System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("TabTip");
                foreach (System.Diagnostics.Process proc in processes)
                {
                    proc.Kill();
                }
            }
        }
    }
}
