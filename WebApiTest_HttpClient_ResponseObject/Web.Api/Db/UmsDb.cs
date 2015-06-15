using System;
using System.Collections.Generic;

namespace Web.Api.Db
{
    public class UmsDb : IUmsDb
    {
        public IList<Admin> Admins { get; set; }
        public IList<Student> Students { get; set; }

        public UmsDb()
        {
            Students = new List<Student>();
            for (int i = 1; i <= 100; i++)
            {
                Students.Add(new Student {Id = i, Name = String.Format("Student{0}", i), IsActive = true});
                Students.Add(new Student { Id = i, Name = String.Format("Admin{0}", i), IsActive = true });
            }
        }
    }
}