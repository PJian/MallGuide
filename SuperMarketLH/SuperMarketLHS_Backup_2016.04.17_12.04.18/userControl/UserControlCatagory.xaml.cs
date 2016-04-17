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
    /// UserControlCatagory.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlCatagory : UserControl
    {
        
        private List<Catagory> catagories;
        private Catagory currentSelectItem;
        public UserControlCatagory()
        {
            InitializeComponent();
        }
       
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            //加载数据
            catagories = SqlHelper.getAllCatagory();
            this.catagory.ItemsSource = catagories;
            this.catagory.SelectedIndex = 0;
        }

        public Catagory SelectItem { 
            get{
                return this.currentSelectItem;
            }
            set { 
                this.catagory.SelectedItem = value;
            }
        }

        private void catagory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentSelectItem = this.catagory.SelectedItem as Catagory;
        }
    }
}
