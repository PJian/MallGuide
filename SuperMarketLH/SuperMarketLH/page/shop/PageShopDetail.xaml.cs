using EntityManagementService.entity;
using SuperMarketLH.usercontrl;
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

namespace SuperMarketLH.page.shop
{
    /// <summary>
    /// PageShopDetail.xaml 的交互逻辑
    /// </summary>
    public partial class PageShopDetail : Page
    {
        private Shop shop;
        private MainWindow rootWin;
       
        public PageShopDetail(Shop shop,MainWindow rootWin)
        {
            InitializeComponent();
            this.shop = shop;
            this.rootWin = rootWin;
           
        }
        public void busy()
        {
            rootWin.loadBusy();
        }

        public void busyDone()
        {
            rootWin.loadNotBusy();
        }

        private void init() {
            userCtrlShop.Shop = this.shop;
            userCtrlShop.ParentPage = this;
        }
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

      
        private void btn_rtn_Click(object sender, RoutedEventArgs e)
        {
            rootWin.frame.GoBack();
        }
    }
}
