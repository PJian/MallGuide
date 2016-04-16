using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using SuperMarketLHS.userControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SuperMarketLHS.win
{
    /// <summary>
    /// ShopInWin.xaml 的交互逻辑
    /// 完成商铺的入驻
    /// </summary>
    public partial class ShopInWin : Window
    {
       
        private Obstacle relativeObstacle;
        private Floor relativeFloor;
        private ObservableCollection<Shop> allShops;
        private Shop currentShop;

        private UserControlMapGrid parent;
        public ShopInWin( UserControlMapGrid parent)
        {
            InitializeComponent();
            this.parent = parent;
            this.relativeObstacle = parent.CurrentEditObstacle;
            this.relativeFloor = parent.CurrentEditFloor;
           
        }

        private void init()
        {
            this.floor.Content = this.relativeFloor.Index+"楼";
            this.room.Content = this.relativeObstacle.Index+"室";
            allShops = new ObservableCollection<Shop>(SqlHelper.getAllShop());
            shopsListView.ItemsSource = allShops;

        }

        public void shopIn(Obstacle o)
        {

            //判断该商铺是否已经入驻了其他区域
            this.currentShop.Floor = this.parent.CurrentEditFloor;
            //商铺上的门只是为了寻路用的，所以直接就关联到地图上的格子上
            this.currentShop.Door = new Point((int)o.Door.X / 4, (int)o.Door.Y / 4);
            this.currentShop.Index = o.Index;
            o.Shop = this.currentShop;
            SqlHelper.updateShop(this.currentShop);
            parent.saveMap();
            MessageBox.Show("已入驻！");
        }

        private void btn_enter_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentShop == null) {
                MessageBox.Show("请选择店铺！");
            }else if(this.currentShop!=null){
                shopIn(this.relativeObstacle);
                this.Close();
            }
            else if (this.currentShop.Floor != null) {
                MessageBox.Show("选择品牌已经入驻其他店铺，无法再次入驻！");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void shopsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.shopsListView.SelectedItem != null) {
                this.currentShop = this.shopsListView.SelectedItem as Shop;
            }
        }
    }
}
