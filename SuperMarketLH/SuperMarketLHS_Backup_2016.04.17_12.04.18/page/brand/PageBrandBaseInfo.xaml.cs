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

namespace SuperMarketLHS.page
{
    /// <summary>
    /// PageBrandBaseInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PageBrandBaseInfo : Page
    {
        private MainWindow rootWin;
        private ObservableCollection<Brand> allBrands;
        private Brand currentEditBrand;
        public PageBrandBaseInfo()
        {
            InitializeComponent();
           
        }
        public PageBrandBaseInfo(MainWindow rootWin)
        {
            InitializeComponent();
            this.rootWin = rootWin;
        }
        private void init() {
            allBrands = new ObservableCollection<Brand>(SqlHelper.getAllBrands());
            this.list_allBrand.ItemsSource = allBrands;
            this.currentEditBrand = new Brand();
            showBrandInfo();

        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            saveBrand();
        }
        /// <summary>
        /// 处理图片信息的移动，然后重新赋值
        /// </summary>
        /// <param name="id"></param>
        private void handleImg(int id) {
            //logo处理
            this.currentEditBrand.Logo = WinUtil.copyOne(userControl_logo.ImgPath, ConstantData.getBrandLogoDataFolder(id));
            //广告图片处理
            this.currentEditBrand.ImgPaths = WinUtil.copyImg(userControl_imgs.ImgPathes, ConstantData.getBrandImgsDataFolder(id)).ToArray();
        }
        private void handleImgDel(int id) {
            WinUtil.delFile(ConstantData.getBrandLogoDataFolder(id), new String[]{this.currentEditBrand.Logo}.ToList());
            WinUtil.delFile(ConstantData.getBrandImgsDataFolder(id), currentEditBrand.ImgPaths.ToList());
        }
        private void handlerCombox() {
            this.currentEditBrand.SortChar = this.combox_sortChar.SelectItem;
            this.currentEditBrand.CatagoryName = this.combox_Catagory.SelectItem;
        }

        /// <summary>
        /// 保存当前正在编辑的brand
        /// </summary>
        private void saveBrand() {
            if (this.currentEditBrand.Name == null || this.currentEditBrand.Name.Trim().Equals("")) {
                MessageBox.Show("请填写品牌名称!");
                return;
            }
            if (this.currentEditBrand == null) {
                return;
            }
            if (this.currentEditBrand.Id != 0) {
                updateBrand(true);
                return;
            }
            //handlerCombox();
            currentEditBrand.Id = SqlHelper.saveBrand(this.currentEditBrand);
            //handleImg(this.currentEditBrand.Id);
            updateBrand(false);
            //handleImgDel(currentEditBrand.Id);
            //init();
            MessageBox.Show("添加成功");
        }
        /// <summary>
        /// 更新当前正在编辑的brand
        /// </summary>
        private void updateBrand(bool isShowMsg) {
            if (this.currentEditBrand.Name == null || "".Equals(this.currentEditBrand.Name.Trim())) {
                MessageBox.Show("请填写品牌名称");
                return;
            }
            if (this.currentEditBrand.Id <= 0) {
                saveBrand();
                return;
            }
            handleImg(currentEditBrand.Id);
            handlerCombox();
            SqlHelper.updateBrand(this.currentEditBrand);
            handleImgDel(currentEditBrand.Id);
            init();
            if (isShowMsg) {
                MessageBox.Show("更新成功");
            }
        }
        /// <summary>
        /// 删除当前brand
        /// </summary>
        private void delBrand() {
            if (this.currentEditBrand != null && this.currentEditBrand.Id>0) {
                SqlHelper.deleteBrand(this.currentEditBrand);
                SqlHelper.deleteRelationBetShopAndBrand(this.currentEditBrand) ;
                this.currentEditBrand.Logo = "";
                this.currentEditBrand.ImgPaths = new string[0];
                handleImgDel(currentEditBrand.Id);
                init();
                MessageBox.Show("删除成功");
            }
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            delBrand();    
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            updateBrand(true);
        }

        private void list_allBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.list_allBrand.SelectedItem != null) {
                this.currentEditBrand = this.list_allBrand.SelectedItem as Brand;
                showBrandInfo();
            }
        }
        /// <summary>
        /// 将品牌信息显示到界面上
        /// </summary>
        private void showBrandInfo() {
            grid_allInfo.DataContext = this.currentEditBrand;
            //图片的显示
            if (this.currentEditBrand != null) {
                userControl_logo.ImgPath = this.currentEditBrand.Logo;
                if (this.currentEditBrand.ImgPaths != null) {
                    userControl_imgs.changeImgs(this.currentEditBrand.ImgPaths.ToList());
                }
            }
            //下拉框的显示
            combox_sortChar.SelectItem = this.currentEditBrand.SortChar;
            combox_Catagory.SelectItem = this.currentEditBrand.CatagoryName;
        }

        private void create() {
            this.currentEditBrand = new Brand();
            showBrandInfo();
        }
        private void btn_create_Click(object sender, RoutedEventArgs e)
        {
            create();
        }
    }
}
