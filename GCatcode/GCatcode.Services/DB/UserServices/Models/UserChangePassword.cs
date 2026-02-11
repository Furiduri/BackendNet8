namespace GCatcode.Repository.DB.UserServices
{
    public class UserChangePassword
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}