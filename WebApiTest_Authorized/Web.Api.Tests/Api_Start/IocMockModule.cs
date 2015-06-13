using System;
using Db;
using Moq;
using Ninject.Activation;
using Ninject.Modules;
using Web.Api.Auth;
using Web.Api.Ioc;

namespace Web.Api.Tests
{
    public class IocMockModule : IocModule
    {
        public IocMockModule()
        {
            AuthContextFunc = x => new Mock<IAuthContext>().Object;
            UmsDbFunc = x => new Mock<IUmsDb>().Object;
        }

        public new void Dispose()
        {
            AuthContextFunc = null;
            UmsDbFunc = null;
            base.Dispose(true);
        }
    }
}