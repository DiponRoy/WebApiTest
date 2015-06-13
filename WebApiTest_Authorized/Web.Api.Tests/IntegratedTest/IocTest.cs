using Db;
using NUnit.Framework;
using Web.Api.Auth;
using Web.Api.Ioc;

namespace Web.Api.Tests.IntegratedTest
{
    [TestFixture]
    class IocTest
    {
        protected IIocModule IocModule { get; set; }

        [SetUp]
        public void SetUp()
        {
            IocModule = new IocModule();
            IocContainer.CreateDefaultKernalIfNotExists(); 
        }

        [TearDown]
        public void TearDown()
        {
            IocModule = null;
            IocContainer.Dispose();
        }

        [Test]
        public void IUmsDb_Injection()
        {
            Assert.IsInstanceOf<UmsDb>(IocContainer.Get<IUmsDb>());
            Assert.IsInstanceOf<UmsDb>(IocModule.UmsDbFunc.Invoke(null));
        }

        [Test]
        public void IUmsDb_Injection_SingletonInScope()
        {
            var value = IocContainer.Get<IUmsDb>();
            value.Admins = null;
            Assert.IsNull(IocContainer.Get<IUmsDb>().Admins); //Singleton
        }

        [Test]
        public void IAuthContext_Injection()
        {
            Assert.IsInstanceOf<AuthContext>(IocContainer.Get<IAuthContext>());
            Assert.IsInstanceOf<AuthContext>(IocModule.AuthContextFunc.Invoke(null));
        }

        [Test]
        public void IAuthContext_Injection_NewInstace()
        {
            var value = IocContainer.Get<IAuthContext>();
            value.Logins = null;
            Assert.IsNotNull(IocContainer.Get<IAuthContext>().Logins); //creates new
        }

    }
}
