using EntityManagementService.entity;
using SuperMarketLH.page.shop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperMarketLH.usercontrl
{
    /// <summary>
    /// UserControlShop.xaml 的交互逻辑
    /// 普通店铺模板
    /// </summary>
    public partial class UserControlShop : UserControl
    {

        private Shop shop;

        public Shop Shop
        {
            get { return shop; }
            set { shop = value; }
        }
        private PageShopDetail _parentPage;

        public PageShopDetail ParentPage
        {
            get { return _parentPage; }
            set { _parentPage = value; }
        }
        // private BackgroundWorker bw;
        public UserControlShop(Shop shop)
        {
            InitializeComponent();
            this.shop = shop;
            //  init();
        }
        public UserControlShop()
        {
            InitializeComponent();

        }


      



        private void init()
        {
            if (this.Shop == null) return;
            this.normalTemplate.DataContext = this.Shop;
           
            //品牌图片
            if (Shop.BrandImgs != null && Shop.BrandImgs.Length > 0)
            {
                UserControlImgCtrl5 ctrl3 = new UserControlImgCtrl5(Shop.BrandImgs);
                this.grid_brandImage.Children.Add(ctrl3);
            }
            //都有活动图片

            userCtrlPromotionImg.Imgs = Shop.getShopPromotionImgOfValidate().ToArray();
           // userCtrlFacilitiesImg.Imgs = Shop.Facilities;
            this.tab_promotion.SetResourceReference(TabItem.StyleProperty, "tabItemStyle2");
            this.tab_promotion.Visibility = Visibility.Visible;
            this.tab_Facilities.Visibility = Visibility.Collapsed;
            //if (this.Shop.Type == ConstantData.SHOP_TYPE_NORMAL)
            //{
                
            //}
            //else
            //{
            //    this.tab_promotion.SetResourceReference(TabItem.StyleProperty, "tabItemStyle");
            //    this.tab_promotion.Visibility = Visibility.Visible;
            //    this.tab_Facilities.Visibility = Visibility.Visible;
            //}
           

        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
           
            init();

        }


    }
}
