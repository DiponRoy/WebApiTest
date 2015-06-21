using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Http;
using Web.Api.Attributes.ControllerSecurity;
using Web.Api.Db;

namespace Web.Api.Controllers
{
    [AuthorizeUser]
    public class HeadersController : ApiController
    {
        // GET api/headers
        [HttpGet]
        public string Get()
        {
            return "ok";
        }

        // POST api/headers
        [HttpPost]
        [AuthorizeProvider("CodeMen")]
        public string Add([FromBody] Admin entity)
        {
            return "ok";
        }
    }
}