namespace GCatcode.Services.DB.UserServices
{
    public class UserFilter
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool? Available { get; set; }
    }
}