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
     * ohh: doesn't care about security attr's
     */
    [TestFixture]
    public class SecuritiesControllerTest
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
        public void Replace()
        {
            int id = 1;
            string name = "value";
            HttpConfig.ShouldMap(String.Format("/api/securities/replace?id={0}", id))
                .WithJsonBody(JsonConvert.SerializeObject(new { value = name }))
                .To<SecuritiesController>(HttpMethod.Put, x => x.Replace(id, name));
        }

        [Test]
        public void Add()
        {
            string name = "value";
            HttpConfig.ShouldMap("/api/securities/add")
                .WithJsonBody(JsonConvert.SerializeObject(new {value = name}))
                .To<SecuritiesController>(HttpMethod.Post, x => x.Add(name));
        }
    }
}
