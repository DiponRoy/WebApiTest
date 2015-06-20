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
            IKernel kernel = IocKernelProvider.Kernel();

            IocAdapter.SetContainer(new NinjectIocContainer(kernel));
            config.DependencyResolver = new NinjectDependencyResolver(kernel);
        }
    }
}