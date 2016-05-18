using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using TransferServerLib;

namespace SuperMarketLH.socketserver
{
    class Server
    {
        bool ServiceStatus = false;
        IBootstrap bootstrap;
        object lockobj = new object();
        DataTable dt;
        
        public bool isRestart { get; set; }


        public void startServer() {
            bootstrap = BootstrapFactory.CreateBootstrap();

            dt = new DataTable();
            dt.Columns.Add("SessionId", typeof(string));
            dt.Columns.Add("RemoteEndPoint", typeof(string));
            dt.Columns.Add("IsUpLoading", typeof(bool));
            dt.Columns.Add("FileName", typeof(string));
            dt.Columns.Add("FileSize", typeof(long));
            dt.Columns.Add("SaveName", typeof(string));
            dt.Columns.Add("TransferPos", typeof(long));
            dt.Columns.Add("TransferLength", typeof(long));
            dt.Columns.Add("TransferedLength", typeof(long));
            dt.Columns.Add("Status", typeof(bool));
            dt.Columns.Add("Speed", typeof(string));

            if (!bootstrap.Initialize())
            {
                ServiceStatus = false;
                MessageBox.Show("Failed to initialize!");
                return;
            }
            else
            {
                StartResult result = bootstrap.Start();
                if (result == StartResult.Failed)
                {
                    ServiceStatus = false;
                }
                else if (result == StartResult.Success)
                {
                    var transferServer = bootstrap.GetServerByName("TransferServer") as TransferServer;
                    transferServer.NewSessionConnected += transferServer_NewSessionConnected;
                    transferServer.SessionClosed += transferServer_SessionClosed;
                    ServiceStatus = true;
                }
                else
                {
                    MessageBox.Show(string.Format("Start result: {0}!", result));
                }
                //this.Text = string.Format("Start result: {0}!", result);
            }
        }

        public void stopServer() {
           
            bootstrap.Stop();
            ServiceStatus = false;
        }

      

        void transferServer_SessionClosed(TransferSession session, SuperSocket.SocketBase.CloseReason value)
        {
           // Application.DoEvents();
        }

        void transferServer_NewSessionConnected(TransferSession session)
        {

            lock (lockobj)
            {
                DataRow dr = dt.NewRow();
                dr["SessionId"] = session.SessionID;
                dr["RemoteEndPoint"] = session.RemoteEndPoint;
                dr["IsUpLoading"] = false;
                dr["FileName"] = string.Empty;
                dr["FileSize"] = 0;
                dr["SaveName"] = string.Empty;
                dr["TransferPos"] = 0;
                dr["TransferLength"] = 0;
                dr["TransferedLength"] = 0;
                dr["Status"] = true;
                dr["Speed"] = "0kb/s";
                dt.Rows.Add(dr);
                dr.AcceptChanges();
            }
            session.UpLoadEngine.StartTransfer += (e) =>
            {
               System.Threading.Thread.Sleep(1000);
                lock (lockobj)
                {
                    var row = dt.Select(string.Format("SessionId='{0}'", session.SessionID))[0];
                    row["IsUpLoading"] = true;
                    row["FileName"] = e.UpLoadInfo.FileName;
                    row["FileSize"] = e.UpLoadInfo.FileSize;
                    row["TransferPos"] = e.UpLoadInfo.TransferPos;
                    row["TransferLength"] = e.UpLoadInfo.TransferLength;
                    row.AcceptChanges();
                }
            };
            DateTime dtTime = DateTime.Now;
            session.UpLoadEngine.TransferStep += (e) =>
            {

                lock (lockobj)
                {
                    var row = dt.Select(string.Format("SessionId='{0}'", session.SessionID))[0];
                    row["TransferedLength"] = e.TransferLen;// session.UpLoadEngine.TransferedLength;
                    row["Speed"] = string.Format("{0}Mb/s", e.TransferLen / (1024 * 1024) / ((DateTime.Now - dtTime).Seconds));
                    row.AcceptChanges();
                }
            };
            session.UpLoadEngine.TransferComplete += (e) =>
            {
                lock (lockobj)
                {
                    var row = dt.Select(string.Format("SessionId='{0}'", session.SessionID))[0];
                    row["IsUpLoading"] = false;
                    row.AcceptChanges();
                }

                //遍历datatable,如果所有的文件都接受完成，则重新启动程序
                //bool isAllComplete = true;
                //foreach (DataRow item in dt.Rows)
                //{
                //    if ((bool)item["IsUpLoading"])
                //    {
                //        isAllComplete = false;
                //        break;
                //    }
                //}
                //if (isAllComplete && isRestart)
                //{
                //    Application.Current.Shutdown();
                //    System.Reflection.Assembly.GetEntryAssembly();
                //    string startpath = System.IO.Directory.GetCurrentDirectory();
                //    System.Diagnostics.Process.Start(startpath + "/SuperMarketLH.exe");
                //    Console.WriteLine("done");
                //}

            };
        }

       

      
    }
}
