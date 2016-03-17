using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityManagementService.entity
{
    /// <summary>
    /// 楼层实体类
    /// </summary>
    [Serializable]
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
        private string _map;

        public string Map
        {
            get { return _map; }
            set { _map = value; }
        }

        private string _label;

        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj is Floor)
            {
                Floor f = (Floor)obj;
                return f.Id == this.Id;
            }
            else {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
