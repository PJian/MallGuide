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
    /// UserControlImgCtrl2.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlImgCtrl2 : UserControl
    {
        private  List<ImageObject> allImages;
        private string[] imgPathes;
        private int currentSelectIndex = -1;
       
        public UserControlImgCtrl2(string [] imgPathes)
        {
            InitializeComponent();
            this.imgPathes = imgPathes;
        }
       
        private void btn_pre_Click(object sender, RoutedEventArgs e)
        {
            this.surfaceListBoxContent.ScrollIntoView(getPre());
            ClosedUtil.isAnyBodyTouched = true;
        }
        
        void init() {
            allImages = new List<ImageObject>();
            for (int i = 0; i < this.imgPathes.Length; i++) {
                allImages.Add(new ImageObject() { 
                    ImgPath = this.imgPathes[i]
                });
            }
            this.surfaceListBoxContent.ItemsSource = this.allImages;
            if (this.allImages.Count > 0) {
                this.currentSelectIndex = 0;
            }
        }

        private  ImageObject getNext() {
            return this.allImages.ElementAt(++this.currentSelectIndex%this.allImages.Count);
        }

        private ImageObject getPre()
        {
            return this.allImages.ElementAt((--this.currentSelectIndex+this.allImages.Count)%this.allImages.Count);
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            this.surfaceListBoxContent.ScrollIntoView(getNext());
            ClosedUtil.isAnyBodyTouched = true;
        }

        private void surfaceListBoxContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.currentSelectIndex = this.surfaceListBoxContent.SelectedIndex;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }
    }
}
