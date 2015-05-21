using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Db.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Web.Api.Tests
{
    [TestFixture]
    public class NinjectIocTest
    {
        private HttpServer _server;
        private string _url = "http://test/";

        [SetUp]
        public void Setup()
        {
            var config = new HttpConfiguration();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            IocConfig.Register(config);
            WebApiConfig.Register(config);
            config.EnsureInitialized();

            _server = new HttpServer(config);
        }

        [TearDown]
        public void TearDown()
        {
            if (_server != null)
            {
                _server.Dispose();
            }
        }

        private HttpRequestMessage CreateRequest(string url, string mthv, HttpMethod method)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(_url + url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));
            request.Method = method;

            return request;
        }

        private HttpRequestMessage CreateRequest(string url, string mthv, HttpMethod method, string jsonString)
        {
            HttpRequestMessage request = CreateRequest(url, mthv, method);
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            return request;
        }

        [Test]
        public void GetAll()
        {
            List<Student> values;

            var client = new HttpClient(_server);
            HttpRequestMessage request = CreateRequest("api/student/all", "application/json", HttpMethod.Get);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                values = JsonConvert.DeserializeObject<List<Student>>(response.Content.ReadAsStringAsync().Result);
            }

            Assert.AreEqual(2, values.Count);
        }

        [Test]
        public void GetById()
        {
            const int studentId = 1;
            Student entity;

            var client = new HttpClient(_server);
            HttpRequestMessage request = CreateRequest(String.Format("api/student/single/{0}", studentId),
                "application/json",
                HttpMethod.Get);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                entity = JsonConvert.DeserializeObject<Student>(response.Content.ReadAsStringAsync().Result);
            }

            Assert.AreEqual(studentId, entity.Id); //passed by url
            Assert.IsNotEmpty(entity.Name);
            Assert.IsTrue(entity.IsActive);
        }


        [Test]
        public void Add()
        {
            var entityToAdd = new Student {Name = "Dipon"};
            Student addedEntity = null;

            var client = new HttpClient(_server);
            HttpRequestMessage request = CreateRequest("api/student/create", "application/json", HttpMethod.Post,
                JsonConvert.SerializeObject(entityToAdd));
            //important; new{Entity: entityToAdd} wasn't working as expected
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                addedEntity = JsonConvert.DeserializeObject<Student>(response.Content.ReadAsStringAsync().Result);
            }

            Assert.Less(0, addedEntity.Id);
            Assert.AreEqual(entityToAdd.Name, addedEntity.Name); //passed by json
            Assert.IsTrue(addedEntity.IsActive);
        }

        [Test]
        public void Replace()
        {
            const int studentId = 1;
            var entity = new Student {Name = "Dipon", IsActive = false};
            Student updatedEntity = null;

            var client = new HttpClient(_server);
            HttpRequestMessage request = CreateRequest(String.Format("api/student/update/{0}", studentId),
                "application/json",
                HttpMethod.Put, JsonConvert.SerializeObject(entity));
            //important; new{Entity: entityToAdd} wasn't working as expected
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                updatedEntity = JsonConvert.DeserializeObject<Student>(response.Content.ReadAsStringAsync().Result);
            }

            Assert.AreEqual(studentId, updatedEntity.Id); //passed by url
            Assert.AreEqual(entity.Name, updatedEntity.Name); //passed by json
            Assert.AreEqual(entity.IsActive, updatedEntity.IsActive); //passed by json
        }

        [Test]
        public void Remove()
        {
            const int studentId = 2;
            Student removedEntity = null;

            var client = new HttpClient(_server);
            HttpRequestMessage request = CreateRequest(String.Format("api/student/remove/{0}", studentId),
                "application/json",
                HttpMethod.Delete);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                removedEntity = JsonConvert.DeserializeObject<Student>(response.Content.ReadAsStringAsync().Result);
            }

            Assert.AreEqual(studentId, removedEntity.Id); //passed by url
            Assert.IsFalse(removedEntity.IsActive);
        }
    }
}
