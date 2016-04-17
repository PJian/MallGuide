using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarketLHS.uientity
{
    public class PerformanceData
    {
        public PerformanceData(string name, string quarter, double performance)
        {
            this._name = name;
            this._quarter = quarter;
            this._performance = performance;
        }

        private string _quarter;

        public string Quarter
        {
            get { return _quarter; }
            set { _quarter = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private double _performance;

        public double Performance
        {
            get { return _performance; }
            set { _performance = value; }
        }
    }
}
