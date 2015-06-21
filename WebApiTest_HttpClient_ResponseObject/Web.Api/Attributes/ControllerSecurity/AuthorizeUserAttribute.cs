using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Web.Api.Attributes.ControllerSecurity
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public const string HeaderName = "Authorization";
        public const string TokenPrefix = "Bearer";


        private static HttpResponseException BadRequest(string message)
        {
            var exception = new HttpResponseException(HttpStatusCode.BadRequest);
            exception.Response.ReasonPhrase = message;
            return exception;
        }

        private static HttpResponseException Unauthorized(string message)
        {
            var exception = new HttpResponseException(HttpStatusCode.Unauthorized);
            exception.Response.ReasonPhrase = message;
            return exception;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var reader = new HttpHeaderReader(actionContext);

            if (!reader.Has(HeaderName))
            {
                throw BadRequest(String.Format("{0} header not found on request.", HeaderName));
            }

            string headerValue = reader.Value(HeaderName);
            if (String.IsNullOrEmpty(headerValue))
            {
                throw BadRequest(String.Format("no value is been set for header {0}.", HeaderName));
            }

            ValidateToken(headerValue);
            return true;    //if not valid access, throw exception
        }

        private void ValidateToken(string token)
        {
            //check if request can access resource or not
            if (!token.StartsWith(TokenPrefix)) 
            {
                throw Unauthorized("token for the user is not valid");
            }
        }
    }
}