using EntityManagementService.entity;
using EntityManageService.sqlUtil;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperMarketLH.page.other
{
    /// <summary>
    /// PageFireEmergency.xaml 的交互逻辑
    /// </summary>
    public partial class PageFireEmergency : Page
    {
        private MainWindow parent;
        public PageFireEmergency()
        {
            InitializeComponent();
        }
        public PageFireEmergency(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
        }
        private ImageADResource imageAdResource;
        private void init()
        {
            this.imageAdResource = SqlHelper.getImageAdResourceByType(ConstantData.IMAGE_AD_RESOURCE_TYPE_FIRE_EMERGENCE);
            this.userContrl_imgs.Imgs = imageAdResource.Imgs;
        }
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }
    }
}
