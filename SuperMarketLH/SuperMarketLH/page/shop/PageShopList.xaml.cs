using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using SuperMarketLH.util;
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
    /// PageShopList.xaml 的交互逻辑
    /// </summary>
    public partial class PageShopList : Page
    {
        private List<Shop> allShops;
        private Catagory currentCatagory;
        private PageShop parent;
        private SplitPage sp;
        private int shopCount;

        public PageShopList(PageShop parent)
        {
            InitializeComponent();
            this.currentCatagory = null;
            this.parent = parent;
        }


        public PageShopList(Catagory currentCatagory, PageShop parent)
        {
            InitializeComponent();
            this.currentCatagory = currentCatagory;
            this.parent = parent;
         
        }

        private void calculatePageCount(){
            shopCount = SqlHelper.selectShopCount();
            if (this.currentCatagory != null)
            {
                shopCount = SqlHelper.selectShopCount(this.currentCatagory);
            }
            else
            {
                shopCount = SqlHelper.selectShopCount();
            }

            if (sp == null)
            {
                sp = new SplitPage()
                {
                    PageSize = WinUtil.PAGE_SIZE,
                    NMax = shopCount,
                    PageCount = shopCount % WinUtil.PAGE_SIZE == 0 ? shopCount / WinUtil.PAGE_SIZE : (shopCount / WinUtil.PAGE_SIZE + 1),
                    PageCurrent = 1
                };
                if (sp.PageCount > 1)
                {
                    this.btn_nextPage.IsEnabled = true;
                }
                else
                {
                    this.btn_nextPage.IsEnabled = false;
                }
                this.btn_prePage.IsEnabled = false;
                grid_page.DataContext = sp;
            }
        }

        private void init()
        {


            if (this.currentCatagory != null)
            {
                allShops = SqlHelper.getShopByCatagory(this.currentCatagory,sp);
            }
            else {
                allShops = SqlHelper.getAllShop(sp);
            }


            //如果店铺只有一个并且该店铺类型还是主推店铺，则显示店铺广告图片
            if (allShops.Count == 1 && allShops.ElementAt(0).Type == ConstantData.SHOP_TYPE_SPECIAL)
            {
                userCtrlMainShop.Visibility = Visibility.Visible;
                this.surfaceListBox.Visibility = Visibility.Collapsed;
                this.grid_page.Visibility = Visibility.Collapsed;
                userCtrlMainShop.Shop = allShops.ElementAt(0);
               // Brand brand = allShops.ElementAt(0).Brand;
              
                // this.userCtrlImgs.Imgs = getShopPromotionImgOfValidate();
            }
            else
            {
                userCtrlMainShop.Visibility = Visibility.Collapsed;
                this.surfaceListBox.Visibility = Visibility.Visible;
                this.grid_page.Visibility = Visibility.Visible;
                this.surfaceListBox.ItemsSource = allShops;

            }
            
        }
      
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            calculatePageCount();
            init();
           
            
        }

        private void surfaceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.surfaceListBox.SelectedItem != null) {
                parent.navigateToShop(this.surfaceListBox.SelectedItem as Shop);
            }
        }

        private void btn_nextPage_Click(object sender, RoutedEventArgs e)
        {
            if (sp != null) {
                sp.PageCurrent++;
                if (sp.PageCurrent >= sp.PageCount) {
                    this.btn_nextPage.IsEnabled = false;
           
                }
                if (sp.PageCurrent >1) {
                    this.btn_prePage.IsEnabled = true;
                }
                init();
            }
            
        }

        private void btn_prePage_Click(object sender, RoutedEventArgs e)
        {
            if (sp != null) {
                sp.PageCurrent--;
                if (sp.PageCurrent <= 1) {
                    this.btn_prePage.IsEnabled = false;
                }
                if (sp.PageCurrent < sp.PageCount)
                {
                    this.btn_nextPage.IsEnabled = true;
                }
                init();
            }
        }



    }
}
