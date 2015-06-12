using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Db;
using Db.Model;
using Microsoft.Owin.Hosting;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Web.Api.Auth;
using Web.Api.Tests.Models;

namespace Web.Api.Tests.UnitTest
{
    [TestFixture]
    public class TokenControllerUnitTest
    {
        protected const string BaseUrl = "http://localhost"; //http://localhost work's fine, else throwing error

        protected string FullUrlWithAlias
        {
            get { return BaseUrl + "/api/"; }
        }

        protected Mock<IUmsDb> Db { get; set; } 
        protected IDisposable Server { get; set; }

        protected HttpClient HttpClient { get; set; }

        [SetUp]
        public void Setup()
        {
            Db = new Mock<IUmsDb>();
            HttpClient = new HttpClient();
            ApiStartup.Setup();
        }

        public void InitializeServer()
        {
            ApiStartup.Ioc.UmsDbProvider = context => Db.Object;
            Server = WebApp.Start<ApiStartup>(BaseUrl);
        }


        [TearDown]
        public void TearDown()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
            Server = null;
            HttpClient = null;
            Db = null;
            ApiStartup.Dispose();
        }

        private HttpRequestMessage CreateRequest(string url, HttpMethod method)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(FullUrlWithAlias + url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = method;

            return request;
        }

        private HttpRequestMessage CreateRequest(string url, HttpMethod method, string jsonString)
        {
            HttpRequestMessage request = CreateRequest(url, method);
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            return request;
        }

        [Test]
        public void UserToken()
        {
            var admin = new Admin { LoginName = "Admin1", Password = "123", IsActive = true};
            Db.Setup(x => x.Admins).Returns(new List<Admin>() {admin});
            InitializeServer();

            //api call
            ApiResponseTmpl<IdentityToken> responseTmplObj;
            HttpRequestMessage request = CreateRequest("token/user", HttpMethod.Post,
                JsonConvert.SerializeObject(admin));
            using (HttpResponseMessage response = HttpClient.SendAsync(request).Result)
            {
                responseTmplObj =
                    JsonConvert.DeserializeObject<ApiResponseTmpl<IdentityToken>>(
                        response.Content.ReadAsStringAsync().Result);
            }

            //returned
            Assert.IsTrue(responseTmplObj.IsSuccess);
            Assert.IsNotNullOrEmpty(responseTmplObj.Data.AccessToken);
        }

    }
}
