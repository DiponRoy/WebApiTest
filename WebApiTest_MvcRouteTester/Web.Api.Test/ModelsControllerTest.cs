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
    [TestFixture]
    public class ModelsControllerTest
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
        public void Add()
        {
            var user = new User { Id = 1, Name = "User", IsActive = true };
            HttpConfig.ShouldMap("/api/models/add")
                .WithJsonBody(JsonConvert.SerializeObject(user))
                .To<ModelsController>(HttpMethod.Post, x => x.Add(user));
        }

        [Test]
        public void Replace()
        {
            var user = new User { Id = 1, Name = "User", IsActive = true };
            HttpConfig.ShouldMap(String.Format("/api/models/replace?id={0}", user.Id))
                .WithJsonBody(JsonConvert.SerializeObject(user))
                .To<ModelsController>(HttpMethod.Put, x => x.Replace(user.Id, user));
        }

        /*
         * ohh: form url body example
         */
        [Test]
        public void Remove()
        {
            var user = new User { Id = 1, Name = "User", IsActive = true };
            HttpConfig.ShouldMap("/api/models/remove")
                .WithFormUrlBody(String.Format("id={0}&name={1}&isActive={2}", user.Id, user.Name, user.IsActive))
                .To<ModelsController>(HttpMethod.Post, x => x.Remove(user));
        }
    }
}
