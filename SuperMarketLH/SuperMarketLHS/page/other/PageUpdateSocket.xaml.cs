using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using PJ.ConTcp;
using ResourceManagementService.helper;
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
    /// PageUpdateSocket.xaml 的交互逻辑
    /// </summary>
    public partial class PageUpdateSocket : Page
    {


        private string identifyFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sshKey", "id_rsa");
        private ClientComputer currentEditClient;
        private List<ClientComputer> allClients;
        private MainWindow rootWin;
        // SshTransferProtocolBase sshCp;
        public PageUpdateSocket()
        {
            InitializeComponent();
            this.currentEditClient = new ClientComputer();
            this.grid_client.DataContext = this.currentEditClient;
        }

        public PageUpdateSocket(MainWindow rootWin)
        {
            InitializeComponent();
            this.currentEditClient = new ClientComputer();
            this.grid_client.DataContext = this.currentEditClient;
            this.rootWin = rootWin;
        }

        private void btn_new_Click(object sender, RoutedEventArgs e)
        {
            currentEditClient = new ClientComputer();
            this.grid_client.DataContext = this.currentEditClient;
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentEditClient.IP == null || this.currentEditClient.IP.Equals(""))
            {
                MessageBox.Show("请填写IP地址！");
                return;
            }
            //if (this.currentEditClient.UserName == null || this.currentEditClient.UserName.Equals("")) {
            //    MessageBox.Show("请填写客户机用户名（主机登陆名）！");
            //    return;
            //}
            Client client = SocketHelper.connectToServer(this.currentEditClient.IP);
            string appDirPath = SocketHelper.getAppDir(client, null, null);
            string userName = SocketHelper.getUserNameOfComputer(client, null, null);
            if (appDirPath == null || appDirPath.Equals("") || userName == null || userName.Equals(""))
            {
                MessageBox.Show("指定主机无法连接，请查看对应主机是否已经正确配置！");
                return;
            }
            this.currentEditClient.UserName = userName;
            this.currentEditClient.AppDir = appDirPath;
            if (this.currentEditClient != null)
            {
                SqlHelper.saveCientHost(this.currentEditClient);
            }
            this.currentEditClient = new ClientComputer();
            this.grid_client.DataContext = this.currentEditClient;
            loadClients();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentEditClient != null)
            {
                SqlHelper.delClientHost(this.currentEditClient.IP);
            }
            this.currentEditClient = new ClientComputer();
            this.grid_client.DataContext = this.currentEditClient;
            loadClients();
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            updateData();
        }
        ClientComputer cc;
        /// <summary>
        /// 更新客户列表数据
        /// </summary>
        private void updateData()
        {
            BackgroundWorker bw = new BackgroundWorker();
            if (this.allClients != null)
            {
                rootWin.loadIn();
                bw.DoWork += bw_DoWork;
                bw.WorkerReportsProgress = true;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                bw.ProgressChanged += bw_ProgressChanged;
                bw.RunWorkerAsync();

            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rootWin.loadHide();
            // throw new NotImplementedException();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            //client = new SSHClient();
            //for (int i = 0; i < allClients.Count; i++)
            //{
            //    cc = null;
            //    cc = allClients.ElementAt(i);
            //    if (cc.IsConnected)
            //    {
            //        //目标程序的目录
            //        client.OnTransferStart += client_OnTransferStart;
            //        client.OnTransferProgress += client_OnTransferProgress;
            //        client.OnTransferEnd += client_OnTransferEnd;
            //        client.ScpTo(cc.IP, cc.UserName, identifyFileName, System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data"), System.IO.Path.Combine(cc.AppDir, "data"));
            //    }
            //}
        }

     


        private void listBox_allServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listBox_allServer.SelectedItem != null)
            {
                this.currentEditClient = this.listBox_allServer.SelectedItem as ClientComputer;
            }
        }

        private void loadClients()
        {
            allClients = SqlHelper.getAllClients();
            //测试联通性
            if (allClients != null)
            {
               // SSHClient client = new SSHClient();
                for (int i = 0; i < allClients.Count; i++)
                {
                 
                    //  allClients.ElementAt(i).IsConnected = client.isConnected(allClients.ElementAt(i).IP, allClients.ElementAt(i).UserName, identifyFileName);
                }
            }

            this.listBox_allServer.ItemsSource = allClients;
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            loadClients();
            // this.listBox_allServer.SelectedIndex = -1;
        }

        private void Page_Unloaded_1(object sender, RoutedEventArgs e)
        {

        }


    }
}
