namespace GCatcode.Repository.DB.UserService
{
    public class UserUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Available { get; set; }
    }
}