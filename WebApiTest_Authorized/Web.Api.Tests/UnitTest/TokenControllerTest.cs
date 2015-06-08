using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Db.Model;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using Web.Api.Auth;
using Web.Api.Tests.Api_Start;

namespace Web.Api.Tests.UnitTest
{
    [TestFixture]
    public class TokenControllerTest
    {
        protected const string BaseUrl = "http://localhost"; //http://localhost work's fine, else throwing error

        protected string FullUrlWithAlias
        {
            get { return BaseUrl + "/api/"; }
        }

        protected string Token { get; set; }
        protected IDisposable Server { get; set; }

        protected HttpClient HttpClient { get; set; }

        [SetUp]
        public void Setup()
        {
            Token = string.Empty;
            HttpClient = new HttpClient();
        }

        public void InitializeServer()
        {
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
            Token = null;
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
            InitializeServer();

            //api call
            var admin = new Admin { LoginName = "Admin1", Password = "123" };

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
