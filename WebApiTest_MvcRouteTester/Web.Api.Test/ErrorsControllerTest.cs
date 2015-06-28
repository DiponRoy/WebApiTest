using System;
using System.Net.Http;
using System.Web.Http;
using MvcRouteTester;
using Newtonsoft.Json;
using NUnit.Framework;
using Web.Api.Controllers;

namespace Web.Api.Test
{
    /*
     * ohh: doesn't care about the exceptions
     */
    [TestFixture]
    public class ErrorsControllerTest
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
        public void Get()
        {
            int id = 1;
            HttpConfig.ShouldMap(String.Format("/api/errors/get?id={0}", id))
                .To<ErrorsController>(HttpMethod.Get, x => x.Get(id));
        }

        [Test]
        public void All()
        {
            HttpConfig.ShouldMap("/api/errors/all")
                .To<ErrorsController>(HttpMethod.Get, x => x.All());
        }
    }
}
