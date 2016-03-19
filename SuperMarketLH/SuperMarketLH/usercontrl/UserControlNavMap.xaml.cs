using EntityManagementService.entity;
using EntityManagementService.nav;
using EntityManagementService.util;
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

namespace SuperMarketLH.usercontrl
{
    /// <summary>
    /// UserControlNavMap.xaml 的交互逻辑
    /// 导航地图
    /// </summary>
    public partial class UserControlNavMap : UserControl
    {
        private Map currentMap;
        private Floor currentFloor;
        //障碍物数组，用来寻路
        private int[,] mapValue;
        private bool isDrawMachine =false;//标识是不是在绘制机器位置
        public UserControlNavMap(Floor currentFloor)
        {
            InitializeComponent();
            this.currentFloor = currentFloor;
        }
        private void init()
        {
            this.currentMap = SerialUtil.readMap(this.currentFloor);
            this.grid_info.DataContext = this.currentFloor;
            mapValue = MapUtil.createObstaclesArra(this.currentMap);
        }
        /// <summary>
        /// 绘制地图的机器位置
        /// </summary>
        private void drawMachine()
        {
            Machine machine = SqlHelper.getAMachine();
            if (machine != null && machine.FloorIndex == this.currentFloor.Index)
            {
                CanvasUtil.drawMachine(this.grid_info, machine.Y, machine.X);
            }
        }
        /// <summary>
        /// 添加一台机器
        /// </summary>
        private void addMachine(Point p)
        {
            Machine machine = SqlHelper.getAMachine();
            if (machine == null || (machine.X == 0 && machine.Y == 0))
            {
                machine = new Machine()
                {
                    FloorIndex = this.currentFloor.Index,
                    X = (int)p.X / CanvasUtil.MAP_CANVAS_GRID_PIX_DIF,
                    Y = (int)p.Y / CanvasUtil.MAP_CANVAS_GRID_PIX_DIF
                };
                SqlHelper.saveMachine(machine);
                MessageBox.Show("添加成功！");
            }
            else
            {
                if (MessageBox.Show("当前已经设置过机器位置，是否更改？", "更改机器位置", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    machine = new Machine()
                    {
                        Id = machine.Id,
                        FloorIndex = this.currentFloor.Index,
                        X = (int)p.X / CanvasUtil.MAP_CANVAS_GRID_PIX_DIF,
                        Y = (int)p.Y / CanvasUtil.MAP_CANVAS_GRID_PIX_DIF
                    };
                    SqlHelper.updateMachine(machine);
                    MessageBox.Show("修改成功！");
                }
            }
        }
        /// <summary>
        /// 如果在地图上点击了店铺，则显示出提示信息
        /// </summary>
        /// <param name="p"></param>
        private void showShopInfo(Point p,NavToShop nav) { 
            Obstacle obstacle = MapUtil.getClickArea(p,this.currentMap.Areas);
            if (obstacle.Type == ObstacleType.SHOP) {
                CanvasUtil.drawShopTips(this.grid_info,p,obstacle.Shop,nav);
            }
        }
        /// <summary>
        /// 显示从机器位置到达商铺的路径
        /// </summary>
        public void drawRoadFromMachineToShop(Shop shop) {
            Machine machine = SqlHelper.getAMachine();
            if (machine.FloorIndex == this.currentFloor.Index && this.currentFloor.Index == shop.Floor.Index) { 
                //都在同一层
                List<Node> nodes = AStar.findRoadAH(this.mapValue, new Point(machine.X, machine.Y), CanvasUtil.getPointOfGrid(shop.Door));
                CanvasUtil.drawRoad(new Grid[]{this.grid_info}, new List<Node>[]{nodes});
            }
        }
        /// <summary>
        /// 显示从机器位置到达电梯的路径
        /// </summary>
        public void drawRoadFromMacchineToElevator() { 
            //遍历电梯的位置，让后找出最近的电梯
            Machine machine = SqlHelper.getAMachine();
            if (machine.FloorIndex == this.currentFloor.Index )
            {
                //都在同一层
                Point p = MapUtil.getNearestElevatorPosstion(new Point(machine.X,machine.Y),this.currentMap);
                //如果存在最近的电梯的话
                if (p.Equals(new Point(0, 0))) {
                    List<Node> nodes = AStar.findRoadAH(this.mapValue, new Point(machine.X, machine.Y), CanvasUtil.getPointOfGrid(p));
                    CanvasUtil.drawRoad(new Grid[] { this.grid_info }, new List<Node>[] { nodes });
                }
            }
        }
        /// <summary>
        /// 显示从电梯到达商铺的路径
        /// </summary>
        public void drawRoadFromElevatorToShop(Point elevator,Shop shop) {
            List<Node> nodes = AStar.findRoadAH(this.mapValue, CanvasUtil.getPointOfGrid(elevator), CanvasUtil.getPointOfGrid(shop.Door));
            CanvasUtil.drawRoad(new Grid[] { this.grid_info }, new List<Node>[] { nodes });
        }
        /// <summary>
        /// 监听键盘事件，用来设定机器位置
        /// ctrl+alt+M
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_PreviewKeyUp_1(object sender, KeyEventArgs e)
        {
            
            if (Keyboard.Modifiers == ModifierKeys.Control && Keyboard.Modifiers == ModifierKeys.Alt && e.Key == Key.M) {
                MessageBox.Show("请在地图上指定机器的位置");
                isDrawMachine = true;
            }
        }

        private void Grid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (isDrawMachine)
            {
                addMachine(e.GetPosition(this.grid_info));
                drawMachine();
            }
            else {
              //  showShopInfo(e.GetPosition(this.grid_info));
            }
        }

    }
}
