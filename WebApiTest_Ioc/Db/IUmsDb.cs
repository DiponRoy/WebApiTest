using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db.Models;

namespace Db
{
    public interface IUmsDb
    {
        IList<Student> Students { get; set; }
        IList<Admin> Admins { get; set; }

    }
}
