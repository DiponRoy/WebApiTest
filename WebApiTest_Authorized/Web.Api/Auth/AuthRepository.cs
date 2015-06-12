using System;
using System.Linq;
using Db.Model;

namespace Web.Api.Auth
{
    public interface IAuthRepository : IDisposable
    {
        Admin FindActive(string userName, string password);
    }

    public class AuthRepository : IAuthRepository
    {
        public readonly IAuthContext Context;

        public AuthRepository(IAuthContext context)
        {
            Context = context;
        }

        public Admin FindActive(string userName, string password)
        {
            return Context.Logins.FirstOrDefault(x => x.LoginName == userName
                                                      && x.Password == password
                                                      && x.IsActive);
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public static IDisposable Create()
        {
            return new AuthRepository(new AuthContext());
        }
    }
}