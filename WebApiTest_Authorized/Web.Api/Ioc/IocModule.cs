using System;
using Db;
using Ninject.Activation;
using Ninject.Modules;
using Web.Api.Auth;

namespace Web.Api.Ioc
{
    public class IocModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUmsDb>().To<UmsDb>();
            Bind<IAuthContext>().To<AuthContext>(); //important used for oauth provider
        }
    }
}