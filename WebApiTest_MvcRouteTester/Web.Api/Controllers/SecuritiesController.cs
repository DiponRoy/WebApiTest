using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Api.Security;

namespace Web.Api.Controllers
{
    public class SecuritiesController : ApiController
    {
        // POST api/securities/add
        [HttpPost]
        [AuthorizeUser]
        public void Add([FromBody]string value)
        {
        }

        // PUT api/securities/replace
        [HttpPut]
        [AuthorizeUser]
        public void Replace([FromUri]int id, [FromBody]string value)
        {
        }
    }
}
