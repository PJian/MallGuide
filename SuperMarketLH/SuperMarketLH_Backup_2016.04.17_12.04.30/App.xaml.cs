using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SuperMarketLH
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App() {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }
        /// <summary>
        /// 抛出运行时异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(string.Format("系统出错!请联系管理员.{0}{1}", Environment.NewLine, e.Exception.Message));
            //Shutdown(1);
            e.Handled = true;
        }
    }
}
