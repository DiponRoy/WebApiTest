namespace Db.Model
{
    public class Admin
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
    }
}