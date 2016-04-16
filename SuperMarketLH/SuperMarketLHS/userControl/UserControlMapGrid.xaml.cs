using EntityManagementService.entity;
using EntityManagementService.nav;
using EntityManagementService.util;
using EntityManageService.sqlUtil;
using SuperMarketLHS.comm;
using SuperMarketLHS.win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SuperMarketLHS.userControl
{
    /// <summary>
    /// UserControlMapGrid.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlMapGrid : UserControl, INotifyPropertyChanged
    {
        private Point startPoint;
        private Point endPoint;

        private Map _currentEditMap;
        private Floor currentEditFloor;

        /// <summary>
        /// 标识当前的阶段，是区域编辑阶段还是店铺入驻阶段
        /// </summary>
        private int _editState;

        public int EditState
        {
            get { return _editState; }
            set { _editState = value; }
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
        private List<Obstacle> allEditTempObstacle;
        private List<Point> currentEditArea;

        //private int[,] mapValue;//这个是临时地图，最后保存时可以讲这里面的元素同步到真的地图里面

        private int draw_state;
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


        //private string _bg;
        ///// <summary>
        ///// 地图背景
        ///// </summary>
        //public string BG
        //{
        //    get {
        //        return this._bg;
        //    }
        //    set {
        //        this._bg = value;
        //        if (this.PropertyChanged != null) {
        //            PropertyChanged(this,new PropertyChangedEventArgs("BG"));
        //        }
        //    }
        //}

        public UserControlMapGrid()
        {
            InitializeComponent();
        }

        public UserControlMapGrid(Floor floor)
        {
            InitializeComponent();
            this.currentEditFloor = floor;
        }

        //public void testMapValue() {
        //    if (this.mapValue == null) return;
        //    for (int i = 0; i < 90; i++) {
        //        for (int j = 0; j < 180; j++)
        //        {
        //            if(this.mapValue[i,j]==1)
        //             CavasUtil.drawEllipseTest(this.grid_info,i,j);
        //        }
        //    }
        //}

        private void init()
        {

            if (this.CurrentEditFloor.Map != null && !this.CurrentEditFloor.Map.Trim().Equals(""))
            {
                this.CurrentEditMap = SerialUtil.readMap(this.CurrentEditFloor);
                //读取导航地图数据
                //this.CurrentEditMap.Obstacles = MapUtil.createObstaclesArra(this.CurrentEditMap);

                //this.mapValue = this.CurrentEditMap.Obstacles;

                //for (int i = 0; i < this.CurrentEditMap.Obstacles.GetLength(0); i++)
                //{
                //    for (int j = 0; j < this.CurrentEditMap.Obstacles.GetLength(1); j++)
                //    {
                //        if (this.CurrentEditMap.Obstacles[i, j] == 1)
                //        {
                //            CavasUtil.drawEllipseTest(this.grid_info, i, j);
                //        }

                //    }
                //}
            }
            else
            {
                this.CurrentEditMap = new Map();
                this.CurrentEditMap.Width = 180;
                this.CurrentEditMap.Height = 90;
                //this.mapValue = new int[90, 180];

                //for (int i = 0; i < 90; i++)
                //{
                //    for (int j = 0; j < 180; j++)
                //    {
                //        this.mapValue[i,j] = 0;
                //    }
                //}


            }
            this.grid_info.DataContext = this;
            CurrentEditObstacle = new Obstacle();
            currentEditArea = new List<Point>();
            allEditTempObstacle = new List<Obstacle>();

            draw_state = CavasUtil.DRAW_STATE_DRAWING_AREA;//此时可以画圈圈
            drawCanvas();
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            // init();
        }

        public event PropertyChangedEventHandler PropertyChanged;



        private void drawArea(Point temp) {
            if (draw_state == CavasUtil.DRAW_STATE_DRAWING_AREA)
            {
                //画区域阶段
                if (startPoint == null || startPoint.Equals(new Point(0, 0)))
                {
                    startPoint = temp;
                }
                else
                {
                    endPoint = temp;
                    CavasUtil.drawLine(this.map_continer, startPoint, endPoint);
                    startPoint = endPoint;
                }
                // CavasUtil.drawEllipse(this.map_continer,temp);
                CavasUtil.drawEllipse(this.grid_info, (int)temp.Y / 4, (int)temp.X / 4);
                currentEditArea.Add(temp);
            }
            else if (draw_state == CavasUtil.DRAW_STATE_DRAWING_DOOR)
            {
                drawDoor(temp);
            }
            else if (draw_state == CavasUtil.DRAW_STATE_DRAWING_DONE)
            {

                drawDoorAgain(temp);
            }
        }

        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Point temp = e.GetPosition(this.grid_info);
            Obstacle o1= MapUtil.getClickArea(temp, this.CurrentEditMap.Areas) ;
            Obstacle o2 = MapUtil.getClickArea(temp, this.allEditTempObstacle);
            if (o1== null && o2 == null )
            {
                if (this.EditState == CavasUtil.DRAW_STATE)
                    drawArea(temp);
               
            }
            else
            {
                if (this.EditState == CavasUtil.DRAW_STATE)
                {
                    if (MessageBox.Show("当前区域已经有内容！不可进行编辑，是否删除原有区域！", "区域删除？", MessageBoxButton.YesNo) ==
                       MessageBoxResult.Yes)
                    {
                        if (o1 != null)
                        {
                            delObstacle(o1);
                            if (this.CurrentEditMap.Areas.Count <= 0)
                            {
                                this.currentEditFloor.Map = "";
                                SqlHelper.updateFloor(this.currentEditFloor);
                            }
                            else
                            {
                                SerialUtil.writeMap(this.currentEditFloor, this.CurrentEditMap);
                            }
                        }
                        else if (o2 != null)
                        {
                            delObstacle(o2);
                        }
                        //初始化
                        // init();
                        MessageBox.Show("删除成功！");
                    }
                }
                else if (this.EditState == CavasUtil.SHOP_IN_STATE)
                {
                    //商铺入驻
                    //o1 被商铺入驻
                    shopIn(o1);
                }
            }



        }
        /// <summary>
        /// 商铺入驻
        /// </summary>
        /// <param name="o"></param>
        private void shopIn(Obstacle o) {
            if (!o.Type.Equals(ObstacleType.SHOP))
            {
                MessageBox.Show("当前区域类型不可入驻商铺！");
                return;
            }
            if (o.Shop != null)
            {
                if (MessageBox.Show("该区域已经有商铺入驻，是否替换？", "商铺入驻", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (this.CurrentShop == null) {
                        MessageBox.Show("请选入驻商铺！");
                        return;
                    }
                    o.Shop.Door = new Point(0,0);
                    o.Shop.Floor = null;
                    o.Shop.Index = "";
                    SqlHelper.updateShop(o.Shop);//更新原有的商铺
                    o.Shop = this.CurrentShop;
                    this.CurrentShop.Index = o.Index;
                    this.CurrentShop.Door = new Point((int)o.Door.X / 4, (int)o.Door.Y / 4);
                    this.CurrentShop.Floor = this.CurrentEditFloor;
                    SqlHelper.updateShop(this.CurrentShop);
                    saveMap(this.CurrentEditFloor, this.CurrentShop);
                    MessageBox.Show("替换成功！");
                }
            }
            else {
                if (this.CurrentShop == null) {
                    MessageBox.Show("商铺不能为空！");
                    return;
                }
                //判断该商铺是否已经入驻了其他区域
                this.CurrentShop.Floor = this.CurrentEditFloor;
                //商铺上的门只是为了寻路用的，所以直接就关联到地图上的格子上
                this.CurrentShop.Door = new Point((int)o.Door.X / 4, (int)o.Door.Y / 4);
                this.CurrentShop.Index = o.Index;
                o.Shop = this.CurrentShop;
                SqlHelper.updateShop(this.CurrentShop);
                saveMap(this.CurrentEditFloor,this.CurrentShop);
                MessageBox.Show("已入驻！");
            }
        }
        /// <summary>
        /// 判断商铺是否已经入驻了
        /// </summary>
        /// <param name="shop"></param>
        private void judgeShopHasIn(Shop shop) { 
            
        }

        /// <summary>
        /// 画门
        /// </summary>
        private void drawDoor(Point temp)
        {
            //画门的阶段
            int [,] tempMapState = new int[MapUtil.MAP_HEIGHT,MapUtil.MAP_WIDHT];
            MapUtil.fillMapState(tempMapState,this.CurrentEditObstacle,1);
            if (tempMapState[(int)temp.Y / 4, (int)temp.X / 4] == 1) {
                MessageBox.Show("门的位置太近了，请稍远些！");
                return ;
            }    
            CavasUtil.drawEllipseDoor(this.grid_info, (int)temp.Y / 4, (int)temp.X / 4);
            CurrentEditObstacle.Door = temp;
            draw_state = CavasUtil.DRAW_STATE_DRAWING_DONE;
        }

        /// <summary>
        /// 重新画门
        /// </summary>
        private void drawDoorAgain(Point temp)
        {
            draw_state = CavasUtil.DRAW_STATE_DRAWING_DOOR;
            this.CurrentEditObstacle.Door = new Point(0, 0);
            drawCanvas();
            drawDoor(temp);
        }

        private void grid_info_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.currentEditArea == null || this.currentEditArea.Count <= 2) {
                return;
            }
            //闭合曲线
            CavasUtil.drawPolygon(this.map_continer, this.currentEditArea);
            startPoint = new Point(0, 0);
            endPoint = startPoint;
            CurrentEditObstacle.Boundary = this.currentEditArea;
            this.allEditTempObstacle.Add(this.CurrentEditObstacle);
            //计算闭合区域中的数组元素的状态
             fillMapState(new int[,]{{}}, this.CurrentEditObstacle, 1);
            //清除
            currentEditArea = new List<Point>();
            //状态变更
            draw_state = CavasUtil.DRAW_STATE_DRAWING_DOOR;//开始画们
        }
        /// <summary>
        /// 填充临时地图的状态
        /// </summary>
        private void fillMapState(int[,] maps, Obstacle obstacle, int value)
        {
            int minX = (int)CavasUtil.getMinX(obstacle.Boundary);
            int minY = (int)CavasUtil.getMinY(obstacle.Boundary);
            int maxX = (int)CavasUtil.getMaxX(obstacle.Boundary);
            int maxY = (int)CavasUtil.getMaxY(obstacle.Boundary);

            obstacle.MinX = minX;
            obstacle.MaxX = maxX;
            obstacle.MinY = minY;
            obstacle.MaxY = maxY;


            //for (int i = minX; i <= maxX; i++)
            //{
            //    for (int j = minY; j <= maxY; j++)
            //    {
            //        if (MapUtil.pnpoly(new Point(i, j), obstacle.Boundary))
            //        {
            //            maps[j / CavasUtil.GRID_PIX_HEIGHT, i / CavasUtil.GRID_PIX_WIDTH] = value;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 验证区域是否可以进行保存了
        /// </summary>
        /// <returns></returns>
        private bool validateObstacle(string obstacleIndex)
        {
            if (this.CurrentEditObstacle.Type == null || "".Equals(this.CurrentEditObstacle.Type.Trim()))
            {
                MessageBox.Show("请选择区域类型！");
                return false;
            }
           
            return setAndValidateObstacleIndex(obstacleIndex);
        }

        /// <summary>
        /// 保存地图
        /// </summary>
        //public void saveMap(Floor floor)
        //{
        //    if (draw_state == CavasUtil.DRAW_STATE_DRAWING_DONE)
        //    {
        //        if (!validateObstacle()) {
        //            return;
        //        }
        //        //这种状态才可以保存
        //        if (this.CurrentEditMap.Areas == null)
        //        {
        //            this.CurrentEditMap.Areas = new List<Obstacle>();
        //        }
        //        foreach (Obstacle obs in this.allEditTempObstacle)
        //        {
        //            this.CurrentEditMap.Areas.Add(obs);
        //        }
        //        copyMapValue();//地图数据的赋值
        //        SerialUtil.writeMap(floor, this.CurrentEditMap);
        //        draw_state = CavasUtil.DRAW_STATE_DRAWING_AREA;
        //        //初始化
        //        init();
        //    }
        //    else
        //    {
        //        if (draw_state == CavasUtil.DRAW_STATE_DRAWING_DOOR) {
        //            MessageBox.Show("请先在地图上标记出门的位置，在进行保存");
        //        }
        //    }

        //}

        /// <summary>
        /// 保存地图
        /// </summary>
        public void saveMap(Floor floor, string obstacleNum)
        {
            if (EditState == CavasUtil.DRAW_STATE)
            {
                if (draw_state == CavasUtil.DRAW_STATE_DRAWING_DONE)
                {
                    //区域绘制阶段保存
                    if (!validateObstacle(obstacleNum))
                    {
                        return;
                    }
                    //这种状态才可以保存
                    if (this.CurrentEditMap.Areas == null)
                    {
                        this.CurrentEditMap.Areas = new List<Obstacle>();
                    }
                    foreach (Obstacle obs in this.allEditTempObstacle)
                    {
                        this.CurrentEditMap.Areas.Add(obs);
                    }
                    // copyMapValue();//地图数据的赋值
                    SerialUtil.writeMap(floor, this.CurrentEditMap);
                    draw_state = CavasUtil.DRAW_STATE_DRAWING_AREA;
                    this.currentEditFloor.Map = ConstantData.getMapDataFileName(this.currentEditFloor);
                    SqlHelper.updateFloor(this.currentEditFloor);
                    //初始化
                    init();
                    MessageBox.Show("保存成功！");
                }
                else
                {
                    if (draw_state == CavasUtil.DRAW_STATE_DRAWING_DOOR)
                    {
                        MessageBox.Show("请先在地图上标记出门的位置，在进行保存");
                    }
                }
            }
            else if (EditState == CavasUtil.SHOP_IN_STATE) { 
                //商铺入驻阶段保存
                SerialUtil.writeMap(floor, this.CurrentEditMap);
            }
           

        }


        public void saveMap(Floor floor, Shop shop)
        {
            SerialUtil.writeMap(floor, this.CurrentEditMap);

        }


        /// <summary>
        /// 验证楼层编号
        /// 每层中每个区域的编号必须唯一
        /// </summary>
        /// <returns></returns>
        private bool setAndValidateObstacleIndex(string str)
        {
            if (( this.CurrentEditObstacle.Index==null || "".Equals(this.CurrentEditObstacle.Index) )&&(this.CurrentEditObstacle.Type.Equals(ObstacleType.ELEVATOR) ||
                this.CurrentEditObstacle.Type.Equals(ObstacleType.ESCALATOR) ||
                this.CurrentEditObstacle.Type.Equals(ObstacleType.STAIRS) ))
            {
                if (str != null)
                {
                    int rtn = 0;
                    if (int.TryParse(str, out rtn))
                    {
                        if (rtn <= 0)
                        {
                            MessageBox.Show("编号必须大于0！");
                            return false;
                        }
                        else
                        {
                            this.currentEditObstacle.Index = str;//设置编号
                            return true;
                        }
                    }
                    else {
                        MessageBox.Show("电梯编号应该为数字！");
                        return false;
                    }

                }
                MessageBox.Show("请填写电梯,扶梯或者楼梯编号！（上下层之间编号需要一致）");
                return false;
            }
            else if(this.CurrentEditObstacle.Type.Equals(ObstacleType.SHOP)){
                if (str != null)
                {
                    int rtn = 0;
                    //查看编号是否已经存在，当前楼层
                    if (this.CurrentEditMap.Areas != null) {
                        List<Obstacle> os = this.CurrentEditMap.Areas;
                        foreach (Obstacle o in os)
                        {
                            if (o.Index != null && o.Index.Equals(str))
                            {
                                MessageBox.Show("编号已经存在，请重新输入！");
                                return false;
                            }
                        }
                    }
                   
                    this.currentEditObstacle.Index = str;//设置编号
                    return true;
                }
                MessageBox.Show("请填区域编号");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 复制临时地图
        /// </summary>
        public void copyMapValue()
        {
            //for (int i = 0; i < mapValue.GetLength(0); i++)
            //{
            //    for (int j = 0; j < mapValue.GetLength(1); j++)
            //    {
            //        if (this.mapValue[i, j] == 1)
            //        {
            //            this.CurrentEditMap.Obstacles[i, j] = 1;
            //        }
            //        else if (this.mapValue[i, j] == -1)
            //        {//删除区域
            //            this.CurrentEditMap.Obstacles[i, j] = 0;
            //        }
            //    }
            //}
        }

        public void draw_stateGotBack()
        {
            draw_state--;
        }

        /// <summary>
        /// 撤销上一步
        /// </summary>
        public void avoidLastStep()
        {
            if ((this.allEditTempObstacle == null || this.allEditTempObstacle.Count == 0 )&&( this.currentEditArea == null || this.currentEditArea.Count == 0)) {
                return;
            }
            if (draw_state == CavasUtil.DRAW_STATE_DRAWING_DOOR)
            {
                draw_stateGotBack();
                this.currentEditArea = this.CurrentEditObstacle.Boundary;
                if (this.currentEditArea.Count > 0)
                {
                    startPoint = this.currentEditArea.ElementAt(this.currentEditArea.Count - 1);//更新结束节点
                }
                else
                {
                    startPoint = new Point(0, 0);
                }
                this.CurrentEditObstacle.Boundary = new List<Point>();
            }
            else if (draw_state == CavasUtil.DRAW_STATE_DRAWING_AREA)
            {
                if (this.currentEditArea != null && this.currentEditArea.Count > 0)
                {
                    this.currentEditArea.RemoveAt(this.currentEditArea.Count - 1);
                    if (this.currentEditArea.Count > 0)
                    {
                        startPoint = this.currentEditArea.ElementAt(this.currentEditArea.Count - 1);//更新结束节点
                    }
                    else
                    {
                        startPoint = new Point(0, 0);
                    }
                }
                else
                {
                    draw_state = CavasUtil.DRAW_STATE_DRAWING_DONE;
                    avoidLastStep();
                }
            }
            else if (draw_state == CavasUtil.DRAW_STATE_DRAWING_DONE)
            {
                draw_stateGotBack();
                if (!this.CurrentEditObstacle.Door.Equals(new Point(0, 0)))
                {
                    this.CurrentEditObstacle.Door = new Point(0, 0);
                }

            }
            //重新绘制面板
            drawCanvas();
        }

        private void clearEllipseOfGrid()
        {
            this.grid_info.Children.RemoveRange(2, this.grid_info.Children.Count - 2);

        }
        //绘制面板
        public void drawCanvas()
        {
            clearEllipseOfGrid();
            this.map_continer.Children.Clear();
            //绘制原有的东西
            drawFloor();
            //绘制缓存中的内容
            drawObstacles(this.allEditTempObstacle);
            //当前正在编辑的障碍物
            drawObstacle(this.CurrentEditObstacle);
            //绘制正在编辑的区域
            drawTempPoints();
        }
        public void drawTempPoints()
        {
            if (this.currentEditArea != null && this.currentEditArea.Count > 0)
            {
                Point startPoint = this.currentEditArea.ElementAt(0);
                Point endPoint = new Point(0, 0);
                CavasUtil.drawEllipse(this.grid_info, (int)startPoint.Y / 4, (int)startPoint.X / 4);
                for (int i = 1; i < this.currentEditArea.Count; i++)
                {

                    endPoint = this.currentEditArea.ElementAt(i);
                    CavasUtil.drawEllipse(this.grid_info, (int)endPoint.Y / 4, (int)endPoint.X / 4);
                    CavasUtil.drawLine(this.map_continer, startPoint, endPoint);
                    startPoint = endPoint;
                }
                foreach (Point p in this.currentEditArea)
                {

                }
            }
        }

        public void drawObstacle(Obstacle obstacle)
        {
            if (obstacle == null) return;
            if (obstacle.Boundary == null || obstacle.Boundary.Count <= 0) return;
            foreach (Point p in obstacle.Boundary)
            {
                CavasUtil.drawEllipse(this.grid_info, (int)p.Y / 4, (int)p.X / 4);
            }
            //闭合区域
            CavasUtil.drawPolygon(this.map_continer, obstacle.Boundary);
            //画门
            if (!obstacle.Door.Equals(new Point(0, 0)))
            {
                CavasUtil.drawEllipseDoor(this.grid_info, (int)obstacle.Door.Y / 4, (int)obstacle.Door.X / 4);
            }
        }

        public void drawObstacles(List<Obstacle> obstacles)
        {
            if (obstacles == null) return; ;
            foreach (Obstacle obstacle in obstacles)
            {
                if (!obstacle.Equals(this.CurrentEditObstacle))
                    drawObstacle(obstacle);
            }

        }
        /// <summary>
        /// 绘制楼层信息
        /// </summary>
        public void drawFloor()
        {
            if (this.CurrentEditMap != null)
            {
                drawObstacles(this.CurrentEditMap.Areas);
            }

        }

        /// <summary>
        /// 重画
        /// </summary>
        public void clearTemp()
        {
            this.allEditTempObstacle = new List<Obstacle>();
            this.CurrentEditObstacle = new Obstacle();
            this.currentEditArea = new List<Point>();
            drawCanvas();
            draw_state = CavasUtil.DRAW_STATE_DRAWING_AREA;
            startPoint = new Point(0,0);
        }
        /// <summary>
        /// 全部删除
        /// </summary>
        public void delData()
        {
            this.CurrentEditMap = null;
            this.CurrentEditFloor.Map = "";
            //数据库保存
            SqlHelper.updateFloor(this.CurrentEditFloor);
           // drawCanvas();
            //draw_state = CavasUtil.DRAW_STATE_DRAWING_AREA;
            init();
        }

        private bool judgeObstacleEqualse(Obstacle o, Obstacle obstacle)
        {
            bool flag = true;
            flag = o.Door.Equals(obstacle.Door) && o.Index == obstacle.Index && o.MaxX == obstacle.MaxX
                && o.MaxY == obstacle.MaxY && o.MinX == obstacle.MinX && o.MinY == obstacle.MinY;

            if (!flag)
            {
                if (o.Boundary == null && obstacle == null)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }


            }

            if (!flag)
            {
                if ((o.Boundary != null && obstacle.Boundary != null && o.Boundary.Count == obstacle.Boundary.Count))
                {
                    for (int i = 0; i < o.Boundary.Count; i++)
                    {
                        if (!o.Boundary.ElementAt(i).Equals(obstacle.Boundary.ElementAt(i)))
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// 从制定列表中删除障碍物
        /// </summary>
        /// <param name="o"></param>
        /// <param name="obstacles"></param>
        private void removeObstacle(Obstacle o, List<Obstacle> obstacles)
        {
            if (obstacles == null) return ;
            foreach (Obstacle obstacle in obstacles)
            {
                bool flag = judgeObstacleEqualse(o,obstacle);
                if (flag)
                {
                    obstacles.Remove(obstacle);

                    break;
                }
            }
        }

        /// <summary>
        /// 删除某区域
        /// </summary>
        /// <param name="o"></param>
        public void delObstacle(Obstacle o)
        {
            if (o == null) return;
           
            //绘画列表中删去
            removeObstacle(o, this.CurrentEditMap.Areas);
            removeObstacle(o, this.allEditTempObstacle);
            if (judgeObstacleEqualse(o, this.CurrentEditObstacle)) {
                this.CurrentEditObstacle = new Obstacle();
                this.currentEditArea = new List<Point>();
            }
            removeObstacle(o, new Obstacle[]{this.CurrentEditObstacle}.ToList());
            //地图中删去
           // fillMapState(this.mapValue, o, -1);
            drawCanvas();
            //更新关联的商铺
            if (o.Shop != null)
            {
                o.Shop.Door = new Point(0, 0);
                o.Shop.Floor = null;
                o.Shop.Index = "";
                SqlHelper.updateShop(o.Shop);//更新原有的商铺
            }

          //  draw_state = CavasUtil.DRAW_STATE_DRAWING_AREA;
        }
        /// <summary>
        /// 删除当前正在编辑的区域
        /// </summary>
        public void delCurrentEditObstacle()
        {
            this.CurrentEditObstacle = null;
            drawCanvas();
        }
        /// <summary>
        /// 设置当前障碍物的类型
        /// </summary>
        public void setCurrentObstacleType(string type)
        {
            this.CurrentEditObstacle.Type = type;
        }

        private void grid_info_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }

        private void btn_new_shop_Click(object sender, RoutedEventArgs e)
        {
            if(this.currentEditObstacle!=null)
                new NewShopWin(this.currentEditObstacle).ShowDialog();
        }

        private void btn_shopIn_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentEditObstacle != null) {
                new ShopInWin(this.currentEditObstacle).ShowDialog();
            }
        }

      
    }
}
