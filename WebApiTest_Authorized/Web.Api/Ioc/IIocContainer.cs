using System;
using Ninject;

namespace Web.Api.Ioc
{
    public interface IIocContainer : IDisposable
    {
        TSource Get<TSource>();
        IKernel Kernel();
    }
}