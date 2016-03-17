using EntityManagementService.entity;
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

namespace SuperMarketLH.page.activity
{
    /// <summary>
    /// PageNormalAcitvities.xaml 的交互逻辑
    /// </summary>
    public partial class PageNormalAcitvities : Page
    {
       private List<SalePromotion> salePromotion;
        private MainWindow parent;
        private SalePromotion currentSalePromotion;
        public PageNormalAcitvities()
        {
            InitializeComponent();
        }
        public PageNormalAcitvities(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
        }
        private void init() {
            salePromotion = SqlHelper.getNormalSalePromotion();
            this.list_allActivities.ItemsSource = salePromotion;
            if (this.list_allActivities.Items.Count > 0) {
                this.list_allActivities.SelectedIndex = 0;
            }
        }

        private void list_allActivities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.list_allActivities.SelectedItem != null) {
                this.currentSalePromotion = this.list_allActivities.SelectedItem as SalePromotion;
                this.userContrl_imgs.Imgs = this.currentSalePromotion.ImgPaths;
            }
        }

        private void btn_join_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentSalePromotion != null) {
                new WindowJoinSalePromotion(this.currentSalePromotion).ShowDialog();
            }
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }
    }
}
