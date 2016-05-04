using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using ShortenMe.DataAccess.Contracts.Models;

namespace ShortenMe.DataAccess
{
    public abstract class BaseDA
    {
        private ISessionFactory sessionFactory;

        public BaseDA()
        {
            #if DEBUG
            string connectionString = "Data Source=.;Initial Catalog=ShortenMe;Integrated Security=True";
            #else
            string connectionString = @"Server=tcp:shortenme.database.windows.net,1433;Database=ShortenMe;User ID=systemadmin@shortenme;Password=Asdfghjkl@322;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            #endif

            sessionFactory = Fluently
                .Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString).ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BaseDA>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();
        }

        protected ISession OpenSession()
        {
            return sessionFactory.OpenSession();
        }
    }
}