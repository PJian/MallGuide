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
    /// UserControlLogo.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlLogo : UserControl, INotifyPropertyChanged
    {
        private string _imgPath;
        public UserControlLogo()
        {
           
            InitializeComponent();
        }
        public string ImgPath
        {
            get {
                return this._imgPath;
            }
            set {
                this._imgPath = value;
                if (this.PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("ImgPath"));
                }
            }
        }

        private void btn_addImg_Click(object sender, RoutedEventArgs e)
        {
            ImgPath = WinUtil.chooseImg();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.grid_content.DataContext = this;
        }
    }
}
