using EntityManagementService.entity;
using EntityManagementService.util;
using EntityManageService.sqlUtil;
using SuperMarketLHS.comm;
using SuperMarketLHS.userControl;
using System;
using System.Collections.Generic;
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
    /// NewShopWin.xaml 的交互逻辑
    /// 完成商铺的新建和入驻功能
    /// </summary>
    public partial class NewShopWin : Window
    {
        private Obstacle relativeObstacle;
        private UserControlMapGrid parent;
        private Shop currentEditShop;

        public NewShopWin(UserControlMapGrid parent)
        {
            InitializeComponent();
            this.relativeObstacle = parent.CurrentEditObstacle;
            this.parent = parent;
        }
        private void init()
        {
            currentEditShop = new Shop();
            this.grid_shopInfo.DataContext = this.currentEditShop;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            saveShop();
        }

        private void handleImg(int id)
        {
            //logo处理
            this.currentEditShop.Logo = WinUtil.copyOne(userControl_logo.ImgPath, ConstantData.getShopLogoDataFolder(id));
            //设施图片
            this.currentEditShop.Facilities = WinUtil.copyImg(userControl_facilities.ImgPathes, ConstantData.getBrandFacilitiesDataFolder(id)).ToArray();
            //品牌图片
            this.currentEditShop.BrandImgs = WinUtil.copyImg(this.userControl_brandImgs.ImgPathes, ConstantData.getShopBrandImgsDataFolder(id)).ToArray();
        }
        private void handleImgDel(int id)
        {
            WinUtil.delFile(ConstantData.getBrandLogoDataFolder(id), new String[] { this.currentEditShop.Logo }.ToList());
            WinUtil.delFile(ConstantData.getBrandFacilitiesDataFolder(id), currentEditShop.Facilities.ToList());
            WinUtil.delFile(ConstantData.getShopBrandImgsDataFolder(id), currentEditShop.BrandImgs.ToList());
        }

        private int getShopType()
        {
            return (bool)this.radio_normal.IsChecked ? ConstantData.SHOP_TYPE_NORMAL : ConstantData.SHOP_TYPE_SPECIAL;
        }


        /// <summary>
        /// 从界面上提取数据
        /// </summary>
        private void updateCurrentEditShopInfo()
        {
            //图片信息
            //type
            this.currentEditShop.Type = getShopType();
            //排序字母
            this.currentEditShop.SortChar = this.combox_sortChar.SelectItem;
            //分类
            this.currentEditShop.CatagoryName = this.combox_Catagory.SelectItem;
            //分类颜色
            this.currentEditShop.CatagoryColor = this.colorPicker_catagoryColor.SelectedColor.ToString();
            //开始时间
            this.currentEditShop.StartTime = time_start.SelectedTime.ToString();
            //结束时间
            this.currentEditShop.EndTime = time_end.SelectedTime.ToString();
        }

        public void shopIn(Obstacle o)
        {

            //判断该商铺是否已经入驻了其他区域
            this.currentEditShop.Floor = this.parent.CurrentEditFloor;
            //商铺上的门只是为了寻路用的，所以直接就关联到地图上的格子上
            this.currentEditShop.Door = new Point((int)o.Door.X / 4, (int)o.Door.Y / 4);
            this.currentEditShop.Index = o.Index;
            o.Shop = this.currentEditShop;
            SqlHelper.updateShop(this.currentEditShop);
            parent.saveMap();
            MessageBox.Show("已入驻！");
        }



        private void saveShop()
        {

            if (this.currentEditShop.Name == null || "".Equals(this.currentEditShop.Name.Trim()))
            {
                MessageBox.Show("请填写店铺名字！");
                return;
            }

            this.currentEditShop.Id = SqlHelper.saveShop(this.currentEditShop);
            //logo
            // handleImg(this.currentEditShop.Id);
            updateCurrentEditShopInfo();
            handleImg(this.currentEditShop.Id);
            SqlHelper.updateShop(this.currentEditShop);
            handleImgDel(currentEditShop.Id);
            MessageBox.Show("添加成功");
            shopIn(this.relativeObstacle);
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void radio_Special_Checked(object sender, RoutedEventArgs e)
        {
            if (this.userControl_brandImgs != null)
            {
                this.userControl_brandImgs.Visibility = Visibility.Collapsed;
            }
            if (this.userControl_facilities != null)
            {
                this.userControl_facilities.Visibility = Visibility.Visible;
            }
            
            
        }

        private void radio_normal_Checked(object sender, RoutedEventArgs e)
        {
            if (this.userControl_brandImgs != null)
            {
                this.userControl_brandImgs.Visibility = Visibility.Visible;
            }
            if (this.userControl_facilities != null)
            {
                this.userControl_facilities.Visibility = Visibility.Collapsed;
            }
           
            
        }
    }
}
