using System;
using System.Collections.Generic;
using Db;
using Db.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Web.Api.Auth
{
    public interface IAuthContext : IDisposable
    {
        IList<Admin> Logins { get; set; }
    }

    public class AuthContext : IdentityDbContext<ApplicationIdentityUser>, IAuthContext
    {
        public AuthContext()
        {
            Logins = new UmsDb().Admins;
        }

        public IList<Admin> Logins { get; set; }

        public static IDisposable Create()
        {
            return new AuthContext();
        }
    }
}