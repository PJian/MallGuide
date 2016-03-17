using EntityManagementService.entity;
using EntityManagementService.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace EntityManagementService.nav
{
    public class AStar
    {
        private static int Ystep = 1;
        private static int Xstep = 1;
       // private static int roadNodeRange = 3;
        private static int DIRECTIVE_COST = 10;
        private static int NON_DIRECTIVE_COST = 14;
        private static bool SEARCH_NON_DIRECTIVE = true;
        private static int DIS_OF_TWO_POINT = 1;//在终点附近的距离定义
       // private static int MAP_WIDTH = 180;
        //private static int MAP_HEIGHT = 90;
        private static int DIRECTIVE_DISTANCE_COST = 10;//H
        /// <summary>
        /// 得到路径点
        /// </summary>
        /// <param name="map"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        //public static List<Node> getRoadNode(Map map, Point startPoint,Point endPoint)
        //{
        //    List<Node> road = new List<Node>();
        //    List<Node> roadNode = new List<Node>();
        //    road = findRoadAH(map.Obstacles, startPoint,endPoint);
        //    for (int i = road.Count - 1; i >= 0; i--)
        //    {
        //        if (i % roadNodeRange == 0)//表示间隔多少个点进行绘制
        //        {
        //            //使用动画将路径画出来
        //            Node node = road.ElementAt(i);
        //            roadNode.Add(node);
        //        }
        //    }
        //    return roadNode;
        //}
        /// <summary>
        /// A* 寻路 2叉堆
        /// </summary>
        /// <param name="obstacles">地图信息 使用而为数组进行表示，1表示当前点位障碍物，0表示当前点可以通过</param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static List<Node> findRoadAH(int[,] obstacles, Point startPoint,Point endPoint)
        {
            //判断一下方向
            int direction = 0;
            BinaryHeap openList = new BinaryHeap();
            List<Node> closeList = new List<Node>();
            List<Node> roadList = new List<Node>();
            //判断你是否点击在了开始位置
            if (startPoint.Equals(endPoint))
            {
                return roadList;//返回null表示没有路径，若是roadList的count ==0 表示就在当前点击的位置的位置
            }

            Node initNode = new Node();
            initNode.P = startPoint;
            initNode.Parent = null;
            initNode.F = 0;
            initNode.H = 0;
            initNode.G = 0;
            //加入openlist中区
            closeList.Add(initNode);
            Node currentNode = initNode;
            int ix = (int)initNode.P.X;
            int iy = (int)initNode.P.Y;
            //判断目标点是不是有障碍，或者是不是死路
            if (obstacles[(int)endPoint.Y,(int)endPoint.X] == 1)
            {
                return new List<Node>();
            }
            //A*开始
            while (true)
            {
                if (ix < endPoint.X && iy < endPoint.Y)
                {
                    direction = 1;//开始节点在结束节点的左上方，此时，开始节点应该王右下角寻路   左，左上就不找 
                }
                if (ix < endPoint.X && iy > endPoint.Y)
                {
                    direction = 2;//开始节点在结束节点的左下方，此时，开始节点应该王右上角寻路   左，左下就不找
                }
                if (ix > endPoint.X && iy < endPoint.Y)
                {
                    direction = 3;//开始节点在结束节点的右上方，此时，开始节点应该王左下角寻路   右，右上就不找
                }
                if (ix > endPoint.X && iy > endPoint.Y)
                {
                    direction = 4;//开始节点在结束节点的右下方，此时，开始节点应该王左上角寻路   右下就不找
                }
                //得到当前节点
                //添加当前节点的上下左右，左上，左下，右上，右下共8个节点
                Node up = createRoadNodeH(currentNode, DIRECTIVE_COST, new Point(ix, iy - Ystep), endPoint, currentNode, obstacles, openList, closeList);
                Node down = createRoadNodeH(currentNode, DIRECTIVE_COST, new Point(ix, iy + Ystep), endPoint, currentNode, obstacles, openList, closeList);
                Node left = createRoadNodeH(currentNode, DIRECTIVE_COST, new Point(ix - Xstep, iy), endPoint, currentNode, obstacles, openList, closeList);
                Node right = createRoadNodeH(currentNode, DIRECTIVE_COST, new Point(ix + Xstep, iy), endPoint, currentNode, obstacles, openList, closeList);

                if (up != null)
                {
                    openList.insert(up);
                }
                if (down != null)
                {
                    openList.insert(down);
                }
                if (left != null)
                {
                    openList.insert(left);

                }
                if (right != null)
                {
                    openList.insert(right);
                }
                
                if (SEARCH_NON_DIRECTIVE) {
                    Node left_up = null;
                    if (direction != 1)
                    {
                        left_up = createRoadNodeH(currentNode, NON_DIRECTIVE_COST, new Point(ix - Xstep, iy - Ystep), endPoint, currentNode, obstacles, openList, closeList);
                    }
                    Node left_down = null;
                    if (direction != 2)
                    {
                        left_down = createRoadNodeH(currentNode, NON_DIRECTIVE_COST, new Point(ix - Xstep, iy + Ystep), endPoint, currentNode, obstacles, openList, closeList);
                    }
                    Node right_up = null;
                    if (direction != 3)
                    {
                        right_up = createRoadNodeH(currentNode, NON_DIRECTIVE_COST, new Point(ix + Xstep, iy - Ystep), endPoint, currentNode, obstacles, openList, closeList);
                    }
                    Node right_down = null;
                    if (direction != 4)
                    {
                        right_down = createRoadNodeH(currentNode, NON_DIRECTIVE_COST, new Point(ix + Xstep, iy + Ystep), endPoint, currentNode, obstacles, openList, closeList);
                    }

                    if (left_up != null)
                    {
                        openList.insert(left_up);
                    }
                    if (left_down != null)
                    {
                        openList.insert(left_down);
                    }
                    if (right_up != null)
                    {
                        openList.insert(right_up);

                    }
                    if (right_down != null)
                    {
                        openList.insert(right_down);

                    }
                }
                //当前点周围的8个点添加完毕

                //判断列表是否已经空了
                if (openList.getHeapSize() == 0)
                {
                    break;
                }
                //取得最小F值得节点
                currentNode = openList.getHeap()[0];
                closeList.Add(currentNode);
                openList.remove(0);
                ix = (int)currentNode.P.X;
                iy = (int)currentNode.P.Y;

                //判断是否找到了路径
                if (Math.Abs(ix - endPoint.X) <= DIS_OF_TWO_POINT && Math.Abs(iy - endPoint.Y) <= DIS_OF_TWO_POINT)
                {
                    break;
                }
            }//A* 寻路结束

            //沿着closed中的最后一个节点的父节点的链往前找，得到路径
            Node node = closeList.ElementAt(closeList.Count - 1);


            while (node != null&& node.Parent!=null && !node.Parent.Equals(startPoint))
            {
                roadList.Add(node);
                node = node.Parent;
            }
            return roadList;
        }

        /// <summary>
        /// 创建A* 寻路节点
        /// </summary>
        /// <param name="minFNode"></param>
        /// <param name="costG"></param>
        /// <param name="p"></param>
        /// <param name="endPoint"></param>
        /// <param name="xie"></param>
        /// <param name="parent"></param>
        /// <param name="obstacles"></param>
        /// <param name="openList"></param>
        /// <param name="closedList"></param>
        /// <returns></returns>
        public static Node createRoadNodeH(Node minFNode, int costG, Point p, Point endPoint, Node parent, int[,] obstacles, BinaryHeap openList, List<Node> closedList)
        {
            int indexOfOpenList;
            //是否到达边界
            if (p.X < 0 || p.X >= MapUtil.MAP_WIDHT || p.Y < 0 || p.Y >= MapUtil.MAP_HEIGHT)
            {
                return null;
            }
            //是否为障碍物
            if (obstacles[(int)p.Y,(int)p.X] == 1)
            {
                return null;
            }
            //是否在closelist中,同一个点只会或者在openlist中或者在closedlist中，不会同时都在的
            if ((indexOfOpenList = isInNodeList(p, closedList)) != -1)
            {
                return null;

            }else if ((indexOfOpenList = openList.isInHeap(p)) != -1)
            {
                //是否在openlist中
                //需要更新openlist中的相应节点，如果代价更小的话，就需要更新节点信息
                int new_g = minFNode.G+ costG;
                Node temp = openList.getHeap()[indexOfOpenList];
                if (new_g < temp.G)
                {
                    //具有更小的g值
                    //更新该点
                    openList.getHeap()[indexOfOpenList].G = (new_g);
                    openList.getHeap()[indexOfOpenList].Parent = (minFNode);
                    openList.getHeap()[indexOfOpenList].F = (temp.H + new_g);
                    //从新排列
                    openList.Min_heap(indexOfOpenList);
                }
                return null;
            }
            //生成节点
            int cx = Math.Abs((int)p.X - (int)endPoint.X);
            int cy = Math.Abs((int)p.Y - (int)endPoint.Y);

            Node node = new Node();
            node.Parent = parent;
            node.P = p;
            if (parent != null)
            {
                node.G = parent.G + costG;
                node.H = ((int)Math.Sqrt(cx * cx + cy * cy) * DIRECTIVE_DISTANCE_COST);
                node.F = (node.G + node.H);
            }
            return node;

        }

        //判断是否已经在openList或者closedList中了
        public static int isInNodeList(Point p, List<Node> roadNodes)
        {
            for (int i = 0; i < roadNodes.Count; i++)
            {
                if (roadNodes.ElementAt(i) != null)
                {
                    if (p.Equals(roadNodes.ElementAt(i).P))
                    {
                        return i;
                    }
                }

            }
            return -1;
        }

        /// <summary>
        ///  寻路算法
        ///     根据起始位置，在map中规划处一系列的点，到达endPoint 处
        /// </summary>
        /// <param name="map"></param>
        /// <param name="start"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static List<Point> findRoad(Map map, Point start, Point endPoint)
        {
            List<Point> points = new List<Point>();
            foreach (Node node in findRoadAH(map.Obstacles,start,endPoint))
	        {
		        points.Add(node.P);
	        }
            return points;
             
        }
        /// <summary>
        /// 寻路算法
        ///     从当前位置，寻路到指定的商铺
        /// </summary>
        /// <param name="currentMachineFloor"></param>
        /// <param name="shop"></param>
        /// <param name="currentPoint"></param>
        /// <returns>
        ///     List<Point>[] {
        ///         0: 当前层的路径
        ///         1: 下一层的路径
        ///     }
        /// </returns>
        public static List<Point>[] findRoad(Floor currentMachineFloor, Shop shop, Point currentPoint)
        {
            //找到目标楼层
            //在同一层，直接寻路
            List<Point>[] points = new List<Point>[2];
            if (currentMachineFloor.Index == shop.Floor.Index)
            {
                Map currentMap = SerialUtil.readMap(currentMachineFloor);
                points[0] = findRoad(currentMap, getObstacleRelatedShop(currentMap, shop).Door, currentPoint);
            }
            else
            {
                //不在同层，通过电梯寻路
                Floor destFloor = shop.Floor;
                Map currentMap = SerialUtil.readMap(currentMachineFloor);
                Obstacle elevater = findElevator(currentMap.Areas, currentMap.CurrentMachine);
                //寻路到电梯
                points[0] = findRoad(currentMap, currentPoint, elevater.Door);
                //另一层电梯的位置
                Map destMap = SerialUtil.readMap(destFloor);
                points[1] = findRoad(destFloor, shop, getElevaterPoint(destMap, elevater.Index))[0];
            }
            return points;
        }


        /// <summary>
        /// 在地图上找到商铺的位置
        /// </summary>
        /// <param name="map"></param>
        /// <param name="shop"></param>
        /// <returns></returns>
        private static Obstacle getObstacleRelatedShop(Map map, Shop shop)
        {
            foreach (Obstacle o in map.Areas)
            {
                if (o.Shop.Id == shop.Id)
                {
                    return o;
                }
            }
            return null;
        }

        /// <summary>
        /// 在楼层中找到指定编号的电梯们
        /// </summary>
        /// <param name="map"></param>
        /// <param name="elevaterNum"></param>
        /// <returns></returns>
        private static Point getElevaterPoint(Map map, int elevaterNum)
        {
            foreach(Obstacle o in map.Areas)
            {
                if (o.Index == elevaterNum)
                {
                    return o.Door;
                }
            }
            return new Point(-1, -1);
        }

        /// <summary>
        /// 找到离商铺最近的电梯
        /// </summary>
        /// <param name="obstacles"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        private static Obstacle findElevator(List<Obstacle> obstacles, Point point)
        {
            if (obstacles == null || obstacles.Count <= 0) return null;
            double minDistance = getDistance(obstacles.ElementAt(0).Door, point);
            Obstacle rtn = obstacles.ElementAt(0);
            for (int i = 1; i < obstacles.Count; i++)
            {
                double dis = getDistance(obstacles.ElementAt(i).Door, point);
                if (dis < minDistance)
                {
                    minDistance = dis;
                    rtn = obstacles.ElementAt(i);
                }
            }
            return rtn;
        }
        /// <summary>
        /// 计算两点之间的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private static double getDistance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
    }
}
