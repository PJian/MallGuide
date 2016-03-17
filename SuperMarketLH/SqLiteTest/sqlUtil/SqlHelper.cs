using EntityManageService.entity;
using NHibernate;
using NHibernate.Cfg;
using SqLiteTest.entity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityManageService.sqlUtil
{
    /// <summary>
    /// sqlite 数据库帮助类
    /// </summary>
    public class SqlHelper
    {
        public static ISessionFactory getSessionFactory()
        {
            Configuration configuration = new Configuration();
            configuration.Configure(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "hibernate.cfg.xml");
            configuration.AddAssembly(System.Reflection.Assembly.GetExecutingAssembly());
            ISessionFactory factory = configuration.BuildSessionFactory();
            return factory;
        }

        public static void saveBrand(Brand brand)
        {
            using (ISession session = getSessionFactory().OpenSession())
            {
                session.SaveOrUpdate(brand);
            }
        }
        public void delete() { 
        
        }
        public void update() { 
        }
    }
}
