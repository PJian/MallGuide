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
    /// PageFloorBaseInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PageFloorBaseInfo : Page
    {
        private MainWindow rootWin;
        public PageFloorBaseInfo()
        {
            InitializeComponent();
        }
        public PageFloorBaseInfo(MainWindow rootWin)
        {
            InitializeComponent();
            this.rootWin = rootWin;
        }

        private Floor currentEditFloor;
        private List<Floor> allFloors;
        private void init()
        {
            allFloors = SqlHelper.getAllFloor();
            this.list_allFloor.ItemsSource = allFloors;
            this.list_allFloor.SelectedIndex = -1;
            this.currentEditFloor = new Floor();
            showFloorInfo();
        }
        private void showFloorInfo()
        {
            this.grid_allInfo.DataContext = null;
            this.grid_allInfo.DataContext = this.currentEditFloor;
            //显示图片
            if (this.currentEditFloor.Img != null && !this.currentEditFloor.Img.Equals(""))
            {
                this.userCtrl_bg.ImgPath = this.currentEditFloor.Img;
            }
            else
            {
                this.userCtrl_bg.ImgPath = null;
            }

        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }
        private bool validate()
        {
            if (this.currentEditFloor.Name == null || this.currentEditFloor.Name.Trim().Equals(""))
            {
                MessageBox.Show("请填写名称!");
                return false;
            }
            if (this.userCtrl_bg.ImgPath == null || "".Equals(this.userCtrl_bg.ImgPath.Trim()))
            {
                MessageBox.Show("请添加楼层平面图!");
                return false;
            }
            return true;
        }
        private void update(bool showMsg)
        {
            if (validate())
            {
                if (this.currentEditFloor.Id <= 0)
                {
                    save();
                    return;
                }
                handleImg(this.currentEditFloor.Id);
                SqlHelper.updateFloor(this.currentEditFloor);
                delImg(this.currentEditFloor.Id);
                init();
                if (showMsg)
                {
                    MessageBox.Show("更新成功！");
                }
            }
        }
        public void delImg(int id)
        {
            WinUtil.delFile(ConstantData.getFloorBgDataFolder(id), new String[] { this.currentEditFloor.Img }.ToList());
        }
        private void handleImg(int id)
        {
            this.currentEditFloor.Img = WinUtil.copyOne(this.userCtrl_bg.ImgPath, ConstantData.getFloorBgDataFolder(id));
        }
        private void save()
        {
            if (validate())
            {
                if (this.currentEditFloor.Id > 0)
                {
                    update(true);
                    return;
                }
                Floor temp = SqlHelper.getFloorByIndex(this.currentEditFloor.Index);
                if (temp != null)
                {
                    if (MessageBox.Show("当前楼层已经存在，是否替换", "楼层替换", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        this.currentEditFloor.Id = temp.Id;
                        update(false);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    this.currentEditFloor.Id = SqlHelper.saveFloor(this.currentEditFloor);
                    update(false);
                }
                MessageBox.Show("添加成功！");
            }
        }
        private void createNew()
        {
            this.currentEditFloor = new Floor();
            showFloorInfo();
        }
        private void delete()
        {
            if (this.currentEditFloor != null)
            {
                SqlHelper.deleteFloor(this.currentEditFloor);
                delImg(this.currentEditFloor.Id);
                init();
                MessageBox.Show("删除成功！");
            }
        }

        private void btn_create_Click(object sender, RoutedEventArgs e)
        {
            createNew();
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            update(true);
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            delete();
        }

        private void list_allFloor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.list_allFloor.SelectedItem != null) {
                this.currentEditFloor = this.list_allFloor.SelectedItem as Floor;
                showFloorInfo();
            }
        }
    }
}
