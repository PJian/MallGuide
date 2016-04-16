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
    /// NewShopWin.xaml 的交互逻辑
    /// 完成商铺的新建和入驻功能
    /// </summary>
    public partial class NewShopWin : Window
    {
        private Obstacle relativeObstacle;
        public NewShopWin(Obstacle relativeObstacle)
        {
            InitializeComponent();
            this.relativeObstacle = relativeObstacle;
        }

    }
}
