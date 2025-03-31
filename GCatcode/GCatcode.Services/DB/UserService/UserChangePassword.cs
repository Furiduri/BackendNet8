namespace GCatcode.Repository.DB.UserService
{
    public class UserChangePassword
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}