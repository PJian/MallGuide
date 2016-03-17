using SuperMarketLH.uiEntity;
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
    /// UserControlShowImgs8.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlShowImgs8 : UserControl
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
        public UserControlShowImgs8()
        {
            InitializeComponent();
            transitioniItem = new TransitionItem();
        }
        public UserControlShowImgs8(string[] Imgs)
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
            //timer.Interval = TimeSpan.FromSeconds(5);
            timer.Interval = TimeSpan.FromMinutes(5);
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
                transitioniItem.ItemTransition = TransitioinUtil.getNewTransition();
                this.transitionC_img.DataContext = transitioniItem;
                loadImgCounter(CurrentShowImgIndex);
            }
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
    }
}
