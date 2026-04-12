using GCatcode.Api.Core;
using System.Net;

namespace GCatcode.Api.Controllers.Admin.Users
{
    public class UserResponse : BaseResponse
    {
        private UserResponse() : base()
        {
        }

        public static StatusResponse SearchTermTooShort() => new StatusResponse { Key = "User-SearchTermTooShort", StatusCode = HttpStatusCode.BadRequest, Message = "The search term must be at least 3 characters long." };

        public static StatusResponse UserNotFound() => new StatusResponse { Key = "User-NotFound", StatusCode = HttpStatusCode.NotFound, Message = "User not found." };

        public static StatusResponse UserNotCreate() => new StatusResponse { Key = "User-NotCreate", StatusCode = HttpStatusCode.BadRequest, Message = "User could not be created." };

        public static StatusResponse UserNotDeleteSelf() => new StatusResponse { Key = "User-NotDeleteSelf", StatusCode = HttpStatusCode.BadRequest, Message = "You cannot delete your own user account." };

        public static StatusResponse UserNotAdmin() => new StatusResponse { Key = "User-NotAdmin", StatusCode = HttpStatusCode.Unauthorized, Message = "You must be an administrator to perform this action." };

        public static StatusResponse UserAlreadyExists() => new StatusResponse { Key = "User-AlreadyExists", StatusCode = HttpStatusCode.BadRequest, Message = "Email already exists." };
    }
}