using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Api.Db;

namespace Web.Api.Controllers
{
    public class AdminsController : ApiController
    {

        //// GET api/admins/exception
        //[HttpGet]
        //public string Exception(int id)
        //{
        //    if (id > 0)
        //    {
        //        throw new Exception("Unauthorized Access.");               
        //    }
        //    return "Ok";
        //}

        // GET api/admins
        [HttpGet]
        public IEnumerable<Admin> GetAll()
        {
            return new Admin[] { new Admin() {Id = 1, Name = "Admin1"}, new Admin(){Id = 2, Name = "Admin2"} };
        }

        // GET api/admins/5
        [HttpGet]
        public Admin GetById(int id)
        {
            return new Admin() { Id = id, Name = "Admin" };
        }

        // POST api/admins
        [HttpPost]
        public Admin Add([FromBody]Admin entity)
        {
            entity.Id = 10;
            entity.IsActive = true;
            return entity;
        }

        // PUT api/admins/5
        [HttpPut]
        public Admin Replace(int id, [FromBody]Admin entity)
        {
            entity.Id = id;
            return entity;
        }

        // DELETE api/admins/5
        [HttpDelete]
        public Admin Remove(int id)
        {
            return new Admin(){Id = id, Name = "Admin", IsActive = false};
        }

    }
}