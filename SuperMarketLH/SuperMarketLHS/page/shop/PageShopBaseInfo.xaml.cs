using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using SuperMarketLHS.comm;
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

namespace SuperMarketLHS.page.shop
{
    /// <summary>
    /// PageShopBaseInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PageShopBaseInfo : Page
    {
        private ObservableCollection<Shop> allShops;
        private Shop currentEditShop;
        private MainWindow rootWin;

        public PageShopBaseInfo(MainWindow rootWin)
        {
            InitializeComponent();
            this.rootWin = rootWin;
        }
        private void init() { 
            //列表显示
            allShops = new ObservableCollection<Shop>(SqlHelper.getAllShop());
            this.list_allshop.ItemsSource = allShops;
            currentEditShop = new Shop();
            this.grid_allInfo.DataContext = currentEditShop;
            showShopInfo();
        }
        /// <summary>
        /// 显示当前商铺的信息到界面上面去
        /// </summary>
        private void showShopInfo() {
            grid_allInfo.DataContext = this.currentEditShop;
            //图片的显示
            if (this.currentEditShop != null)
            {
                userControl_logo.ImgPath = this.currentEditShop.Logo;
                //设施图片
                if (this.currentEditShop.Facilities != null)
                {
                    userControl_facilities.changeImgs(this.currentEditShop.Facilities.ToList());
                }
                else {
                    userControl_facilities.changeImgs(new string[0].ToList());
                }
                //品牌图片
                if (this.currentEditShop.BrandImgs != null)
                {
                    this.userControl_brandImgs.changeImgs(this.currentEditShop.BrandImgs.ToList());
                }
                else
                {
                    userControl_brandImgs.changeImgs(new string[0].ToList());
                }

            }
            

            //下拉框的显示
            combox_sortChar.SelectItem = this.currentEditShop.SortChar;
            combox_Catagory.SelectItem = this.currentEditShop.CatagoryName;
            combox_floor.SelectItem = this.currentEditShop.Floor;
            //店铺类型
            setShopType();
            //分类颜色
            if (this.currentEditShop.CatagoryColor != null && !this.currentEditShop.CatagoryColor.Trim().Equals(""))
            {
                this.colorPicker_catagoryColor.SelectedColor = (Color)ColorConverter.ConvertFromString(this.currentEditShop.CatagoryColor);
            }
            else {
                this.colorPicker_catagoryColor.SelectedColor = Colors.Black;
            }
            //时间
            if (this.currentEditShop.EndTime != null && !this.currentEditShop.EndTime.Trim().Equals(""))
            {
                this.time_end.SelectedTime = TimeSpan.Parse(this.currentEditShop.EndTime);
            }
            else {
                this.time_end.SelectedTime = null;
            }
            if (this.currentEditShop.StartTime != null && !this.currentEditShop.StartTime.Trim().Equals(""))
            {
                this.time_start.SelectedTime = TimeSpan.Parse(this.currentEditShop.StartTime);
            }
            else {
                this.time_start.SelectedTime = null;
            }

            //品牌图片的显示


        }
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }
        private void setShopType() {
            if (this.currentEditShop.Type == ConstantData.SHOP_TYPE_SPECIAL)
            {
                this.radio_Special.IsChecked = true;
            }
            else if (this.currentEditShop.Type == ConstantData.SHOP_TYPE_NORMAL)
            {
                this.radio_normal.IsChecked = true;
            }
            else {
                this.radio_normal.IsChecked = true;
            }
        }
        private int getShopType(){
            return (bool)this.radio_normal.IsChecked?ConstantData.SHOP_TYPE_NORMAL:ConstantData.SHOP_TYPE_SPECIAL;
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

        /// <summary>
        /// 从界面上提取数据
        /// </summary>
        private void updateCurrentEditShopInfo(){
            //图片信息
           
            //type
            this.currentEditShop.Type = getShopType();
            //楼层
            this.currentEditShop.Floor = this.combox_floor.SelectItem;
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

        private void saveShop() {
            if (this.currentEditShop == null) {
                return;
            }

            if (this.currentEditShop.Name == null || "".Equals(this.currentEditShop.Name.Trim())) {
                MessageBox.Show("请填写店铺名字！");
                return;
            }

            if(this.currentEditShop.Id>0){
                updateShop(true);
                return;
            }
            //先生成ID
            //updateCurrentEditShopInfo();
            this.currentEditShop.Id = SqlHelper.saveShop(this.currentEditShop);
            //logo
           // handleImg(this.currentEditShop.Id);
            updateShop(false);
            //handleImgDel(currentEditShop.Id);
            //init();
            MessageBox.Show("添加成功");

            allShops = new ObservableCollection<Shop>(SqlHelper.getAllShop());
            this.list_allshop.ItemsSource = allShops;
            this.list_allshop.SelectedItem = this.currentEditShop;
            this.currentEditShop = this.list_allshop.SelectedItem as Shop;
            this.grid_allInfo.DataContext = this.currentEditShop;
            showShopInfo();

        }
        private void updateShop(bool isShowMsg)
        {
            if (this.currentEditShop == null)
            {
                return;
            }

            if (this.currentEditShop.Name == null || "".Equals(this.currentEditShop.Name.Trim()))
            {
                MessageBox.Show("请填写店铺名字！");
                return;
            }

            if (this.currentEditShop.Id <= 0) {
                saveShop();
                return;
            }
            updateCurrentEditShopInfo();
            handleImg(this.currentEditShop.Id);
            SqlHelper.updateShop(this.currentEditShop);
            handleImgDel(currentEditShop.Id);
            //init();
            if (isShowMsg) {
                MessageBox.Show("更新成功");
            }

            allShops = new ObservableCollection<Shop>(SqlHelper.getAllShop());
            this.list_allshop.ItemsSource = allShops;
            this.list_allshop.SelectedItem = this.currentEditShop;
            this.currentEditShop = this.list_allshop.SelectedItem as Shop;
            this.grid_allInfo.DataContext = this.currentEditShop;
            showShopInfo();

        }
        private void delShop() {
            if (this.currentEditShop != null) {
                SqlHelper.deletetShop(this.currentEditShop);
                SqlHelper.deleteAdditionalRelation(this.currentEditShop);
                this.currentEditShop.Logo = "";
                handleImgDel(currentEditShop.Id);
                init();
                MessageBox.Show("删除成功");
            }
        }
        private void createNew() {
            this.currentEditShop = new Shop();
            showShopInfo();
        }

        private void btn_create_Click(object sender, RoutedEventArgs e)
        {
            createNew();
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            updateShop(true);
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            saveShop();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            delShop();
        }

        private void list_allshop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.list_allshop.SelectedItem != null) {
                this.currentEditShop = this.list_allshop.SelectedItem as Shop;
                this.grid_allInfo.DataContext = this.currentEditShop;
                showShopInfo();
            }
            
        }

      


    }
}
