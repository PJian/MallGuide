using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSqLiteTest
{
    public class Helper
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
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(brand);
                    transaction.Commit();
                }
            }
        }
    }
}
