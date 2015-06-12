using System.Collections.Generic;
using System.Linq;
using Db;
using Db.Model;
using NUnit.Framework;

namespace Web.Api.Tests.IntegratedTest
{
    [TestFixture]
    public class DbTest
    {
        protected IUmsDb Db { get; set; }

        [SetUp]
        public void Setup()
        {
            Db = new UmsDb();
        }

        [Test]
        public void Addmins()
        {
            Assert.IsInstanceOf<List<Admin>>(Db.Admins);
            Assert.AreEqual(1, Db.Admins.Count);
        }

        [Test]
        public void Addmins_Data_Fast()
        {
            Assert.AreEqual(1, Db.Admins.First().Id);
            Assert.AreEqual("Admin1", Db.Admins.First().LoginName);
            Assert.AreEqual("123", Db.Admins.First().Password);
            Assert.IsTrue(Db.Admins.First().IsActive);
        }

        [TearDown]
        public void TearDown()
        {
            Db = null;
        }
    }
}
