using System.Web.Http.Dependencies;
using Ninject;

namespace Web.Api.Ioc
{
    public class NinjectIocContainer :  IIocContainer
    {
        private readonly IKernel _kernel;
        public NinjectIocContainer(IKernel kernel)
        {
            _kernel = kernel;
        }
        public TSource Get<TSource>()
        {
            return _kernel.Get<TSource>();
        }

        public IKernel Kernel()
        {
            return _kernel;
        }

        public void Dispose()
        {
            if (_kernel!= null && !_kernel.IsDisposed)
            {
                _kernel.Dispose();
            }
        }
    }
}