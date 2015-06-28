using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values/all
        [HttpGet]
        public IEnumerable<string> All()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/get
        [HttpGet]
        public string Get([FromUri]int id)
        {
            return "value";
        }

        // POST api/values/add
        [HttpPost]
        public void Add([FromBody]string value)
        {
        }

        // PUT api/values/replace
        [HttpPut]
        public void Replace([FromUri]int id, [FromBody]string value)
        {
        }

        // DELETE api/values/remove
        [HttpDelete]
        public void Remove([FromUri]int id)
        {
        }
    }
}
