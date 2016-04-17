using SuperMarketLH.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperMarketLH.page.mall
{
    /// <summary>
    /// PageMall.xaml 的交互逻辑
    /// </summary>
    public partial class PageMall : Page
    {
        private int pageIndex;

        public PageMall()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 改变按钮的背景
        /// </summary>
        private void changeBtnBG(string buttonName, string imgPath)
        {
            if (!File.Exists(imgPath)) return;
            BinaryReader binReader = new BinaryReader(File.Open(imgPath, FileMode.Open));
            FileInfo fileInfo = new FileInfo(imgPath);
            byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
            binReader.Close();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.EndInit();

            Button btn = FindName(buttonName) as Button;
            if (btn != null)
            {
                btn.Background = new ImageBrush(bitmap);
            }
        }


        /// <summary>
        /// 进行页面之间的导航
        /// </summary>
        /// <param name="index"></param>
        private void frameNavigate(int index)
        {
            Page page = null;
            switch (index)
            {
                case WinUtil.PAGE_NEWSQURE: page = new PageIntroduction(); break;//新联华广场
                case WinUtil.PAGE_HOTEL_INDEX: page = new PageHotel(); break;//酒店
                case WinUtil.PAGE_SHOPPING_CENTER_INDEX:page = new PageShoppingMall() ; break;//购物中心
                case WinUtil.PAGE_GLOBAL_PROJECT: page = new PageGlobalProject(); break;//全国项目简介
               
            }
            if (this.pageIndex != index)
            {
                this.frame.Navigate(page);
                this.pageIndex = index;
            }
            ClosedUtil.isAnyBodyTouched = true;
        }



        private void btn_squre_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_squre", "resource/images/navBtn/btn11_press.png");
        }

        private void btn_squre_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_squre", "resource/images/navBtn/btn11.png");
        }

        private void btn_shoppingCenter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_shoppingCenter", "resource/images/navBtn/btn12_press.png");
        }

        private void btn_shoppingCenter_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_shoppingCenter", "resource/images/navBtn/btn12.png");
        }

        private void btn_Hotel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_Hotel", "resource/images/navBtn/btn13_press.png");
        }

        private void btn_Hotel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_Hotel", "resource/images/navBtn/btn13.png");
        }

        private void btn_globalProject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_globalProject", "resource/images/navBtn/btn14_press.png");
        }

        private void btn_globalProject_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_globalProject", "resource/images/navBtn/btn14.png");
        }

        private void btn_squre_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_squre", "resource/images/navBtn/btn11_press.png");
        }

        private void btn_squre_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_squre", "resource/images/navBtn/btn11.png");
        }

        private void btn_shoppingCenter_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_shoppingCenter", "resource/images/navBtn/btn12_press.png");
        }

        private void btn_shoppingCenter_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_shoppingCenter", "resource/images/navBtn/btn12.png");
        }

        private void btn_Hotel_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_Hotel", "resource/images/navBtn/btn13_press.png");
        }

        private void btn_Hotel_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_Hotel", "resource/images/navBtn/btn13.png");
        }

        private void btn_globalProject_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_globalProject", "resource/images/navBtn/btn14_press.png");
        }

        private void btn_globalProject_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_globalProject", "resource/images/navBtn/btn14.png");
        }

        private void btn_squre_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_NEWSQURE);
        }

        private void btn_shoppingCenter_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_SHOPPING_CENTER_INDEX);
        }

        private void btn_Hotel_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_HOTEL_INDEX);
        }

        private void btn_globalProject_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_GLOBAL_PROJECT);
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_NEWSQURE);
        }
    }
}
