using System;
using Db;
using Ninject.Activation;
using Ninject.Modules;
using Web.Api.Auth;

namespace Web.Api.Ioc
{
    public class IocModule : NinjectModule, IIocModule
    {
        //this helps for mocking
        public Func<IContext, IUmsDb> UmsDbFunc { get; set; }
        public Func<IContext, IAuthContext> AuthContextFunc { get; set; }

        public IocModule()
        {
            UmsDbFunc = x => new UmsDb();
            AuthContextFunc = x => new AuthContext();
        }

        public override void Load()
        {
            Bind<IUmsDb>().ToMethod(UmsDbFunc).InSingletonScope();
            Bind<IAuthContext>().ToMethod(AuthContextFunc); //important used for oauth provider
        }

    }

    public interface IIocModule
    {
        Func<IContext, IUmsDb> UmsDbFunc { get; set; }
        Func<IContext, IAuthContext> AuthContextFunc { get; set; }
    }
}