using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using Db;
using Db.Model;
using Web.Api.Model;

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
        public ApiResponse<IEnumerable<Student>> GetAll()
        {
            return new ApiResponse<IEnumerable<Student>>(Db.Students.ToList());
        }

        // GET api/student/single/5
        [HttpGet]
        [Route("student/single/{id}")]
        public ApiResponse<Student> GetById(int id)
        {
            return new ApiResponse<Student>(Db.Students.Single(x => x.Id == id));
        }

        // POST api/student/create
        [HttpPost]
        [Route("student/create")]
        public ApiResponse<Student> Add([FromBody]Student entity)
        {
            entity.Id = Db.Students.Max(x => x.Id) + 1;
            entity.IsActive = true;
            Db.Students.Add(entity);

            return new ApiResponse<Student>(Db.Students.Single(x => x.Id == entity.Id));
        }

        // PUT api/student/update/5
        [HttpPut]
        [Route("student/update/{id}")]
        public ApiResponse<Student> Replace(int id, [FromBody]Student entity)
        {
            var student = Db.Students.Single(x => x.Id == id);
            student.Name = entity.Name;
            student.IsActive = entity.IsActive;

            return new ApiResponse<Student>(student);
        }

        // DELETE api/student/remove/5
        [HttpDelete]
        [Route("student/remove/{id}")]
        public ApiResponse<Student> Remove(int id)
        {
            var student = Db.Students.Single(x => x.Id == id);
            student.IsActive = false;

            return new ApiResponse<Student>(student);
        }
    }
}