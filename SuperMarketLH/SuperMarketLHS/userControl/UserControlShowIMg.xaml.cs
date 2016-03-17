using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperMarketLHS.userControl
{
    /// <summary>
    /// UserControlShowIMg.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlShowIMg : UserControl
    {
        private List<string> imgPaths;
        private int imgCount = 0;
        private int currentSelectIndex;

        public int CurrentSelectIndex
        {
            get { return currentSelectIndex; }
            set { currentSelectIndex = value; }
        }
        public string CurrentSelectItem {
            get {
                if (this.currentSelectIndex >= this.imgPaths.Count) {
                    return null;
                }
                return this.imgPaths.ElementAt(currentSelectIndex);
            }
        }
        public void clear() {
            this.imgPaths = new List<string>();
            this.imgCount = 0;
            this.CurrentSelectIndex = 0;
            loadImg(0);
            loadImgCounter(0);
        }

        public UserControlShowIMg()
        {
            InitializeComponent();
            this.imgPaths = new List<string>();
        }
        public UserControlShowIMg(List<String> imgPaths)
        {
            InitializeComponent();
            this.imgPaths = imgPaths;
            imgCount = this.imgPaths.Count;
            loadImg(0);
            loadImgCounter(0);
        }
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="path"></param>
        public void addImg(List<String> path) {
            this.imgPaths.Clear();
            if (path != null) {
                for (int i = 0; i < path.Count; i++)
                {
                    if ("".Equals(path.ElementAt(i))) continue;
                    addImg(path.ElementAt(i));
                }
            }
           
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="path"></param>
        public void addImg(String path) {
            this.imgPaths.Add(path);
            imgCount = this.imgPaths.Count;
            this.CurrentSelectIndex = this.imgPaths.Count - 1 ;
            loadImg(CurrentSelectIndex);
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="index"></param>
        public void removeImg(int index) {
            if (index >= this.imgPaths.Count) {
                MessageBox.Show("图片不存在，无法删除");
                return;
            }
            this.imgPaths.RemoveAt(index);
            imgCount = this.imgPaths.Count;
            
            if (index == CurrentSelectIndex) {
                CurrentSelectIndex = this.imgPaths.Count==0?0:(index + 1) % this.imgPaths.Count;
                loadImg(CurrentSelectIndex);
            }
        }

        public void removeAllImg() {

            //for (int i = imgCount-1; i >= 0; i--) {
            //    removeImg(i);
            //}
        }
        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="index"></param>
        private void loadImg(int index) {
            if (this.imgPaths != null && this.imgPaths.Count > 0)
            {
                if (this.imgPaths.ElementAt(index) != null && !"".Equals(this.imgPaths.ElementAt(index))) {
                    if(!File.Exists(this.imgPaths.ElementAt(index)))return;
                    BinaryReader binReader = new BinaryReader(File.Open(this.imgPaths.ElementAt(index), FileMode.Open));
                    FileInfo fileInfo = new FileInfo(this.imgPaths.ElementAt(index));
                    byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
                    binReader.Close();
                    // Init bitmap
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(bytes);
                    bitmap.EndInit();
                    img_preview.Source = bitmap;
                    loadImgCounter(index);
                }
            }
            else {
                img_preview.Source = null;
            }
        }

        /// <summary>
        /// 加载图片计数
        /// </summary>
        private void loadImgCounter(int selectIndex) {
            stackPanel_imgCounter.Children.Clear();
            for (int i = 0; i < imgCount; i++) {
                stackPanel_imgCounter.Children.Add(i == selectIndex ? getSelectEllipse() : getUnSelectEllipse());
            }
        }
        /// <summary>
        /// 构造选中的圆圈效果
        /// </summary>
        /// <returns></returns>
        private Ellipse getUnSelectEllipse() {
            return new Ellipse()
            {
                Fill=Brushes.White,
                Width=10,
                Height=10,
                Margin=new Thickness(2,0,2,0)
            };
        }
        /// <summary>
        /// 构造未选中的圆圈效果
        /// </summary>
        /// <returns></returns>
        private Ellipse getSelectEllipse()
        {
            return new Ellipse()
            {
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0099ff")),
                Width = 10,
                Height = 10,
                Margin = new Thickness(2, 0, 2, 0)
            };
        }
        /// <summary>
        /// 前翻
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_pre_Click(object sender, RoutedEventArgs e)
        {
            if (this.imgPaths.Count > 0) {
                this.CurrentSelectIndex = Math.Abs((this.CurrentSelectIndex - 1 + this.imgPaths.Count) % this.imgPaths.Count);
                loadImg(CurrentSelectIndex);
            }
        }
        /// <summary>
        /// 后翻
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            if (this.imgPaths.Count > 0) {
                this.CurrentSelectIndex = Math.Abs((this.CurrentSelectIndex + 1) % this.imgPaths.Count);
                loadImg(CurrentSelectIndex);
            }
        }
    }
}
