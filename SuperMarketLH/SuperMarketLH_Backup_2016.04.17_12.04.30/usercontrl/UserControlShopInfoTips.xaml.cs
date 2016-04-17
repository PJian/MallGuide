using EntityManagementService.entity;
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

namespace SuperMarketLH.usercontrl
{
    public delegate void NavToShop(Shop shop);
    public delegate void ShowDetailInfo(Shop shop);
    /// <summary>
    /// UserControlShopInfoTips.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlShopInfoTips : UserControl
    {
        

        public NavToShop Nav { get; set; }//导航委托
        public ShowDetailInfo ShowDetailInfo { get; set; }//显示详细信息

        public UserControlShopInfoTips()
        {
            InitializeComponent();
        }
        public UserControlShopInfoTips(Shop currentShop)
        {
            InitializeComponent();
            this.CurrentShop = currentShop;
        }
        private Shop _currentShop;

        public Shop CurrentShop
        {
            get { return _currentShop; }
            set { _currentShop = value; this.DataContext = this.CurrentShop; }
        }
       

        private void btn_nav_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void btn_detail_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn_nav_TouchDown(object sender, TouchEventArgs e)
        {
            Nav(this.CurrentShop);
            ClosedUtil.isAnyBodyTouched = true;
        }

        private void btn_detail_TouchDown(object sender, TouchEventArgs e)
        {
            ShowDetailInfo(this.CurrentShop);
            ClosedUtil.isAnyBodyTouched = true;
        }

       
        private void btn_detail_Click_1(object sender, RoutedEventArgs e)
        {

            ShowDetailInfo(this.CurrentShop);
            ClosedUtil.isAnyBodyTouched = true;
        }

        private void btn_nav_Click_1(object sender, RoutedEventArgs e)
        {
            Nav(this.CurrentShop);
            ClosedUtil.isAnyBodyTouched = true;
        }

          
      

    }
}
