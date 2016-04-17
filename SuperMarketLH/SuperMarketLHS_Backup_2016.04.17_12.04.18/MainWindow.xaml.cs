using SuperMarketLHS.page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace SuperMarketLHS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_mallInfo_Click(object sender, RoutedEventArgs e)
        {
            navigateToPage(new Page_MallEdit(this));
            //if (isSaved)
            //{
                
            //}
            //else {
            //    if (MessageBox.Show("当前内容还没有保存，确认离开？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
            //        navigateToPage(new Page_MallEdit(this));
            //    }
            //}
        }
        /// <summary>
        /// 页面跳转
        /// </summary>
        private void navigateToPage(Page page) {
            frame.Navigate(page);
        }

        /// <summary>
        /// 加载遮罩层
        /// </summary>
        public void loadIn() {
            this.userCtrl_loading.Visibility = System.Windows.Visibility.Visible;
        }
        /// <summary>
        /// 取消遮罩
        /// </summary>
        public void loadHide() {
            this.userCtrl_loading.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btn_brandInfo_Click(object sender, RoutedEventArgs e)
        {
            navigateToPage(new BrandEdit(this));
        }

        private void btn_shopInfo_Click(object sender, RoutedEventArgs e)
        {
            navigateToPage(new ShopInfoEdit(this));
        }

        private void btn_floorInfo_Click(object sender, RoutedEventArgs e)
        {
            navigateToPage(new PageFloorEdit(this));
        }

        private void btn_otherInfo_Click(object sender, RoutedEventArgs e)
        {
            navigateToPage(new PageOtherSetting(this));
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            navigateToPage(new Page_MallEdit(this));
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
