using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using Db;
using Db.Models;

namespace Web.Api.Controllers
{
    [RoutePrefix("api")]    //mimic for IIS alias
    public class StudentsController : ApiController
    {
        public readonly IUmsDb Db;
        public StudentsController(IUmsDb db)
        {
            Db = db;
        }

        // GET api/student/all
        [HttpGet]
        [Route("student/all")]
        public IEnumerable<Student> GetAll()
        {
            return Db.Students.ToList();
        }

        // GET api/student/single/5
        [HttpGet]
        [Route("student/single/{id}")]
        public Student GetById(int id)
        {
            return Db.Students.Single(x => x.Id == id);
        }

        // POST api/student/create
        [HttpPost]
        [Route("student/create")]
        public Student Add([FromBody]Student entity)
        {
            entity.Id = Db.Students.Max(x => x.Id) + 1;
            entity.IsActive = true;
            Db.Students.Add(entity);

            return Db.Students.Single(x => x.Id == entity.Id);
        }

        // PUT api/student/update/5
        [HttpPut]
        [Route("student/update/{id}")]
        public Student Replace(int id, [FromBody]Student entity)
        {
            var student = Db.Students.Single(x => x.Id == id);
            student.Name = entity.Name;
            student.IsActive = entity.IsActive;
            return student;
        }

        // DELETE api/student/remove/5
        [HttpDelete]
        [Route("student/remove/{id}")]
        public Student Remove(int id)
        {
            var student = Db.Students.Single(x => x.Id == id);
            student.IsActive = false;
            return student;
        }
    }
}