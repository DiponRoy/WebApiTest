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
    public class ValuesControllerTest
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
            HttpConfig.ShouldMap(String.Format("/api/values/get?id={0}", id))
                .To<ValuesController>(HttpMethod.Get, x => x.Get(id));
        }

        [Test]
        public void All()
        {
            HttpConfig.ShouldMap("/api/values/all")
                .To<ValuesController>(HttpMethod.Get, x => x.All());
        }

        [Test]
        public void Add()
        {
            string name = "value";
            HttpConfig.ShouldMap("/api/values/add")
                .WithJsonBody(JsonConvert.SerializeObject(new {value = name}))
                .To<ValuesController>(HttpMethod.Post, x => x.Add(name));
        }

        [Test]
        public void Replace()
        {
            int id = 1;
            string name = "value";
            HttpConfig.ShouldMap(String.Format("/api/values/replace?id={0}", id))
                .WithJsonBody(JsonConvert.SerializeObject(new {value = name}))
                .To<ValuesController>(HttpMethod.Put, x => x.Replace(id, name));
        }

        [Test]
        public void Remove()
        {
            int id = 1;
            HttpConfig.ShouldMap(String.Format("/api/values/remove?id={0}", id))
                .To<ValuesController>(HttpMethod.Delete, x => x.Remove(id));
        }
    }
}
