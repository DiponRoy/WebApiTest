using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Api.Models;

namespace Web.Api.Controllers
{
    public class StudentsController : ApiController
    {
        // GET api/students
        [HttpGet]
        public IEnumerable<Student> GetAll()
        {
            return new Student[] { new Student() {Id = 1, Name = "Student1"}, new Student(){Id = 2, Name = "Student2"} };
        }

        // GET api/students/5
        [HttpGet]
        public Student GetById(int id)
        {
            return new Student() { Id = id, Name = "Student" };
        }

        // POST api/students
        [HttpPost]
        public Student Add([FromBody]Student entity)
        {
            entity.Id = 10;
            entity.IsActive = true;
            return entity;
        }

        // PUT api/students/5
        [HttpPut]
        public Student Replace(int id, [FromBody]Student entity)
        {
            entity.Id = id;
            return entity;
        }

        // DELETE api/students/5
        [HttpDelete]
        public Student Remove(int id)
        {
            return new Student(){Id = id, Name = "Student", IsActive = false};
        }
    }
}