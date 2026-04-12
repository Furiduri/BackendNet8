namespace GCatcode.Services.DB.UserServices.Models
{
    public class UserUpdate
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Available { get; set; }
    }
}