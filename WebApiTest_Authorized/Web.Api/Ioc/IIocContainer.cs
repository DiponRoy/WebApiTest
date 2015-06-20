using System;

namespace Web.Api.Ioc
{
    public interface IIocContainer : IDisposable
    {
        TSource Get<TSource>();
    }
}