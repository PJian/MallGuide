using EntityManagementService.entity;
using EntityManageService.sqlUtil;
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
using System.Windows.Threading;

namespace SuperMarketLHS.page.other
{
    /// <summary>
    /// PageUpdate.xaml 的交互逻辑
    /// </summary>
    public partial class PageUpdate : Page
    {
        private MainWindow rootWin;
        private List<DataUpdateServer> allServer;
        private DataUpdateServer server;
        //  Thread t1 = null;
        private string dataFileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"temp\data.zip");
        private string dataFileDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"temp");
        // DispatcherTimer dt;
        BackgroundWorker sendFileWorker;
        BackgroundWorker zipFileWorker;
        public PageUpdate(MainWindow rootWin)
        {
            InitializeComponent();
            this.rootWin = rootWin;
        }

        private void btn_new_Click(object sender, RoutedEventArgs e)
        {
            this.txt_ip.Text = null;
        }

        private void init()
        {
            allServer = SqlHelper.getAllServer();
            for (int i = 0; i < allServer.Count; i++)
            {
                this.allServer.ElementAt(i).NodeStateImg = getNodeStateImg(ConstantData.SERVER_NODE_NOT_CONNECT);
            }

            for (int i = 0; i < allServer.Count; i++)
            {
                DataUpdateServer server = this.allServer.ElementAt(i);
                if (server.UpdateTime != null && !"".Equals(server.UpdateTime))
                {
                    if (DateTime.Parse(server.UpdateTime).DayOfYear - DateTime.Now.DayOfYear <= 1)
                    {
                        server.State = ConstantData.SERVER_NODE_UPDATE+"";
                        server.StateImg = getNodeStateImg(ConstantData.SERVER_NODE_UPDATE);
                    }
                }
                else
                {

                    server.State = ConstantData.SERVER_NODE_UPDATE_NOT_YET + "";
                    server.StateImg = getNodeStateImg(ConstantData.SERVER_NODE_UPDATE_NOT_YET);
                }

            }
            this.listBox_allServer.ItemsSource = allServer;
            //测试联通性
            Thread t = new Thread(delegate()
            {
                testConnect();
            });
            t.IsBackground = true;
            t.Start();
        }
        /// <summary>
        /// 测试连通性
        /// </summary>
        private void testConnect()
        {
            for (int i = 0; i < allServer.Count; i++)
            {
                SocketHelper.sendHeartData(SocketHelper.connectToServer(allServer.ElementAt(i).IP), System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"data\test"), delegate()
                {
                    sendHeardComplete(i);
                }, delegate()
                {
                    sendHeardFailed(i);
                });
            }
        }

        private string getNodeStateImg(int i)
        {
            switch (i)
            {
                case ConstantData.SERVER_NODE_CONNNECT: return "resource/circle_green.png";
                case ConstantData.SERVER_NODE_NOT_CONNECT: return "resource/circle_red.png";
            }
            return "";
        }
        private string getUpdateStateImg(int i)
        {
            switch (i)
            {
                case ConstantData.SERVER_NODE_UPDATE: return "resource/circle_green.png";
                case ConstantData.SERVER_NODE_UPDATE_NOT_YET: return "resource/circle_red.png";
            }
            return "";
        }

        /// <summary>
        /// 联通行测试完毕
        /// </summary>
        /// <param name="i"></param>
        private void sendHeardComplete(int i)
        {
            this.allServer.ElementAt(i).NodeState = ConstantData.SERVER_NODE_CONNNECT + "";
            this.allServer.ElementAt(i).NodeStateImg = getNodeStateImg(ConstantData.SERVER_NODE_CONNNECT);
        }
        private void sendHeardFailed(int i)
        {
            this.allServer.ElementAt(i).NodeState = ConstantData.SERVER_NODE_NOT_CONNECT + "";
            this.allServer.ElementAt(i).NodeStateImg = getNodeStateImg(ConstantData.SERVER_NODE_NOT_CONNECT);
        }

        private void sendDataComplete(int i)
        {
            this.allServer.ElementAt(i).NodeState = ConstantData.SERVER_NODE_UPDATE + "";
            this.allServer.ElementAt(i).NodeStateImg = getUpdateStateImg(ConstantData.SERVER_NODE_UPDATE);
            this.allServer.ElementAt(i).UpdateTime = DateTime.Now.ToString();
            SqlHelper.updateServer(this.allServer.ElementAt(i));

        }
        private void sendDataFailed(int i)
        {
            this.allServer.ElementAt(i).NodeState = ConstantData.SERVER_NODE_UPDATE_NOT_YET + "";
            this.allServer.ElementAt(i).NodeStateImg = getUpdateStateImg(ConstantData.SERVER_NODE_UPDATE_NOT_YET);
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (this.txt_ip.Text != null && !this.txt_ip.Text.Trim().Equals(""))
            {
                SqlHelper.saveUpdateServer(new DataUpdateServer()
                {
                    IP = this.txt_ip.Text
                });
                init();
            }
            else
            {
                MessageBox.Show("ip地址不正确");
            }
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            this.rootWin.loadIn();

            //压缩文件
            zipFileWorker = new BackgroundWorker();
            zipFileWorker.DoWork += zipFileWorker_DoWork;
            zipFileWorker.RunWorkerCompleted += zipFileWorker_RunWorkerCompleted;
            zipFileWorker.RunWorkerAsync();
        }

        private void zipFileWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            sendFile();
        }
        private void sendFile()
        {
            sendFileWorker = new BackgroundWorker();
            sendFileWorker.DoWork += sendFileWorker_DoWork;
            sendFileWorker.RunWorkerCompleted += sendFileWorker_RunWorkerCompleted;
            sendFileWorker.RunWorkerAsync();
        }

        void sendFileWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.rootWin.loadHide();
        }

        void zipFileWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            zip();
        }

        void sendFileWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            sendData();
        }

        private void zip()
        {
            ZipHelperHaoZip.zipDir(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data"), dataFileName);
        }

        private void sendData()
        {
            //init();//先去测试连通性
            for (int i = 0; i < allServer.Count; i++)
            {
                if (this.allServer.ElementAt(i).NodeState.Equals(ConstantData.SERVER_NODE_CONNNECT + ""))
                {
                    SendComplete sendComplete = delegate()
                    {
                        sendDataComplete(i);
                    };

                    SocketHelper.sendData(SocketHelper.connectToServer(allServer.ElementAt(i).IP), dataFileDir, sendComplete, delegate()
                    {
                        sendDataFailed(i);
                    });

                    
                }
            }


        }


        //void dt_Tick(object sender, EventArgs e)
        //{
        //    if (!t1.IsAlive)
        //    {

        //        Thread t2 = new Thread(new ThreadStart(sendData));
        //        t2.IsBackground = true;
        //        t2.Start();
        //        dt.Stop();

        //    }
        //}

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            if (this.server != null)
                SqlHelper.delUpdateServer(this.server.IP);
            init();
        }

        private void listBox_allServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.listBox_allServer.SelectedItem != null)
            {
                this.server = this.listBox_allServer.SelectedItem as DataUpdateServer;
                this.listBox_allServer.SelectedIndex = -1;
            }
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            init();
        }

    }
}
