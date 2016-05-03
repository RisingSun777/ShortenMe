using Microsoft.Practices.Unity;
using ShortenMe.DataAccess;
using ShortenMe.DataAccess.Contracts;
using ShortenMe.Services;
using ShortenMe.Services.Contracts;
using System.Web.Http;
using Unity.WebApi;

namespace ShortenMe.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<IDateTimeProvider, DateTimeProvider>();
            container.RegisterType<ILinkInfoDA, LinkInfoDA>();
            container.RegisterType<IDateAccessDA, DateAccessDA>();

            container.RegisterType<ILinkInfoService, LinkInfoService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}