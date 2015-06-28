using System;
using System.Net.Http;
using System.Web.Http;
using MvcRouteTester;
using Newtonsoft.Json;
using NUnit.Framework;
using Web.Api.Controllers;

namespace Web.Api.Test
{
    [TestFixture]
    public class NamesControllerTest
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
            HttpConfig.ShouldMap(String.Format("/api/names/get?id={0}", id))
                .To<NamesController>(HttpMethod.Get, x => x.GetSingle(id));
        }

        [Test]
        public void All()
        {
            HttpConfig.ShouldMap("/api/names/all")
                .To<NamesController>(HttpMethod.Get, x => x.GetAll());
        }
    }
}
