using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityManageService.entity
{
    /// <summary>
    /// 楼层实体类
    /// </summary>
    public class Floor
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _img;

        public string Img
        {
            get { return _img; }
            set { _img = value; }
        }
        private int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
}
