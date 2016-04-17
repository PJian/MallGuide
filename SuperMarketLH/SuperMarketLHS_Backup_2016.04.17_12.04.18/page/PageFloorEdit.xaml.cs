using SuperMarketLHS.comm;
using SuperMarketLHS.page.floor;
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
    /// PageFloorEdit.xaml 的交互逻辑
    /// </summary>
    public partial class PageFloorEdit : Page
    {
        private MainWindow parent;
        public PageFloorEdit()
        {
            InitializeComponent();
        }
        public PageFloorEdit(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
            showBaseInfo();
        }

        private void showBaseInfo() {
            WinUtil.chengToSelectBtn(this.btn_editBaseInfo, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_salePromotion, btn_inBrand }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
            label_location.Content = "地图信息 > 基本信息";
            this.frame.Navigate(new PageFloorBaseInfo(this.parent));
        }

        private void btn_editBaseInfo_Click(object sender, RoutedEventArgs e)
        {
            showBaseInfo();
        }

        private void btn_salePromotion_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_salePromotion, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_editBaseInfo, btn_inBrand }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
            label_location.Content = "地图信息 > 区域编辑";
            this.frame.Navigate(new PageZoneEdit(this.parent));
        }

        private void btn_inBrand_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_inBrand, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_editBaseInfo, btn_salePromotion }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
            label_location.Content = "地图信息 > 商铺入驻";
            this.frame.Navigate(new PageZoneShop(this.parent));
        }
    }
}
