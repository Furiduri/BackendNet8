using System.Net;

namespace GCatcode.Api.Core.Auth
{
    public class AuthResponse : BaseResponse
    {
        private AuthResponse() : base()
        {
        }

        public static StatusResponse FailedUserCreation() => new StatusResponse
        {
            Key = "UserCreationFailed",
            isSuccess = false,
            StatusCode = HttpStatusCode.BadRequest,
            Message = "User creation failed due to invalid data or server error."
        };

        public static StatusResponse UserNotFound() => new StatusResponse
        {
            Key = "UserNotFound",
            isSuccess = false,
            StatusCode = HttpStatusCode.NotFound,
            Message = "The specified user was not found."
        };
    }
}