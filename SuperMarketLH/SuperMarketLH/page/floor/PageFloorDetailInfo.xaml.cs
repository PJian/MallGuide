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

namespace SuperMarketLH.page.floor
{
    /// <summary>
    /// PageFloorDetailInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PageFloorDetailInfo : Page
    {
        public Floor currentFloor { get; set; }
       
        private PageFloorBaseInfo parent;
        public PageFloorDetailInfo(Floor floor)
        {
            InitializeComponent();
            this.currentFloor = floor;
        }
        public PageFloorDetailInfo(Floor floor, PageFloorBaseInfo parent)
        {
            InitializeComponent();
            this.currentFloor = floor;
            this.parent = parent;
        }
        private void init() {
            this.userCtrlMapGrid.CurrentEditFloor = this.currentFloor;
            this.userCtrlMapGrid.RootPage = parent;
            
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

       

        /// <summary>
        /// 绘制出机器的位置
        /// </summary>
        public void drawMachine() {
            this.userCtrlMapGrid.IsDrawMahine = true;
        }

        public void drawMachineDone()
        {
            this.userCtrlMapGrid.IsDrawMahine = false;
            this.userCtrlMapGrid.saveMachine();
            MessageBox.Show("保存成功！");
        }
     

       

        
    }
}
