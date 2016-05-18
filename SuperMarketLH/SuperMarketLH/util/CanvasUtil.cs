using EntityManagementService.entity;
using EntityManagementService.nav;
using EntityManageService.sqlUtil;
using SuperMarketLH.usercontrl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SuperMarketLH.util
{
    public class CanvasUtil
    {

        public static Boolean isShowTwoFloors = false;//表示是否显示了两成地图
        public static int MAP_CANVAS_GRID_PIX_DIF = 4;
        public static int lINE_ADJUST_PIX = 2;//路线微调
        private static int LINE_STROKE = 4;
        private static Brush LINE_BRUSH = Brushes.LawnGreen;

        private static int USER_HEIGHT = 20;
        private static int USER_WIDHT = 10;
        private static int USER_COL_SPAN = 3;
        private static int USER_ROW_SPAN = 5;

        private static int MACHINE_COL_SPAN = 4;
        private static int MACHINE_ROW_SPAN = 4;
        private static int SHOP_INFO_TIPS_COL_SPAN = 20;
        private static int SHOP_INFO_TIPS_ROW_SPAN = 20;

        private static int COMMON_INFO_TIPS_COL_SPAN = 8;
        private static int COMMON_INFO_TIPS_ROW_SPAN = 8;

        private static int ELLIPSE_ROAD_WIDTH = 6;
        private static int ELLIPSE_ROAD_HEIGHT = 6;
        private static int ELLPSE_ROAD_COL_SPAN = 2;
        private static int ELLPSE_ROAD_ROW_SPAN = 2;
        private static Brush ELLPSE_ROAD_FILL = Brushes.Red;
        /// <summary>
        /// 在指定的位置显示机器标志
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public static void drawMachine(Grid grid, int row, int col)
        {
            int col_span_offset = MACHINE_COL_SPAN / 2;
            int row_span_offset = MACHINE_ROW_SPAN / 2;
            int row_actual = row - row_span_offset;
            int col_actual = col - col_span_offset;
            if (row_actual < 0)
            {
                row_actual = row;
            }
            if (col_actual < 0)
            {
                col_actual = col;
            }
            UserControlGps gps = new UserControlGps();
            grid.Children.Add(gps);
            Grid.SetColumn(gps, col_actual);
            Grid.SetColumnSpan(gps, MACHINE_COL_SPAN);
            Grid.SetRow(gps, row_actual);
            Grid.SetRowSpan(gps, MACHINE_ROW_SPAN);
        }

        /// <summary>
        /// 在指定的位置显示机器标志
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public static void drawMachine(Grid grid, Point p)
        {
            drawMachine(grid,getRowOfGridByPoint(p),getColOfGridByPoint(p));
        }
        /// <summary>
        /// 在指定的位置显示机器标志
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public static void drawMachine(Grid grid, Machine machine)
        {

            drawMachine(grid, machine.Y,machine.X);
        }
        /// <summary>
        /// 根据鼠标的点击的点，计算出对应在grid上的行号
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int getRowOfGridByPoint(Point p)
        {
            return (int)p.Y / 4;
        }
        /// <summary>
        /// 根据鼠标的点击的点，计算出对应在grid上的列号
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int getColOfGridByPoint(Point p)
        {
            return (int)p.X / 4;
        }
        /// <summary>
        /// 绘制商铺提示框
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="p"></param>
        public static void drawShopTips(Grid grid, Point p, Shop shop,NavToShop nav)
        {
            UserControlShopInfoTips tips = new UserControlShopInfoTips()
            {
                CurrentShop = shop,
                Nav = nav
            };
            grid.Children.Add(tips);
            int initialCalOfGrid = getColOfGridByPoint(p);
            int initialRowOfGrid = getRowOfGridByPoint(p);
            Grid.SetColumn(tips, initialCalOfGrid - SHOP_INFO_TIPS_COL_SPAN / 2 <= 0 ? 0 : initialCalOfGrid - SHOP_INFO_TIPS_COL_SPAN / 2);
            Grid.SetRow(tips, initialRowOfGrid - SHOP_INFO_TIPS_ROW_SPAN / 2 <= 0 ? 0 : initialRowOfGrid - SHOP_INFO_TIPS_ROW_SPAN / 2);
            Grid.SetColumnSpan(tips, SHOP_INFO_TIPS_COL_SPAN);
            Grid.SetRowSpan(tips, SHOP_INFO_TIPS_ROW_SPAN);
        }
        public static void drawShopTips(Grid grid, Point p, Shop shop, NavToShop nav,ShowDetailInfo showDetailInfo)
        {
            UserControlShopInfoTips tips = new UserControlShopInfoTips()
            {
                CurrentShop = shop,
                Nav = nav,
                ShowDetailInfo = showDetailInfo
            };
            grid.Children.Add(tips);
            int initialCalOfGrid = getColOfGridByPoint(p);
            int initialRowOfGrid = getRowOfGridByPoint(p);
            Grid.SetColumn(tips, initialCalOfGrid - SHOP_INFO_TIPS_COL_SPAN / 2<= 0 ? 0 : initialCalOfGrid - SHOP_INFO_TIPS_COL_SPAN / 2);
            Grid.SetRow(tips, initialRowOfGrid - SHOP_INFO_TIPS_ROW_SPAN +2 <= 0 ? 0 : initialRowOfGrid - SHOP_INFO_TIPS_ROW_SPAN+2 );
            //Grid.SetColumn(tips, initialCalOfGrid - SHOP_INFO_TIPS_COL_SPAN  <= 0 ? 0 : initialCalOfGrid - SHOP_INFO_TIPS_COL_SPAN);
            //Grid.SetRow(tips, initialRowOfGrid - SHOP_INFO_TIPS_ROW_SPAN  <= 0 ? 0 : initialRowOfGrid - SHOP_INFO_TIPS_ROW_SPAN );
            Grid.SetColumnSpan(tips, SHOP_INFO_TIPS_COL_SPAN);
            Grid.SetRowSpan(tips, SHOP_INFO_TIPS_ROW_SPAN);
        }

        /// <summary>
        /// 在地图上显示商铺的位置
        /// </summary>
        /// <param name="g"></param>
        /// <param name="map"></param>
        /// <param name="shop"></param>
        public static void drawShopTips(Grid g, Map map, Shop shop,NavToShop nav,ShowDetailInfo showDetailInfo)
        {
            foreach (Obstacle o in map.Areas)
            {
                if (o.Shop == null) continue;
                if (o.Shop.Id == shop.Id)
                {
                    drawShopTips(g, getMiddlePointOfArea(o), shop, nav, showDetailInfo);
                    break;
                }
            }
        }
        /// <summary>
        /// 如果指定障碍物入驻了商铺，则进行商铺提示
        /// </summary>
        /// <param name="g"></param>
        /// <param name="o"></param>
        /// <param name="shop"></param>
        public static void drawShopTips(Grid g,Obstacle o,NavToShop nav,ShowDetailInfo showDetailInfo) {
            releaseRoadTimer();//停止已经存在的计时器
            if (o.Type == ObstacleType.SHOP && o.Shop!=null && o.Shop.Id > 0 && SqlHelper.getShopById(o.Shop.Id) !=null)
            {
                Shop s = SqlHelper.getShopById(o.Shop.Id);
                o.Shop = s;
                if (s.Door.Equals(getPointOfGrid(o.Door)))
                {
                    drawShopTips(g, getMiddlePointOfArea(o), s, nav, showDetailInfo);
                }
            }
        }
        /// <summary>
        /// 绘制出公共建筑
        /// </summary>
        /// <param name="g"></param>
        /// <param name="o"></param>
        public static void drawCommonBuildingTips(Grid g,Obstacle o) {
          
            UserControlForCommonBuildings tips = new UserControlForCommonBuildings();
            g.Children.Add(tips);
            Point p = getMiddlePointOfArea(o);
            int initialCalOfGrid = getColOfGridByPoint(p);
            int initialRowOfGrid = getRowOfGridByPoint(p);

            Grid.SetColumn(tips, initialCalOfGrid - COMMON_INFO_TIPS_COL_SPAN / 2 <= 0 ? 0 : initialCalOfGrid - COMMON_INFO_TIPS_COL_SPAN / 2);
            Grid.SetRow(tips, initialRowOfGrid - COMMON_INFO_TIPS_ROW_SPAN / 2 <= 0 ? 0 : initialRowOfGrid - COMMON_INFO_TIPS_ROW_SPAN / 2);
            Grid.SetColumnSpan(tips, COMMON_INFO_TIPS_COL_SPAN);
            Grid.SetRowSpan(tips, COMMON_INFO_TIPS_ROW_SPAN);
        }


        /// <summary>
        /// 取得一块区域的中心点
        /// </summary>
        /// <returns></returns>
        private static Point getMiddlePointOfArea(Obstacle area)
        {
            //double sumX = 0;
            //double sumY = 0;
            //for (int i = 0; i < area.Boundary.Count; i++)
            //{
            //    sumX += area.Boundary.ElementAt(i).X;
            //    sumY += area.Boundary.ElementAt(i).Y;
            //}
            //return new Point(sumX / area.Boundary.Count, sumY / area.Boundary.Count);
            double areaM = 0;
            Point center = new Point(0,0);


            for (int i = 0; i < area.Boundary.Count - 1; i++)
            {
                Point p = area.Boundary.ElementAt(i);
                Point p1 = area.Boundary.ElementAt(i + 1);
                areaM += (p.X * p1.Y - p1.X * p.Y) / 2;
                center.X += (p.X * p1.Y - p1.X * p.Y) * (p.X + p1.X);
                center.Y += (p.X * p1.Y - p1.X * p.Y) * (p.Y + p1.Y);
            }
            Point p0 = area.Boundary.ElementAt(0);
            Point pn = area.Boundary.ElementAt(area.Boundary.Count-1);
            areaM += (area.Boundary.ElementAt(area.Boundary.Count - 1).X * area.Boundary.ElementAt(0).Y - area.Boundary.ElementAt(0).X * area.Boundary.ElementAt(area.Boundary.Count - 1).Y) / 2;
            center.X += (pn.X * p0.Y - p0.X * pn.Y) * (pn.X + p0.X);
            center.Y += (pn.X * p0.Y - p0.X * pn.Y) * (pn.Y + p0.Y);

            center.X /= 6 * areaM;
            center.Y /= 6 * areaM;

            return center;

        }


        /// <summary>
        /// 取得一块区域的中心点
        /// </summary>
        /// <returns></returns>
        private static Point getMiddlePointOfAreaByMMXY(Obstacle area)
        {
            if (area.MaxX == 0 && area.MaxY == 0 && area.MinX == 0 && area.MinY == 0)
            {
                area.MaxX = (int)MapUtil.getMaxX(area.Boundary);
                area.MaxY = (int)MapUtil.getMaxY(area.Boundary);
                area.MinX = (int)MapUtil.getMinX(area.Boundary);
                area.MinY = (int)MapUtil.getMinY(area.Boundary);
            }
            return new Point((area.MaxX + area.MinX) / 2, (area.MinY + area.MaxY) / 2);
        }
        /// <summary>
        /// 将绘画中的点变为grid上的点
        /// </summary>
        /// <returns></returns>
        public static Point getPointOfGrid(Point p)
        {
            return new Point(getColOfGridByPoint(p), getRowOfGridByPoint(p));
        }

        private static DispatcherTimer drawRoadTimer;

        private static int roadNodeIndex = 0;
        
        private static List<Node>[] nodes ;
        private static List<Node> currentRoadNodes;
        private static int roadListIndex = 0;
        private static int roadListCount = 0;
        private static Grid[] mapGrid;
        private static Grid curremtMapGrid;
        private static Canvas[] mapCanvas;
        private static Canvas currentMapCanvas;
        private static int mapGridIndex = 0;
        private static int mapCanvasIndex = 0;
        //判断是双层导航还是单层导航
       static  bool isOneFloor = true;//默认是单层导航
        /// <summary>
        /// 绘制路径
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="nodes"></param>
        public static void drawRoad(Grid[] grid, List<Node>[] node)
        {

            nodes = node;

            if (nodes != null && nodes.Length > 0)
            {
                roadListIndex = 0;
                currentRoadNodes = nodes[roadListIndex++];
                roadNodeIndex = currentRoadNodes.Count - 1;
                roadListCount = nodes.Length;
                
            }
            else {
                return;
            }

            mapGrid = grid;
            if (grid != null && grid.Length > 0 && grid.Length == nodes.Length)
            {
                mapGridIndex = 0;
                curremtMapGrid = mapGrid[mapGridIndex++];
            }
            else
            {
                return;
            }

            
            drawRoadTimer = new DispatcherTimer();
            drawRoadTimer.Tick += drawRoadTimer_Tick;

            drawRoadTimer.Interval = TimeSpan.FromMilliseconds(50);
            drawRoadTimer.IsEnabled = true;

        }

        public static void drawRoad(Canvas[] canvas, Grid[] grid, List<Node>[] node)
        {

            if (grid.Length >= 1) {
                isOneFloor = false;
            }
            nodes = node;

            if (nodes != null && nodes.Length > 0)
            {
                roadListIndex = 0;
                currentRoadNodes = nodes[roadListIndex++];
                roadNodeIndex = currentRoadNodes.Count - 1;
                roadListCount = nodes.Length;

            }
            else
            {
                return;
            }

            mapCanvas = canvas;
            mapGrid = grid;
            if (grid != null && grid.Length > 0 && grid.Length == nodes.Length)
            {
                mapGridIndex = 0;
                curremtMapGrid = mapGrid[mapGridIndex++];
            }
            else
            {
                return;
            }

            if (canvas != null && canvas.Length > 0 && canvas.Length == nodes.Length)
            {
                mapCanvasIndex = 0;
                currentMapCanvas = mapCanvas[mapCanvasIndex++];
            }
            else
            {
                return;
            }
            drawRoadOfLine();
            drawRoadTimer = new DispatcherTimer();
            drawRoadTimer.Tick += drawRoadTimer_Tick;
            drawRoadTimer.Interval = TimeSpan.FromMilliseconds(90);
            drawRoadTimer.IsEnabled = true;
        }

        public static void releaseRoadTimer(){
            if(drawRoadTimer!=null){
              drawRoadTimer.IsEnabled = false;
            }
           
        }
        /// <summary>
        /// 把路径使用圆圈画出来
        /// <param name="isClearPrevious">true 清除上一个点</param>
        /// </summary>
        private static void drawRoadOfEllipse(bool isClearPrevious,string tipType) {
            if (currentRoadNodes == null)
            {
                releaseRoadTimer();
                return;
            }
            if (roadNodeIndex < 0)
            {
                if (roadListIndex >= roadListCount)
                {
                    releaseRoadTimer();
                    return;
                }
                curremtMapGrid = mapGrid[mapGridIndex++];
                currentRoadNodes = nodes[roadListIndex++];
                roadNodeIndex = currentRoadNodes.Count - 1;
            }
           


            if (isClearPrevious)
            {
                if (isOneFloor)
                {
                    curremtMapGrid.Children.RemoveRange(3, curremtMapGrid.Children.Count - 1);
                }
                else {
                    if (roadListIndex > 1)
                    {
                        //清除上一层的东西
                        mapGrid[roadListIndex - 2].Children.RemoveRange(3, mapGrid[roadListIndex - 2].Children.Count - 1);
                        curremtMapGrid.Children.RemoveRange(2, curremtMapGrid.Children.Count - 1);//总是从机器位置开始导航，所以当不是导航第一层的时候，地图上没有有机器位置的标志了
                    }
                    else {
                        curremtMapGrid.Children.RemoveRange(3, curremtMapGrid.Children.Count - 1);//原有机器位置，地图背景，画布三个东西
                    }
                }
               
            }
            Point p = currentRoadNodes.ElementAt(roadNodeIndex--).P;
           // roadNodeIndex--;//跳跃
            if (tipType.Equals("ellipse")) {
                Ellipse ellipse = new Ellipse()
                {
                    Width = ELLIPSE_ROAD_WIDTH,
                    Height = ELLIPSE_ROAD_HEIGHT,
                    Fill = ELLPSE_ROAD_FILL
                };
                curremtMapGrid.Children.Add(ellipse);
                Grid.SetRow(ellipse, (int)p.Y);
                Grid.SetColumn(ellipse, (int)p.X);
                Grid.SetColumnSpan(ellipse, ELLPSE_ROAD_COL_SPAN);
                Grid.SetRowSpan(ellipse, ELLPSE_ROAD_ROW_SPAN);
            }
            else if (tipType.Equals("triangle")) {

                PathGeometry myPathGeometry = new PathGeometry();
                PathFigure pathFigure1 = new PathFigure();



                myPathGeometry.Figures.Add(pathFigure1);
                Path myPath = new Path();
                myPath.Stroke = LINE_BRUSH;
                myPath.StrokeThickness = LINE_STROKE;
                myPath.Data = myPathGeometry;
                currentMapCanvas.Children.Add(myPath);
            }
            else if (tipType.Equals("user"))
            {
                BitmapImage bitmap = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"resource/images/people.png")));
             
                Image image = new Image();
                image.Width = USER_WIDHT;
                image.Height = USER_HEIGHT;
                image.Source = bitmap;
                curremtMapGrid.Children.Add(image);
                Grid.SetRow(image, (int)p.Y - USER_ROW_SPAN+1);
                Grid.SetColumn(image, (int)p.X - USER_COL_SPAN/2);
                Grid.SetColumnSpan(image, USER_COL_SPAN);
                Grid.SetRowSpan(image, USER_ROW_SPAN);
            }
          
        }
        /// <summary>
        /// 把路径使用线画出来
        /// </summary>
        private static void drawRoadOfLine()
        {
            if (nodes == null || nodes.Length<=0)
            {
                return;
            }


            for (int i = 0; i < nodes.Length; i++)
            {
                currentMapCanvas = mapCanvas[i];
                Point startP = new Point(0, 0);
                Point endP = new Point(0, 0);
                Node startNode = null;
                PathGeometry myPathGeometry = new PathGeometry();
                PathFigure pathFigure1 = new PathFigure();

                if (nodes[i] != null) {
                    foreach (Node item in nodes[i])
                    {
                        if (startNode == null)
                        {
                            startNode = item;
                            startP = item.P;
                            pathFigure1.StartPoint = new Point(startP.X * MAP_CANVAS_GRID_PIX_DIF + lINE_ADJUST_PIX, startP.Y * MAP_CANVAS_GRID_PIX_DIF + lINE_ADJUST_PIX);
                            continue;
                        }
                        endP = item.P;
                        LineSegment ls = new LineSegment(
                       new Point(endP.X * MAP_CANVAS_GRID_PIX_DIF + lINE_ADJUST_PIX, endP.Y * MAP_CANVAS_GRID_PIX_DIF + lINE_ADJUST_PIX),
                       true /* IsStroked */ );
                        ls.IsSmoothJoin = true;
                        pathFigure1.Segments.Add(ls);
                    }

                    myPathGeometry.Figures.Add(pathFigure1);
                    Path myPath = new Path();
                    myPath.Stroke = LINE_BRUSH;
                    myPath.StrokeThickness = LINE_STROKE;
                    myPath.Data = myPathGeometry;
                    currentMapCanvas.Children.Add(myPath);
                }


            }
           
        }

        static void drawRoadTimer_Tick(object sender, EventArgs e)
        {
            drawRoadOfEllipse(true, "user");
        }


    }
}
