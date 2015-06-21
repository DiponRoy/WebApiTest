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
    public class AdminsControllerTest
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

        #region Json Serialize and Deserialize

        [Test]
        public void GetAll_With_Object()
        {
            var values = Client.Get<List<Admin>>("api/admins");
            Assert.AreEqual(2, values.Count);
        }

        [Test]
        public void GetById_With_Object()
        {
            const int adminId = 5;
            var entity = Client.Get<Admin>(String.Format("api/admins/{0}", adminId));
            Assert.AreEqual(adminId, entity.Id); //passed by url
            Assert.AreEqual("Admin", entity.Name);
        }

        [Test]
        public void Add_With_Object()
        {
            var entityToAdd = new Admin {Name = "Dipon"};
            Admin addedEntity = Client.Post<Admin, Admin>("api/admins", entityToAdd);

            Assert.Less(0, addedEntity.Id);
            Assert.AreEqual(entityToAdd.Name, addedEntity.Name); //passed by json
            Assert.IsTrue(addedEntity.IsActive);
        }

        [Test]
        public void Replace_With_Object()
        {
            const int adminId = 5;
            var entity = new Admin {Name = "Dipon", IsActive = false};
            Admin updatedEntity = Client.Put<Admin, Admin>(String.Format("api/admins/{0}", adminId), entity);

            Assert.AreEqual(adminId, updatedEntity.Id); //passed by url
            Assert.AreEqual(entity.Name, updatedEntity.Name); //passed by json
            Assert.AreEqual(entity.IsActive, updatedEntity.IsActive); //passed by json
        }

        [Test]
        public void Remove_With_Object()
        {
            const int adminId = 5;
            var removedEntity = Client.Delete<Admin>(String.Format("api/admins/{0}", adminId));

            Assert.AreEqual(adminId, removedEntity.Id); //passed by url
            Assert.IsFalse(removedEntity.IsActive);
        }

        #endregion
    }
}
