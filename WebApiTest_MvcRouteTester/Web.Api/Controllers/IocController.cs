using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Db;

namespace Web.Api.Controllers
{
    public class IocController : ApiController
    {
        public readonly ICmsDb Db;

        public IocController(ICmsDb db)
        {
            Db = db;
        }

        [HttpGet]
        public IEnumerable<User> All()
        {
            return Db.Users.ToList();
        } 
    }
}
