using System.Net;

namespace GCatcode.Api.Core
{
    public class BaseResponse
    {
        protected BaseResponse()
        { }

        public static StatusResponse Success() => new StatusResponse { Key = "Success", isSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Operation completed successfully." };

        public static StatusResponse NotContent() => new StatusResponse { Key = "NoContent", isSuccess = true, StatusCode = HttpStatusCode.NoContent, Message = "No content to return." };

        public static StatusResponse NotFound() => new StatusResponse { Key = "NotFound", StatusCode = HttpStatusCode.NotFound, Message = "The requested resource was not found." };

        public static StatusResponse Error(string message) => new StatusResponse { Key = "Error", StatusCode = HttpStatusCode.BadRequest, Message = message };

        public static StatusResponse Warning(string message) => new StatusResponse { Key = "Warning", isSuccess = true, StatusCode = HttpStatusCode.BadRequest, Message = message };

        public static StatusResponse InvalidCredentials() => new StatusResponse { Key = "InvalidCredentials", StatusCode = HttpStatusCode.Unauthorized, Message = "Invalid credentials provided." };

        public static StatusResponse InternalServerError(string message = null) => new StatusResponse { Key = "InternalServerError", StatusCode = HttpStatusCode.InternalServerError, Message = string.IsNullOrEmpty(message) ? "Internal Server Error, contact support" : message };
    }

    public class StatusResponse
    {
        public string Key { get; set; }
        public bool isSuccess { get; set; } = false;
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}