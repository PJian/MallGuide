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


        private void startThread() {
            ParentPage.busy();
            Thread t = new Thread(init);
            t.IsBackground = true;
            t.Start();
            //bw = new BackgroundWorker();
            //bw.DoWork += bw_DoWork;
            //bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            //bw.RunWorkerAsync();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            ParentPage.busyDone();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();
          //  init();
        }



        private void init()
        {
            this.normalTemplate.DataContext = this.Shop;
            if (Shop.Brand != null)
            {

                UserControlImgCtrl5 ctrl3 = new UserControlImgCtrl5(Shop.Brand.ImgPaths);
                this.grid_brandImage.Children.Add(ctrl3);
            }
            //都有活动图片
            userCtrlPromotionImg.Imgs = Shop.getShopPromotionImgOfValidate().ToArray();
            userCtrlFacilitiesImg.Imgs = Shop.Facilities;
            if (this.Shop.Type == ConstantData.SHOP_TYPE_NORMAL)
            {
                this.tab_promotion.SetResourceReference(TabItem.StyleProperty, "tabItemStyle2");
                this.tab_promotion.Visibility = Visibility.Visible;
                this.tab_Facilities.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.tab_promotion.SetResourceReference(TabItem.StyleProperty, "tabItemStyle");
                this.tab_promotion.Visibility = Visibility.Visible;
                this.tab_Facilities.Visibility = Visibility.Visible;
            }
           // ParentPage.busyDone();

        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
          // startThread();
           init();
           
        }


    }
}
