using System.Collections.Generic;
using Db.Model;

namespace Db
{
    public class UmsDb : IUmsDb
    {
        public IList<Admin> Admins { get; set; }

        public UmsDb()
        {
            Admins = new List<Admin>()
            {
                new Admin() {Id = 1, LoginName = "Admin1", Password = "123", IsActive = true },    
            };
        }
    }
}
