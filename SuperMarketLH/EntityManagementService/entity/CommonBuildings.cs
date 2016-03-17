using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityManagementService.entity
{
    public class CommonBuildings
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
        private string _logo;

        public string Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }
        private string _sortChar;

        public string SortChar
        {
            get { return _sortChar; }
            set { _sortChar = value; }
        }
    }
}
