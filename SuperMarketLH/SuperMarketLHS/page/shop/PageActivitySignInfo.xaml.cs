using EntityManagementService.entity;
using EntityManagementService.sqlUtil;
using EntityManageService.sqlUtil;
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

namespace SuperMarketLHS.page.shop
{
    /// <summary>
    /// PageActivitySignInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PageActivitySignInfo : Page
    {
        private SalePromotion currentSalePromotion;
        private List<SalePromotion> allSalePromotion;
        private MainWindow root;
        private Boolean isRemoteDBUsed = false;
        public PageActivitySignInfo()
        {
            InitializeComponent();
        }
        public PageActivitySignInfo(MainWindow root)
        {
            InitializeComponent();
            this.root = root;
        }
        private void init()
        {
            DBServer server = SqlHelper.getDBServer();
            if (server == null || !server.Used)
            {
                this.isRemoteDBUsed = false;
            }
            else
            {
                this.isRemoteDBUsed = true;
            }
            this.allSalePromotion = SqlHelper.getAllSalePromotions();
            this.list_allSalePromotions.ItemsSource = this.allSalePromotion;
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void list_allSalePromotions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.list_allSalePromotions.SelectedItem != null)
            {
                currentSalePromotion = this.list_allSalePromotions.SelectedItem as SalePromotion;
                if (this.isRemoteDBUsed)
                {
                    this.label_acctivityCount.Content = SqlHelperDB.getSignerNumOfActivity(currentSalePromotion);
                }
                else
                {
                    this.label_acctivityCount.Content = SqlHelper.getSignerNumOfActivity(currentSalePromotion);
                }
             
            }
        }

    }
}
