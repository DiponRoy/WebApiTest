using System.Collections.Generic;
using Db.Model;

namespace Db
{
    public class UmsDb : IUmsDb
    {
        public IList<Student> Students { get; set; }
        public IList<Admin> Admins { get; set; }

        public UmsDb()
        {
            Students = new List<Student>
            {
                new Student {Id = 1, Name = "Student1", IsActive = true},
                new Student {Id = 2, Name = "Student2", IsActive = true},
            };

            Admins = new List<Admin>()
            {
                new Admin() {Id = 1, Name = "Admin1", IsActive = true},            
            };
        }
    }
}
