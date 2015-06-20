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
        public static MockIocModule MockIoc;

        public static void Setup()
        {
            MockIoc = new MockIocModule();
        }
        
        public void Configuration(IAppBuilder app)
        {
            IocKernelProvider.CreateKernelWith(MockIoc);
            new Startup().Configuration(app); /*or set the configuration's again*/
        }

        public static void Dispose()
        {
            IocKernelProvider.Dispose();
            MockIoc.Dispose();
        }
    }
}