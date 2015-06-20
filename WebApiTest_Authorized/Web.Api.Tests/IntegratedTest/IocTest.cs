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
            IocModule = new NinjectIocModule();
            IocKernelProvider.CreateDefaultKernalIfNotExists(); 
            IocAdapter.SetContainer(new NinjectIocContainer(IocKernelProvider.Kernel()));
        }

        [TearDown]
        public void TearDown()
        {
            IocModule = null;
            IocKernelProvider.Dispose();
        }

        [Test]
        public void IUmsDb_Injection()
        {
            Assert.IsInstanceOf<UmsDb>(IocAdapter.Container.Get<IUmsDb>());
            Assert.IsInstanceOf<UmsDb>(IocModule.UmsDbFunc.Invoke(null));
        }

        [Test]
        public void IUmsDb_Injection_SingletonInScope()
        {
            var value = IocAdapter.Container.Get<IUmsDb>();
            value.Admins = null;
            Assert.IsNull(IocAdapter.Container.Get<IUmsDb>().Admins); //Singleton
        }

        [Test]
        public void IAuthContext_Injection()
        {
            Assert.IsInstanceOf<AuthContext>(IocAdapter.Container.Get<IAuthContext>());
            Assert.IsInstanceOf<AuthContext>(IocModule.AuthContextFunc.Invoke(null));
        }

        [Test]
        public void IAuthContext_Injection_NewInstace()
        {
            var value = IocAdapter.Container.Get<IAuthContext>();
            value.Logins = null;
            Assert.IsNotNull(IocAdapter.Container.Get<IAuthContext>().Logins); //creates new
        }

    }
}
