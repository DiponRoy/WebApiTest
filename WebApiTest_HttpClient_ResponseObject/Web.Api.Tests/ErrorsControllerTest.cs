using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using NUnit.Framework;
using Web.Api.Db;
using Web.Api.Tests.WebRequest;

namespace Web.Api.Tests
{
    [TestFixture]
    public class ErrorsControllerTest
    {
        protected HttpServer Server { get; set; }
        protected ClientHttp Client { get; set; }

        [SetUp]
        public void Setup()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            Server = new HttpServer(config);

            const string url = "http://test/"; /*http://test or http://test/ both works fine*/
            Client = new ClientHttp(url, Server);
        }

        [TearDown]
        public void TearDown()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
            if (Client != null && !Client.IsDisposed)
            {
                Client.Dispose();
            }

            Server = null;
            Client = null;
        }

        [Test]
        public void GetAll_Throws_Exception()
        {
            var httpError = Assert.Catch<HttpException>(() => Client.Get<List<Admin>>("api/errors"));
        }

        [Test]
        public void Add_Throws_Exception()
        {
            var httpError = Assert.Catch<HttpException>(() => Client.Post<Admin, Admin>("api/errors", new Admin()));
            Assert.AreEqual(500, httpError.GetHttpCode());
        }
    }
}
