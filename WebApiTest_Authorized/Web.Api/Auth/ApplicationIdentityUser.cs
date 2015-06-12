using System.Security.Claims;
using Db.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Web.Api.Auth
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public ClaimsIdentity GenerateUserIdentity(Admin userLogin, string authenticationType)
        {
            var userIdentity = new ClaimsIdentity(authenticationType);
            userIdentity.AddClaim(new Claim(ClaimTypes.Name, userLogin.LoginName));
            userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userLogin.Id.ToString()));
            // Add custom user claims here
            return userIdentity;
        }
    }
}