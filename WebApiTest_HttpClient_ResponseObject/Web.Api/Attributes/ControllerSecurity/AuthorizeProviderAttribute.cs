using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Web.Api.Attributes.ControllerSecurity
{
    public class AuthorizeProviderAttribute : AuthorizeAttribute
    {
        public const string HeaderName = "Authorization-Provider";
        public const string TokenPrefix = "Provider";

        public readonly string ProviderName;

        public AuthorizeProviderAttribute(string providerName)
        {
            if (String.IsNullOrEmpty(providerName))
            {
                throw new Exception("provider name is null.");
            }
            ProviderName = providerName;
        }

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
            if (!token.StartsWith(TokenPrefix) || !token.Contains(ProviderName)) 
            {
                throw Unauthorized("token for the provider is not valid");
            }
        }
    }
}