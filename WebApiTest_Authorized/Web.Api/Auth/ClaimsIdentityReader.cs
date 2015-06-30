using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Web.Api.Auth
{
    public class ClaimsIdentityReader
    {
        public readonly ClaimsIdentity Identity;
        public ClaimsIdentityReader(ClaimsIdentity identity)
        {
            Identity = identity;
        }

        public bool AnyClaim(string typeName)
        {
            return Identity.Claims.Any(x => x.Type == typeName);
        }

        public Claim GetClaim(string typeName)
        {
            return Identity.Claims.First(x => x.Type == typeName);
        }

        public string FindClaimValue(string typeName)
        {
            string value = string.Empty;
            if (!AnyClaim(typeName))
            {
                return value;
            }

            value = GetClaimValue(typeName);
            return value;
        }

        public string GetClaimValue(string typeName)
        {
            return GetClaim(typeName).Value;
        }
    }
}