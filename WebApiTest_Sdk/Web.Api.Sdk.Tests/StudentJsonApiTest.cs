using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Db;
using Db.Model;
using FizzWare.NBuilder;
using Microsoft.Owin.Hosting;
using Moq;
using Newtonsoft.Json.Linq;
using Ninject;
using NUnit.Framework;
using Owin;


namespace Web.Api.Sdk.Tests
{
    [TestFixture]
    public class StudentJsonApiTest
    {
        protected const string BaseUrl = "http://localhost"; //http://localhost work's fine, else throwing error

        protected string FullUrlWithAlias
        {
            get { return BaseUrl + "/api/"; }
        }

        protected Mock<IUmsDb> Db { get; set; }
        protected HttpConfiguration Config { get; set; }
        protected IDisposable Server { get; set; }
        protected StudentJsonApi JsonApi { get; set; }


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

        [TearDown]
        public void TearDown()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
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
            if (String.IsNullOrEmpty(BaseUrl))
            {
                throw new NullReferenceException("BaseUrl is null");
            }

            // Ninject IoC for mocked db
            var kernel = new StandardKernel();
            kernel.Bind<IUmsDb>().ToMethod(x => Db.Object);
            Config.DependencyResolver = new NinjectDependencyResolver(kernel);

            //set configuration
            ApiStartup.SetConfig(Config);
            Server = WebApp.Start<ApiStartup>(BaseUrl);

            //api sdk
            JsonApi = new StudentJsonApi(FullUrlWithAlias);
        }

        [Test]
        public void GetAll()
        {
            //mock
            const int studentCount = 10;
            Db.Setup(x => x.Students).Returns(Builder<Student>.CreateListOfSize(studentCount).Build());
            InitializeServer();

            //api
            JObject response = JsonApi.GetAll();
            IEnumerable<Student> list = response.ToObject<ApiResponseTmpl<IEnumerable<Student>>>().Data;

            Assert.AreEqual(studentCount, list.Count());
        }

        [Test]
        public void GetById()
        {
            //mock
            Student student = Builder<Student>.CreateNew().With(x => x.Id = 1).Build();
            Db.Setup(x => x.Students).Returns(new List<Student> {student});
            InitializeServer();

            //api
            JObject response = JsonApi.GetById(student.Id);
            Student entity = response.ToObject<ApiResponseTmpl<Student>>().Data;

            Assert.AreEqual(student.Id, entity.Id);
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

            //api
            var entityToAdd = new Student {Name = "Dipon"};
            JObject response = JsonApi.Add(entityToAdd);
            Student addedEntity = response.ToObject<ApiResponseTmpl<Student>>().Data;

            //at db
            Assert.AreEqual(11, students.Count);
            Assert.AreEqual(entityToAdd.Name, students.Last().Name);
            Assert.IsTrue(students.Last().IsActive);

            //returned
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

            //api
            var entity = new Student {Name = "Dipon", IsActive = false};
            JObject response = JsonApi.Replace(studentId, entity);
            Student updatedEntity = response.ToObject<ApiResponseTmpl<Student>>().Data;

            //at db
            Assert.AreEqual(entity.Name, students.First().Name);
            Assert.AreEqual(entity.IsActive, students.First().IsActive);

            //returned
            Assert.AreEqual(studentId, updatedEntity.Id);
            Assert.AreEqual(entity.Name, updatedEntity.Name);
            Assert.AreEqual(entity.IsActive, updatedEntity.IsActive);
        }

        [Test]
        public void Remove()
        {
            //mock
            const int studentId = 2;
            IList<Student> students = Builder<Student>.CreateListOfSize(2).All().With(x => x.IsActive = true).Build();
            Db.Setup(x => x.Students).Returns(students);
            InitializeServer();

            //api
            JObject response = JsonApi.Remove(studentId);
            Student removedEntity = response.ToObject<ApiResponseTmpl<Student>>().Data;

            //at db
            Assert.AreEqual(2, students.Count);
            Assert.IsFalse(students.Last().IsActive);

            //returend
            Assert.AreEqual(studentId, removedEntity.Id);
            Assert.IsFalse(removedEntity.IsActive);
        }

        /* web application startup*/

        public class ApiStartup
        {
            public static HttpConfiguration Config { get; private set; }

            public static void SetConfig(HttpConfiguration config)
            {
                Config = config;
            }

            public void Configuration(IAppBuilder appBuilder)
            {
                if (Config == null)
                {
                    throw new NullReferenceException("Config shouldn't be null at ApiStartup.");
                }
                appBuilder.UseWebApi(Config);
            }
        }
    }
}
