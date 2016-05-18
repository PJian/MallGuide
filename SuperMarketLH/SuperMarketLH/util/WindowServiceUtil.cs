using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;

namespace SuperMarketLH.util
{
    class WindowServiceUtil
    {
        private static AssemblyInstaller AssemblyInstaller1;
        private static string service_name = "";
        #region 安装服务  
        /// <summary>  
        /// 安装服务  
        /// </summary>  
        public static  bool InstallService(string exePath,string NameService)
        {
            bool flag = true;
            if (!IsServiceIsExisted(NameService))
            {
                try
                {
                    //string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string serviceFileName = Path.Combine(exePath, NameService+ ".exe");
                    service_name = NameService;
                    InstallmyService(null, serviceFileName);
                    //服务自启动
                    ChangeServiceStartType(2, NameService);
                    //启动服务
                    StartService(NameService);
                }
                catch
                {
                    flag = false;
                }

            }
            return flag;
        }
        #endregion

        #region 卸载服务  
        /// <summary>  
        /// 卸载服务  
        /// </summary>  
        public bool UninstallService(string NameService)
        {
            bool flag = true;
            if (IsServiceIsExisted(NameService))
            {
                try
                {
                    string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string serviceFileName = location.Substring(0, location.LastIndexOf('\\') + 1) + NameService + ".exe";
                    UnInstallmyService(serviceFileName);
                }
                catch
                {
                    flag = false;
                }
            }
            return flag;
        }
        #endregion

        #region 检查服务存在的存在性  
        /// <summary>  
        /// 检查服务存在的存在性  
        /// </summary>  
        /// <param name=" NameService ">服务名</param>  
        /// <returns>存在返回 true,否则返回 false;</returns>  
        public static bool IsServiceIsExisted(string NameService)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName.ToLower() == NameService.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 安装Windows服务  
        /// <summary>  
        /// 安装Windows服务  
        /// </summary>  
        /// <param name="stateSaver">集合</param>  
        /// <param name="filepath">程序文件路径</param>  
        public static void InstallmyService(IDictionary stateSaver, string filepath)
        {
            AssemblyInstaller1 = new AssemblyInstaller();
            AssemblyInstaller1.UseNewContext = true;
            AssemblyInstaller1.Path = filepath;
            AssemblyInstaller1.Install(stateSaver);
            AssemblyInstaller1.Commit(stateSaver);
            AssemblyInstaller1.Committed += AssemblyInstaller1_Committed;
            
            AssemblyInstaller1.Dispose();
        }

        private static void AssemblyInstaller1_Committed(object sender, InstallEventArgs e)
        {
            try
            {
                ConnectionOptions myConOptions = new ConnectionOptions();
                myConOptions.Impersonation = ImpersonationLevel.Impersonate;
                ManagementScope mgmtScope = new System.Management.ManagementScope(@"root\CIMV2", myConOptions);

                mgmtScope.Connect();
                ManagementObject wmiService = new ManagementObject("Win32_Service.Name='" + service_name + "'");

                ManagementBaseObject InParam = wmiService.GetMethodParameters("Change");

                InParam["DesktopInteract"] = true;

                ManagementBaseObject OutParam = wmiService.InvokeMethod("Change", InParam, null);

            }
            catch (Exception err)
            {
                //Common.wLog(err.ToString());
            }
        }
        #endregion

        #region 卸载Windows服务  
        /// <summary>  
        /// 卸载Windows服务  
        /// </summary>  
        /// <param name="filepath">程序文件路径</param>  
        public static void UnInstallmyService(string filepath)
        {
            AssemblyInstaller AssemblyInstaller1 = new AssemblyInstaller();
            AssemblyInstaller1.UseNewContext = true;
            AssemblyInstaller1.Path = filepath;
            AssemblyInstaller1.Uninstall(null);
            AssemblyInstaller1.Dispose();
        }
        #endregion

        #region 判断window服务是否启动  
        /// <summary>  
        /// 判断某个Windows服务是否启动  
        /// </summary>  
        /// <returns></returns>  
        public static bool IsServiceStart(string serviceName)
        {
            ServiceController psc = new ServiceController(serviceName);
            bool bStartStatus = false;
            try
            {
                if (!psc.Status.Equals(ServiceControllerStatus.Stopped))
                {
                    bStartStatus = true;
                }

                return bStartStatus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region  修改服务的启动项  
        /// <summary>    
        /// 修改服务的启动项 2为自动,3为手动    
        /// </summary>    
        /// <param name="startType"></param>    
        /// <param name="serviceName"></param>    
        /// <returns></returns>    
        public static bool ChangeServiceStartType(int startType, string serviceName)
        {
            try
            {
                RegistryKey regist = Registry.LocalMachine;
                RegistryKey sysReg = regist.OpenSubKey("SYSTEM");
                RegistryKey currentControlSet = sysReg.OpenSubKey("CurrentControlSet");
                RegistryKey services = currentControlSet.OpenSubKey("Services");
                RegistryKey servicesName = services.OpenSubKey(serviceName, true);
                servicesName.SetValue("Start", startType);
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;


        }
        #endregion

        #region 启动服务  
        public static  bool StartService(string serviceName)
        {
            bool flag = true;
            if (IsServiceIsExisted(serviceName))
            {
                System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                if (service.Status != System.ServiceProcess.ServiceControllerStatus.Running && service.Status != System.ServiceProcess.ServiceControllerStatus.StartPending)
                {
                    service.Start();
                    for (int i = 0; i < 60; i++)
                    {
                        service.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                        {
                            break;
                        }
                        if (i == 59)
                        {
                            flag = false;
                        }
                    }
                }
            }
            return flag;
        }
        #endregion

        #region 停止服务  
        private bool StopService(string serviceName)
        {
            bool flag = true;
            if (IsServiceIsExisted(serviceName))
            {
                System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    service.Stop();
                    for (int i = 0; i < 60; i++)
                    {
                        service.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (service.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                        {
                            break;
                        }
                        if (i == 59)
                        {
                            flag = false;
                        }
                    }
                }
            }
            return flag;
        }
        #endregion

       


    }
}
