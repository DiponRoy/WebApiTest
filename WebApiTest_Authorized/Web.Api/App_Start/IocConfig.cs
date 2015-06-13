using System.Web.Http;
using Web.Api.Ioc;

namespace Web.Api
{
    public class IocConfig
    {
        public static void Register(HttpConfiguration config)
        {
            IocContainer.CreateDefaultKernalIfNotExists();
            config.DependencyResolver = new NinjectDependencyResolver(IocContainer.Kernel());
        }
    }
}