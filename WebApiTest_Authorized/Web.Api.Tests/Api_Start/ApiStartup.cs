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
        }
        
        public void Configuration(IAppBuilder app)
        {
            IocContainer.CreateKernelWith(Ioc);
            new Startup().Configuration(app); /*or set the configuration's again*/
        }

        public static void Dispose()
        {
            IocContainer.Dispose();
            Ioc.Dispose();
        }
    }
}