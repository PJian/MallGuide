using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using SuperMarketLH.page.shop;
using SuperMarketLH.uiEntity;
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

namespace SuperMarketLH.page.floor
{
    /// <summary>
    /// PageFloorBaseInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PageFloorBaseInfo : Page
    {
        private List<Floor> allFloors;
        private MainWindow parent;
        private TransitionItem transitioniItem = null;
        private PageFloorDetailInfo currentFrameContentPage;
        private List<Shop> allShops;
        public Floor currentFloor { get; set; }
        public PageFloorBaseInfo()
        {
            InitializeComponent();
        }
        public PageFloorBaseInfo(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        public void loadFloor()
        {
            this.allFloors = SqlHelper.getAllFloor();
            this.listbox_allFloors.ItemsSource = this.allFloors;
            if (this.allFloors != null && this.allFloors.Count > 0) {
                this.listbox_allFloors.SelectedIndex = 0;
            }
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            loadFloor();
        }

        private void listbox_allFloors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listbox_allFloors.SelectedItem != null)
            {
                currentFloor = this.listbox_allFloors.SelectedItem as Floor;
                //加载楼层
                transitioniItem = new TransitionItem();
                transitioniItem.ItemTransition = TransitioinUtil.getFadeTransition();
                currentFrameContentPage = new PageFloorDetailInfo(currentFloor, this);
                transitioniItem.FrameNavigatePage = new FrameNavigate()
                {
                    Source = currentFrameContentPage
                };
                this.transitionFloor.DataContext = transitioniItem;

                this.allShops = SqlHelper.getAllShopByFloor(this.currentFloor);
                this.dataGrid_shops.ItemsSource = this.allShops;

            }
        }

       

        /// <summary>
        /// 双层楼面导航
        /// </summary>
        public void navigateTo(Page page) { 
                transitioniItem = new TransitionItem();
                transitioniItem.ItemTransition = TransitioinUtil.getFadeTransition();
                transitioniItem.FrameNavigatePage = new FrameNavigate()
                {
                    Source = page
                };
                this.transitionFloor.DataContext = transitioniItem;
        }

        public void showShopDetailInfo(Shop shop) {

            parent.frame.Navigate(new PageShopDetail(shop,this.parent));
        
        }

        public void busy() {
            this.parent.loadBusy("努力导航中...");
        }
        public void busyDone() {
            this.parent.loadNotBusy();
        }
        /// <summary>
        /// 功能键出现
        /// Ctrl+M
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key==Key.M)
            {
                //MessageBox.Show("draw machine");
                if (this.currentFrameContentPage != null) {
                    this.currentFrameContentPage.drawMachine();
                }
                this.btn_saveMachine.Visibility = Visibility.Visible;
            }
        }

        private void btn_saveMachine_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentFrameContentPage != null) {
                this.currentFrameContentPage.drawMachineDone();
                this.btn_saveMachine.Visibility = Visibility.Collapsed;
            }
          
        }

        private void dataGrid_shops_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.dataGrid_shops.SelectedItem != null)
            {
                this.currentFrameContentPage.userCtrlMapGrid.drawShopTips(this.dataGrid_shops.SelectedItem as Shop);
            }
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

        private void btn_elevator_handle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_elevator_handle", "resource/images/navBtn/btn15_press.png");
        }

        private void btn_elevator_handle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_elevator_handle", "resource/images/navBtn/btn15.png");
        }

        private void btn_elevator_handle_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_elevator_handle", "resource/images/navBtn/btn15_press.png");
        }

        private void btn_elevator_handle_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_elevator_handle", "resource/images/navBtn/btn15.png");
        }

        private void btn_elevator_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_elevator", "resource/images/navBtn/btn16_press.png");
        }

        private void btn_elevator_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_elevator", "resource/images/navBtn/btn16.png");
        }

        private void btn_elevator_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_elevator", "resource/images/navBtn/btn16_press.png");
        }

        private void btn_elevator_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_elevator", "resource/images/navBtn/btn16.png");
        }

        private void btn_service_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_service", "resource/images/navBtn/btn17_press.png");
        }

        private void btn_service_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_service", "resource/images/navBtn/btn17.png");
        }

        private void btn_service_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_service", "resource/images/navBtn/btn17_press.png");
        }

        private void btn_service_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_service", "resource/images/navBtn/btn17.png");
        }

        private void btn_pay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_pay", "resource/images/navBtn/btn18_press.png");
        }

        private void btn_pay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_pay", "resource/images/navBtn/btn18.png");
        }

        private void btn_pay_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_pay", "resource/images/navBtn/btn18_press.png");
        }

        private void btn_pay_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_pay", "resource/images/navBtn/btn18.png");
        }

        private void btn_smoke_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_smoke", "resource/images/navBtn/btn19_press.png");
        }

        private void btn_smoke_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_smoke", "resource/images/navBtn/btn19.png");
        }

        private void btn_smoke_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_smoke", "resource/images/navBtn/btn19_press.png");
        }

        private void btn_smoke_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_smoke", "resource/images/navBtn/btn19.png");
        }

        private void btn_baby_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_baby", "resource/images/navBtn/btn20_press.png");
        }

        private void btn_baby_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_baby", "resource/images/navBtn/btn20.png");
        }

        private void btn_baby_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_baby", "resource/images/navBtn/btn20_press.png");
        }

        private void btn_baby_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_baby", "resource/images/navBtn/btn20.png");
        }

        private void btn_bathroom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_bathroom", "resource/images/navBtn/btn21_press.png");
        }

        private void btn_bathroom_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_bathroom", "resource/images/navBtn/btn21.png");
        }

        private void btn_bathroom_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_bathroom", "resource/images/navBtn/btn21_press.png");
        }

        private void btn_bathroom_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_bathroom", "resource/images/navBtn/btn21.png");
        }

        private void btn_elevator_handle_Click(object sender, RoutedEventArgs e)
        {
            this.currentFrameContentPage.userCtrlMapGrid.showCommonBuildingTips(ObstacleType.ESCALATOR);
        }

        private void btn_elevator_Click(object sender, RoutedEventArgs e)
        {
            this.currentFrameContentPage.userCtrlMapGrid.showCommonBuildingTips(ObstacleType.ELEVATOR);
        }

        private void btn_service_Click(object sender, RoutedEventArgs e)
        {
            this.currentFrameContentPage.userCtrlMapGrid.showCommonBuildingTips(ObstacleType.SERVICE);
        }

        private void btn_pay_Click(object sender, RoutedEventArgs e)
        {
            this.currentFrameContentPage.userCtrlMapGrid.showCommonBuildingTips(ObstacleType.CHECKSTAND);
        }

        private void btn_smoke_Click(object sender, RoutedEventArgs e)
        {
            this.currentFrameContentPage.userCtrlMapGrid.showCommonBuildingTips(ObstacleType.SMOKING_ROOM);
        }

        private void btn_baby_Click(object sender, RoutedEventArgs e)
        {
            this.currentFrameContentPage.userCtrlMapGrid.showCommonBuildingTips(ObstacleType.BABY_ROOM);
        }

        private void btn_bathroom_Click(object sender, RoutedEventArgs e)
        {
            this.currentFrameContentPage.userCtrlMapGrid.showCommonBuildingTips(ObstacleType.TOLITE);
        }

    }
}
