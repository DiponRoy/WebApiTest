using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Api.Db;

namespace Web.Api.Controllers
{
    public class ErrorsController : ApiController
    {
        [HttpGet]
        public IEnumerable<Admin> GetAll()
        {
            throw new NotImplementedException();
        }

        // GET api/Errors/5
        [HttpGet]
        public Admin GetById(int id)
        {
            return new Admin() { Id = id, Name = "Admin" };
        }

        // POST api/Errors
        [HttpPost]
        public Admin Add([FromBody]Admin entity)
        {
            throw new Exception("error to add admin.");
        }

        // PUT api/Errors/5
        [HttpPut]
        public Admin Replace(int id, [FromBody]Admin entity)
        {
            entity.Id = id;
            return entity;
        }

        // DELETE api/Errors/5
        [HttpDelete]
        public Admin Remove(int id)
        {
            return new Admin() { Id = id, Name = "Admin", IsActive = false };
        }

    }
}