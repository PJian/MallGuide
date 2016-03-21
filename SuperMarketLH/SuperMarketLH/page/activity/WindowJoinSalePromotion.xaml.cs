using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using SuperMarketLH.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SuperMarketLH.page.activity
{
    /// <summary>
    /// WindowJoinSalePromotion.xaml 的交互逻辑
    /// </summary>
    public partial class WindowJoinSalePromotion : Window
    {
        private SalePromotion currentSalePromotion;
        public WindowJoinSalePromotion(SalePromotion currentSalePromotion)
        {
            InitializeComponent();
            this.currentSalePromotion = currentSalePromotion;
        }
        public bool validateMobileNum() {
            return Regex.IsMatch(this.txt_mobileNum.Text, "\\d{11}");
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            if (validateMobileNum())
            {
                SqlHelper.saveAssignActivitiesInfo(this.txt_mobileNum.Text, this.currentSalePromotion);
                this.Close();
            }
            else {
                MessageBox.Show("请输入正确的手机号！");
            }
            ClosedUtil.isAnyBodyTouched = true;
        }

        private void btn_cancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            ClosedUtil.isAnyBodyTouched = true;
        }

    }
}
