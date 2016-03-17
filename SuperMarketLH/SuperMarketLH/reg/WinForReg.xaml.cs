using SerialNum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SuperMarketLH.reg
{
    /// <summary>
    /// WinForReg.xaml 的交互逻辑
    /// </summary>
    public partial class WinForReg : Window
    {
        public Soft RegSoft { get; set; }//需要进行注册的软件实体
        public WinForReg()
        {
            InitializeComponent();
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //显示本机机器码
            this.CMPID.Text = HardWareInfo.getMachineCode();
        }

        /// <summary>
        /// 进行注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            string code = this.serialNumTxt.Text;
            if ("".Equals(code))
            {
                MessageBox.Show("请填写验证码");
            }
            else
            {
                RegisterTableMsg.registSoft = RegSoft;
                CodeUtil.regist(code, success, failed);
            }
        }

        public void success()
        {
            MessageBox.Show("注册成功，请重新启动程序");
        }
        public void failed()
        {
            MessageBox.Show("注册码错误");
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();//试用期结束，结束程序
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();//试用期结束，结束程序
        }
    }
}
