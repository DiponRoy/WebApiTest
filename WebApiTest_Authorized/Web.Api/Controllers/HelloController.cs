using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using Web.Api.Model;

namespace Web.Api.Controllers
{
    [RoutePrefix("api")]
    public class HelloController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("hello/anonymous")]
        public ApiResponse<string> AnonymousGet()
        {
            return new ApiResponse<string>("Hello anonymous to get.");
        }

        [Authorize]
        [HttpGet]
        [Route("hello/authorize")]
        public ApiResponse<string> AuthorizeGet()
        {
            return new ApiResponse<string>("Hello authorize to get.");
        } 
    }
}
