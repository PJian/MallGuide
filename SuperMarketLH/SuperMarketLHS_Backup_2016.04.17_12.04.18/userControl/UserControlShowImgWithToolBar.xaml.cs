using SuperMarketLHS.comm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// UserControlShowImgWithToolBar.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlShowImgWithToolBar : UserControl ,INotifyPropertyChanged
    {
        private string _labelImgSize;

        public string LabelImgSize
        {
            get { return _labelImgSize; }
            set { _labelImgSize = value;
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("LabelImgSize"));
            }
            }
        }

        private string _labelChooseImgContent;
        public string LabelChooseImgContent
        {
            set {
                this._labelChooseImgContent = value;
                if (this.PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("LabelChooseImgContent"));
                }
            }
            get {
                return this._labelChooseImgContent;
            }
        }
        private string currentTempImgPath = "";
        public List<string> ImgPathes {
            get {
                List<string> strs = new List<string>();
                if (this.InitImgPath != null) {
                    strs.AddRange(this.InitImgPath);
                }
                if (this.TempImgPath != null) {
                    strs.AddRange(this.TempImgPath);
                }
                return strs;
            }
        }

        public UserControlShowImgWithToolBar()
        {
            InitializeComponent();
        }
        public UserControlShowImgWithToolBar(List<string> initImgPath)
        {
            InitializeComponent();
            this._initImgPath = initImgPath;
        }

        private void init() {
            this.grid_content.DataContext = this;
            this._tempImgPath = new List<string>();
            this.userCtrl_imgShow.addImg(this._initImgPath);
        }

        private List<string> _initImgPath;

        public List<string> InitImgPath
        {
            get { return _initImgPath; }
            set { _initImgPath = value; }
        }
        private List<string> _tempImgPath;

        public List<string> TempImgPath
        {
            get { return _tempImgPath; }
           
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void btn_chooseImg_Click(object sender, RoutedEventArgs e)
        {
            String[] pathes = WinUtil.chooseMultiImg();
            if (pathes == null) return;
            string path = "";
            for (int i = 0; i < pathes.Length; i++) { 
                path +=","+pathes[i];
            }
            this.txb_imgPath.Text = path.Substring(1,path.Length-1);
        }
        private void btn_addImg_Click(object sender, RoutedEventArgs e)
        {
            currentTempImgPath = this.txb_imgPath.Text;
            if (currentTempImgPath != null && !currentTempImgPath.Trim().Equals(""))
            {
                String[] pathes = currentTempImgPath.Split(',');
                for (int i = 0; i < pathes.Length; i++) {
                    this.userCtrl_imgShow.addImg(pathes[i]);
                    _tempImgPath.Add(pathes[i]);
                }
                    // currentMall.ImgPath.Add(currentTempImgPath);
                    this.txb_imgPath.Text = "";
              
            }
        }

        private void btn_delImg_Click(object sender, RoutedEventArgs e)
        {
            if (_initImgPath != null) {
                _initImgPath.Remove(this.userCtrl_imgShow.CurrentSelectItem);
            }
            _tempImgPath.Remove(this.userCtrl_imgShow.CurrentSelectItem);
            this.userCtrl_imgShow.removeImg(this.userCtrl_imgShow.CurrentSelectIndex);
        }
        /// <summary>
        /// 改变图片内容
        /// </summary>
        /// <param name="imgs"></param>
        public void changeImgs(List<string> imgs) {
            if (_tempImgPath != null) {
                _tempImgPath.Clear();
            }
            userCtrl_imgShow.clear();
            this.userCtrl_imgShow.removeAllImg();
            this.InitImgPath = imgs;
            this.userCtrl_imgShow.addImg(imgs);
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
