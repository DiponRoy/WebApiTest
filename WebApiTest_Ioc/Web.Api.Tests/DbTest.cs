using System.Collections.Generic;
using System.Linq;
using Db;
using Db.Models;
using NUnit.Framework;

namespace Web.Api.Tests
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
        public void Students()
        {
            Assert.IsInstanceOf<List<Student>>(Db.Students);
            Assert.AreEqual(2, Db.Students.Count);
        }

        [Test]
        public void Students_Data_Last()
        {
            Assert.AreEqual(2, Db.Students.Last().Id);
            Assert.AreEqual("Student2", Db.Students.Last().Name);
            Assert.IsTrue(Db.Students.Last().IsActive);
        }

        [Test]
        public void Stuents_Data_First()
        {
            Assert.AreEqual(1, Db.Students.First().Id);
            Assert.AreEqual("Student1", Db.Students.First().Name);
            Assert.IsTrue(Db.Students.First().IsActive);
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
            Assert.AreEqual("Admin1", Db.Admins.First().Name);
            Assert.IsTrue(Db.Admins.First().IsActive);
        }

        [TearDown]
        public void Tea()
        {
            Db = null;
        }
    }
}
