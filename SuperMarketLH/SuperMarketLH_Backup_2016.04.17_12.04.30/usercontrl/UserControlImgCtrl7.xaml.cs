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
    /// UserControlImgCtrl7.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlImgCtrl7 : UserControl
    {
      
          public UserControlImgCtrl7()
        {
            InitializeComponent();
            transitioniItem = new TransitionItem();
        }
        private List<ImageObject> allImages;
        private string[] imgPathes;

        public string[] ImgPathes
        {
            get { return imgPathes; }
            set { imgPathes = value; init(); }
        }
        private int currentSelectIndex = -1;
        private DispatcherTimer timer;//定时器，每隔一段时间切换图片
        private TransitionItem transitioniItem = null;
        public UserControlImgCtrl7(string[] imgPathes)
        {
            InitializeComponent();
            this.imgPathes = imgPathes;
            transitioniItem = new TransitionItem();
           
        }
        

        void init()
        {
            allImages = new List<ImageObject>();
            if (this.ImgPathes == null) return;
            for (int i = 0; i < this.ImgPathes.Length; i++)
            {
                allImages.Add(new ImageObject()
                {
                    ImgPath = this.ImgPathes[i]
                });
            }
           // this.surfaceListBoxContent.ItemsSource = this.allImages;
            if (this.allImages.Count > 0)
            {
                this.currentSelectIndex = 0;
               // this.surfaceListBoxContent.SelectedIndex = 0;
            }

            if (timer != null)
            {
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
            if (this.ImgPathes != null && this.ImgPathes.Length > 0)
            {
                currentSelectIndex = ++currentSelectIndex % this.ImgPathes.Length;
                transitioniItem.Img = new ImageObject() { ImgPath = ImgPathes[this.currentSelectIndex] };
                /// transitioniItem.ItemTransition = TransitioinUtil.getNewTransition();
                transitioniItem.ItemTransition = TransitioinUtil.getFadeTransition();
                this.transitionC_img.DataContext = transitioniItem;
                loadImgCounter(currentSelectIndex);
            }
        }

        private void getTrangsitioniItem(int index)
        {
            if (this.ImgPathes != null && this.ImgPathes.Length > 0 && index<this.ImgPathes.Length)
            {

                transitioniItem.Img = new ImageObject() { ImgPath = ImgPathes[index] };
                transitioniItem.ItemTransition = TransitioinUtil.getNewTransition();
                this.transitionC_img.DataContext = transitioniItem;
                loadImgCounter(currentSelectIndex);
            }
        }

        private ImageObject getNext()
        {
            return this.allImages.ElementAt(++this.currentSelectIndex % this.allImages.Count);
        }

        private ImageObject getPre()
        {
            return this.allImages.ElementAt((--this.currentSelectIndex + this.allImages.Count) % this.allImages.Count);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
          //  init();
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

        /// <summary>
        /// 加载图片计数
        /// </summary>
        private void loadImgCounter(int selectIndex)
        {
            stackPanel_imgCounter.Children.Clear();
            for (int i = 0; i < this.ImgPathes.Length; i++)
            {
                stackPanel_imgCounter.Children.Add(i == selectIndex ? getSelectEllipse() : getUnSelectEllipse());
            }
        }

        /// <summary>
        /// 取得上一页
        /// </summary>
        private void getPreItem()
        {
            if (this.ImgPathes != null && this.ImgPathes.Length > 0)
            {
                currentSelectIndex = (--currentSelectIndex + this.ImgPathes.Length) % this.ImgPathes.Length;
                transitioniItem.Img = new ImageObject() { ImgPath = ImgPathes[this.currentSelectIndex] };
                transitioniItem.ItemTransition = TransitioinUtil.getNewTransition();
                this.transitionC_img.DataContext = transitioniItem;
                loadImgCounter(currentSelectIndex);
            }
            ClosedUtil.isAnyBodyTouched = true;
            //init();
        }

        /// <summary>
        /// 取得下一页
        /// </summary>
        private void getNextItem()
        {
            getTrangsitioniItem();
            // init();
        }

        //初始鼠标位置
        private double startX = 0;
        //结尾鼠标位置
        private double endX = 0;
        private void compareX(double startX, double endX)
        {
            if ((endX - startX) > 200)
            {
                getNextItem();
            }
            else if ((startX - endX) > 200)
            {
                getPreItem();
            }
        }

        private void transitionC_img_MouseMove_1(object sender, MouseEventArgs e)
        {
            endX = e.GetPosition(transitionC_img).X;
        }

        private void transitionC_img_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            compareX(startX, endX);
        }

        private void transitionC_img_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            startX = e.GetPosition(transitionC_img).X;
        }

        private void transitionC_img_TouchMove_1(object sender, TouchEventArgs e)
        {
            endX = e.GetTouchPoint(transitionC_img).Position.X;
        }

        private void transitionC_img_TouchUp_1(object sender, TouchEventArgs e)
        {
            compareX(startX, endX);
        }

        private void transitionC_img_TouchDown_1(object sender, TouchEventArgs e)
        {
            startX = e.GetTouchPoint(transitionC_img).Position.X;
        }
    }
}
