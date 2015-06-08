using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using Web.Api.Auth;
using Web.Api.Tests.Api_Start;

namespace Web.Api.Tests.UnitTest
{
    [TestFixture]
    public class OAuthProviderTest
    {
        protected const string BaseUrl = "http://localhost"; //http://localhost work's fine, else throwing error

        protected string FullUrlWithAlias
        {
            get { return BaseUrl + "/oauth/"; }
        }

        protected IDisposable Server { get; set; }

        protected HttpClient HttpClient { get; set; }

        [SetUp]
        public void Setup()
        {
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
            InitializeServer();

            //api call
            string contentBody = string.Format("grant_type={0}&username={1}&password={2}", "password", "Admin1", "123");
                //important

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
