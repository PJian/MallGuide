using SuperMarketLHS.comm;
using SuperMarketLHS.page.shop;
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
    /// ShopInfoEdit.xaml 的交互逻辑
    /// </summary>
    public partial class ShopInfoEdit : Page
    {
        private MainWindow parent;
        public ShopInfoEdit()
        {
            InitializeComponent();
        }
        public ShopInfoEdit(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
            showBaseInfo();
        }

        private void showBaseInfo() {
            WinUtil.chengToSelectBtn(this.btn_editBaseInfo, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_salePromotion, btn_inBrand, btn_activityCountInfo }.ToList(), FindResource("leftNavBtnStyle") as Style);
            label_location.Content = "店铺信息 > 基本信息";
            this.frame.Navigate(new PageShopBaseInfo(this.parent));
        }
        private void btn_editBaseInfo_Click(object sender, RoutedEventArgs e)
        {
            showBaseInfo();
        }

        private void btn_salePromotion_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_salePromotion, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_editBaseInfo, btn_inBrand, btn_activityCountInfo }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
            label_location.Content = "店铺信息 > 促销活动";
            this.frame.Navigate(new Page_SalePromotions(this.parent));
        }

        private void btn_inBrand_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_inBrand, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_editBaseInfo,btn_activityCountInfo, btn_salePromotion }.ToList(), FindResource("leftNavBtnStyle") as Style);
            label_location.Content = "店铺信息 > 品牌、活动关联";
            this.frame.Navigate(new PageShopBrandSalePromotions(this.parent));
        }

        private void btn_activityCountInfo_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_activityCountInfo, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_editBaseInfo, btn_salePromotion, btn_inBrand }.ToList(), FindResource("leftNavBtnStyle") as Style);
            label_location.Content = "店铺信息 > 活动报名信息";
            this.frame.Navigate(new PageActivitySignInfo(this.parent));
        }
    }
}
