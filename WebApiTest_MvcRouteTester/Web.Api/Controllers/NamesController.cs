using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api.Controllers
{
    public class NamesController : ApiController
    {
        // GET api/names/getall
        [HttpGet]
        [ActionName("All")]
        public IEnumerable<string> GetAll()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/names/get
        [HttpGet]
        [ActionName("Get")]
        public string GetSingle([FromUri]int id)
        {
            return "value";
        }
    }
}
