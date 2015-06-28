using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api.Controllers
{
    public class ErrorsController : ApiController
    {
        // GET api/errors/all
        [HttpGet]
        public IEnumerable<string> All()
        {
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "Error."));
        }

        // GET api/errors/get
        [HttpGet]
        public string Get([FromUri]int id)
        {
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, "Error."));
        }
    }
}
