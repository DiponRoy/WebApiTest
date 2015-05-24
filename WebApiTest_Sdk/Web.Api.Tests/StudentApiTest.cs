using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Db;
using Db.Model;
using FizzWare.NBuilder;
using Moq;
using Newtonsoft.Json;
using Ninject;
using NUnit.Framework;

namespace Web.Api.Sdk.Tests
{
    [TestFixture]
    public class StudentApiTest
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

        public void InitializeServer()
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
            request.RequestUri = new Uri("http://test/api/" + url);
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
            //mock
            int studentCount = 10;
            Db.Setup(x => x.Students).Returns(Builder<Student>.CreateListOfSize(studentCount).Build());
            InitializeServer();

            //api call
            ApiResponseTmpl<IEnumerable<Student>> responseTmplObj;
            var client = new HttpClient(Server);
            HttpRequestMessage request = CreateRequest("student/all", "application/json", HttpMethod.Get);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                responseTmplObj = JsonConvert.DeserializeObject<ApiResponseTmpl<IEnumerable<Student>>>(response.Content.ReadAsStringAsync().Result);
            }

            //returned
            IEnumerable<Student> entityList = responseTmplObj.Data;
            Assert.AreEqual(studentCount, entityList.Count());
        }

        [Test]
        public void GetById()
        {
            //mock
            Student student = Builder<Student>.CreateNew().With(x => x.Id = 1).Build();
            Db.Setup(x => x.Students).Returns(new List<Student>(){student});
            InitializeServer();

            //api call
            ApiResponseTmpl<Student> responseTmplObj;
            var client = new HttpClient(Server);
            HttpRequestMessage request = CreateRequest(String.Format("student/single/{0}", student.Id), "application/json", HttpMethod.Get);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                responseTmplObj = JsonConvert.DeserializeObject<ApiResponseTmpl<Student>>(response.Content.ReadAsStringAsync().Result);
            }

            //returned
            Student entity = responseTmplObj.Data;
            Assert.AreEqual(student.Id, entity.Id); //passed by url
            Assert.AreEqual(student.Name, entity.Name);
            Assert.AreEqual(student.IsActive, entity.IsActive);
        }


        [Test]
        public void Add()
        {
            //mock
            int studentCount = 10;
            IList<Student> students = Builder<Student>.CreateListOfSize(studentCount).Build();
            Db.Setup(x => x.Students).Returns(students);
            InitializeServer();

            //api call
            var entityToAdd = new Student {Name = "Dipon"};
            ApiResponseTmpl<Student> responseTmplObj;

            var client = new HttpClient(Server);
            HttpRequestMessage request = CreateRequest("student/create", "application/json", HttpMethod.Post, JsonConvert.SerializeObject(entityToAdd)); //important; new{Entity: entityToAdd} wasn't working as expected
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                responseTmplObj = JsonConvert.DeserializeObject<ApiResponseTmpl<Student>>(response.Content.ReadAsStringAsync().Result);
            }

            //at db
            Assert.AreEqual(11, students.Count);
            Assert.AreEqual(entityToAdd.Name, students.Last().Name);
            Assert.IsTrue(students.Last().IsActive);

            //returned
            Student addedEntity = responseTmplObj.Data;
            Assert.AreEqual(11, addedEntity.Id);
            Assert.AreEqual(entityToAdd.Name, addedEntity.Name); //passed by json
            Assert.IsTrue(addedEntity.IsActive);
        }

        [Test]
        public void Replace()
        {
            //mock
            const int studentId = 1;
            IList<Student> students = Builder<Student>.CreateListOfSize(2).All().With(x => x.IsActive = true).Build();
            Db.Setup(x => x.Students).Returns(students);
            InitializeServer();

            //api call
            var entity = new Student {Name = "Dipon", IsActive = false};
            ApiResponseTmpl<Student> responseTmplObj;

            var client = new HttpClient(Server);
            HttpRequestMessage request = CreateRequest(String.Format("student/update/{0}", studentId), "application/json", HttpMethod.Put, JsonConvert.SerializeObject(entity)); //important; new{Entity: entityToAdd} wasn't working as expected
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                responseTmplObj = JsonConvert.DeserializeObject<ApiResponseTmpl<Student>>(response.Content.ReadAsStringAsync().Result);
            }

            //at db
            Assert.AreEqual(entity.Name, students.First().Name);
            Assert.AreEqual(entity.IsActive, students.First().IsActive);

            //returned
            Student updatedEntity = responseTmplObj.Data;
            Assert.AreEqual(studentId, updatedEntity.Id); //passed by url
            Assert.AreEqual(entity.Name, updatedEntity.Name); //passed by json
            Assert.AreEqual(entity.IsActive, updatedEntity.IsActive); //passed by json
        }

        [Test]
        public void Remove()
        {
            //mock
            const int studentId = 2;
            IList<Student> students = Builder<Student>.CreateListOfSize(2).All().With(x => x.IsActive = true).Build();
            Db.Setup(x => x.Students).Returns(students);
            InitializeServer();

            //api call
            ApiResponseTmpl<Student> responseTmplObj;

            var client = new HttpClient(Server);
            HttpRequestMessage request = CreateRequest(String.Format("student/remove/{0}", studentId),
                "application/json",
                HttpMethod.Delete);
            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                responseTmplObj = JsonConvert.DeserializeObject<ApiResponseTmpl<Student>>(response.Content.ReadAsStringAsync().Result);
            }

            //at db
            Assert.AreEqual(2, students.Count);
            Assert.IsFalse(students.Last().IsActive);

            //returend
            Student removedEntity = responseTmplObj.Data;
            Assert.AreEqual(studentId, removedEntity.Id); //passed by url
            Assert.IsFalse(removedEntity.IsActive);
        }
    }
}
