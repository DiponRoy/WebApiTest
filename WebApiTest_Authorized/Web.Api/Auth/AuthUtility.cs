using System;
using System.Security.Claims;
using Db.Model;

namespace Web.Api.Auth
{
    public static class AuthUtility
    {
        public static ClaimsIdentity Identity(Admin login, string authenticationType = "")
        {
            var userIdentity = new ClaimsIdentity(authenticationType);
            userIdentity.AddClaim(new Claim(ClaimTypes.Name, login.LoginName));
            userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, login.Id.ToString()));
            // Add more user claims here
            return userIdentity;
        }

        public static Admin GetAdmin(ClaimsIdentity claimsIdentity)
        {
            var reader = new ClaimsIdentityReader(claimsIdentity);
            var admin = new Admin
            {
                Id = Convert.ToInt32(reader.GetClaimValue(ClaimTypes.NameIdentifier)),
                LoginName = reader.GetClaimValue(ClaimTypes.Name)
            };
            return admin;
        }
    }
}