

using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using ResourceManagementService.helper;
using SerialNum;
using Socket;
using SuperMarketLH.page;
using SuperMarketLH.page.activity;
using SuperMarketLH.page.floor;
using SuperMarketLH.page.mall;
using SuperMarketLH.page.other;
using SuperMarketLH.page.shop;
using SuperMarketLH.reg;
using SuperMarketLH.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SuperMarketLH
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (this.SuperMarketLH == null)
            {
                this.SuperMarketLH = createSoft();
            }
            RegisterTableMsg.registSoft = SuperMarketLH;
            //初始化,如果软件信息在注册表中没有，则写入信息
            CodeUtil.iniSoft();
        }

       

        private Soft SuperMarketLH { get; set; }
        private DispatcherTimer regTimer = null;
        private WinRegist regWin { get; set; }
        private DispatcherTimer goToIndexTimer = null;

        private bool isDefaultBG = true;

        private int pageIndex;
        //private int currentGridVisibleIndex = 0;

        private Server dataTransferServer;

        private DispatcherTimer restartTimer = null;

        private void init()
        {
            goToIndexTimer = new DispatcherTimer();
            goToIndexTimer.Interval = TimeSpan.FromMinutes(5);
            goToIndexTimer.Tick += goToIndexTimer_Tick;
            goToIndexTimer.IsEnabled = true;

            restartTimer = new DispatcherTimer();
            restartTimer.Interval = TimeSpan.FromMilliseconds(500);
            restartTimer.Tick += RestartTimer_Tick;
            restartTimer.IsEnabled = true;

            //回到首页
            //frameForIndex.Navigate(new PageIndex(this));
            goToEnter();
            
            //startUpdateServer();

            //  dataTransferServer = new Server();
            // dataTransferServer.startServer();
            //dataTransferServer.isRestart = true;
            //WinUtil.startDemonWatch();
           // startAssert();
        }
        
        /// <summary>
        /// 结束当前进程，
        /// 由守护进程重新启动应用程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestartTimer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //检查当前目录下是否有指定文件，有，则表示需要重启
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"sConfig","updateStart");

            if (File.Exists(path))
            {
                File.Delete(path);
                Application.Current.Shutdown();
            }

           
        }



        /// <summary>
        /// 开启更新服务器
        /// </summary>
        //private void startUpdateServer()
        //{
        //    //string dataFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data\data.zip");
        //    //string testFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data\1.zip");
        //    //string unzipDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data");
        //    //SocketHelper.recvData(SocketHelper.createServer(), @"data\data.zip", @"data\1.zip", @"data");
        //   // SocketHelper.receMsg(SocketHelper.createServer());


        //}

        /// <summary>
        /// 跳转到首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void goToIndexTimer_Tick(object sender, EventArgs e)
        {
            if (!ClosedUtil.isAnyBodyTouched)
            {
                //回到首页
                //frame.Navigate(new PageIndex(this));
                gotoindexPage();
                ClosedUtil.isAnyBodyTouched = true;
            }
            else
            {
                ClosedUtil.isAnyBodyTouched = false;
            }
        }

        private void checkRegist()
        {
            //注册检查
            CodeUtil.checkRegisterSuc(showRegWin, expired, inTrial, regErr);

        }
        private Soft createSoft()
        {
            SuperMarketLH = new Soft();
            SuperMarketLH.SoftName = "SuperMarketLH";
            SuperMarketLH.SoftVersion = "Beta1.0";
            SuperMarketLH.TryDays = 30;
            SuperMarketLH.KeySalt = 10;
            return SuperMarketLH;
        }

        /// <summary>
        /// 试用期满
        /// </summary>
        private void expired()
        {
            WinForReg regNumWin = new WinForReg();
            regNumWin.RegSoft = SuperMarketLH;
            regNumWin.ShowDialog();

        }
        /// <summary>
        /// 还在试用期
        /// </summary>
        private void inTrial(int r)
        {
            //启动定时器，定期显示注册窗口
            //显示注册窗口
            regTimer = new DispatcherTimer();
            regTimer.Interval = TimeSpan.FromMinutes(5);
            //  regTimer.Interval = TimeSpan.FromMinutes(10);
            regTimer.Tick += timer_Tick;
            regTimer.IsEnabled = true;
        }
        void timer_Tick(object sender, EventArgs e)
        {
            int trialDays = CodeUtil.getTrailDays();
            if (trialDays < 0)
            {
                expired();
            }
            else
            {
                showRegWin();
            }
        }
        /// <summary>
        /// 显示注册窗口
        /// </summary>
        private void showRegWin()
        {
            this.regWin = new WinRegist();
            if (this.SuperMarketLH == null)
            {
                this.SuperMarketLH = createSoft();
            }
            this.regWin.soft = this.SuperMarketLH;
            if (!ClosedUtil.regWinIsOpen)
            {
                ClosedUtil.regWinIsOpen = true;
                this.regWin.ShowDialog();
            }

        }
        private void regErr()
        {
            if (MessageBox.Show("注册信息有误!") == MessageBoxResult.OK)
            {
                Process.GetCurrentProcess().Kill();//试用期结束，结束程序
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //进行注册检查
            checkRegist();
            init();
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            clearResource();
            System.Environment.Exit(0);
        }

        private void clearResource()
        {
            if (this.regTimer != null)
            {
                this.regTimer.IsEnabled = false;
                this.regTimer = null;
            }
            if (this.restartTimer != null)
            {
                this.restartTimer.IsEnabled = false;
                this.restartTimer = null;
            }
            // dataTransferServer.stopServer();
        }

        //private void btn_introducePage_Click(object sender, RoutedEventArgs e)
        //{
        //    gridHidden(WinUtil.GRID_BTN_CONTAINER_ALL_INDEX,WinUtil.GRID_BTN_MALL_INTRODUCTION_INDEX);
        //    frameNavigate(WinUtil.PAGE_INTRODUCTION_INDEX);
        //}



        /// <summary>
        /// home page btn click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region
        //private void btn_homePage_Click(object sender, RoutedEventArgs e)
        //{
        //  //  gridHidden(WinUtil.GRID_BTN_MALL_INTRODUCTION_INDEX, WinUtil.GRID_BTN_CONTAINER_ALL_INDEX);
        //    frameNavigate(WinUtil.PAGE_HOME_INDEX);
        //}
        //private void btn_homePage6_Click(object sender, RoutedEventArgs e)
        //{

        //   // gridHidden(WinUtil.GRID_BTN_FIRE_EMERGENCY_INDEX,WinUtil.GRID_BTN_CONTAINER_ALL_INDEX);
        //    frameNavigate(WinUtil.PAGE_HOME_INDEX);
        //}
        //private void btn_homePage4_Click(object sender, RoutedEventArgs e)
        //{
        //   // gridHidden(WinUtil.GRID_BTN_ACTIVITIES_INDEX, WinUtil.GRID_BTN_CONTAINER_ALL_INDEX);
        //    frameNavigate(WinUtil.PAGE_HOME_INDEX);
        //}
        #endregion

        //private void btn_hotelPage_Click(object sender, RoutedEventArgs e)
        //{

        //    frameNavigate(WinUtil.PAGE_HOTEL_INDEX);

        //}

        /// <summary>
        /// 控制表格的隐藏和显示
        /// </summary>
        /// <param name="hiddenIndex"></param>
        /// <param name="showIndex"></param>
        //private void gridHidden(int hiddenIndex,int showIndex) {
        //    Grid hiddenGrid = FindName("grid_btn" + hiddenIndex) as Grid;
        //    Grid showGrid = FindName("grid_btn" + showIndex) as Grid;

        //    if (this.currentGridVisibleIndex != showIndex) {
        //        hiddenGrid.Visibility = Visibility.Hidden;
        //        showGrid.Visibility = Visibility.Visible;
        //        this.currentGridVisibleIndex = showIndex;
        //    }
        //}

        /// <summary>
        /// 进行页面之间的导航
        /// </summary>
        /// <param name="index"></param>
        private void frameNavigate(int index)
        {
            Page page = null;
           
            switch (index)
            {
                //  case WinUtil.PAGE_NULL: page = null; break;
                case WinUtil.PAGE_HOME_INDEX: page = new PageIndex(this); break;//首页广告
                case WinUtil.PAGE_INTRODUCTION_INDEX: page = new PageMall(); break;
                case WinUtil.PAGE_FLOOR: page = new PageFloorBaseInfo(this); break;
                case WinUtil.PAGE_SHOP: page = new PageShop(this); break;
                //case WinUtil.PAGE_HOTEL_INDEX: page = new PageHotel(this); break;
                //case WinUtil.PAGE_SHOPPING_INDEX: page = new PageShoppingMall(this); break;
                case WinUtil.PAGE_FIRE_EMERGENCY: page = new PageFireEmergency(this); break;
                case WinUtil.PAGE_MAGAZINE: page = new PageMagzaine(this); break;
                case WinUtil.PAGE_AD: page = new PageAD(this); break;
                case WinUtil.PAGE_EMPLOYEE: page = new PageEmployee(this); break;
                case WinUtil.PAGE_SURROUND: page = new PageSurroundInfo(this); break;
                case WinUtil.PAGE_MEMBER_ACTIVITIES: page = new PageMemberShopActivities(this); break;
                //case WinUtil.PAGE_NORMAL_ACTIVITIES: page = new PageNormalAcitvities(this); break;
                //case WinUtil.PAGE_SHOP: page = new PageShop(this); break;
                //case WinUtil.PAGE_FLOOR: page = new PageFloorBaseInfo(); break;
                case WinUtil.PAGE_QUESTION: page = new PageQuestionnaire(); break;
            }
          //  MessageBox.Show(page.GetType().ToString()+"");
            if (frame.Content == null || !page.GetType().Equals(frame.Content.GetType()))
            {
                this.frame.Navigate(page);
            }
           // MessageBox.Show(frame.Content.GetType().ToString());
            //if (this.pageIndex != index)
            //{
               
            //    this.pageIndex = index;
            //}
            ClosedUtil.isAnyBodyTouched = true;
            if (isDefaultBG)
            {
                changeWinBG("resource/images/bg/bg2.png");
                isDefaultBG = false;
            }
            if (this.frame.CanGoBack) { 
                this.frame.RemoveBackEntry();
            }
        }

        private void btn_mall_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_INTRODUCTION_INDEX);
        }

        private void btn_floor_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_FLOOR);
        }

        private void btn_shop_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_SHOP);
        }

        private void btn_active_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_MEMBER_ACTIVITIES);
        }

        private void btn_question_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_QUESTION);
        }

        private void btn_fire_engine_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_FIRE_EMERGENCY);
        }

        private void btn_magazine_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_MAGAZINE);
        }

        private void btn_investment_ad_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_AD);
        }

        private void btn_recruit_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_EMPLOYEE);
        }

        private void btn_preipheral_Click(object sender, RoutedEventArgs e)
        {
            frameNavigate(WinUtil.PAGE_SURROUND);
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
        /// 改变按钮的背景
        /// </summary>
        private void changeWinBG(string imgPath)
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
            this.Background = new ImageBrush(bitmap);
        }





        private void btn_mall_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_mall", "resource/images/navBtn/btn1_press.png");
        }

        private void btn_mall_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_mall", "resource/images/navBtn/btn1.png");
        }

        private void btn_mall_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_mall", "resource/images/navBtn/btn1_press.png");
        }

        private void btn_mall_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_mall", "resource/images/navBtn/btn1.png");
        }

        private void btn_floor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_floor", "resource/images/navBtn/btn2_press.png");
        }

        private void btn_floor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_floor", "resource/images/navBtn/btn2.png");
        }

        private void btn_floor_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_floor", "resource/images/navBtn/btn2_press.png");
        }

        private void btn_floor_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_floor", "resource/images/navBtn/btn2.png");
        }

        private void btn_mall_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_mall", "resource/images/navBtn/btn1_press.png");
        }

        private void btn_mall_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_mall", "resource/images/navBtn/btn1.png");
        }

        private void btn_floor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_floor", "resource/images/navBtn/btn2_press.png");
        }

        private void btn_floor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_floor", "resource/images/navBtn/btn2.png");
        }

        private void btn_shop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_shop", "resource/images/navBtn/btn3_press.png");
        }

        private void btn_shop_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_shop", "resource/images/navBtn/btn3.png");
        }

        private void btn_shop_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_shop", "resource/images/navBtn/btn3.png");
        }

        private void btn_shop_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_shop", "resource/images/navBtn/btn3_press.png");
        }

        private void btn_active_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_active", "resource/images/navBtn/btn4_press.png");
        }

        private void btn_active_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_active", "resource/images/navBtn/btn4.png");
        }

        private void btn_active_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_active", "resource/images/navBtn/btn4_press.png");
        }

        private void btn_active_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_active", "resource/images/navBtn/btn4.png");
        }

        private void btn_question_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_question", "resource/images/navBtn/btn5_press.png");
        }

        private void btn_question_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_question", "resource/images/navBtn/btn5.png");
        }

        private void btn_question_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_question", "resource/images/navBtn/btn5_press.png");
        }

        private void btn_question_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_question", "resource/images/navBtn/btn5.png");
        }

        private void btn_fire_engine_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_fire_engine", "resource/images/navBtn/btn6_press.png");
        }

        private void btn_fire_engine_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_fire_engine", "resource/images/navBtn/btn6.png");
        }

        private void btn_fire_engine_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_fire_engine", "resource/images/navBtn/btn6_press.png");
        }

        private void btn_fire_engine_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_fire_engine", "resource/images/navBtn/btn6.png");
        }

        private void btn_magazine_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_magazine", "resource/images/navBtn/btn7_press.png");
        }

        private void btn_magazine_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_magazine", "resource/images/navBtn/btn7.png");
        }

        private void btn_magazine_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_magazine", "resource/images/navBtn/btn7_press.png");
        }

        private void btn_magazine_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_magazine", "resource/images/navBtn/btn7.png");
        }

        private void btn_investment_ad_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_investment_ad", "resource/images/navBtn/btn8_press.png");
        }

        private void btn_investment_ad_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_investment_ad", "resource/images/navBtn/btn8.png");
        }

        private void btn_investment_ad_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_investment_ad", "resource/images/navBtn/btn8_press.png");
        }

        private void btn_investment_ad_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_investment_ad", "resource/images/navBtn/btn8.png");
        }

        private void btn_recruit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_recruit", "resource/images/navBtn/btn9_press.png");
        }

        private void btn_recruit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_recruit", "resource/images/navBtn/btn9.png");
        }

        private void btn_recruit_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_recruit", "resource/images/navBtn/btn9_press.png");
        }

        private void btn_recruit_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_recruit", "resource/images/navBtn/btn9.png");
        }

        private void btn_preipheral_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_preipheral", "resource/images/navBtn/btn10_press.png");
        }

        private void btn_preipheral_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_preipheral", "resource/images/navBtn/btn10.png");
        }

        private void btn_preipheral_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_preipheral", "resource/images/navBtn/btn10_press.png");
        }

        private void btn_preipheral_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_preipheral", "resource/images/navBtn/btn10.png");
        }

        /// <summary>
        /// 进入初始界面布局
        /// </summary>
        private void gotoindexPage()
        {
            //跳转到首页
            //frame 切换
            frameForIndex.Visibility = Visibility.Visible;
            frame.Visibility = Visibility.Collapsed;
            //按钮隐藏
            grid_menu.Visibility = Visibility.Collapsed;
            btn_enter.Visibility = Visibility.Visible;
            //背景图片切换
            changeWinBG("resource/images/bg/bg.png");
            isDefaultBG = true;
        }

        private void btn_enter_Click(object sender, RoutedEventArgs e)
        {
            goToEnter();
        }

        private void goToEnter() {
            //frame 切换
            frameForIndex.Visibility = Visibility.Collapsed;
            frame.Visibility = Visibility.Visible;
            //按钮隐藏
            grid_menu.Visibility = Visibility.Visible;
            btn_enter.Visibility = Visibility.Collapsed;
            //选中第一个按钮
            ClosedUtil.isAnyBodyTouched = true;
            frameNavigate(WinUtil.PAGE_INTRODUCTION_INDEX);
        }

        private void btn_menu_Click(object sender, RoutedEventArgs e)
        {
            gotoindexPage();
            ClosedUtil.isAnyBodyTouched = true;
        }

        public void loadBusy() {
            this.userCtrl_loading.Visibility = System.Windows.Visibility.Visible;
           // busyIndicator.IsBusy = true;
        }
        public void loadBusy(string tips)
        {
            this.userCtrl_loading.Visibility = System.Windows.Visibility.Visible;
            //busyIndicator.BusyContent = tips;
            //busyIndicator.IsBusy = true;
            

        }
        public void loadNotBusy()
        {
            this.userCtrl_loading.Visibility = System.Windows.Visibility.Collapsed;
           // busyIndicator.IsBusy = false;
        }
    }
}
