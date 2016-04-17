using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using SuperMarketLHS.comm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SuperMarketLHS.page.mall
{
    /// <summary>
    /// PageBaseInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PageBaseInfo : Page
    {
        private Mall currentMall;
        private  List<string> tempImgPaths;
        private string currentTempImgPath;
        private string currentTempMoviePath;
        private MainWindow rootWin;

        public PageBaseInfo()
        {
            InitializeComponent();
            loadMall();
           // init();
        }
        public PageBaseInfo(MainWindow rootWin)
        {
            InitializeComponent();
            loadMall();
            //init();
            this.rootWin = rootWin;
        }
        public PageBaseInfo(Mall mall)
        {
            InitializeComponent();
            currentMall = mall;
           // init();
        }
        private void init() {
            loadMall();
            this.grid_allInfo.DataContext = currentMall;
            tempImgPaths = new List<string>();
            currentTempImgPath = "";

            //加载图片
            if (currentMall.ImgPath != null && currentMall.ImgPath.Count > 0)
            {
                this.userCtrl_imgShow.addImg(this.currentMall.ImgPath);
            }
            //播放影片
            if (this.currentMall.MoviePath != null && !this.currentMall.MoviePath.Equals(""))
            {
                this.mediaElement.Source = new Uri(this.currentMall.MoviePath);
            }

            if (this.currentMall.ResourceType == ConstantData.RESOURCE_TYPE_IMG)
            {
                this.radioBtn_img.IsChecked = true;
                radioBtn_imgCheck();
                
            }
            else
            {
                this.radioBtn_movie.IsChecked = true;
               
                radioBtn_movieCheck();
            }
            
        }
        public void loadMall() {
            this.currentMall = SqlHelper.getMall();
            if (this.currentMall == null)
            {
                this.currentMall = new Mall()
                {
                    ResourceType = ConstantData.RESOURCE_TYPE_IMG
                };
            }
            currentMall.ImgPath = WinUtil.getPathesFull(this.currentMall.ImgPath);
            currentMall.MoviePath = WinUtil.getMoviePathFull(this.currentMall.MoviePath);
        }
        private void btn_chooseImg_Click(object sender, RoutedEventArgs e)
        {
            String path = WinUtil.chooseImg();
            this.txb_imgPath.Text = path;
        }

        private void btn_addImg_Click(object sender, RoutedEventArgs e)
        {
            currentTempImgPath = this.txb_imgPath.Text;
            if (currentTempImgPath != null && !currentTempImgPath.Trim().Equals("")) {
               // currentMall.ImgPath.Add(currentTempImgPath);
                this.txb_imgPath.Text = "";
                this.userCtrl_imgShow.addImg(currentTempImgPath);
                tempImgPaths.Add(currentTempImgPath);
            }
        }

        private void btn_delImg_Click(object sender, RoutedEventArgs e)
        {
            currentMall.ImgPath.Remove(this.userCtrl_imgShow.CurrentSelectItem);
            tempImgPaths.Remove(this.userCtrl_imgShow.CurrentSelectItem);
            this.userCtrl_imgShow.removeImg(this.userCtrl_imgShow.CurrentSelectIndex);
        }
        

        private void radioBtn_imgCheck() {
            if (this.IsLoaded)
            {
                this.grid_imgSelect.Visibility = System.Windows.Visibility.Visible;
                this.grid_movie.Visibility = System.Windows.Visibility.Collapsed;
                this.currentMall.ResourceType = ConstantData.RESOURCE_TYPE_IMG;
                //暂停影片
                this.mediaElement.Stop();
            }
        }
        private void radioBtn_movieCheck() {
            if (this.IsLoaded)
            {
                this.grid_imgSelect.Visibility = System.Windows.Visibility.Collapsed;
                this.grid_movie.Visibility = System.Windows.Visibility.Visible;
                this.currentMall.ResourceType = ConstantData.RESOURCE_TYPE_MOVIE;
                if (mediaElement.Source != null) {
                    mediaElement.Play();
                } 
               
            }
        }
        private void radioBtn_img_Checked(object sender, RoutedEventArgs e)
        {
            radioBtn_imgCheck();
        }

        private void radioBtn_movie_Checked(object sender, RoutedEventArgs e)
        {
            radioBtn_movieCheck();
        }

        private void movieCopy(string moviePath,string folder) {

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += DoWork_Handler;
            bw.RunWorkerAsync(new string[] { moviePath, folder });
            bw.RunWorkerCompleted += RunWorkerCompleted_Handler;
        }
       
        private static void DoWork_Handler(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            string[] a = args.Argument as string[];
            WinUtil.copyOne(a[0],a[1]);
        }


        private void RunWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs args)
        {
            currentMall.MoviePath = args.Result as string;
            rootWin.loadHide();
            if (this.currentMall.Id > 0)
            {
                SqlHelper.updateMall(currentMall);
            }
            else
            {
                SqlHelper.saveMall(currentMall);
            }
            
            //之前的资源的删除
            WinUtil.delFile(ConstantData.MALL_RESOURCE_MOVIE_FOLDER, new String[] { currentMall.MoviePath }.ToList());
            init();
            MessageBox.Show("更新成功！");
        }
        private void update() {
            if (currentMall.ResourceType == ConstantData.RESOURCE_TYPE_IMG)
            {
                //图片、影片资源的复制
                List<string> tempNewAddImgPath = WinUtil.copyImg(this.tempImgPaths, ConstantData.MALL_RESOURCE_IMG_FOLDER);
                currentMall.ImgPath.AddRange(tempNewAddImgPath);
                //将绝对路径变为相对路径
                handlePathBeforeSave();
                SqlHelper.updateMall(currentMall);
                //之前的资源的删除
                WinUtil.delFile(ConstantData.MALL_RESOURCE_IMG_FOLDER, currentMall.ImgPath);
                init();
                MessageBox.Show("添加成功！");
            }
            else
            {
                //将绝对路径变为相对路径
                handlePathBeforeSave();
                //图片、影片资源的复制
                rootWin.loadIn();
                WinUtil.movieCopy(this.currentTempMoviePath, ConstantData.MALL_RESOURCE_MOVIE_FOLDER, RunWorkerCompleted_Handler);
            }
           // init();
           // MessageBox.Show("更新成功！");
        }

        private void handlePathBeforeSave() {
            currentMall.ImgPath = WinUtil.getRelativePath(currentMall.ImgPath, ConstantData.MALL_RESOURCE_IMG_FOLDER);
            currentMall.MoviePath = WinUtil.getRelativePath(currentMall.MoviePath, ConstantData.MALL_RESOURCE_MOVIE_FOLDER);
        }

        private void save() {
            if (this.currentMall.Id > 0) {
                update();
                return;
            }
            if (currentMall.ResourceType == ConstantData.RESOURCE_TYPE_IMG)
            {
                //图片、影片资源的复制
                currentMall.MoviePath = "";
                List<string> tempNewAddImgPath = WinUtil.copyImg(this.tempImgPaths, ConstantData.MALL_RESOURCE_IMG_FOLDER);
                currentMall.ImgPath.AddRange(tempNewAddImgPath);
                //将绝对路径变为相对路径
                handlePathBeforeSave();
                SqlHelper.saveMall(currentMall);
                //之前的资源的删除
                WinUtil.delFile(ConstantData.MALL_RESOURCE_IMG_FOLDER, currentMall.ImgPath);
                init();
                MessageBox.Show("添加成功！");
            }
            else
            {
                //将绝对路径变为相对路径
                handlePathBeforeSave();
                //图片、影片资源的复制
                rootWin.loadIn();
                WinUtil.movieCopy(this.currentTempMoviePath, ConstantData.MALL_RESOURCE_MOVIE_FOLDER, RunWorkerCompleted_Handler);
            }
           
        }
        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            if (currentMall.Id == 0)
            {
                save();
            }
            else
            {
                update();
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void btn_chooseMovie_Click(object sender, RoutedEventArgs e)
        {
            String path = WinUtil.chooseMovie();
            this.txb_moviePath.Text = path;
        }

        private void btn_addMovie_Click(object sender, RoutedEventArgs e)
        {
            this.currentTempMoviePath = this.txb_moviePath.Text;
            this.txb_moviePath.Text = "";
            this.mediaElement.Source = new Uri(this.currentTempMoviePath);
             mediaElement.Play();
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

       
    }
}
