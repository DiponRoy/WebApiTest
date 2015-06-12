using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Web.Api.Auth
{
    internal class AuthUtil
    {
        private static IDictionary<int, TimeZoneInfo> _userTimeZone;

        public static string Token(string userName, int userId, int providerId)
        {
            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            identity.AddClaim(new Claim("provider", providerId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, "LoggedIn"));

            //Maybe use something like this
            //identity.AddClaim(new Claim(ClaimTypes.Role, userTypeId.ToString() == "0" ? "NonAdmin" : "Admin")); 

            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            var currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;

            // Set hard expiration to 24 hours, stored in web.config as 23:59:59 to make the time format easily understandable
            // by people who read the config.  See #2909  https://www.assembla.com/spaces/snap-md/tickets/2909
            ticket.Properties.ExpiresUtc = currentUtc.Add(Settings.Default.TokenHardExpiration);
            var token = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
            return token;
        }

        public static void ConfigureWebApiToUseOnlyBearerTokenAuthentication(HttpConfiguration config)
        {
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
        }

        public static void ClearCacheUserTimeZone(int userId)
        {
            if (_userTimeZone!=null && _userTimeZone.ContainsKey(userId))
            {
                _userTimeZone.Remove(userId);
            }
        }

        public static TimeZoneInfo GetUserTimeZone(int userId)
        {
            if (_userTimeZone == null)
            {
                _userTimeZone = new Dictionary<int, TimeZoneInfo>();
            }
            lock (_userTimeZone)
            {
                if (_userTimeZone.ContainsKey(userId))
                {
                    return _userTimeZone[userId];
                }

                using (var db = new SnapContext())
                {
                    var repo = new UserRepository(db);
                    var userTimeZone = repo.GetUserTimeZone(userId);
                    _userTimeZone.Add(userId, userTimeZone);
                    return userTimeZone;
                }
            }
        }

        public static TimeZoneInfo GetLoggedInUserTimeZone()
        {
            var user = HttpContext.Current.User;
            if (user == null)
            {
                // user will not null so never happen. but for the safe
                return TimeZoneInfo.Local;
            }
            var userId = GetUserFromToken((ClaimsIdentity) user.Identity).UserId;
            return GetUserTimeZone(userId);
        }

        public static TokenIdentity GetUserFromToken(ClaimsIdentity identity)
        {
            var tokenIdentity = new TokenIdentity();
            foreach (var claim in identity.Claims)
            {
                if (claim.Type.EndsWith("name"))
                {
                    tokenIdentity.User = claim.Value;
                }
                if (claim.Type.EndsWith("nameidentifier"))
                {
                    tokenIdentity.UserId = int.Parse(claim.Value);
                }
                if (claim.Type.EndsWith("provider"))
                {
                    tokenIdentity.ProviderId = int.Parse(claim.Value);
                }
            }

            return tokenIdentity;
        }

        public static IDeveloperIdentity GetDeveloperIdentity()
        {
            try
            {
                var headers = HttpContext.Current.Request.Headers;
                var devId = headers["X-Developer-Id"];
                var guid = Guid.ParseExact(devId, "N");
                var hash = headers["X-Api-Key"];

                var identity = new DeveloperIdentity {DeveloperId = guid};
                if (!identity.IsValid(hash))
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                return identity;
            }
            catch (Exception ex)
            {
                SnapMDLogger.LogException(ex, "Error parsing developer ID, throwing 401");
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }

        internal static TokenIdentity GetUserFromToken(ClaimsIdentity identity, out int userId, out string email)
        {
            var tokenIdentity = GetUserFromToken(identity);
            userId = tokenIdentity.UserId;
            email = tokenIdentity.User;
            return tokenIdentity;
        }
    }
}