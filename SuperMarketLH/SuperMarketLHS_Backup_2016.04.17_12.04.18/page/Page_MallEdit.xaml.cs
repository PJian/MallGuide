using SuperMarketLHS.comm;
using SuperMarketLHS.page.mall;
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

namespace SuperMarketLHS.page
{
    /// <summary>
    /// Page_MallEdit.xaml 的交互逻辑
    /// </summary>
    public partial class Page_MallEdit : Page
    {
        private MainWindow parent;
        public Page_MallEdit()
        {
            InitializeComponent();
        }
        public Page_MallEdit(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
            showBaseInfo();
        }

        private void showBaseInfo() {
            frame.Navigate(new PageBaseInfo(parent));
            label_location.Content = "商场简介 > 基本信息";
            WinUtil.chengToSelectBtn(this.btn_editBaseInfo, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { this.btn_editHotelInfo, this.btn_editShopMallInfo, btn_editGlobalProjectInfo }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
        }

        private void btn_editBaseInfo_Click(object sender, RoutedEventArgs e)
        {
            showBaseInfo();
        }

        private void btn_editHotelInfo_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new PageHotelEdit(parent));
            label_location.Content = "商场简介 > 酒店信息";
            WinUtil.chengToSelectBtn(this.btn_editHotelInfo, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { this.btn_editBaseInfo, this.btn_editShopMallInfo, btn_editGlobalProjectInfo }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
        }

        private void btn_editShopMallInfo_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new PageShopMall(parent));
            label_location.Content = "商场简介 > 购物中心信息";
            WinUtil.chengToSelectBtn(this.btn_editShopMallInfo, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { this.btn_editBaseInfo, this.btn_editHotelInfo, btn_editGlobalProjectInfo }.ToList(), FindResource("leftNavBtnStyle") as Style);
        }

        private void btn_editGlobalProjectInfo_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new PageGlobalProjectInfo(parent));
            label_location.Content = "商场简介 > 全国项目简介";
            WinUtil.chengToSelectBtn(this.btn_editGlobalProjectInfo, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { this.btn_editBaseInfo, this.btn_editHotelInfo, btn_editShopMallInfo }.ToList(), FindResource("leftNavBtnStyle") as Style);
     
        }

    }
}
