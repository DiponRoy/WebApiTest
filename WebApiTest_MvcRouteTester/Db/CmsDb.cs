using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db
{
    public class CmsDb : ICmsDb
    {
        public IList<User> Users { get; set; }

        public CmsDb()
        {
            Users = new List<User>()
            {
                new User(){Id = 1, Name = "User1", IsActive = true }
            };
        }
    }
}
