using System.Net;

namespace GCatcode.Api.Core.Authorization
{
    public class AuthorizationResponse : BaseResponse
    {
        private AuthorizationResponse() : base()
        {
        }

        public static StatusResponse Unauthorized(string message = null) => new StatusResponse { Key = "Unauthorized", StatusCode = HttpStatusCode.Unauthorized, Message = "Unauthorized access." };

        public static StatusResponse Forbidden(string permission) => new StatusResponse { Key = "Forbidden", StatusCode = HttpStatusCode.Forbidden, Message = $"You do not have permission '{permission}' to access this resource." };
    }
}