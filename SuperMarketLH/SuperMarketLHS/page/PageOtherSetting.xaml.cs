using SuperMarketLHS.comm;
using SuperMarketLHS.page.other;
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

namespace SuperMarketLHS.page
{
    /// <summary>
    /// PageOtherSetting.xaml 的交互逻辑
    /// </summary>
    public partial class PageOtherSetting : Page
    {
        private MainWindow parent;
        public PageOtherSetting()
        {
            InitializeComponent();
        }
        public PageOtherSetting(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
        }
        private void init() {
            WinUtil.chengToSelectBtn(this.btn_defaultAD, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_updateInfo, btn_questionnaireInfo, btn_surroundInfo, btn_employee, btn_JoinUs, btn_magazine, btn_fireEscape }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
            this.label_location.Content = "其他 > 屏保广告";
            this.frame.Navigate(new PageScreenProtectAD(this.parent));
        }

        private void btn_fireEscape_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_fireEscape, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_updateInfo, btn_questionnaireInfo, btn_surroundInfo, btn_employee, btn_JoinUs, btn_magazine, btn_defaultAD }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
            this.label_location.Content = "其他 > 逃生通道";
            this.frame.Navigate(new PageFireEmergence(this.parent));
        }

        private void btn_magazine_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_magazine, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_updateInfo, btn_questionnaireInfo, btn_surroundInfo, btn_employee, btn_JoinUs, btn_fireEscape, btn_defaultAD }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
            this.label_location.Content = "其他 > 电子杂志";
            this.frame.Navigate(new PageMagazine(this.parent));
        }

        private void btn_JoinUs_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_JoinUs, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_updateInfo, btn_questionnaireInfo, btn_surroundInfo, btn_employee, btn_magazine, btn_fireEscape, btn_defaultAD }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
            this.label_location.Content = "其他 > 招商信息";
            this.frame.Navigate(new PageJoinUsAD(this.parent));
        }

        private void btn_employee_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_employee, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_updateInfo, btn_questionnaireInfo, btn_surroundInfo, btn_JoinUs, btn_magazine, btn_fireEscape, btn_defaultAD }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
            this.label_location.Content = "其他 > 人才招聘";
            this.frame.Navigate(new PageEmployee(this.parent));
        }

        private void btn_surroundInfo_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_surroundInfo, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_updateInfo, btn_questionnaireInfo, btn_employee, btn_JoinUs, btn_magazine, btn_fireEscape, btn_defaultAD }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
            this.label_location.Content = "其他 > 周边信息";
            this.frame.Navigate(new PageSurroundInfo(this.parent));
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void btn_defaultAD_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_defaultAD, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_updateInfo,btn_questionnaireInfo, btn_surroundInfo, btn_employee, btn_JoinUs, btn_magazine, btn_fireEscape }.ToList(), FindResource("leftNavBtnStyle") as Style);
   
            this.label_location.Content = "其他 > 屏保广告";
            this.frame.Navigate(new PageScreenProtectAD(this.parent));
        }

        private void btn_questionnaireInfo_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_questionnaireInfo, FindResource("leftNavBtnSelectStyle") as Style, new Button[] {btn_updateInfo,btn_defaultAD, btn_surroundInfo, btn_employee, btn_JoinUs, btn_magazine, btn_fireEscape }.ToList(), FindResource("leftNavBtnStyle") as Style);

            this.label_location.Content = "其他 > 问卷调查";
            this.frame.Navigate(new PageQuestionnaire(this.parent));
        }

        private void btn_updateInfo_Click(object sender, RoutedEventArgs e)
        {
            WinUtil.chengToSelectBtn(this.btn_updateInfo, FindResource("leftNavBtnSelectStyle") as Style, new Button[] { btn_questionnaireInfo, btn_defaultAD, btn_surroundInfo, btn_employee, btn_JoinUs, btn_magazine, btn_fireEscape }.ToList(), FindResource("leftNavBtnStyle") as Style);
            this.label_location.Content = "其他 > 数据更新";
            this.frame.Navigate(new PageUpdateSSH(this.parent));
        }
    }
}
