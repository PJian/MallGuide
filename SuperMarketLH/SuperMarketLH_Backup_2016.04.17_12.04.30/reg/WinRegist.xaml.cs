using SerialNum;
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
using System.Windows.Shapes;

namespace SuperMarketLH.reg
{
    /// <summary>
    /// WinRegist.xaml 的交互逻辑
    /// </summary>
    public partial class WinRegist : Window
    {
        public Soft soft { get; set; }
        public WinRegist()
        {
            InitializeComponent();
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.reg.soft = soft;
        }

        private void reg_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.reg.Visibility == System.Windows.Visibility.Collapsed)
            {
                this.Close();
            }
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            ClosedUtil.regWinIsOpen = false;
        }
    }
}
