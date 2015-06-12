using System;
using Ninject;
using Ninject.Modules;

namespace Web.Api.Ioc
{
    public class IocContainer
    {
        private static NinjectModule _module;

        public static void SetModule(NinjectModule module)
        {
            _module = module;
        }

        private static NinjectModule GetModule()
        {
            return _module ?? new IocModule();
        }

        public static IKernel Kernel()
        {
            return new StandardKernel(GetModule());
        }

        public static TSource Get<TSource>()
        {
            IKernel kernel = Kernel();
            if (kernel == null)
            {
                throw new NullReferenceException("kernel is null at IocContainer");
            }
            return kernel.Get<TSource>();
        }
    }
}