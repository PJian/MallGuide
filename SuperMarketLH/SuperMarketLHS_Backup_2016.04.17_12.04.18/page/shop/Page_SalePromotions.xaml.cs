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

namespace SuperMarketLHS.page.shop
{
    /// <summary>
    /// Page_SalePromotions.xaml 的交互逻辑
    /// </summary>
    public partial class Page_SalePromotions : Page
    {
        private SalePromotion currentEditSalePromotion;
        private List<SalePromotion> allSalePromotion;
        private MainWindow rootWin;
        public Page_SalePromotions()
        {
            InitializeComponent();
        }
        public Page_SalePromotions(MainWindow rootWin)
        {
            InitializeComponent();
            this.rootWin = rootWin;
        }
        private void init()
        {
            this.allSalePromotion = SqlHelper.getAllSalePromotions();
            this.list_allSalePromotions.ItemsSource = this.allSalePromotion;
            creaateNew();
        }
        /// <summary>
        /// 处理图片的移动
        /// </summary>
        /// <param name="id"></param>
        private void handleImg(int id)
        {
            this.currentEditSalePromotion.ImgPaths = WinUtil.copyImg(userControl_imgs.ImgPathes, ConstantData.getSalePromotionImgsDataFolder(id)).ToArray();
        }
        /// <summary>
        /// 删除多余的图片
        /// </summary>
        /// <param name="id"></param>
        private void delImg(int id)
        {
            WinUtil.delFile(ConstantData.getSalePromotionImgsDataFolder(id), currentEditSalePromotion.ImgPaths.ToList());
        }
        /// <summary>
        /// 将界面上的信息写入currentEditSalePromotion  entity 
        /// </summary>
        private void fillSalePromotionInfo()
        {
            //促销对象
            if ((bool)this.radio_member.IsChecked)
            {
                this.currentEditSalePromotion.Range = ConstantData.SALE_PROMOITON_OBJECT_MEMBER;
            }
            else
            {
                this.currentEditSalePromotion.Range = ConstantData.SALE_PROMOTION_OBJECT_ALL;
            }
            //时间
            if (this.time_start.SelectedValue != null)
            {
                this.currentEditSalePromotion.StartTime = this.time_start.SelectedValue.ToString();
            }
            if (this.time_end.SelectedValue != null)
            {
                this.currentEditSalePromotion.EndTime = this.time_end.SelectedValue.ToString();
            }
            //活动图片

        }
        /// <summary>
        /// 将信息填入界面
        /// </summary>
        private void showCurrentSelectSalePromotionInfo()
        {
            this.grid_allInfo.DataContext = this.currentEditSalePromotion;
            if (this.currentEditSalePromotion != null)
            {
                //促销对象
                if (this.currentEditSalePromotion.Range == ConstantData.SALE_PROMOITON_OBJECT_MEMBER)
                {
                    this.radio_member.IsChecked = true;
                }
                else
                {
                    this.radio_noLimition.IsChecked = true;
                }
                //时间
                if (this.currentEditSalePromotion.StartTime != null && !"".Equals(this.currentEditSalePromotion.StartTime.Trim()))
                {
                    this.time_start.SelectedValue = DateTime.Parse(this.currentEditSalePromotion.StartTime);
                }
                else
                {
                    this.time_start.SelectedValue = null;
                }
                if (this.currentEditSalePromotion.EndTime != null && !"".Equals(this.currentEditSalePromotion.EndTime.Trim()))
                {
                    this.time_end.SelectedValue = DateTime.Parse(this.currentEditSalePromotion.EndTime);
                }
                else
                {
                    this.time_end.SelectedValue = null;
                }
                //活动图片
                if (this.currentEditSalePromotion.ImgPaths != null)
                {
                    userControl_imgs.changeImgs(this.currentEditSalePromotion.ImgPaths.ToList());
                }
                else
                {
                    userControl_imgs.changeImgs(new String[0].ToList());
                }
            }
        }

        private void save()
        {
            if (this.currentEditSalePromotion.Name == null || this.currentEditSalePromotion.Name.Trim().Equals("")) {
                MessageBox.Show("请输入活动名称");
                return;
            }
            if (this.currentEditSalePromotion.Id > 0) {
                update(true);
                return;
            }
            this.currentEditSalePromotion.Id = SqlHelper.saveSalePromotion(this.currentEditSalePromotion);
            update(false);
            MessageBox.Show("添加成功！");
        }

        private void update(bool showMsg)
        {
            if (this.currentEditSalePromotion.Name == null || this.currentEditSalePromotion.Name.Trim().Equals(""))
            {
                MessageBox.Show("请输入活动名称");
                return;
            }
            if (this.currentEditSalePromotion.Id <= 0) {
                save();
                return;
            }
            fillSalePromotionInfo();
            handleImg(this.currentEditSalePromotion.Id);
            SqlHelper.updateSalePromotion(this.currentEditSalePromotion);
            delImg(this.currentEditSalePromotion.Id);
            init();
            if (showMsg) {
                MessageBox.Show("更新成功！");
            }
        }

        private void del()
        {
            if (this.currentEditSalePromotion != null && this.currentEditSalePromotion.Id > 0) {
                SqlHelper.deleteSalePromotion(this.currentEditSalePromotion);
                //删除与店铺的联系
                SqlHelper.deleteSalePromotionFromShop(this.currentEditSalePromotion);
                this.currentEditSalePromotion.ImgPaths = new string[0];
                delImg(currentEditSalePromotion.Id);
                init();
                MessageBox.Show("删除成功！");
            }
        }

        private void creaateNew()
        {
            this.currentEditSalePromotion = new SalePromotion();
            this.grid_allInfo.DataContext = null;
            this.grid_allInfo.DataContext = currentEditSalePromotion;
            showCurrentSelectSalePromotionInfo();
        }

        private void btn_create_Click(object sender, RoutedEventArgs e)
        {
            creaateNew();
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
            del();
        }

        private void list_allSalePromotions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.list_allSalePromotions.SelectedItem != null) {
                this.currentEditSalePromotion = this.list_allSalePromotions.SelectedItem as SalePromotion;
                showCurrentSelectSalePromotionInfo();
            }
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }
    }
}
