using System.Web.Http;
using System.Web.Http.Dependencies;
using Ninject;
using Web.Api.Ioc;

namespace Web.Api
{
    public class IocConfig
    {
        public static void Register(HttpConfiguration config)
        {
            IocKernelProvider.CreateDefaultKernalIfNotExists();
            NinjectScope ninjectScope = new NinjectIocContainer(IocKernelProvider.Kernel());
            IocAdapter.SetContainer(ninjectScope as IIocContainer);
            config.DependencyResolver = ninjectScope as IDependencyResolver;
        }
    }
}