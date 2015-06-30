using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Db.Model;
using Web.Api.Auth;
using Web.Api.Model;
using Web.Api.Model.Response;

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

        [AllowAnonymous]
        [HttpGet]
        [Route("hello/anonymous/http/error")]
        public IHttpActionResult AnonymousGetHttpError()
        {
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "Error."));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hello/anonymous/http/success")]
        public IHttpActionResult AnonymousGetHttpSuccess()
        {
            return Ok("Success.");
        }

        [Authorize]
        [HttpGet]
        [Route("hello/authorize")]
        public ApiResponse<string> AuthorizeGet()
        {
            return new ApiResponse<string>("Hello authorize to get.");
        }

        [Authorize]
        [HttpGet]
        [Route("hello/identity")]
        public ApiResponse<Admin> GetIdentity()
        {
            Admin loggedInAdmin = AuthUtility.GetAdmin((ClaimsIdentity) User.Identity);
            return new ApiResponse<Admin>(loggedInAdmin);
        } 
    }
}
