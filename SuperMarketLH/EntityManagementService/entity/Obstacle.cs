using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace EntityManagementService.entity
{
    /// <summary>
    /// 地图障碍物
    /// </summary>
    [Serializable] 
    public class Obstacle
    {
        private Point _door;

        public Point Door
        {
            get { return _door; }
            set { _door = value; }
        }

        private Shop _shop;//入驻商铺

        public Shop Shop
        {
            get { return _shop; }
            set { _shop = value; }
        }


        private string _index;

        public string Index
        {
            get { return _index; }
            set { _index = value; }
        }

        //private int _index;//编号

        //public int Index
        //{
        //    get { return _index; }
        //    set { _index = value; }
        //}
        private string _type;//类型，商铺与公共设施，公共设施中还有电梯啊

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 一系列的点，用来表示区域的边界
        /// </summary>
        private List<Point> _boundary;


        public List<Point> Boundary
        {
            get { return _boundary; }
            set { _boundary = value; }
        }
        private int _minX;

        public int MinX
        {
            get { return _minX; }
            set { _minX = value; }
        }
        private int _minY;

        public int MinY
        {
            get { return _minY; }
            set { _minY = value; }
        }
        private int _maxX;

        public int MaxX
        {
            get { return _maxX; }
            set { _maxX = value; }
        }
        private int _maxY;

        public int MaxY
        {
            get { return _maxY; }
            set { _maxY = value; }
        }

    }
}
