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
            string connectionString = "Data Source=.;Initial Catalog=ShortenMe;Integrated Security=True";

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