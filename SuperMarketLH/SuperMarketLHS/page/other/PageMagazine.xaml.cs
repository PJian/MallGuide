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

namespace SuperMarketLHS.page.other
{
    /// <summary>
    /// PageMagazine.xaml 的交互逻辑
    /// </summary>
    public partial class PageMagazine : Page
    {
        private MainWindow rootWin;
        public PageMagazine()
        {
            InitializeComponent();
        }
        public PageMagazine(MainWindow rootWin)
        {
            InitializeComponent();
            this.rootWin = rootWin;
        }
        private ImageADResource imageAdResource;
        private void init()
        {
            this.imageAdResource = SqlHelper.getImageAdResourceByType(ConstantData.IMAGE_AD_RESOURCE_TYPE_ELEC_MAGAZINE);
            if (this.imageAdResource == null)
            {
                this.imageAdResource = new ImageADResource();
                this.imageAdResource.Type = ConstantData.IMAGE_AD_RESOURCE_TYPE_ELEC_MAGAZINE;
            }
            if (this.imageAdResource.Imgs != null)
            {
                this.userCtrol_image.changeImgs(this.imageAdResource.Imgs.ToList());
            }
            else
            {
                userCtrol_image.changeImgs(new String[0].ToList());
            }
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }
        /// <summary>
        /// 保存
        /// </summary>
        public void save()
        {

            if (this.imageAdResource.Id <= 0)
            {
                //新增
                this.imageAdResource.Id = SqlHelper.saveImageADResource(this.imageAdResource);
            }
            update();
            MessageBox.Show("保存成功");
            init();
        }

        private void update()
        {
            string imgFolder = ConstantData.getImgADResourceFolder(this.imageAdResource.Id);
            this.imageAdResource.Imgs = WinUtil.copyImg(userCtrol_image.ImgPathes, imgFolder).ToArray();
            SqlHelper.updateImageADResource(this.imageAdResource);
            WinUtil.delFile(imgFolder, imageAdResource.Imgs.ToList());
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

       
    }
}
