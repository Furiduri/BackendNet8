using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json.Serialization;

namespace GCatcode.Api.Core
{
    public class ApiResponse
    {
        public bool Success { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; } = null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public HttpStatusCode StatusCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Errors { get; set; } = null;

        public static ApiResponse SuccessResult(object data, string message = "Success")
        {
            return new ApiResponse
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = data,
                Message = message
            };
        }

        public static ApiResponse ErrorResult(HttpStatusCode httpStatusCode, string error)
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = httpStatusCode,
                Errors = new List<string>() { error }
            };
        }

        public static ApiResponse ErrorResult(HttpStatusCode httpStatusCode, List<string> errors = null)
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = httpStatusCode,
                Errors = errors ?? new List<string>()
            };
        }

        public static ApiResponse InternalServerError(Exception internalServerError)
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Data = internalServerError,
                Errors = new List<string> { internalServerError.Message }
            };
        }
    }
}