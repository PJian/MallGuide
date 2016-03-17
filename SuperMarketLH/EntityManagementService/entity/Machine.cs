using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityManagementService.entity
{
    /// <summary>
    /// 机器位置，寻路的开始位置
    /// 每台机器都有单独的一个位置
    /// </summary>
    [Serializable]
    public class Machine
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _floorIndex;

        public int FloorIndex
        {
            get { return _floorIndex; }
            set { _floorIndex = value; }
        }

        private int _x;

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }
        private int _y;

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }
    }
}
