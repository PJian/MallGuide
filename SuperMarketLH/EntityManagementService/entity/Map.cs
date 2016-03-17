using EntityManagementService.nav;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace EntityManagementService.entity
{
    [Serializable] 
    public class Map
    {
        Point _currentMachine;

        public Point CurrentMachine
        {
            get { return _currentMachine; }
            set { _currentMachine = value; }
        }

        private int _width = 180;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private int _height = 90;

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        /// <summary>
        /// 地图区域
        /// </summary>
        private List<Obstacle> _areas;

        public List<Obstacle> Areas
        {
            get { return _areas; }
            set { _areas = value; }
        }

        /// 地图内容，障碍物数组，用于寻路
        /// <summary>
        /// 值为 1 表示该点是不能通过的
        /// 值为 0 表示该点没有东西，可以通过
        /// </summary>

        private int[,] _obstacles;
        public int[,] Obstacles
        {
            get { return this._obstacles; }
            set { _obstacles = value; }
        }
    }
}
