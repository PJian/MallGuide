using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Telerik.Charting;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;

namespace SuperMarketLHS.uientity
{
    public class PerformanceViewModel : ViewModelBase
    {
        private IEnumerable<PerformanceData> _q1;
        private IEnumerable<PerformanceData> _q2;
        private IEnumerable<PerformanceData> _q3;
        private IEnumerable<PerformanceData> _q4;

        private ChartSeriesCombineMode _barCombineMode = ChartSeriesCombineMode.Cluster;
        private Orientation _chartOrientation = Orientation.Vertical;
        private bool _isShowLabelsEnabled = true;
        private bool _showLabels = true;

        private double _gapLength = 0.2d;
        private double _axisMaxValue = 100;
        private double _axisStep = 5d;
        private string _axisTitle = "答案情况";
        private string _axisLabelFormat = "N0";
        private GridLineVisibility _majorLinesVisibility = GridLineVisibility.Y;

        public PerformanceViewModel()
        {
        }

        public IEnumerable<PerformanceData> Q1
        {
            get
            {
                if (this._q1 == null)
                {
                    this._q1 = new List<PerformanceData>() {
                        new PerformanceData("Jason Harley", "Q1, 2010", 17790),
                        new PerformanceData("Adam White", "Q1, 2010", 12820),
                        new PerformanceData("Barbara Smith", "Q1, 2010", 14350),
                        new PerformanceData("Susan Jones", "Q1, 2010", 11150),
                        new PerformanceData("Tom Marshall", "Q1, 2010", 11800)
                    };
                }

                return this._q1;
            }
            set {
                this._q1 = value;
            }
        }

        public IEnumerable<PerformanceData> Q2
        {
            get
            {
                if (this._q2 == null)
                {
                    this._q2 = new List<PerformanceData>(){
                        new PerformanceData("Jason Harley", "Q2, 2010", 15320),
                        new PerformanceData("Adam White", "Q2, 2010", 14100),
                        new PerformanceData("Barbara Smith", "Q2, 2010", 13000),
                        new PerformanceData("Susan Jones", "Q2, 2010", 8850),
                        new PerformanceData("Tom Marshall", "Q2, 2010", 10900)
                    };
                }

                return this._q2;
            }
        }

        public IEnumerable<PerformanceData> Q3
        {
            get
            {
                if (this._q3 == null)
                {
                    this._q3 = new List<PerformanceData>(){
                        new PerformanceData("Jason Harley", "Q3, 2010", 13800),
                        new PerformanceData("Adam White", "Q3, 2010", 12300),
                        new PerformanceData("Barbara Smith", "Q3, 2010", 14900),
                        new PerformanceData("Susan Jones", "Q3, 2010", 10100),
                        new PerformanceData("Tom Marshall", "Q3, 2010", 8700)
                    };
                }

                return this._q3;
            }
        }

        public IEnumerable<PerformanceData> Q4
        {
            get
            {
                if (this._q4 == null)
                {
                    this._q4 = new List<PerformanceData>(){
                        new PerformanceData("Jason Harley", "Q4, 2010", 15850),
                        new PerformanceData("Adam White", "Q4, 2010", 11200),
                        new PerformanceData("Barbara Smith", "Q4, 2010", 14000),
                        new PerformanceData("Susan Jones", "Q4, 2010", 7500),
                        new PerformanceData("Tom Marshall", "Q4, 2010", 10000)
                    };
                }

                return this._q4;
            }
        }

        public ChartSeriesCombineMode BarCombineMode
        {
            get
            {
                return this._barCombineMode;
            }
            set
            {
                if (this._barCombineMode != value)
                {
                    this._barCombineMode = value;
                    this.OnPropertyChanged("BarCombineMode");

                    this.UpdateLabelsConfiguration(this._barCombineMode);
                    this.UpdateAxisConfiguration(this._barCombineMode);
                }
            }
        }

        public bool ShowLabels
        {
            get
            {
                return this._showLabels;
            }
            set
            {
                if (this._showLabels != value)
                {
                    this._showLabels = value;
                    this.OnPropertyChanged("ShowLabels");
                }
            }
        }

        public bool IsShowLabelsEnabled
        {
            get
            {
                return this._isShowLabelsEnabled;
            }
            set
            {
                if (this._isShowLabelsEnabled != value)
                {
                    this._isShowLabelsEnabled = value;
                    this.OnPropertyChanged("IsShowLabelsEnabled");
                }
            }
        }

        public Orientation ChartOrientation
        {
            get
            {
                return this._chartOrientation;
            }
            set
            {
                if (this._chartOrientation != value)
                {
                    this._chartOrientation = value;
                    this.OnPropertyChanged("ChartOrientation");

                    this.UpdateMajorLinesVisibility(this._chartOrientation);
                }
            }
        }

        public double GapLength
        {
            get
            {
                return this._gapLength;
            }
            set
            {
                if (this._gapLength != value)
                {
                    this._gapLength = value;
                    this.OnPropertyChanged("GapLength");
                }
            }
        }

        public double AxisMaxValue
        {
            get
            {
                return this._axisMaxValue;
            }
            set
            {
                if (this._axisMaxValue != value)
                {
                    this._axisMaxValue = value;
                    this.OnPropertyChanged("AxisMaxValue");
                }
            }
        }

        public double AxisStep
        {
            get
            {
                return this._axisStep;
            }
            set
            {
                if (this._axisStep != value)
                {
                    this._axisStep = value;
                    this.OnPropertyChanged("AxisStep");
                }
            }
        }

        public string AxisTitle
        {
            get
            {
                return this._axisTitle;
            }
            set
            {
                if (this._axisTitle != value)
                {
                    this._axisTitle = value;
                    this.OnPropertyChanged("AxisTitle");
                }
            }
        }

        public string AxisLabelFormat
        {
            get
            {
                return this._axisLabelFormat;
            }
            set
            {
                if (this._axisLabelFormat != value)
                {
                    this._axisLabelFormat = value;
                    this.OnPropertyChanged("AxisLabelFormat");
                }
            }
        }

        public GridLineVisibility MajorLinesVisibility
        {
            get
            {
                return this._majorLinesVisibility;
            }
            set
            {
                if (this._majorLinesVisibility != value)
                {
                    this._majorLinesVisibility = value;
                    this.OnPropertyChanged("MajorLinesVisibility");
                }
            }
        }

        private void UpdateMajorLinesVisibility(Orientation chartOrientation)
        {
            if (chartOrientation == Orientation.Horizontal)
                this.MajorLinesVisibility = GridLineVisibility.X;
            else
                this.MajorLinesVisibility = GridLineVisibility.Y;
        }

        private void UpdateLabelsConfiguration(ChartSeriesCombineMode mode)
        {
            this.ShowLabels = false;
            this.IsShowLabelsEnabled = mode == ChartSeriesCombineMode.Cluster;
        }

        private void UpdateAxisConfiguration(ChartSeriesCombineMode mode)
        {
            this.GapLength = 0.2d;
            this.AxisMaxValue = 100;
            this.AxisStep = 5;
            this.AxisTitle = "答案统计";
            this.AxisLabelFormat = "NO";
            //if (mode == ChartSeriesCombineMode.Cluster)
            //{
            //    this.GapLength = 0.2d;

            //    this.AxisMaxValue = 20000d;
            //    this.AxisStep = 5000d;

            //    this.AxisTitle = "PROFIT (USD)";
            //    this.AxisLabelFormat = "N0";
            //}
            //else if (mode == ChartSeriesCombineMode.Stack)
            //{
            //    this.GapLength = 0.5d;

            //    this.AxisMaxValue = 70000d;
            //    this.AxisStep = 16500d;

            //    this.AxisTitle = "PROFIT (USD)";
            //    this.AxisLabelFormat = "N0";
            //}
            //else if (mode == ChartSeriesCombineMode.Stack100)
            //{
            //    this.GapLength = 0.5d;

            //    this.AxisMaxValue = 1d;
            //    this.AxisStep = 0.25d;

            //    this.AxisTitle = "PROFIT (%)";
            //    this.AxisLabelFormat = "P0";
            //}
        }
    }
}
