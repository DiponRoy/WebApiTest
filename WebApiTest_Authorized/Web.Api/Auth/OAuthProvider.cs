using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Db.Model;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Web.Api.Ioc;

namespace Web.Api.Auth
{
    public class OAuthProvider : OAuthAuthorizationServerProvider
    {
        public readonly string AuthenticationType;

        public OAuthProvider(string authenticationType)
        {
            if (string.IsNullOrEmpty(authenticationType))
            {
                throw new ArgumentNullException("authenticationType");
            }
            AuthenticationType = authenticationType;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.OwinContext.Request.Method == "OPTIONS" && context.IsTokenEndpoint)
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "POST" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "accept", "authorization", "content-type" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                context.OwinContext.Response.StatusCode = 200;
                context.RequestCompleted();

                return Task.FromResult<object>(null);
            }

            return base.MatchEndpoint(context);
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var repo = new AuthRepository(IocContainer.Get<IAuthContext>());
            Admin user = repo.FindActive(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = new ApplicationIdentityUser().GenerateUserIdentity(user, AuthenticationType);
            var ticket = new AuthenticationTicket(oAuthIdentity, null);

            context.Validated(ticket);
        }
    }
}