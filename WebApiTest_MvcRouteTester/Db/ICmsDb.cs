using System.Collections.Generic;

namespace Db
{
    public interface ICmsDb
    {
        IList<User> Users { get; set; }
    }
}