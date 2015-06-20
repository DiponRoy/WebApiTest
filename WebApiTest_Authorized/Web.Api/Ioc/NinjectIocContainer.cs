using System.Web.Http.Dependencies;
using Ninject;

namespace Web.Api.Ioc
{
    public class NinjectIocContainer : NinjectDependencyResolver, IIocContainer
    {
        public NinjectIocContainer(IKernel kernel)
            : base(kernel)
        {
        }
        public TSource Get<TSource>()
        {
            return Kernel.Get<TSource>();
        }
    }
}