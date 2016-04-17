using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using System;
using System.Collections.Generic;
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
    /// UserControlFloorCombox.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlFloorCombox : UserControl
    {
        public UserControlFloorCombox()
        {
            InitializeComponent();
        }

        private List<Floor> allFloors;
        private Floor currentSelectItem;
       
       
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            //加载数据
            allFloors = SqlHelper.getAllFloor();
            this.floor.ItemsSource = allFloors;
            this.floor.SelectedIndex = 0;
        }

        public Floor SelectItem { 
            get{
                return this.currentSelectItem;
            }
            set {
                this.floor.SelectedItem = value;
            }
        }

        private void catagory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentSelectItem = this.floor.SelectedItem as Floor;
        }
    }
}
