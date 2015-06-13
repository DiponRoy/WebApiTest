using Db;
using NUnit.Framework;
using Web.Api.Auth;
using Web.Api.Ioc;

namespace Web.Api.Tests.IntegratedTest
{
    [TestFixture]
    class IocContainerTest
    {

        [Test]
        public void IUmsDb_Injection()
        {
            IocContainer.CreateDefaultKernalIfNotExists();
            Assert.IsInstanceOf<UmsDb>(IocContainer.Get<IUmsDb>());
        }

        [Test]
        public void IAuthContext_Injection()
        {
            IocContainer.CreateDefaultKernalIfNotExists();
            Assert.IsInstanceOf<AuthContext>(IocContainer.Get<IAuthContext>());
        }
    }
}
