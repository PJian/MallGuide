using EntityManagementService.entity;
using EntityManagementService.sqlUtil;
using EntityManagementService.util;
using PlatformDev;
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
using System.Windows.Threading;

namespace SuperMarketLHS.page.other
{
    /// <summary>
    /// PageUpdateHttp.xaml 的交互逻辑
    /// </summary>
    public partial class PageUpdateHttp : Page
    {
        
        private ClientComputer currentEditClient;
        private List<ClientComputer> allClients;
        private MainWindow rootWin;
        private DispatcherTimer progressTimer;
        // SshTransferProtocolBase sshCp;
        private int transferFileNum = 0;

        BackgroundWorker bw;
        private List<String> filePathNeedToSend;

        public PageUpdateHttp()
        {
            InitializeComponent();
            this.currentEditClient = new ClientComputer();
            this.grid_client.DataContext = this.currentEditClient;
            transferFileNum = Util.countFileNum(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data"));
            filePathNeedToSend = Util.getAllFiles(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data"));
        }

        public PageUpdateHttp(MainWindow rootWin)
        {
            InitializeComponent();
            this.currentEditClient = new ClientComputer();
            this.grid_client.DataContext = this.currentEditClient;
            this.rootWin = rootWin;
            transferFileNum = Util.countFileNum(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data"));
            filePathNeedToSend = Util.getAllFiles(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data"));

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
            
            if (!HttpHelper.heart(this.currentEditClient.IP)) {
                MessageBox.Show("指定主机无法连接，请查看对应主机是否已经正确配置！");
                return;
            }
            if (this.currentEditClient != null)
            {
                transferFileNum = Util.countFileNum(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data"));
                this.currentEditClient.TotalFileNum = transferFileNum;
                this.currentEditClient.UpdateFileNum = 0;
                SqlHelperForDataTransfer.saveCientHost(this.currentEditClient);
            }
            this.currentEditClient = new ClientComputer();
            this.grid_client.DataContext = this.currentEditClient;
            loadClients();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentEditClient != null)
            {
                SqlHelperForDataTransfer.delClientHost(this.currentEditClient.IP);
            }
            this.currentEditClient = new ClientComputer();
            this.grid_client.DataContext = this.currentEditClient;
            loadClients();
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            updateData();
        }
        //ClientComputer cc;
        /// <summary>
        /// 更新客户列表数据
        /// </summary>
        private void updateData()
        {

            bw = new BackgroundWorker();
            if (this.allClients != null)
            {
                rootWin.loadIn();
                bw.DoWork += bw_DoWork;
                bw.WorkerReportsProgress = false;
                bw.RunWorkerCompleted += bw_RunWorkerCompleted;
                bw.ProgressChanged += Bw_ProgressChanged; ;
                bw.RunWorkerAsync(this);
            }
          //  progressTimer = new DispatcherTimer();
          //  progressTimer.Tick += ProgressTimer_Tick;
          //  progressTimer.Interval = TimeSpan.FromMilliseconds(500);
          //  progressTimer.IsEnabled = true;
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
            this.allClients.ElementAt(0).UpdateFileNum = e.ProgressPercentage;
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            List<ClientComputer> clients = XmlUtil.getAllClientComputer();
            if (this.allClients != null)
            {
                foreach (ClientComputer client in this.allClients)
                {
                    foreach (ClientComputer cc in clients)
                    {
                        if (cc.IP.Equals(client.IP))
                        {
                            client.TotalFileNum = cc.TotalFileNum;
                            client.UpdateFileNum = cc.UpdateFileNum;
                            break;
                        }
                    }
                }
                //判断是否全部更新完成
                Boolean complete = true;
                foreach (ClientComputer client in this.allClients)
                {
                    if (!client.State.Equals("更新失败") && client.UpdateFileNum < client.TotalFileNum)
                    {
                        complete = false;
                    }
                }
                if (complete)
                {
                    XmlUtil.clearClient();
                    //   progressTimer.Stop();
                }
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rootWin.loadHide();

        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            int transferFileNum = Util.countFileNum(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data"));
            for (int i = 0; i < allClients.Count; i++)
            {
                ClientComputer cc = allClients.ElementAt(i);
                cc.TotalFileNum = transferFileNum;
                cc.UpdateFileNum = 0;
                updateData(cc, transferFileNum);
            }

        }

        public void updateData(ClientComputer cc, int totalFileNum)
        {
            // clientComputer.UpdateFileNum = 0;
            //clientComputer.TotalFileNum = transferFileNum;
            
            cc.UpdateFileNum = 0;
            cc.TotalFileNum = totalFileNum;
            int count = 0;
            while (!HttpHelper.stop(cc.IP)) ;
            foreach (String file in filePathNeedToSend)
            {

                string saveFileName = file.Replace(AppDomain.CurrentDomain.BaseDirectory, "");
                while (count++ <= 10 && !HttpHelper.sendFileShap(cc.IP, file, saveFileName)) ;//发送失败的就一直发送
                if (count > 10)
                {
                    cc.State = "更新失败";
                    cc.TotalFileNum = transferFileNum;
                   // XmlUtil.writeClientComputer(cc);
                    break;
                }
                cc.UpdateFileNum += 1;
                cc.TotalFileNum = transferFileNum;
               // XmlUtil.writeClientComputer(cc);
                count = 0;
            }
            while(!HttpHelper.start(cc.IP));
        }

        private void listBox_allServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listBox_allServer.SelectedItem != null)
            {
                this.currentEditClient = this.listBox_allServer.SelectedItem as ClientComputer;
            }
        }

      
       

        /// <summary>
        /// 测试连通性
        /// </summary>
        private void testConnect()
        {
            for (int i = 0; i < allClients.Count; i++)
            {
                allClients.ElementAt(i).IsConnected = HttpHelper.heart(allClients.ElementAt(i).IP);
            }
        }



        private void loadClients()
        {
            allClients = SqlHelperForDataTransfer.getAllClients();
            //测试联通性
            if (allClients != null)
            {
                // SSHClient client = new SSHClient();
                testConnect();
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
