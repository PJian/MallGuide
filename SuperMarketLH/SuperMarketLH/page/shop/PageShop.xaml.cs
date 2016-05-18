using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using SuperMarketLH.uiEntity;
using SuperMarketLH.util;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// PageShop.xaml 的交互逻辑
    /// </summary>
    public partial class PageShop : Page
    {
        private TransitionItem transitioniItem = null;
        private MainWindow parent;
        public PageShop()
        {
            InitializeComponent();

        }
        public PageShop(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private Shop initialShowShop = null;
        public PageShop(Shop shop, MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
            initialShowShop = shop;
        }

        private void btn_movieCity_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_MOVIE);
            ClosedUtil.isAnyBodyTouched = true;
        }
        private void init()
        {
            int navtoIndex = WinUtil.FRAME_SHOP_ALL;
            transitioniItem = new TransitionItem() { ItemTransition = TransitioinUtil.getFadeTransition() };
            if (this.initialShowShop != null)
            {
                //显示指定店铺信息
                switch (initialShowShop.CatagoryName.Name) {
                    case "大润发超市": navtoIndex = WinUtil.FRAME_SHOP_SUPER_MARKET_DARUNFA; break;
                    case "KTV": navtoIndex = WinUtil.FRAME_SHOP_KTV; break;
                    case "苏宁易购": navtoIndex = WinUtil.FRAME_SHOP_SUNINGYIGOU; break;
                    case "健身": navtoIndex = WinUtil.FRAME_SHOP_JIANSHENG; break;
                    case "大白鲸世界儿童乐园": navtoIndex = WinUtil.FRAME_SHOP_DABAIJINGSHIJIEERTONGLEYUAN; break;
                    case "影城": navtoIndex = WinUtil.FRAME_SHOP_MOVIE; break;
                }
            }
            navigateTo(navtoIndex);
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        /// <summary>
        /// 进行页面导航
        /// </summary>
        /// <param name="catagory"></param>
        private void navigateTo(int index)
        {
            try
            {
                transitioniItem = new TransitionItem() { ItemTransition = TransitioinUtil.getFadeTransition() };
                Page page = null;
                switch (index)
                {
                    case WinUtil.FRAME_SHOP_ALL: page = new PageShopList(this); break;
                    case WinUtil.FRAME_SHOP_MOVIE: page = new PageShopMovie(); break;
                    case WinUtil.FRAME_SHOP_CAN_YIN: page = new PageShopList(SqlHelper.getCatagoryByName("餐饮"), this); break;
                    case WinUtil.FRAME_SHOP_SHOPPING: page = new PageShopList(SqlHelper.getCatagoryByName("购物"), this); break;
                    case WinUtil.FRAME_SHOP_CHILDREN: page = new PageShopList(SqlHelper.getCatagoryByName("儿童"), this); break;
                    case WinUtil.FRAME_SHOP_KTV: page = new PageShopList(SqlHelper.getCatagoryByName("KTV"), this); break;
                    case WinUtil.FRAME_SHOP_SUPER_MARKET_DARUNFA: page = new PageShopList(SqlHelper.getCatagoryByName("大润发超市"), this); break;
                    case WinUtil.FRAME_SHOP_SUNINGYIGOU: page = new PageShopList(SqlHelper.getCatagoryByName("苏宁易购"), this); break;
                    case WinUtil.FRAME_SHOP_JIANSHENG: page = new PageShopList(SqlHelper.getCatagoryByName("健身"), this); break;
                    case WinUtil.FRAME_SHOP_DABAIJINGSHIJIEERTONGLEYUAN: page = new PageShopList(SqlHelper.getCatagoryByName("大白鲸世界儿童乐园"), this); break;
                    case WinUtil.FRAME_SHOP_FUN: page = new PageShopList(SqlHelper.getCatagoryByName("娱乐"), this); break;
                }
                transitioniItem.FrameNavigatePage = new FrameNavigate()
                {
                    Source = page
                };
                this.transitionShop.DataContext = transitioniItem;


            }
            catch (Exception e)
            {
                MessageBox.Show("系统错误！" + e.Message);
            }
            finally
            {
                ClosedUtil.isAnyBodyTouched = true;
            }


        }

        private void btn_canyin_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_CAN_YIN);
        }

        private void btn_shopping_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_SHOPPING);
        }

        private void btn_children_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_CHILDREN);
        }

        private void btn_superMarketBigRunFa_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_SUPER_MARKET_DARUNFA);
        }

        private void btn_suning_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_SUNINGYIGOU);
        }

        private void btn_dabaijing_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_DABAIJINGSHIJIEERTONGLEYUAN);
        }

        private void btn_ktv_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_KTV);
        }

        private void btn_jianshen_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_JIANSHENG);
        }

        public void navigateToShop(Shop shop)
        {
            //transitioniItem = new TransitionItem();
            //transitioniItem.ItemTransition = TransitioinUtil.getFlipTransition();
            //transitioniItem.FrameNavigatePage = new FrameNavigate()
            //{
            //    Source = new PageShopDetail(shop)
            //};
            //this.transitionShop.DataContext = transitioniItem;
            parent.frame.Navigate(new PageShopDetail(shop, this.parent));
        }

        /// <summary>
        /// 改变按钮的背景
        /// </summary>
        private void changeBtnBG(string buttonName, string imgPath)
        {
            if (!File.Exists(imgPath)) return;
            BinaryReader binReader = new BinaryReader(File.Open(imgPath, FileMode.Open));
            FileInfo fileInfo = new FileInfo(imgPath);
            byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
            binReader.Close();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.EndInit();

            Button btn = FindName(buttonName) as Button;
            if (btn != null)
            {
                btn.Background = new ImageBrush(bitmap);
            }
        }

        private void btn_food_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_CAN_YIN);
        }

        private void btn_cloth_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_SHOPPING);
        }

        private void children_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_CHILDREN);
        }

        private void btn_fun_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_FUN);
        }

        private void btn_all_Click(object sender, RoutedEventArgs e)
        {
            navigateTo(WinUtil.FRAME_SHOP_ALL);
        }
    }
}
