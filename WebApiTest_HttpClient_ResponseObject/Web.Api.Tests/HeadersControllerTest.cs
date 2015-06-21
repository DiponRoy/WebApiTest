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
    public class HeadersControllerTest
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
            Client.Headers.Clear();
            Client.Headers.Add("Authorization", "Bearer theAccessTokenHere");
        }

        [TearDown]
        public void TearDown()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
            Server = null;
            Client = null;
        }

        [Test]
        public void Get()
        {
            Assert.AreEqual("ok", Client.Get<string>("api/headers"));
        }

        [Test]
        public void Get_Needs_Authorization_Header()
        {
            Client.Headers.Clear();
            var error = Assert.Catch<HttpException>(() => Client.Get<string>("api/headers"));
            Assert.AreEqual(400, error.GetHttpCode()); //bad request, header not found
        }

        [Test]
        public void Get_Authorization_TokenWith_Bearer()
        {
            Client.Headers.Clear();
            Client.Headers.Add("Authorization", "theAccessTokenHere");   //contains no word "Bearer"

            var error = Assert.Catch<HttpException>(() => Client.Get<string>("api/headers"));
            Assert.AreEqual(401, error.GetHttpCode()); //unauthorized
        }

        [Test]
        public void Add()
        {
            Client.Headers.Add("Authorization-Provider", "Provider CodeMen theAccessTokenHere");
            Assert.AreEqual("ok", Client.Post<string, Admin>("api/headers", new Admin()));
        }

        [Test]
        public void Add_Needs_AuthorizationProvider_Header()
        {
            var error = Assert.Catch<HttpException>(() => Client.Post<string, Admin>("api/headers", new Admin()));
            Assert.AreEqual(400, error.GetHttpCode()); //bad request, header not found
        }

        [Test]
        public void Add_AuthorizationProvider_TokenWith_Provider()
        {
            Client.Headers.Add("Authorization-Provider", "theAccessTokenHere");   //contains no word "Provider"

            var error = Assert.Catch<HttpException>(() => Client.Post<string, Admin>("api/headers", new Admin()));
            Assert.AreEqual(401, error.GetHttpCode()); //unauthorized
        }

        [Test]
        public void Add_AuthorizationProvider_TokenWith_Provider_Name()
        {
            Client.Headers.Add("Authorization-Provider", "theAccessTokenHere");   //contains no word "CodeMen"

            var error = Assert.Catch<HttpException>(() => Client.Post<string, Admin>("api/headers", new Admin()));
            Assert.AreEqual(401, error.GetHttpCode()); //unauthorized
        }

    }
}
