using GCatcode.Repository.DB.UserServices;

namespace GCatcode.Api.Core.Auth
{
    public class LoginResponse
    {
        public string Language { get; set; } = "es-mx";
        public UserDTO User { get; set; }
        public string Token { get; set; }

    }
}
