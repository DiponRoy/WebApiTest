using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Db;
using Db.Model;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Web.Api.Auth;
using Web.Api.Model.Response;

namespace Web.Api.Controllers
{
    [RoutePrefix("api")]
    public class TokenController : ApiController
    {
        private readonly IUmsDb Context;

        public TokenController(IUmsDb context)
        {
            Context = context;
        }

        [HttpPost]
        [Route("token/user")]
        public ApiResponse<IdentityToken> UserToken(Admin admin)
        {
            Admin user = Context.Admins.FirstOrDefault(x => x.LoginName == admin.LoginName
                                                            && x.Password == admin.Password
                                                            && x.IsActive);
            if (user == null)
            {
                throw new UnauthorizedAccessException("");
            }

            ClaimsIdentity oAuthIdentity = new ApplicationIdentityUser().GenerateUserIdentity(user, "Jwt");
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            DateTimeOffset currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.AddDays(1);

            string token = AuthConfig.OAuthServerOptions.AccessTokenFormat.Protect(ticket);


            return
                new ApiResponse<IdentityToken>(new IdentityToken
                {
                    AccessToken = token,
                    ExpiresIn = (long) AuthConfig.OAuthServerOptions.AuthorizationCodeExpireTimeSpan.TotalSeconds,
                    TokenType = AuthConfig.OAuthServerOptions.AuthenticationType
                });
        }
    }
}
