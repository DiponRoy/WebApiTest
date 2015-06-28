using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api.Controllers
{
    [RoutePrefix("api")]
    public class RoutesController : ApiController
    {
        // GET api/route/all
        [HttpGet]
        [Route("settings/routes/all")]
        public IEnumerable<string> GetAll()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/route/get
        [HttpGet]
        [Route("settings/routes/Get")]
        public string GetSingle([FromUri]int id)
        {
            return "value";
        }
    }
}
