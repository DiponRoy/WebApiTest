using System;
using Db;
using Ninject.Activation;
using Ninject.Modules;
using Web.Api.Auth;

namespace Web.Api.Tests
{
    public class IocMockModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUmsDb>().ToMethod(UmsDbProvider);
            Bind<IAuthContext>().ToMethod(AuthContextProvider);
        }

        public Func<IContext, IAuthContext> AuthContextProvider { get; set; }
        public Func<IContext, IUmsDb> UmsDbProvider { get; set; }
    }
}