using System;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Modules;

namespace Web.Api.Ioc
{
    public class IocContainer
    {
        private static IKernel _kernel;

        public static IKernel Kernel()
        {
            if (_kernel == null || _kernel.IsDisposed)
            {
                throw new NullReferenceException("kernel is null or disposed at IocContainer");
            }
            return _kernel;
        }

        public static void CreateKernelWith(params INinjectModule[] modules)
        {
            _kernel = new StandardKernel(modules);
        }

        public static void CreateDefaultKernalIfNotExists()
        {
            if (_kernel == null || _kernel.IsDisposed)
            {
                CreateKernelWith(new IocModule());
            }
        }

        public static TSource Get<TSource>()
        {
            IKernel kernel = Kernel();
            return kernel.Get<TSource>();
        }

        public static void Dispose()
        {
            if (_kernel != null && !_kernel.IsDisposed)
            {
                _kernel.Dispose();
            }
            _kernel = null;
        }
    }
}