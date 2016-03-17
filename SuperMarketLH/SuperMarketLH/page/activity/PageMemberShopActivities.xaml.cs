﻿using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using SuperMarketLH.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SuperMarketLH.page.activity
{
    /// <summary>
    /// PageMemberShopActivities.xaml 的交互逻辑
    /// </summary>
    public partial class PageMemberShopActivities : Page
    {
        private List<SalePromotion> salePromotion;
        private MainWindow parent;
        private SalePromotion currentSalePromotion;
        private DispatcherTimer startTabTipsTimer;

        public PageMemberShopActivities()
        {
            InitializeComponent();
        }
        public PageMemberShopActivities(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
        }
        private void init() {
            salePromotion = SqlHelper.getNormalSalePromotion();
            showActivityImgs();


            //salePromotion = SqlHelper.getMemberShipSalePromotion();
            //this.list_allActivities.ItemsSource = salePromotion;
            //if (this.list_allActivities.Items.Count > 0) {
            //    this.list_allActivities.SelectedIndex = 0;
            //}
        }

        private void setCurrentSalePromotionByImgIndex() {
            int index = this.userContrl_imgs.CurrentShowImgIndex;
            int salPromotionIndex = 0;
            int totalPreImgCount = 0;
            while (salPromotionIndex < this.salePromotion.Count)
            {
                totalPreImgCount += salePromotion.ElementAt(salPromotionIndex).ImgPaths.Length;
                if (index < totalPreImgCount)
                {
                    break;
                }
                salPromotionIndex++;
            }
            currentSalePromotion = salePromotion.ElementAt(salPromotionIndex);
        }
      

        private void btn_join_Click(object sender, RoutedEventArgs e)
        {
            setCurrentSalePromotionByImgIndex();
            this.grid_jion.Visibility = Visibility.Visible;


            //if (this.currentSalePromotion != null)
            //{
            //    new WindowJoinSalePromotion(this.currentSalePromotion).ShowDialog();
            //}
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

        /// <summary>
        /// 改变按钮的背景
        /// </summary>
        private void changeBtnBG(string buttonName, string imgPath)
        {
            if (!File.Exists(imgPath)) return;
            BinaryReader binReader = new BinaryReader(File.Open(imgPath, FileMode.Open));
            FileInfo fileInfo = new FileInfo(imgPath);
            byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
            binReader.Close();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.EndInit();
            Button btn = FindName(buttonName) as Button;
            if (btn != null)
            {
                btn.Background = new ImageBrush(bitmap);
            }
        }

        private void btn_promotion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_promotion", "resource/images/navBtn/btn28_press.png");
        }

        private void btn_promotion_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_promotion", "resource/images/navBtn/btn28.png");
        }

        private void btn_promotion_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_promotion", "resource/images/navBtn/btn28_press.png");
        }

        private void btn_promotion_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_promotion", "resource/images/navBtn/btn28.png");
        }

        private void btn_promotionMembers_MouseDown(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_promotionMembers", "resource/images/navBtn/btn27_press.png");
        }

        private void btn_promotionMembers_MouseUp(object sender, MouseButtonEventArgs e)
        {
            changeBtnBG("btn_promotionMembers", "resource/images/navBtn/btn27.png");
        }

        private void btn_promotionMembers_TouchDown(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_promotionMembers", "resource/images/navBtn/btn27_press.png");
        }

        private void btn_promotionMembers_TouchUp(object sender, TouchEventArgs e)
        {
            changeBtnBG("btn_promotionMembers", "resource/images/navBtn/btn27.png");
        }

        private void btn_promotion_Click(object sender, RoutedEventArgs e)
        {
            salePromotion = SqlHelper.getNormalSalePromotion();
            showActivityImgs();
        }

        /// <summary>
        /// 显示当前活动的图片
        /// </summary>
        private void showActivityImgs()
        {
            
            List<string> imgs = new List<string>();
            DateTime now = DateTime.Now;
            for (int i = 0; i < salePromotion.Count; i++)
            {
                DateTime salPromotionEndDate = DateTime.Parse(salePromotion.ElementAt(i).EndTime);
                //在有效期内，则显示活动列表
                if (now < salPromotionEndDate)
                {
                    imgs.AddRange(salePromotion.ElementAt(i).ImgPaths);
                }
            }
            userContrl_imgs.Imgs = imgs.ToArray();
        }

        private void btn_promotionMembers_Click(object sender, RoutedEventArgs e)
        {
            salePromotion = SqlHelper.getMemberShipSalePromotion();
            showActivityImgs();
        }

        /// <summary>
        /// 报名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_sign_Click(object sender, RoutedEventArgs e)
        {
            if (validateMobileNum())
            {
                SqlHelper.saveAssignActivitiesInfo(this.text_phoneNumber.Text, this.currentSalePromotion);
                MessageBox.Show("报名成功！");
                this.grid_jion.Visibility = Visibility.Collapsed;
                endTabTip();
            }
            else
            {
                MessageBox.Show("请输入正确的手机号！");
            }
        }
        public bool validateMobileNum()
        {
            return Regex.IsMatch(this.text_phoneNumber.Text, "\\d{11}");
        }

        private void btn_cancle_Click(object sender, RoutedEventArgs e)
        {
            endTabTip();
            this.grid_jion.Visibility = Visibility.Collapsed;
        }

        private void text_phoneNumber_GotFocus(object sender, RoutedEventArgs e)
        {

            startTabTipsTimer = new DispatcherTimer();
            startTabTipsTimer.Tick += startTabTipsTimer_Tick;
            startTabTipsTimer.Interval = TimeSpan.FromSeconds(1);
            startTabTipsTimer.IsEnabled = true;
        }

        void startTabTipsTimer_Tick(object sender, EventArgs e)
        {
            WinUtil.startTabTip();
        }

        private void endTabTip() {
            if (this.startTabTipsTimer != null) {
                this.startTabTipsTimer.IsEnabled = false;
                this.startTabTipsTimer.Stop();
            }
            WinUtil.killTabTip();
        }
        //private void text_phoneNumber_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    WinUtil.killTabTip();
        //}

        

    }
}
