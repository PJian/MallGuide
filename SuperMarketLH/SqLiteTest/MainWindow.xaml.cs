using EntityManageService.entity;
using EntityManageService.sqlUtil;
using ResourceManagementService.helper;
using SqLiteTest.entity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using ZipLib;

namespace SqLiteTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteConnection sqLiteConnection;
        Thread t1 =null;
        public MainWindow()
        {
            InitializeComponent();

           // createNewDataBase();
            //connection();
            //createTable();
            //fillTable();

            //showMsg();
           // t1 = new Thread(new ThreadStart(zip));
           // t1.IsBackground = true;
           // t1.Start();
           //// zip();
           // DispatcherTimer dt = new DispatcherTimer();
           // dt.Tick += dt_Tick;
           // dt.Interval = TimeSpan.FromMinutes(1);
           // dt.IsEnabled = true;
          //  MessageBox.Show(DateTime.Now.ToString());
           // ZipHelperHaoZip.zipDir(@"E:\项目\SuperMarketLH\SuperMarketLHS\bin\Debug\data", "d:\\test.zip");
            ZipHelperHaoZip.unZip("d:\\test.zip", "D:\\test");
        }

        void dt_Tick(object sender, EventArgs e)
        {
            if (!t1.IsAlive) {
                MessageBox.Show("压缩完成！");
            }
        }


        private void zip() {
            //ZipHelper zipHelper = new ZipHelper();
           // zipHelper.UnZip(@"E:\test.zip", @"E:\test");
           //zipHelper.ZipFileFromDirectory(@"E:\项目\SuperMarketLH\SuperMarketLH\bin\Debug\data", @"E:\test.zip", 9);
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        private void createNewDataBase() {
            SQLiteConnection.CreateFile("MyDataBase.sqLite");
        }
        public void connection() {
            sqLiteConnection = new SQLiteConnection("DataSource=MyDataBase.sqLite;Version=3;");
            sqLiteConnection.Open();
        }

        private void createTable() {
            string sql = "create table test(name varchar(20),score int)";
            SQLiteCommand cmd = new SQLiteCommand(sql,sqLiteConnection);
            cmd.ExecuteNonQuery();
            
        }

        private void fillTable(){
            string sql = "insert into  test(name,score) values('Me',3000)";
            SQLiteCommand cmd = new SQLiteCommand(sql,sqLiteConnection);
            cmd.ExecuteNonQuery();

            sql = "insert into  test(name,score) values('Myself',5000)";
            cmd = new SQLiteCommand(sql,sqLiteConnection);
            cmd.ExecuteNonQuery();

            sql = "insert into  test(name,score) values('and i',6000)";
            cmd = new SQLiteCommand(sql,sqLiteConnection);
            cmd.ExecuteNonQuery();
           
        }

        private void showMsg() {
            string sql = "select * from test order by score";
            SQLiteCommand cmd = new SQLiteCommand(sql,sqLiteConnection);
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                Console.WriteLine("Name:"+reader["name"]+"\tscore:"+reader["score"]);
            }
            Console.WriteLine();
        }
    }
}
