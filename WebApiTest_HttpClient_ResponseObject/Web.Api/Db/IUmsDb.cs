using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Db
{
    public interface IUmsDb
    {
        IList<Admin> Admins { get; set; }
        IList<Student> Students { get; set; }
    }
}
