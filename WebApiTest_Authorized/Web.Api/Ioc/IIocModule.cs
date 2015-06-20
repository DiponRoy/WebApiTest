using System;
using Db;
using Ninject.Activation;
using Web.Api.Auth;

namespace Web.Api.Ioc
{
    public interface IIocModule
    {
        Func<IContext, IUmsDb> UmsDbFunc { get; set; }
        Func<IContext, IAuthContext> AuthContextFunc { get; set; }
    }
}