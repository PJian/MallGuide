using EntityManagementService.entity;
using EntityManagementService.nav;
using EntityManagementService.util;
using EntityManageService.sqlUtil;
using SuperMarketLH.page.floor;
using SuperMarketLH.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// <summary>
    /// UserControlMapGridWith3D.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlMapGridWith3D : UserControl
    {
        public UserControlMapGridWith3D()
        {
            InitializeComponent();
        }
        
        private Map _currentEditMap;
        private Floor currentEditFloor;

        private bool isDrawMahine = false;

        private Point machineLoaction;
        private double StartPositionX = 0;
        private double StartPositionY = 0;
        public Point MachineLoaction
        {
            get { return machineLoaction; }
            set { machineLoaction = value; }
        }

        public Machine CurrentMachine { get; set; }

        public bool IsDrawMahine
        {
            get { return isDrawMahine; }
            set { isDrawMahine = value; }
        }
        public Floor CurrentEditFloor
        {
            get { return currentEditFloor; }
            set
            {
                currentEditFloor = value;
                if (this.PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentEditFloor"));
                }
                //初始化
                init();
            }
        }
        private Obstacle currentEditObstacle;

        public Obstacle CurrentEditObstacle
        {
            get { return currentEditObstacle; }
            set { currentEditObstacle = value; }
        }

        public Map CurrentEditMap
        {
            get { return _currentEditMap; }
            set { _currentEditMap = value; }
        }

        private Shop _currentShop;

        public Shop CurrentShop
        {
            get { return _currentShop; }
            set { _currentShop = value; }
        }





        public UserControlMapGridWith3D(Floor floor)
        {
            InitializeComponent();
            this.currentEditFloor = floor;
        }
        private PageFloorBaseInfo rootPage;

        public PageFloorBaseInfo RootPage
        {
            get { return rootPage; }
            set { rootPage = value; }
        }





        private void init()
        {

            if (this.CurrentEditFloor.Map != null && !this.CurrentEditFloor.Map.Trim().Equals(""))
            {
                this.CurrentEditMap = SerialUtil.readMap(this.CurrentEditFloor);

            }

            this.grid_info.DataContext = this;

            Machine m = SerialUtil.readMachine();
            if (m != null)
            {
                this.CurrentMachine = m;
            }
            else
            {
                this.CurrentMachine = null;
            }
            drawMap();
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {


        }

        public event PropertyChangedEventHandler PropertyChanged;





        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (this.isDrawMahine)
            {
                Point p = e.GetPosition(this.grid_info);
                Point gridLocation = new Point(CanvasUtil.getColOfGridByPoint(p), CanvasUtil.getRowOfGridByPoint(p));
                if (MapUtil.getClickArea(p, this.CurrentEditMap.Areas) == null && MapUtil.getClickArea(gridLocation, this.CurrentEditMap.Areas) == null)
                {
                    drawMap();
                    this.MachineLoaction = gridLocation;
                    CanvasUtil.drawMachine(this.grid_info, (int)this.machineLoaction.Y, (int)this.machineLoaction.X);
                }
                else
                {
                    MessageBox.Show("不能放在物体上！请重画");
                    return;
                }


            }
            else
            {
                if (this.CurrentEditMap == null) return;
                Obstacle clickObstacle = MapUtil.getClickArea(e.GetPosition(this.grid_info), this.CurrentEditMap.Areas);
                if (clickObstacle != null)
                {
                    drawMap();
                    CanvasUtil.drawShopTips(this.grid_info, clickObstacle, navToShop, showShopDetail);
                }

            }
        }


        /// <summary>
        /// 绘制商铺提示
        /// </summary>
        public void drawShopTips(Shop shop)
        {
            //清除之前的内容
            //this.grid_info.Children.RemoveRange(2, this.grid_info.Children.Count - 2);
            drawMap();
            CanvasUtil.drawShopTips(this.grid_info, this.CurrentEditMap, shop, navToShop, showShopDetail);
        }

        public void showShopDetail(Shop shop)
        {
            RootPage.showShopDetailInfo(shop);
        }


        /// <summary>
        /// 保存机器位置
        /// </summary>
        public void saveMachine()
        {
            ///throw new NotImplementedException();

            SerialUtil.writeMachine(new Machine()
            {
                FloorIndex = this.CurrentEditFloor.Index,
                X = (int)this.machineLoaction.X,
                Y = (int)this.machineLoaction.Y
            });
            drawMap();
        }
        /// <summary>
        /// 重绘地图
        /// </summary>
        public void drawMap()
        {
            if (this.grid_info.Children.Count >= 3)
            {
                this.grid_info.Children.RemoveRange(2, this.grid_info.Children.Count - 2);
            }

            if (this.CurrentMachine != null && this.CurrentMachine.FloorIndex == this.CurrentEditFloor.Index)
            {
                CanvasUtil.drawMachine(this.grid_info, this.CurrentMachine);
            }
        }

        private BackgroundWorker bw;

        private Shop destShop;

        private bool isOneFloor = false;
        private List<Node> roadNodes;
        /// <summary>
        /// 导航到商铺
        /// </summary>
        public void navToShop(Shop shop)
        {
            if (shop.Floor == null) return;
            destShop = shop;
            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted +=bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (isOneFloor)
            {
                drawMap();
                CanvasUtil.drawRoad(new Grid[] { this.grid_info }, new List<Node>[]{roadNodes});
            }
            else {

                RootPage.navigateTo(new PageFloorNavTwoFloor(RootPage)
                {
                    CurrentMachine = this.CurrentMachine,
                    MachineFloor = SqlHelper.getFloorByIndex(this.CurrentMachine.FloorIndex),
                    ShopFloor = CurrentEditFloor,
                    Destination = destShop.Door
                });
            }
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {

            if (this.CurrentMachine != null && this.CurrentMachine.FloorIndex == this.CurrentEditFloor.Index && destShop.Floor.Index == this.currentEditFloor.Index)
            {
                isOneFloor = true;
                //在同一层楼
                int[,] mapState = MapUtil.fillMapState(this.CurrentEditMap);
                Point startPoint = new Point(this.CurrentMachine.X, this.CurrentMachine.Y);
                roadNodes = AStar.findRoadAH(mapState, startPoint, destShop.Door);
              
            }
            else if (this.CurrentMachine != null && this.CurrentMachine.FloorIndex != this.CurrentEditFloor.Index && destShop.Floor.Index == this.currentEditFloor.Index)
            {
                //机器位置跟商铺位置不在同一层楼
                isOneFloor = false;
               
            }
        }

        /// <summary>
        /// 表示出公共设施即可
        /// </summary>
        /// <param name="buildingType">公共设施类型</param>
        public void showCommonBuildingTips(string buildingType)
        {
            if (this.CurrentEditMap == null || this.CurrentEditMap.Areas == null || this.CurrentEditMap.Areas.Count <= 0) return;
            //判断地图上有没有对应的建筑
            foreach (Obstacle item in this.CurrentEditMap.Areas)
            {
                if (item.Type.Equals(buildingType)) {
                    drawMap();
                    CanvasUtil.drawShopTips(this.grid_info, item, navToShop, showShopDetail);
                    break;
                }
            }

        }

      


    }
}
