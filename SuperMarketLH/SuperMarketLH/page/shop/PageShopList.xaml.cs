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
        public PageShopList(Catagory currentCatagory, PageShop parent)
        {
            InitializeComponent();
            this.currentCatagory = currentCatagory;
            this.parent = parent;
        }
        private void init()
        {
            allShops = SqlHelper.getShopByCatagory(this.currentCatagory);
            //如果店铺只有一个并且该店铺类型还是主推店铺，则显示店铺广告图片
            if (allShops.Count == 1 && allShops.ElementAt(0).Type == ConstantData.SHOP_TYPE_SPECIAL)
            {
                userCtrlImgs.Visibility = Visibility.Visible;
                this.surfaceListBox.Visibility = Visibility.Collapsed;
                Brand brand = allShops.ElementAt(0).Brand;
                if (brand!=null )
                 this.userCtrlImgs.Imgs = allShops.ElementAt(0).Brand.ImgPaths;
               // this.userCtrlImgs.Imgs = getShopPromotionImgOfValidate();
            }
            else
            {
                userCtrlImgs.Visibility = Visibility.Collapsed;
                this.surfaceListBox.Visibility = Visibility.Visible;
                this.surfaceListBox.ItemsSource = allShops;
            }
            
        }
      
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void surfaceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.surfaceListBox.SelectedItem != null) {
                parent.navigateToShop(this.surfaceListBox.SelectedItem as Shop);
            }
        }

    }
}
