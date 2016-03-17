using EntityManagementService.entity;
using EntityManageService.sqlUtil;
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

namespace SuperMarketLHS.page.shop
{
    /// <summary>
    /// PageShopBrandSalePromotions.xaml 的交互逻辑
    /// </summary>
    public partial class PageShopBrandSalePromotions : Page
    {
        private MainWindow rootWin;
        private List<SalePromotion> allSalePromotion;
        private List<Brand> allBrand;
        private List<Shop> allShop;
        private Brand currentSelectBrand;
        private SalePromotion currentSelectSalePromotion;
        private Shop currentShop;
        private SalePromotion currentComboxSelectSalePromotion;
        private int preSelectShopIndex = 0;
        public PageShopBrandSalePromotions()
        {
            InitializeComponent();
        }
        public PageShopBrandSalePromotions(MainWindow rootWin)
        {
            InitializeComponent();
            this.rootWin = rootWin;
        }

        private void init(){
            allSalePromotion = SqlHelper.getAllSalePromotions();
            allBrand = SqlHelper.getAllBrands();
            this.list_allSalePromotions.ItemsSource = allSalePromotion;
            this.list_allBrands.ItemsSource = allBrand;
            reloadShops();
            if (this.list_allShops.Items.Count > 0) {
                this.list_allShops.SelectedIndex = 0;
            }
        }

        private void list_allShops_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.list_allShops.SelectedItem != null)
            {
                this.currentShop = this.list_allShops.SelectedItem as Shop;
                this.stackPanel_currentShop.DataContext = this.currentShop;
                preSelectShopIndex = this.list_allShops.SelectedIndex;
            }
            else {
                this.currentShop = new Shop();
                this.stackPanel_currentShop.DataContext = this.currentShop;
            }
        }

        private void list_allBrands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.list_allBrands.SelectedItem != null) {
                this.currentSelectBrand = this.list_allBrands.SelectedItem as Brand;
                this.list_allShopsBrandin.ItemsSource = SqlHelper.getAllShopBrandIn(this.currentSelectBrand);
            }
        }

        private void list_allSalePromotions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.list_allSalePromotions.SelectedItem != null) {
                this.currentSelectSalePromotion = this.list_allSalePromotions.SelectedItem as SalePromotion;
                this.list_allShopsSalePromotions.ItemsSource = SqlHelper.getAllShopSalePromotionIn(this.currentSelectSalePromotion);
            }
        }

        private void combox_currentShopSalePromotions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.combox_currentShopSalePromotions.SelectedItem != null) {
                this.currentComboxSelectSalePromotion = this.combox_currentShopSalePromotions.SelectedItem as SalePromotion;
            }
        }
        /// <summary>
        /// 品牌入驻
        /// </summary>
        private void addBrandIn() {
            if (this.currentShop == null) {
                MessageBox.Show("请选择商铺！");
                return;
            }
            if (this.currentSelectBrand == null) {
                MessageBox.Show("请选择需要入驻的品牌！");
                return;
            }
            if (this.currentShop.Brand != null) {
                if (this.currentShop.Brand.Id == this.currentSelectBrand.Id) {
                    MessageBox.Show("品牌已入驻！");
                    return;
                }
                if (MessageBox.Show("当前店铺已经入驻了品牌，是否替换？", "品牌替换", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SqlHelper.updateRelationBetShopAndBrand(this.currentShop, this.currentSelectBrand);
                    reloadShops();
                    this.list_allShops.SelectedIndex = preSelectShopIndex;
                    MessageBox.Show("替换成功！");
                    return;
                }
                else {
                    return;
                }
            }

            SqlHelper.createRelationBetShopAndBrand(this.currentShop,this.currentSelectBrand);
            reloadShops();
            this.list_allShops.SelectedIndex = preSelectShopIndex;
            MessageBox.Show("添加成功！");
        }
        /// <summary>
        /// 品牌撤销
        /// </summary>
        private void delBrandIn() {
            if (this.currentShop == null)
            {
                MessageBox.Show("请选择商铺！");
                return;
            }
            if (this.currentShop.Brand != null)
            {
                SqlHelper.deleteRalationBetShopAndBrand(this.currentShop);
                reloadShops();
                this.list_allShops.SelectedIndex = preSelectShopIndex;
                MessageBox.Show("品牌撤销成功！");
            }
            
        }
        

        /// <summary>
        /// 活动入驻
        /// </summary>
        private void salePromotionIn() {
            if (this.currentShop == null)
            {
                MessageBox.Show("请选择商铺！");
                return;
            }
            if(this.currentSelectSalePromotion==null){
                MessageBox.Show("请选择要添加的促销活动！");
                return ;
            }
            if (this.currentShop.SalePromotion != null && this.currentShop.SalePromotion.Contains(this.currentSelectSalePromotion)) {
                MessageBox.Show("活动已经添加！");
                return;
            }  
            SqlHelper.addSalePromotionToShop(this.currentShop,this.currentSelectSalePromotion);
            reloadShops();
            this.list_allShops.SelectedIndex = preSelectShopIndex;
            MessageBox.Show("添加成功！");
        }

        private void delsalePromotion() {
            if (this.currentShop == null)
            {
                MessageBox.Show("请选择商铺！");
                return;
            }
            SqlHelper.deleteSalePromotionFromShop(this.currentShop,this.currentComboxSelectSalePromotion);
            reloadShops();
            this.list_allShops.SelectedIndex = preSelectShopIndex;
            MessageBox.Show("撤销活动成功！");
        }
        private void reloadShops() {
            allShop = SqlHelper.getAllShop();
            this.list_allShops.ItemsSource = allShop;
            this.list_allShops.SelectedIndex = -1;
        }

        

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void btn_saveBrandIn_Click(object sender, RoutedEventArgs e)
        {
            addBrandIn();
        }

        private void btn_delBrandIn_Click(object sender, RoutedEventArgs e)
        {
            delBrandIn();
        }

        private void btn_saveSalePromotionIn_Click(object sender, RoutedEventArgs e)
        {
            salePromotionIn();
        }

        private void btn_delSalePromotionIn_Click(object sender, RoutedEventArgs e)
        {
            delsalePromotion();
        }

    }
}
