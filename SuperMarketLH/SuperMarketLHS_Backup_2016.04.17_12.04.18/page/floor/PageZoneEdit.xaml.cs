using EntityManagementService.entity;
using EntityManagementService.util;
using EntityManageService.sqlUtil;
using SuperMarketLHS.comm;
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

namespace SuperMarketLHS.page.floor
{
    /// <summary>
    /// PageZoneEdit.xaml 的交互逻辑
    /// </summary>
    public partial class PageZoneEdit : Page
    {
        private MainWindow rootWin;
        private List<Floor> allFloors;
        private Floor currentEditFloor;
        public PageZoneEdit()
        {
            InitializeComponent();
        }
        public PageZoneEdit(MainWindow rootWin)
        {
            InitializeComponent();
            this.rootWin = rootWin;
        }
        private void init()
        {
            //楼层
            allFloors = SqlHelper.getAllFloor();
            this.combox_allFloors.ItemsSource = this.allFloors;
            //区域
            this.combox_areaType.ItemsSource = ObstacleType.Type;
            this.userCtrl_map.EditState = CavasUtil.DRAW_STATE;
        }

        private void showFloorInfo()
        {
            //this.userCtrl_map.BG = this.currentEditFloor.Img;
            this.userCtrl_map.CurrentEditFloor = this.currentEditFloor;
            //地图数据
            // this.currentEditMap = SerialUtil.readMap(this.currentEditFloor);
        }
        private void combox_allFloors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.combox_allFloors.SelectedItem != null)
            {
                this.currentEditFloor = this.combox_allFloors.SelectedItem as Floor;
                showFloorInfo();
            }
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (this.combox_areaType.SelectedItem != null)
            {
                this.userCtrl_map.setCurrentObstacleType(this.combox_areaType.SelectedItem as string);
            }
            this.userCtrl_map.saveMap(this.currentEditFloor,this.txb_ObstacleIndex.Text);
            this.txb_ObstacleIndex.Text = "";
            //this.combox_areaType.SelectedIndex = -1;
        }

        private void btn_delCurrent_Click(object sender, RoutedEventArgs e)
        {
            this.userCtrl_map.delCurrentEditObstacle();
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            this.userCtrl_map.delData();
            
        }

        private void btn_goBack_Click(object sender, RoutedEventArgs e)
        {
            this.userCtrl_map.avoidLastStep();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            this.userCtrl_map.clearTemp();
        }

        private void combox_areaType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.combox_areaType.SelectedItem != null)
            {
                this.userCtrl_map.setCurrentObstacleType(this.combox_areaType.SelectedItem as string);
            }
        }
        


    }
}
