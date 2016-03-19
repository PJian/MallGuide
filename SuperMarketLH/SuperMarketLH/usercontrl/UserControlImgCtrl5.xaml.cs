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
    /// UserControlImgCtrl5.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlImgCtrl5 : UserControl
    {
        public UserControlImgCtrl5()
        {
            InitializeComponent();
        }
        private List<ImageObject> allImages;
        private string[] imgPathes;

        public string[] ImgPathes
        {
            get { return imgPathes; }
            set { imgPathes = value; }
        }
        private int currentSelectIndex = -1;
        private DispatcherTimer timer;//定时器，每隔一段时间切换图片
        private TransitionItem transitioniItem = null;
        public UserControlImgCtrl5(string[] imgPathes)
        {
            InitializeComponent();
            this.imgPathes = imgPathes;
            transitioniItem = new TransitionItem();
           
        }
        private void btn_pre_Click(object sender, RoutedEventArgs e)
        {
            this.surfaceListBoxContent.ScrollIntoView(getPre());
        }

        void init()
        {
            allImages = new List<ImageObject>();
            for (int i = 0; i < this.ImgPathes.Length; i++)
            {
                allImages.Add(new ImageObject()
                {
                    ImgPath = this.ImgPathes[i]
                });
            }
            this.surfaceListBoxContent.ItemsSource = this.allImages;
            if (this.allImages.Count > 0)
            {
                this.currentSelectIndex = 0;
                this.surfaceListBoxContent.SelectedIndex = 0;
            }

            if (timer != null)
            {
                timer.Stop();
                timer.IsEnabled = false;
            }
            timer = new DispatcherTimer();
            timer.Tick += changeShowImgTimer_Tick;
            //timer.Interval = TimeSpan.FromSeconds(5);
            timer.Interval = TimeSpan.FromSeconds(10);
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
                transitioniItem.ItemTransition = TransitioinUtil.getNewTransition();
                this.transitionC_img.DataContext = transitioniItem;
               // loadImgCounter(currentSelectIndex);
            }
        }

        private void getTrangsitioniItem(int index)
        {
            if (this.ImgPathes != null && this.ImgPathes.Length > 0 && index<this.ImgPathes.Length)
            {

                transitioniItem.Img = new ImageObject() { ImgPath = ImgPathes[index] };
                transitioniItem.ItemTransition = TransitioinUtil.getNewTransition();
                this.transitionC_img.DataContext = transitioniItem;
                // loadImgCounter(currentSelectIndex);
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

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            this.surfaceListBoxContent.ScrollIntoView(getNext());
        }

        private void surfaceListBoxContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.currentSelectIndex = this.surfaceListBoxContent.SelectedIndex;
           // this.imgContent.DataContext = this.surfaceListBoxContent.SelectedItem as ImageObject;
            getTrangsitioniItem(this.currentSelectIndex);
        }

      

        private void UserControl_Loaded_2(object sender, RoutedEventArgs e)
        {
            init();
        }
    }
}
