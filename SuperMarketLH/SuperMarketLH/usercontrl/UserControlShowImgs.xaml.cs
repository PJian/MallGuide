using SuperMarketLH.uiEntity;
using SuperMarketLH.util;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace SuperMarketLH.usercontrl
{
    /// <summary>
    /// UserControlShowImgs.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlShowImgs : UserControl
    {
        //private 
        public string[] Imgs { get; set; }
        private string bg { get; set; }
        private DispatcherTimer timer;//定时器，每隔一段时间切换图片
        private TransitionItem transitioniItem = null;
        private int currentShowImgIndex = 0;
        public int CurrentShowImgIndex
        {
            get { return currentShowImgIndex; }
            set { currentShowImgIndex = value; }
        }
        public UserControlShowImgs()
        {
            InitializeComponent();
            transitioniItem = new TransitionItem();
        }
        public UserControlShowImgs(string[] Imgs)
        {
            InitializeComponent();
            this.Imgs = Imgs;
            transitioniItem = new TransitionItem();
        }
        /// <summary>
        /// 图片初始化
        /// </summary>
        private void init() {
            if (timer != null) {
                timer.Stop();
                timer.IsEnabled = false;
            }
            timer = new DispatcherTimer();
            timer.Tick += changeShowImgTimer_Tick;
            timer.Interval = TimeSpan.FromSeconds(30);
            // timer.Interval = TimeSpan.FromMinutes(5);
            timer.IsEnabled = true;
            getTrangsitioniItem();
        }
        void changeShowImgTimer_Tick(object sender, EventArgs e)
        {
            getTrangsitioniItem();
        }
        private void getTrangsitioniItem()
        {
            if (this.Imgs != null && this.Imgs.Length > 0)
            {
                CurrentShowImgIndex = ++CurrentShowImgIndex % this.Imgs.Length;
                transitioniItem.Img = new ImageObject() { ImgPath = Imgs[this.CurrentShowImgIndex] };
                /// transitioniItem.ItemTransition = TransitioinUtil.getNewTransition();
                transitioniItem.ItemTransition = TransitioinUtil.getFadeTransition();
                this.transitionC_img.DataContext = transitioniItem;
                loadImgCounter(CurrentShowImgIndex);
            }
            ClosedUtil.isAnyBodyTouched = true;

        }
        /// <summary>
        /// 取得上一页
        /// </summary>
        private void getPreItem() {
            if (this.Imgs != null && this.Imgs.Length > 0)
            {
                CurrentShowImgIndex = (--CurrentShowImgIndex + this.Imgs.Length) % this.Imgs.Length;
                transitioniItem.Img = new ImageObject() { ImgPath = Imgs[this.CurrentShowImgIndex] };
                transitioniItem.ItemTransition = TransitioinUtil.getNewTransition();
                this.transitionC_img.DataContext = transitioniItem;
                loadImgCounter(CurrentShowImgIndex);
            }
            ClosedUtil.isAnyBodyTouched = true;
            //init();
        }
        /// <summary>
        /// 取得下一页
        /// </summary>
        private void getNextItem() {
            getTrangsitioniItem();
           // init();
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void btn_pre_Click(object sender, RoutedEventArgs e)
        {
            getPreItem();
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            getNextItem();
        }

        /// <summary>
        /// 加载图片计数
        /// </summary>
        private void loadImgCounter(int selectIndex)
        {
            stackPanel_imgCounter.Children.Clear();
            for (int i = 0; i < this.Imgs.Length; i++)
            {
                stackPanel_imgCounter.Children.Add(i == selectIndex ? getSelectEllipse() : getUnSelectEllipse());
            }
        }
        /// <summary>
        /// 构造选中的圆圈效果
        /// </summary>
        /// <returns></returns>
        private Ellipse getUnSelectEllipse()
        {
            return new Ellipse()
            {
                Fill = Brushes.White,
                Width = 20,
                Height = 20,
                Margin = new Thickness(2, 0, 2, 0)
            };
        }
        /// <summary>
        /// 构造未选中的圆圈效果
        /// </summary>
        /// <returns></returns>
        private Ellipse getSelectEllipse()
        {
            return new Ellipse()
            {
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0099ff")),
                Width = 20,
                Height = 20,
                Margin = new Thickness(2, 0, 2, 0)
            };
        }

        //初始鼠标位置
        private double startX = 0;
        //结尾鼠标位置
        private double endX = 0;
        private void compareX(double startX, double endX) {
            if ((endX - startX) > 200)
            {
                getNextItem();
            }
            else if ((startX - endX) > 200)
            {
                getPreItem();
            }
        }

        private void transitionC_img_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            startX = e.GetPosition(transitionC_img).X;
        }

        private void transitionC_img_MouseMove_1(object sender, MouseEventArgs e)
        {
            endX = e.GetPosition(transitionC_img).X;
        }

        private void transitionC_img_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            compareX(startX, endX);
        }

        private void transitionC_img_TouchUp_1(object sender, TouchEventArgs e)
        {
            compareX(startX, endX);
        }

        private void transitionC_img_TouchDown_1(object sender, TouchEventArgs e)
        {
            startX = e.GetTouchPoint(transitionC_img).Position.X;
        }

        private void transitionC_img_TouchMove_1(object sender, TouchEventArgs e)
        {
            endX = e.GetTouchPoint(transitionC_img).Position.X;
        }

    }
}
