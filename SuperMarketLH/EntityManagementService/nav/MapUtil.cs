using EntityManagementService.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace EntityManagementService.nav
{
    public class MapUtil
    {

        public static int MAP_WIDHT = 180;
        public static int MAP_HEIGHT = 90;
        public static double getMinX(List<Point> ps)
        {
            if (ps.Count <= 0) return 0;
            double minX = ps.ElementAt(0).X;
            for (int i = 0; i < ps.Count; i++)
            {
                if (minX > ps.ElementAt(i).X)
                {
                    minX = ps.ElementAt(i).X;
                }
            }
            return minX;
        }
        public static double getMaxX(List<Point> ps)
        {
            if (ps.Count <= 0) return 0;
            double maxX = ps.ElementAt(0).X;
            for (int i = 0; i < ps.Count; i++)
            {
                if (maxX < ps.ElementAt(i).X)
                {
                    maxX = ps.ElementAt(i).X;
                }
            }
            return maxX;
        }
        public static double getMinY(List<Point> ps)
        {
            if (ps.Count <= 0) return 0;
            double minY = ps.ElementAt(0).Y;
            for (int i = 0; i < ps.Count; i++)
            {
                if (minY > ps.ElementAt(i).Y)
                {
                    minY = ps.ElementAt(i).Y;
                }
            }
            return minY;
        }
        public static double getMaxY(List<Point> ps)
        {
            if (ps.Count <= 0) return 0;
            double maxY = ps.ElementAt(0).Y;
            for (int i = 0; i < ps.Count; i++)
            {
                if (maxY < ps.ElementAt(i).Y)
                {
                    maxY = ps.ElementAt(i).Y;
                }
            }
            return maxY;
        }


        /// <summary>
        /// 填充临时地图的状态
        /// </summary>
        public static void fillMapState(int[,] maps, Obstacle obstacle, int value)
        {
            int minX = (int)getMinX(obstacle.Boundary);
            int minY = (int)getMinY(obstacle.Boundary);
            int maxX = (int)getMaxX(obstacle.Boundary);
            int maxY = (int)getMaxY(obstacle.Boundary);

            obstacle.MinX = minX;
            obstacle.MaxX = maxX;
            obstacle.MinY = minY;
            obstacle.MaxY = maxY;


            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    if (MapUtil.pnpoly(new Point(i, j), obstacle.Boundary))
                    {
                        maps[j / 4, i / 4] = value;
                    }
                }
            }
        }

        /// <summary>
        /// 填充临时地图的状态
        /// </summary>
        public static int[,] fillMapState(Map map)
        {
            int[,] mapState = new int[MAP_HEIGHT, MAP_WIDHT];
            foreach (Obstacle o in map.Areas)
            {
                fillMapState(mapState, o, 1);
            }
            return mapState;
        }

        /// <summary>
        /// 创建每层地图用于寻路用的障碍物数组
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public static int[,] createObstaclesArra(Map map)
        {
            int[,] contents = new int[0, 0];
            if (map != null)
            {
                contents = new int[map.Height, map.Width];
                foreach (Obstacle o in map.Areas)
                {
                    fillMapState(contents, o, 1);
                }
            }
            return contents;
        }

        /// <summary>
        ///  判断点是否在多边形内
        /// </summary>
        /// <param name="point"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public static bool pnpoly(Point point, Obstacle area)
        {
            int i, j;
            bool c = false;
            int nvert = area.Boundary.Count;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                double verty = area.Boundary.ElementAt(i).Y;
                double testy = point.Y;
                double testx = point.X;
                if (((area.Boundary.ElementAt(i).Y > testy) != (area.Boundary.ElementAt(j).Y > testy)) &&
        (testx < (area.Boundary.ElementAt(j).X - area.Boundary.ElementAt(i).X) * (testy - area.Boundary.ElementAt(i).Y) / (area.Boundary.ElementAt(j).Y - area.Boundary.ElementAt(i).Y) + area.Boundary.ElementAt(i).X))
                    c = !c;
            }
            return c;
        }
        /// <summary>
        ///  判断点是否在多边形内
        /// </summary>
        /// <param name="point"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public static bool pnpoly(Point point, List<Point> points)
        {
            int i, j;
            bool c = false;
            int nvert = points.Count;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                double verty = points.ElementAt(i).Y;
                double testy = point.Y;
                double testx = point.X;
                if (((points.ElementAt(i).Y > testy) != (points.ElementAt(j).Y > testy)) &&
        (testx < (points.ElementAt(j).X - points.ElementAt(i).X) * (testy - points.ElementAt(i).Y) / (points.ElementAt(j).Y - points.ElementAt(i).Y) + points.ElementAt(i).X))
                    c = !c;
            }
            return c;
        }

        /// <summary>
        /// 取得用户点击的区域
        /// </summary>
        /// <param name="point"> 鼠标点击的坐标</param>
        /// <param name="areas">全部的区域</param>
        /// <returns></returns>
        public static Obstacle getClickArea(Point point, List<Obstacle> obstacles)
        {
            if (obstacles == null)
            {
                return null;
            }
            for (int i = 0; i < obstacles.Count; i++)
            {
                Obstacle obstacle = obstacles.ElementAt(i);
                if (point.X >= obstacle.MinX && point.X <= obstacle.MaxX && point.Y >= obstacle.MinY && point.Y <= obstacle.MaxY)
                {

                    if (pnpoly(point, obstacle))
                    {
                        return obstacle;
                    }
                }
            }
            return null;
        }

        public static bool getClickArea(Point temp, Obstacle obstacle)
        {
            if (pnpoly(temp, obstacle))
            {
                return true;
            }
            return false;
        }
        public static double getDistanceOfTwoPoint(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        /// <summary>
        /// 在地图上找出距离指定点最近的电梯
        /// </summary>
        /// <param name="p"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Point getNearestElevatorPosstion(Point p, Map map)
        {
            int index = 1;
            double minDistance = 0;
            Point pp = new Point(0, 0);
            foreach (Obstacle o in map.Areas)
            {
                if (o.Type == ObstacleType.ELEVATOR)
                {
                    double dis = getDistanceOfTwoPoint(p, o.Door);
                    if (index == 1)
                    {
                        minDistance = dis;
                        pp = o.Door;
                        index = 2;
                    }
                    else if (dis < minDistance)
                    {
                        minDistance = dis;
                        pp = o.Door;
                    }
                }
            }
            return pp;
        }
        /// <summary>
        /// 在地图上找出距离指定点最近的电梯
        /// </summary>
        /// <param name="p"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Obstacle getNearestElevator(Point p, Map map)
        {
            return getNearestTransportation(p, map, ObstacleType.ELEVATOR);
        }
        /// <summary>
        /// 找到最近的扶梯
        /// </summary>
        /// <param name="p"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Obstacle getNearestEscalator(Point p, Map map)
        {
            return getNearestTransportation(p, map, ObstacleType.ESCALATOR);
        }
        /// <summary>
        /// 找到最近的楼梯
        /// </summary>
        /// <param name="p"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Obstacle getNearestStair(Point p, Map map)
        {
            return getNearestTransportation(p, map, ObstacleType.STAIRS);
        }

        /// <summary>
        /// 找到最近的交通工具
        /// </summary>
        /// <param name="p"></param>
        /// <param name="map"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Obstacle getNearestTransportation(Point p, Map map, string type)
        {
            int index = 1;
            double minDistance = 0;
            Obstacle temp = null;
            Point pp = new Point(0, 0);
            foreach (Obstacle o in map.Areas)
            {
                if (o.Type == type)
                {
                    double dis = getDistanceOfTwoPoint(p, o.Door);
                    if (index == 1)
                    {
                        minDistance = dis;
                        pp = o.Door;
                        temp = o;
                        index = 2;
                    }
                    else if (dis < minDistance)
                    {
                        minDistance = dis;
                        pp = o.Door;
                        temp = o;
                    }
                }
            }
            return temp;
        }



        /// <summary>
        /// 根据电梯编号找到电梯
        /// </summary>
        /// <param name="map"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static Obstacle getElevatorByNum(Map map, string num)
        {
            return getTransportationByNumAndType(map,num,ObstacleType.ELEVATOR);
        }

        public static Obstacle getEscalatorByNum(Map map, string num)
        {
            return getTransportationByNumAndType(map, num, ObstacleType.ESCALATOR);
        }

        /// <summary>
        /// 任选一个扶梯
        /// </summary>
        /// <param name="map"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static Obstacle getEscalator(Map map)
        {
            return getTransportationByType(map, ObstacleType.ESCALATOR);
        }

        public static Obstacle getStairByNum(Map map, string num)
        {
            return getTransportationByNumAndType(map, num, ObstacleType.STAIRS);
        }

        public static Obstacle getStair(Map map) {
            return getTransportationByType(map, ObstacleType.STAIRS);
        }

        public static Obstacle getTransportationByNumAndType(Map map, String num, String type)
        {
            if (map != null)
            {
                foreach (Obstacle o in map.Areas)
                {
                    if (o.Type.Equals(type) && o.Index.Equals(num))
                    {
                        return o;
                    }
                }
            }
            return null;
        }
        public static Obstacle getTransportationByType(Map map ,String type)
        {
            if (map != null)
            {
                foreach (Obstacle o in map.Areas)
                {
                    if (o.Type.Equals(type) )
                    {
                        return o;
                    }
                }
            }
            return null;
        }
    }
}
