﻿using System;
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
    public class HelloControllerUnitTest
    {
        protected const string BaseUrl = "http://localhost"; //http://localhost work's fine, else throwing error
        protected string FullUrlWithAlias
        {
            get { return BaseUrl + "/api/"; }
        }
        protected IDisposable Server { get; set; }
        protected Mock<IUmsDb> Db { get; set; }
        protected Admin LoginAdmin { get; set; }
        protected HttpClient HttpClient { get; set; }

        [SetUp]
        public void Setup()
        {
            LoginAdmin = new Admin { LoginName = "hoho", Password = "123", IsActive = true };
            Db = new Mock<IUmsDb>();
            HttpClient = new HttpClient();
            ApiStartup.Setup();
        }

        public void InitializeServer()
        {
            Db.Setup(x => x.Admins).Returns(new List<Admin>() { LoginAdmin });

            ApiStartup.MockIoc.UmsDbFunc = context => Db.Object;
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
            LoginAdmin = null;
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

        protected string GetToken()
        {
            //api call
            var admin = LoginAdmin;

            ApiResponseTmpl<IdentityToken> responseTmplObj;
            HttpRequestMessage request = CreateRequest("token/user", HttpMethod.Post,
                JsonConvert.SerializeObject(admin));
            using (HttpResponseMessage response = HttpClient.SendAsync(request).Result)
            {
                responseTmplObj =
                    JsonConvert.DeserializeObject<ApiResponseTmpl<IdentityToken>>(
                        response.Content.ReadAsStringAsync().Result);
            }

            return responseTmplObj.Data.AccessToken;
        }

        [Test]
        public void AnonymousGet()
        {
            InitializeServer();

            //api call
            ApiResponseTmpl<string> responseTmplObj;
            HttpRequestMessage request = CreateRequest("hello/anonymous", HttpMethod.Get);
            using (HttpResponseMessage response = HttpClient.SendAsync(request).Result)
            {
                responseTmplObj = JsonConvert.DeserializeObject<ApiResponseTmpl<string>>(response.Content.ReadAsStringAsync().Result);
            }

            //returned
            Assert.IsTrue(responseTmplObj.IsSuccess);
            Assert.AreEqual("Hello anonymous to get.", responseTmplObj.Data);
        }

        [Test]
        public void AuthorizeGet()
        {
            InitializeServer();  
          
            //token
            string token = GetToken();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //api call
            ApiResponseTmpl<string> responseTmplObj;
            HttpRequestMessage request = CreateRequest("hello/authorize", HttpMethod.Get);
            using (HttpResponseMessage response = HttpClient.SendAsync(request).Result)
            {
                responseTmplObj = JsonConvert.DeserializeObject<ApiResponseTmpl<string>>(response.Content.ReadAsStringAsync().Result);
            }

            //returned
            Assert.IsTrue(responseTmplObj.IsSuccess);
            Assert.AreEqual("Hello authorize to get.", responseTmplObj.Data);
        }

        [Test]
        public void AuthorizeGet_Works_With_NewServer()
        {
            InitializeServer();
            string token = GetToken();

            /*initiazlise new server*/
            TearDown();
            Setup();
            InitializeServer();

            //adding bearer token to request header
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //api call
            ApiResponseTmpl<string> responseTmplObj;
            HttpRequestMessage request = CreateRequest("hello/authorize", HttpMethod.Get);
            using (HttpResponseMessage response = HttpClient.SendAsync(request).Result)
            {
                responseTmplObj = JsonConvert.DeserializeObject<ApiResponseTmpl<string>>(response.Content.ReadAsStringAsync().Result);
            }

            //returned
            Assert.IsTrue(responseTmplObj.IsSuccess);
            Assert.AreEqual("Hello authorize to get.", responseTmplObj.Data);
        }

        [Test]
        public void AuthorizeGet_Needs_Token()
        {
            InitializeServer();

            //api call
            Exception error = null;
            HttpRequestMessage request = CreateRequest("hello/authorize", HttpMethod.Get);
            using (HttpResponseMessage response = HttpClient.SendAsync(request).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                   error = new Exception(response.Content.ReadAsStringAsync().Result);                   
                }
            }

            //returned
            Assert.IsNotNull(error);
        }

    }
}
