using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using SuperMarketLHS.comm;
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

namespace SuperMarketLHS.page.floor
{
    /// <summary>
    /// PageZoneShop.xaml 的交互逻辑
    /// </summary>
    public partial class PageZoneShop : Page
    {
        private MainWindow rootWin;
        private Floor currentFloor;
        private List<Floor> allFloors;
        private Shop currentShop;
        private List<Shop> allShops;
        public PageZoneShop()
        {
            InitializeComponent();
        }
        public PageZoneShop(MainWindow rootWin)
        {
            InitializeComponent();
            this.rootWin = rootWin;
        }

        private void init() {
            allFloors = SqlHelper.getAllFloor();
            this.combox_allFloors.ItemsSource = this.allFloors;
            allShops = SqlHelper.getAllShop();
            this.combox_allShops.ItemsSource = this.allShops;
            this.userCtrl_map.EditState = CavasUtil.SHOP_IN_STATE;
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentFloor == null) {
                MessageBox.Show("楼层不能为空！");
                return;
            }
            if(this.currentShop==null){
                MessageBox.Show("商铺不能为空！");
                return;
            }

            if (this.currentFloor != null && this.currentShop != null) {
                this.userCtrl_map.saveMap(this.currentFloor, this.currentShop);
            }
        }

        private void combox_allFloors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.combox_allFloors.SelectedItem != null) {
                this.currentFloor = this.combox_allFloors.SelectedItem as Floor;
                this.userCtrl_map.CurrentEditFloor = this.currentFloor;
            }
        }

        private void combox_allShops_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.combox_allShops.SelectedItem != null) {
                this.currentShop = this.combox_allShops.SelectedItem as Shop;
                this.userCtrl_map.CurrentShop = this.currentShop;
                if (this.currentShop.Floor != null && this.currentShop.Index !=null && !this.currentShop.Index.Equals(""))
                {
                    this.label_shopIndex.Content = "已入驻" + this.currentShop.Floor.Name + " " + this.currentShop.Index;
                }
                else {
                    this.label_shopIndex.Content = "未入驻";
                }
               
            }
        }

       

    }
}
