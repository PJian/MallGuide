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
                //Brand brand = allShops.ElementAt(0).Brand;
                //this.userCtrlImgs.Imgs = getShopPromotionImgOfValidate();
            }
            else
            {
                userCtrlMainShop.Visibility = Visibility.Collapsed;
                this.surfaceListBox.Visibility = Visibility.Visible;
                this.grid_page.Visibility = Visibility.Visible;
                this.surfaceListBox.ItemsSource = allShops;

            }
            loadImgCounter(sp.PageCurrent);
            
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

        private void getNextItem()
        {
            if (sp != null&&sp.PageCurrent<sp.PageCount) {
                sp.PageCurrent++;
                init();
            }
            else
            {
                sp.PageCurrent = 0;
                init();
            }
        }

        private void getPreItem()
        {
            if (sp != null && sp.PageCurrent > 1)
            {
                sp.PageCurrent--;
                init();
            }
            else {
                sp.PageCurrent = sp.PageCount;
                init();
            }
        }

        /// <summary>
        /// 加载图片计数
        /// </summary>
        private void loadImgCounter(int selectIndex)
        {
            pageCountStackPanel.Children.Clear();
            for (int i = 1; i <= sp.PageCount; i++)
            {
                pageCountStackPanel.Children.Add(i == selectIndex ? getSelectEllipse() : getUnSelectEllipse());
            }
        }
        /// <summary>
        /// 构造选中的圆圈效果
        /// </summary>
        /// <returns></returns>
        private Ellipse getUnSelectEllipse()
        {
            return new Ellipse()
            {
                Fill = Brushes.White,
                Width = 20,
                Height = 20,
                Margin = new Thickness(2, 0, 2, 0)
            };
        }
        /// <summary>
        /// 构造未选中的圆圈效果
        /// </summary>
        /// <returns></returns>
        private Ellipse getSelectEllipse()
        {
            return new Ellipse()
            {
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0099ff")),
                Width = 20,
                Height = 20,
                Margin = new Thickness(2, 0, 2, 0)
            };
        }

        //初始鼠标位置
        private double startX = 0;
        //结尾鼠标位置
        private double endX = 0;
        private void compareX(double startX, double endX)
        {
            if ((endX - startX) > 200)
            {
                getNextItem();
            }
            else if ((startX - endX) > 200)
            {
                getPreItem();
            }
        }

        private void Grid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            startX = e.GetPosition(shopListGrid).X;
        }

        private void Grid_MouseMove_1(object sender, MouseEventArgs e)
        {
            endX = e.GetPosition(shopListGrid).X;
        }

        private void Grid_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            compareX(startX, endX);
        }

        private void Grid_TouchDown_1(object sender, TouchEventArgs e)
        {
            startX = e.GetTouchPoint(shopListGrid).Position.X;
        }

        private void Grid_TouchMove_1(object sender, TouchEventArgs e)
        {
            endX = e.GetTouchPoint(shopListGrid).Position.X;
        }

        private void Grid_TouchUp_1(object sender, TouchEventArgs e)
        {
            compareX(startX, endX);
        }

        private void shopListGrid_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            startX = e.GetPosition(shopListGrid).X;
        }

        private void shopListGrid_PreviewMouseMove_1(object sender, MouseEventArgs e)
        {
            endX = e.GetPosition(shopListGrid).X;
        }

        private void shopListGrid_PreviewMouseUp_1(object sender, MouseButtonEventArgs e)
        {
            compareX(startX, endX);
        }

        private void shopListGrid_PreviewTouchDown_1(object sender, TouchEventArgs e)
        {
            startX = e.GetTouchPoint(shopListGrid).Position.X;
        }

        private void shopListGrid_PreviewTouchMove_1(object sender, TouchEventArgs e)
        {
            endX = e.GetTouchPoint(shopListGrid).Position.X;
        }

        private void shopListGrid_PreviewTouchUp_1(object sender, TouchEventArgs e)
        {
            compareX(startX, endX);
        }



    }
}
