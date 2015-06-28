using System;
using System.Net.Http;
using System.Web.Http;
using Db;
using MvcRouteTester;
using Newtonsoft.Json;
using NUnit.Framework;
using Web.Api.Controllers;

namespace Web.Api.Test
{
    /*
     * ohh: doesn't care about the ioc
     */
    [TestFixture]
    public class IocControllerTest
    {
        protected HttpConfiguration HttpConfig { get; set; }

        [SetUp]
        public void Setup()
        {
            HttpConfig = new HttpConfiguration();
            WebApiConfig.Register(HttpConfig);
        }

        [TearDown]
        public void Teardown()
        {
            HttpConfig = null;
        }


        [Test]
        public void All()
        {
            HttpConfig.ShouldMap("/api/ioc/all")
                .To<IocController>(HttpMethod.Get, x => x.All());
        }
    }
}
