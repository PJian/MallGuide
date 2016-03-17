using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace EntityManagementService.nav
{
    public class Node
    {
        Node _parent;

        public Node Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        Point _p;

        public Point P
        {
            get { return _p; }
            set { _p = value; }
        }
        int _g;

        public int G
        {
            get { return _g; }
            set { _g = value; }
        }
        int _h;

        public int H
        {
            get { return _h; }
            set { _h = value; }
        }
        int _f;

        public int F
        {
            get { return _f; }
            set { _f = value; }
        }

    }
}
