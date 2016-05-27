using EntityManagementService.entity;
using EntityManageService.sqlUtil;
using PlatformDev;
using ResourceManagementService.helper;
using Socket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        private ClientComputer currentEditClient;
        private List<ClientComputer> allClients;
        private MainWindow rootWin;
        // SshTransferProtocolBase sshCp;
        private int transferFileNum = 0;
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
            transferFileNum = Util.countFileNum(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data"));
            for (int i = 0; i < allClients.Count; i++)
            {
                ClientComputer cc = allClients.ElementAt(i);
                updateData(cc);
                //Thread t = new Thread(()=> {
                    
                //});
                //t.Start();
            }

        }

        public void updateData(ClientComputer clientComputer) {
            clientComputer.UpdateFileNum = 0;
            clientComputer.TotalFileNum = transferFileNum;
            SocketHelper.sendData(SocketHelper.connectToServer(clientComputer.IP), System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"sConfig"), delegate ()
            {
                Thread.Sleep(1000);//休息5秒钟，让客户端程序关闭
                SocketHelper.sendData(SocketHelper.connectToServer(clientComputer.IP), System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data"), delegate ()
                {
                    clientComputer.UpdateFileNum = transferFileNum;
                    clientComputer.TotalFileNum = transferFileNum;
                    SocketHelper.sendData(SocketHelper.connectToServer(clientComputer.IP), System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"updateConfig"), delegate ()
                    {

                    }, delegate (string filename1) {
                        Console.WriteLine(filename1 + "send success");
                    }, delegate ()
                    {

                    });

                }, delegate (string filename1) {
                    clientComputer.UpdateFileNum += 1;
                    clientComputer.TotalFileNum = transferFileNum;
                }, delegate ()
                {
                    MessageBox.Show("客户端: " + clientComputer.IP + "更新失败！");
                });
            }, delegate (string filename) {
                Console.WriteLine(filename + "send success");
               
            }, delegate ()
            {
               
            });


         
        }
        
        private void listBox_allServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listBox_allServer.SelectedItem != null)
            {
                this.currentEditClient = this.listBox_allServer.SelectedItem as ClientComputer;
            }
        }

        /// <summary>
        /// 联通行测试完毕
        /// </summary>
        /// <param name="i"></param>
        private void sendHeardComplete(int i)
        {
            this.allClients.ElementAt(i).IsConnected = true; 
           // this.allClients.ElementAt(i).NodeStateImg = getNodeStateImg(ConstantData.SERVER_NODE_CONNNECT);
        }
        private void sendHeardFailed(int i)
        {
            this.allClients.ElementAt(i).IsConnected = false;
        }

        /// <summary>
        /// 测试连通性
        /// </summary>
        private void testConnect()
        {
            for (int i = 0; i < allClients.Count; i++)
            {
                SocketHelper.sendHeartData(SocketHelper.connectToServer(allClients.ElementAt(i).IP), System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data\test"), delegate ()
                {
                    sendHeardComplete(i);
                }, delegate ()
                {
                    sendHeardFailed(i);
                });
            }
        }



        private void loadClients()
        {
            allClients = SqlHelper.getAllClients();
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
