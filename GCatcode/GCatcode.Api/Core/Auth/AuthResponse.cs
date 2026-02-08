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
            StatusCode = HttpStatusCode.InternalServerError,
            Message = "User creation failed due to invalid data or server error."
        };

        public static StatusResponse UserNotFound() => new StatusResponse
        {
            Key = "UserNotFound",
            isSuccess = false,
            StatusCode = HttpStatusCode.NotFound,
            Message = "The specified user was not found."
        };

        public static StatusResponse InvalidRefreshToken() => new StatusResponse
        {
            Key = "InvalidRefreshToken",
            isSuccess = false,
            StatusCode = HttpStatusCode.Unauthorized,
            Message = "The refresh token is invalid, expired, or has been revoked."
        };

        public static StatusResponse RevokeFailed() => new StatusResponse
        {
            Key = "RevokeFailed",
            StatusCode = HttpStatusCode.InternalServerError,
            Message = "Failed to revoke token"
        };
    }
}
