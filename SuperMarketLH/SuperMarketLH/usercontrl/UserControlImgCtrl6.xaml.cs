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
    /// UserControlImgCtrl6.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlImgCtrl6 : UserControl
    {
        public UserControlImgCtrl6()
        {
            InitializeComponent();
        }
        private string[] _imgs;

        public string[] Imgs
        {
            get { return _imgs; }
            set { _imgs = value; init(); }
        }
       // public string[] Imgs { get; set; }
        private List<ImageObject> allImages;
        
        public UserControlImgCtrl6(string[] imgPathes)
        {
            InitializeComponent();
            this.Imgs = imgPathes;
        }

        void init()
        {
            allImages = new List<ImageObject>();
            for (int i = 0; i < this.Imgs.Length; i++)
            {
                //allImages.Add(new Lazy<ImageObject>(
                //    () => new ImageObject() {
                //        ImgPath = this.Imgs[i]
                //    })
                //);
                allImages.Add( new ImageObject()
                    {
                        ImgPath = this.Imgs[i]
                    }
                );
            }
            this.surfaceListBoxContent.ItemsSource = this.allImages;
        }

      
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }
    }
}
