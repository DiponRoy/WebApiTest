using System.Web.Http.Dependencies;
using Ninject;

namespace Web.Api.Ioc
{
    public class NinjectDependencyResolver : NinjectScope, IDependencyResolver
    {
        private readonly IKernel _kernel;
        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            _kernel = kernel;
        }
        public IDependencyScope BeginScope()
        {
            return new NinjectScope(_kernel.BeginBlock());
        }
    }
}