using EntityManagementService.entity;
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
using System.Windows.Shapes;

namespace SuperMarketLHS.win
{
    /// <summary>
    /// ShopInWin.xaml 的交互逻辑
    /// 完成商铺的入驻
    /// </summary>
    public partial class ShopInWin : Window
    {
       
        private Obstacle relativeObstacle;
        public ShopInWin(Obstacle relativeObstacle)
        {
            InitializeComponent();
          
            this.relativeObstacle = relativeObstacle;
        }

        private void btn_enter_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
