using System;
using Ninject;
using Ninject.Modules;

namespace Web.Api.Ioc
{
    public class IocKernelProvider
    {
        private static IKernel _kernel;

        public static IKernel Kernel()
        {
            if (_kernel == null || _kernel.IsDisposed)
            {
                throw new NullReferenceException("kernel is null or disposed at IocKernelProvider");
            }
            return _kernel;
        }

        public static void CreateKernelWith(params INinjectModule[] modules)
        {
            SetKernel(new StandardKernel(modules));
        }

        public static void SetKernel(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new NullReferenceException("kernel is null.");
            }
            _kernel = kernel;
        }

        public static void CreateDefaultKernalIfNotExists()
        {
            if (_kernel == null || _kernel.IsDisposed)
            {
                CreateKernelWith(new NinjectIocModule());
            }
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