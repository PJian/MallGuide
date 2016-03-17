using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SuperMarketLHS.comm
{
    /// <summary>
    /// 绘图相关类
    /// </summary>
    public class CavasUtil
    {
        public const int DRAW_STATE_DRAWING_AREA = 1;
        public const int DRAW_STATE_DRAWING_DOOR = 2;
        public const int DRAW_STATE_DRAWING_DONE = 3;

        private const int ELLIPSE_WIDTH = 8;
        private const int ELLIPSE_HEIGHT = 8;
        private static Brush ELLPSE_FILL = Brushes.Green;
        private static Brush ELLPSE_FILL_DOOR = Brushes.Red;
        private static Brush LINE_STROKE = Brushes.Black;
        private static Brush POLYGON_FILL = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4C82C79E"));
        public static int GRID_PIX_WIDTH = 4;
        public static int GRID_PIX_HEIGHT = 4;
        private const int LINE_STROKETICKNESS = 2;


        public const int DRAW_STATE = 1;
        public const int SHOP_IN_STATE = 2;

        /// <summary>
        /// 往grid中添加圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public static void drawEllipseDoor(Grid g, int row, int col)
        {
            Ellipse ellipse = new Ellipse()
            {
                Width = ELLIPSE_WIDTH +4,
                Height = ELLIPSE_HEIGHT+4,
                Fill = ELLPSE_FILL_DOOR
            };

            Grid.SetColumn(ellipse, col);
            Grid.SetColumnSpan(ellipse, 3);
            Grid.SetRow(ellipse, row);
            Grid.SetRowSpan(ellipse, 3);
            g.Children.Add(ellipse);
        }

        /// <summary>
        /// 往grid中添加圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public static void drawEllipse(Grid g,int row,int col) {
            Ellipse ellipse = new Ellipse() { 
                Width= ELLIPSE_WIDTH,
                Height =ELLIPSE_HEIGHT,
                Fill = ELLPSE_FILL,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(ellipse,col);
            Grid.SetColumnSpan(ellipse,2);
            Grid.SetRow(ellipse,row);
            Grid.SetRowSpan(ellipse,2);
            g.Children.Add(ellipse);
        }

        /// <summary>
        /// 往grid中添加圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public static void drawEllipseTest(Grid g, int row, int col)
        {
            Ellipse ellipse = new Ellipse()
            {
                Width = 5,
                Height = 5,
                Fill = Brushes.Black
            };

            Grid.SetColumn(ellipse, col);
            Grid.SetRow(ellipse, row);
            g.Children.Add(ellipse);
        }


        /// <summary>
        /// 往grid中添加线段
        /// </summary>
        /// <param name="g"></param>
        /// <param name="startRow"></param>
        /// <param name="starCol"></param>
        /// <param name="endRow"></param>
        /// <param name="endCol"></param>
        public static void drawLine(Canvas c,Point startPoint,Point endPoint) {

            LineGeometry myLineGeometry = new LineGeometry();
            myLineGeometry.StartPoint = startPoint;
            myLineGeometry.EndPoint = endPoint;

            Path myPath = new Path();
            myPath.Stroke = LINE_STROKE;
            myPath.StrokeThickness = LINE_STROKETICKNESS;
            myPath.Data = myLineGeometry;
            c.Children.Add(myPath);
           
        }
        public  static void drawEllipse(Canvas c, Point temp)
        {
            Ellipse ellipse = new Ellipse()
            {
                Width = ELLIPSE_WIDTH,
                Height = ELLIPSE_HEIGHT,
                Fill = ELLPSE_FILL,
                
            };
            Canvas.SetTop(ellipse, temp.Y - GRID_PIX_HEIGHT);
            Canvas.SetLeft(ellipse, temp.X - GRID_PIX_WIDTH);
            c.Children.Add(ellipse);
        }

        /// <summary>
        /// 绘制封闭多边形
        /// </summary>
        /// <param name="c"></param>
        /// <param name="points"></param>
        public static void drawPolygon(Canvas c,List<Point> points) {
            Polygon polygon = new Polygon();//定义封闭多边形对象
            //属性设置，边颜色、填充颜色、边粗细等
            polygon.Stroke = LINE_STROKE;
            //填充，浅海蓝色
            polygon.Fill = POLYGON_FILL;
            polygon.StrokeThickness = LINE_STROKETICKNESS;
            
            //定义点集合对象
            PointCollection pointCollection = new PointCollection();
            foreach (Point p in points)
            {
                pointCollection.Add(p); //将顶点添加到点集合对象
            }
            polygon.Points = pointCollection; //设置Polygon属性Points的点集合
            c.Children.Add(polygon);
        }


        public static double getMinX(List<Point> ps) {
            if (ps.Count <= 0) return 0;
            double minX = ps.ElementAt(0).X;
            for (int i = 0; i < ps.Count; i++)
            {
                if (minX > ps.ElementAt(i).X) {
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
        
    }
}
