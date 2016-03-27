using EntityManagementService.entity;
using EntityManagementService.sqlUtil;
using EntityManageService.sqlUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SuperMarketLHS.page.other
{
    /// <summary>
    /// PageRemoteDataSetting.xaml 的交互逻辑
    /// </summary>
    public partial class PageRemoteDataSetting : Page
    {
        private DBServer server;
        private MainWindow rootWin;
        public PageRemoteDataSetting()
        {
            InitializeComponent();
        }

        public PageRemoteDataSetting(MainWindow root)
        {
            InitializeComponent();
            rootWin = root;
        }

        private void init() {
            server = SqlHelper.getDBServer();
            if (server == null)
            {
                server = new DBServer()
                {
                    Used = false
                };
            }
            else {
                if (server.Used) this.btn_init.IsEnabled = true;
            }
            this.grid_allInfo.DataContext = server;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (server.Ip == null || server.Ip.Equals("") || server.UserName == null || server.UserName.Equals("") || server.Password == null || server.Password.Equals("")) {
                MessageBox.Show("IP地址、用户名、密码不能为空！");
                return;
            }
            SqlHelper.saveServer(server);
            MessageBox.Show("配置成功！");
            this.server = SqlHelper.getDBServer();
            this.grid_allInfo.DataContext = this.server;
            this.btn_init.IsEnabled = true;
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            if (server.Ip == null || server.Ip.Equals("") || server.UserName == null || server.UserName.Equals("") || server.Password == null || server.Password.Equals(""))
            {
                MessageBox.Show("IP地址、用户名、密码不能为空！");
                return;
            }
            SqlHelper.updateServer(server);
            MessageBox.Show("配置更新成功！");
            this.server = SqlHelper.getDBServer();
            this.grid_allInfo.DataContext = this.server;
            this.btn_init.IsEnabled = true;
        }


        private BackgroundWorker bw;

        private void btn_init_Click(object sender, RoutedEventArgs e)
        {
           

            if (server.Used)
            {
                this.rootWin.loadIn();
                bw = new BackgroundWorker();
                bw.DoWork += bw_DoWork;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                bw.RunWorkerAsync();
            }
            else {
                MessageBox.Show("数据库配置未启用");
            }
            
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.rootWin.loadHide();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            SqlHelperDB.initDataBase();
        }
    }
}
