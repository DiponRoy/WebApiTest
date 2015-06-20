using System.Web.Http.Dependencies;
using Ninject;

namespace Web.Api.Ioc
{
    public class NinjectDependencyResolver : NinjectScope, IDependencyResolver
    {
        protected readonly IKernel Kernel;
        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            Kernel = kernel;
        }
        public IDependencyScope BeginScope()
        {
            return new NinjectScope(Kernel.BeginBlock());
        }
    }
}