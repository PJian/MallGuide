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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SuperMarketLH.util
{
    public class CanvasUtil
    {
        public static int MAP_CANVAS_GRID_PIX_DIF = 4;

        private static int MACHINE_COL_SPAN = 4;
        private static int MACHINE_ROW_SPAN = 4;
        private static int SHOP_INFO_TIPS_COL_SPAN = 20;
        private static int SHOP_INFO_TIPS_ROW_SPAN = 20;

        private static int ELLIPSE_ROAD_WIDTH = 2;
        private static int ELLIPSE_ROAD_HEIGHT = 2;
        private static int ELLPSE_ROAD_COL_SPAN = 1;
        private static int ELLPSE_ROAD_ROW_SPAN = 1;
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
            Grid.SetColumn(tips, initialCalOfGrid - SHOP_INFO_TIPS_COL_SPAN / 2 <= 0 ? 0 : initialCalOfGrid - SHOP_INFO_TIPS_COL_SPAN / 2);
            Grid.SetRow(tips, initialRowOfGrid - SHOP_INFO_TIPS_ROW_SPAN / 2 <= 0 ? 0 : initialRowOfGrid - SHOP_INFO_TIPS_ROW_SPAN / 2);
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
            
            if (o.Type == ObstacleType.SHOP && o.Shop!=null && o.Shop.Id > 0 && SqlHelper.getShopById(o.Shop.Id) !=null)
            {
                Shop s = SqlHelper.getShopById(o.Shop.Id);
                if (s.Door.Equals(getPointOfGrid(o.Door)))
                {
                    drawShopTips(g, getMiddlePointOfArea(o), o.Shop, nav, showDetailInfo);
                }
            }
        }

        /// <summary>
        /// 取得一块区域的中心点
        /// </summary>
        /// <returns></returns>
        private static Point getMiddlePointOfArea(Obstacle area)
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
        private static int mapGridIndex = 0;
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



        public static void releaseRoadTimer(){
            if(drawRoadTimer!=null){
              drawRoadTimer.IsEnabled = false;
            }
           
        }
        static void drawRoadTimer_Tick(object sender, EventArgs e)
        {
            if (currentRoadNodes == null)
            {
               releaseRoadTimer();
               return ;
            }
            if (roadNodeIndex < 0) {
                if (roadListIndex >= roadListCount) {
                    releaseRoadTimer();
                    return;
                }
                curremtMapGrid = mapGrid[mapGridIndex++];
                currentRoadNodes = nodes[roadListIndex++];
                roadNodeIndex = currentRoadNodes.Count - 1;
            }

                Point p = currentRoadNodes.ElementAt(roadNodeIndex--).P;
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


    }
}
