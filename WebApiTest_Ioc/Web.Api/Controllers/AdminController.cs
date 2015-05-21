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
    public class AdminController : ApiController
    {
        public readonly IUmsDb Db;
        public AdminController()
        {
            Db = new UmsDb();
        }
        public AdminController(IUmsDb db)
        {
            Db = db;
        }

        // GET api/student/all
        [HttpGet]
        [Route("admin/all")]
        public IEnumerable<Student> GetAll()
        {
            return Db.Students.ToList();
        }

    }
}