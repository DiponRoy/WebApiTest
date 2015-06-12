using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Db.Model;
using Microsoft.Owin.Hosting;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Web.Api.Auth;

namespace Web.Api.Tests.UnitTest
{
    [TestFixture]
    public class OAuthProviderUnitTest
    {
        protected const string BaseUrl = "http://localhost"; //http://localhost work's fine, else throwing error

        protected string FullUrlWithAlias
        {
            get { return BaseUrl + "/oauth/"; }
        }

        protected Mock<IAuthContext> Db { get; set; } 
        protected IDisposable Server { get; set; }

        protected HttpClient HttpClient { get; set; }

        [SetUp]
        public void Setup()
        {
            Db = new Mock<IAuthContext>();
            HttpClient = new HttpClient();
            ApiStartup.Setup();
        }

        public void InitializeServer()
        {
            ApiStartup.Ioc.AuthContextProvider = context => Db.Object;
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
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/x-www-form-urlencoded");
                //important

            return request;
        }

        [Test]
        public void UserToken()
        {
            var admin = new Admin { LoginName = "Admin1", Password = "123", IsActive = true };
            Db.Setup(x => x.Logins).Returns(new List<Admin>() { admin });
            InitializeServer();

            //api call
            string contentBody = string.Format("grant_type={0}&username={1}&password={2}", "password", admin.LoginName, admin.Password); //important

            IdentityToken responseTmplObj;
            HttpRequestMessage request = CreateRequest("token/user", HttpMethod.Post, contentBody);
            using (HttpResponseMessage response = HttpClient.SendAsync(request).Result)
            {
                responseTmplObj =
                    JsonConvert.DeserializeObject<IdentityToken>(
                        response.Content.ReadAsStringAsync().Result);
            }

            //returned
            Assert.IsNotNullOrEmpty(responseTmplObj.AccessToken);
        }
    }
}
