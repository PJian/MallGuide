using EntityManagementService.entity;
using EntityManagementService.nav;
using EntityManagementService.util;
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

namespace SuperMarketLH.page.floor
{
    /// <summary>
    /// PageFloorNavTwoFloor.xaml 的交互逻辑
    /// 两层楼的寻路配置
    /// </summary>
    public partial class PageFloorNavTwoFloor : Page
    {
        public PageFloorNavTwoFloor()
        {
            InitializeComponent();
        }
        PageFloorBaseInfo parentPage;
        public PageFloorNavTwoFloor(PageFloorBaseInfo parent)
        {
            InitializeComponent();
            this.parentPage = parent;
        }

        public Floor MachineFloor { get; set; }
        public Floor ShopFloor { get; set; }
        public Point Destination { get; set; }
        public Machine CurrentMachine { get; set; }
        private Obstacle machineTo = null;
        private BackgroundWorker bw;
        List<Node> toElevator;
        List<Node> toShop;

        private bool hasRoad = false;
        /// <summary>
        /// 进行寻路
        /// </summary>
        private void init()
        {

            usctrl_mapGrid_1.RootPage = this.parentPage;
            usctrl_mapGrid_2.RootPage = this.parentPage;
            parentPage.busy();
            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (hasRoad)
            {
                if (MachineFloor.Index < ShopFloor.Index)
                {
                    this.usctrl_mapGrid_2.CurrentEditFloor = ShopFloor;
                    this.usctrl_mapGrid_1.CurrentEditFloor = MachineFloor;
                    CanvasUtil.drawMachine(this.usctrl_mapGrid_1.grid_info, this.CurrentMachine);
                    // CanvasUtil.drawRoad(this.usctrl_mapGrid_2.grid_info, toShop);
                    //  CanvasUtil.drawRoad(new Grid[] { this.usctrl_mapGrid_1.grid_info, this.usctrl_mapGrid_2.grid_info}, new List<Node>[] { toElevator, toShop });
                    CanvasUtil.drawRoad(new Canvas[] { this.usctrl_mapGrid_1.map_continer, this.usctrl_mapGrid_2.map_continer }, new Grid[] { this.usctrl_mapGrid_1.grid_info, this.usctrl_mapGrid_2.grid_info }, new List<Node>[] { toElevator, toShop });

                }
                else
                {
                    this.usctrl_mapGrid_2.CurrentEditFloor = ShopFloor;
                    this.usctrl_mapGrid_1.CurrentEditFloor = MachineFloor;

                    CanvasUtil.drawMachine(this.usctrl_mapGrid_1.grid_info, this.CurrentMachine);
                    CanvasUtil.drawRoad(new Canvas[] { this.usctrl_mapGrid_1.map_continer, this.usctrl_mapGrid_2.map_continer }, new Grid[] { this.usctrl_mapGrid_1.grid_info, this.usctrl_mapGrid_2.grid_info }, new List<Node>[] { toElevator, toShop });
                    //  CanvasUtil.drawRoad(this.usctrl_mapGrid_2.grid_info, toShop);

                }
                parentPage.busyDone();
            }
            else {
                parentPage.busyDone();
                //throw new Exception("商铺不可达");
                MessageBox.Show("无法导航到指定商铺");
            }
           
        }

        private bool findElevator()
        {
            return false;
        }


        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Map machineMap = SerialUtil.readMap(MachineFloor);
            Map shopMap = SerialUtil.readMap(ShopFloor);
            Point machinePoint = new Point(this.CurrentMachine.X, this.CurrentMachine.Y);
            Obstacle elevator = MapUtil.getNearestElevator(machinePoint, machineMap);
            machineTo = elevator;

            Obstacle elevatorToShop = MapUtil.getElevatorByNum(shopMap, this.machineTo.Index);
            //判断两层楼是否都有电梯
            if (elevator == null || elevatorToShop == null)
            {
                //如果没有，则走扶梯
                elevator = MapUtil.getNearestEscalator(machinePoint, machineMap);
                machineTo = elevator;
                if (machineTo != null) {
                    elevatorToShop = MapUtil.getEscalatorByNum(shopMap, this.machineTo.Index);
                }
                if (elevatorToShop == null)
                {
                     elevatorToShop = MapUtil.getEscalator(shopMap);
                }

                //判断两层楼是否都有扶梯，如果没有则走楼梯
                if (elevator == null || elevatorToShop == null)
                {
                    elevator = MapUtil.getNearestStair(machinePoint, machineMap);
                    machineTo = elevator;
                    if (machineTo != null) {
                        elevatorToShop = MapUtil.getStairByNum(shopMap, this.machineTo.Index);
                    }
                    if (elevatorToShop == null)
                    {
                        elevatorToShop = MapUtil.getStair(shopMap);
                    }
                }
            }

            //判断两层楼是否都有楼梯，没有则走重点不可达
            if (elevator != null && elevatorToShop != null)
            {
                toElevator = navToElevator(machineMap, machineTo);
                toShop = elevatorNavToShop(shopMap, elevatorToShop);
                hasRoad = true;
            }

            else
            {
                hasRoad = false;
            }
        }


        /// <summary>
        /// 从电梯位置导航到商铺
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        private List<Node> elevatorNavToShop(Floor floor)
        {
            Map tempMap = SerialUtil.readMap(floor);
            //找到电梯
            Obstacle elevator = MapUtil.getElevatorByNum(tempMap, this.machineTo.Index);
            if (elevator != null)
            {
                if (tempMap != null)
                {
                    int[,] mapState = MapUtil.fillMapState(tempMap);
                    List<Node> nodes = AStar.findRoadAH(mapState, CanvasUtil.getPointOfGrid(elevator.Door), Destination);
                    return nodes;
                }
            }
            return null;
        }

        public List<Node> elevatorNavToShop(Map map, Obstacle elevator)
        {
            //Map tempMap = SerialUtil.readMap(floor);
            //找到电梯
            //Obstacle elevator = MapUtil.getElevatorByNum(tempMap, this.machineTo.Index);
            if (elevator != null)
            {
                if (map != null)
                {
                    int[,] mapState = MapUtil.fillMapState(map);
                    List<Node> nodes = AStar.findRoadAH(mapState, CanvasUtil.getPointOfGrid(elevator.Door), Destination);
                    return nodes;
                }
            }
            return null;
        }

        /// <summary>
        /// 从机器位置寻路到电梯
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<Node> navToElevator(Floor floor)
        {
            Map tempMap = SerialUtil.readMap(floor);
            Point machinePoint = new Point(this.CurrentMachine.X, this.CurrentMachine.Y);
            Obstacle elevator = MapUtil.getNearestElevator(machinePoint, tempMap);
            machineTo = elevator;
            if (tempMap != null)
            {
                int[,] mapState = MapUtil.fillMapState(tempMap);
                List<Node> nodes = AStar.findRoadAH(mapState, machinePoint, CanvasUtil.getPointOfGrid(elevator.Door));
                return nodes;
            }
            return null;
        }

        private List<Node> navToElevator(Map map, Obstacle elevator)
        {
            // Map tempMap = SerialUtil.readMap(floor);
            Point machinePoint = new Point(this.CurrentMachine.X, this.CurrentMachine.Y);
            /// Obstacle elevator = MapUtil.getNearestElevator(machinePoint, tempMap);
            machineTo = elevator;
            if (map != null)
            {
                int[,] mapState = MapUtil.fillMapState(map);
                List<Node> nodes = AStar.findRoadAH(mapState, machinePoint, CanvasUtil.getPointOfGrid(elevator.Door));
                return nodes;
            }
            return null;
        }


        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void Page_Unloaded_1(object sender, RoutedEventArgs e)
        {
            CanvasUtil.releaseRoadTimer();
        }

    }
}
