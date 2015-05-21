using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Db;
using Db.Models;
using FizzWare.NBuilder;
using Moq;
using Newtonsoft.Json;
using Ninject;
using NUnit.Framework;

namespace Web.Api.Tests
{
    [TestFixture]
    public class LocAndDefaultConstructorTest
    {
        protected Mock<IUmsDb> Db { get; set; }
        protected HttpServer Server { get; set; }
        protected HttpConfiguration Config { get; set; }

        [SetUp]
        public void Setup()
        {
            //configuration
            Config = new HttpConfiguration();
            Config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            WebApiConfig.Register(Config);
            Config.EnsureInitialized();

            //mock db
            Db = new Mock<IUmsDb>();
        }

        public void InitializeServerWithMockedIoc()
        {
            if (Db == null)
            {
                throw new NullReferenceException("Mocked Db is null");
            }
            if (Config == null)
            {
                throw new NullReferenceException("Config is null");
            }

            // Ninject IoC for mocked db
            var kernel = new StandardKernel();
            kernel.Bind<IUmsDb>().ToMethod(x => Db.Object);
            Config.DependencyResolver = new NinjectDependencyResolver(kernel);

            //set configuration
            Server = new HttpServer(Config);
        }

        public void InitializeServerWithIoc()
        {
            if (Config == null)
            {
                throw new NullReferenceException("Config is null");
            }

            // Ninject IoC
            IocConfig.Register(Config);

            //set configuration
            Server = new HttpServer(Config);
        }

        public void InitializeServerWithoutIoc()
        {
            if (Config == null)
            {
                throw new NullReferenceException("Config is null");
            }

            //set configuration
            Server = new HttpServer(Config);
        }

        [TearDown]
        public void TearDown()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
        }

        private HttpRequestMessage CreateRequest(string url, string mthv, HttpMethod method)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://test/" + url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));
            request.Method = method;

            return request;
        }

        /*
         * Ioc injection has the priority
         *      if loc bind, the the default Constructor doesn't work
         */

        [Test]
        public void GetAll__WithLoc_MockedDb_Injection_Worked()
        {
            int adminCount = 10;
            Db.Setup(x => x.Admins).Returns(Builder<Admin>.CreateListOfSize(adminCount).Build());
            InitializeServerWithMockedIoc();

            List<Admin> values;
            var client = new HttpClient(Server);
            HttpRequestMessage request = CreateRequest("api/admin/all", "application/json", HttpMethod.Get);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                values = JsonConvert.DeserializeObject<List<Admin>>(response.Content.ReadAsStringAsync().Result);
            }

            Assert.AreEqual(adminCount, values.Count);
        }

        [Test]
        public void GetAll_WithIoc_DefaultDb_Injection_Worked()
        {
            InitializeServerWithIoc();

            List<Admin> values;
            var client = new HttpClient(Server);
            HttpRequestMessage request = CreateRequest("api/admin/all", "application/json", HttpMethod.Get);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                values = JsonConvert.DeserializeObject<List<Admin>>(response.Content.ReadAsStringAsync().Result);
            }

            Assert.AreEqual(1, values.Count);
        }

        [Test]
        public void GetAll_WithoutIoc_DefaultConstructor_Worked()
        {
            InitializeServerWithoutIoc();

            List<Admin> values;
            var client = new HttpClient(Server);
            HttpRequestMessage request = CreateRequest("api/admin/all", "application/json", HttpMethod.Get);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                values = JsonConvert.DeserializeObject<List<Admin>>(response.Content.ReadAsStringAsync().Result);
            }

            Assert.AreEqual(0, values.Count);
        }

    }
}
