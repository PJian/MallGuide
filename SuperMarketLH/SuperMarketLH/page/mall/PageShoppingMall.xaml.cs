using EntityManagementService.entity;
using EntityManageService.sqlUtil;
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

namespace SuperMarketLH.page.mall
{
    /// <summary>
    /// PageShoppingMall.xaml 的交互逻辑
    /// </summary>
    public partial class PageShoppingMall : Page
    {
        private MainWindow parent;
        private Mall currentMall;
        public PageShoppingMall()
        {
            InitializeComponent();
        }
        public PageShoppingMall(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
        }
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }
        private void init()
        {
            this.currentMall = SqlHelper.getShopMall();
           
            if (this.currentMall != null)
            {
                if (this.currentMall.ResourceType == ConstantData.RESOURCE_TYPE_IMG)
                {
                    this.userContrl_imgs.Visibility = Visibility.Visible;
                    this.mediaElement.Visibility = Visibility.Collapsed;
                    this.userContrl_imgs.Imgs = this.currentMall.ImgPath.ToArray();

                }
                else if (this.currentMall.ResourceType == ConstantData.RESOURCE_TYPE_MOVIE)
                {
                    this.userContrl_imgs.Visibility = Visibility.Collapsed;
                    this.mediaElement.Visibility = Visibility.Visible;
                    this.mediaElement.LoadedBehavior = MediaState.Manual;
                    this.mediaElement.MediaEnded += MediaElement_MediaEnded;
                    this.mediaElement.Source = new Uri(WinUtil.getMoviePathFull(this.currentMall.MoviePath));
                    this.mediaElement.Play();
                }
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.mediaElement.Position = TimeSpan.FromMilliseconds(0);
            this.mediaElement.Play();
        }
    }
}
