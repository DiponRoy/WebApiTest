using System;
using System.Linq;
using System.Security.Claims;
using Db.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Web.Api.Auth
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public ClaimsIdentity GenerateUserIdentity(Admin login, string authenticationType)
        {        
            return AuthUtility.Identity(login, authenticationType);
        }
    }
}