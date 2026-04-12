namespace GCatcode.Services.DB.RefreshTokenServices.Models
{
    public class RefreshTokenInsert
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CreatedByIp { get; set; }
    }
}
