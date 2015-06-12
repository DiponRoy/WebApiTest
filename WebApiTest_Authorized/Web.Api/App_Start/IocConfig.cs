using System.Web.Http;
using Db;
using Ninject;
using Web.Api.Auth;
using Web.Api.Ioc;

namespace Web.Api
{
    public class IocConfig
    {
        public static void Register(HttpConfiguration config)
        {          
            config.DependencyResolver = new NinjectDependencyResolver(IocContainer.Kernel());
        }
    }
}