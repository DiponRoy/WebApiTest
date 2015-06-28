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
            IKernel kernel = new StandardKernel(new NinjectIocModule());
            config.DependencyResolver = new NinjectDependencyResolver(kernel);
        }
    }
}