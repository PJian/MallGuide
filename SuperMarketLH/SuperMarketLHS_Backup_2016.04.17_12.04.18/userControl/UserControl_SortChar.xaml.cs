using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// UserControl_SortChar.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl_SortChar : UserControl
    {
        private List<string> sortChars;
        private string currentSelectItem;
        public UserControl_SortChar()
        {
            InitializeComponent();
        }
       
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            //加载数据
            sortChars = SqlHelper.getAllSortChar();
            this.catagory.ItemsSource = sortChars;
            this.catagory.SelectedIndex = 0;
        }

        public string SelectItem{
            get { return this.currentSelectItem; }
            set { this.catagory.SelectedItem = value; }
        }
        private void catagory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentSelectItem = this.catagory.SelectedItem as string;
        }
    }
}
