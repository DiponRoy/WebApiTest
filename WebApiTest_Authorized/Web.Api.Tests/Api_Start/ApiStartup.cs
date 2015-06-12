using System;
using System.Web.Http;
using System.Web.Mvc;
using Db;
using Moq;
using Owin;
using Web.Api.Auth;
using Web.Api.Ioc;

namespace Web.Api.Tests
{
    public class ApiStartup
    {
        public static IocMockModule Ioc;

        public static void Setup()
        {
            Ioc = new IocMockModule();
            Ioc.AuthContextProvider = x => new Mock<IAuthContext>().Object;
            Ioc.UmsDbProvider = x => new Mock<IUmsDb>().Object;
        }
        
        public void Configuration(IAppBuilder app)
        {
            IocContainer.SetModule(Ioc);
            new Startup().Configuration(app); /*or set the configuration's again*/
        }

        public static void Dispose()
        {
            Ioc = null;
        }
    }
}