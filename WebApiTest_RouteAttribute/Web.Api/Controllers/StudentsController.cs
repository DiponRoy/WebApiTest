using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using Web.Api.Models;

namespace Web.Api.Controllers
{
    [RoutePrefix("api")]    //mimic for IIS alias
    public class StudentsController : ApiController
    {
        // GET api/student/all
        [HttpGet]
        [Route("student/all")]
        public IEnumerable<Student> GetAll()
        {
            return new Student[] { new Student() {Id = 1, Name = "Student1"}, new Student(){Id = 2, Name = "Student2"} };
        }

        // GET api/student/single/5
        [HttpGet]
        [Route("student/single/{id}")]
        public Student GetById(int id)
        {
            return new Student() { Id = id, Name = "Student" };
        }

        // POST api/student/create
        [HttpPost]
        [Route("student/create")]
        public Student Add([FromBody]Student entity)
        {
            entity.Id = 10;
            entity.IsActive = true;
            return entity;
        }

        // PUT api/student/update/5
        [HttpPut]
        [Route("student/update/{id}")]
        public Student Replace(int id, [FromBody]Student entity)
        {
            entity.Id = id;
            return entity;
        }

        // DELETE api/student/remove/5
        [HttpDelete]
        [Route("student/remove/{id}")]
        public Student Remove(int id)
        {
            return new Student(){Id = id, Name = "Student", IsActive = false};
        }
    }
}