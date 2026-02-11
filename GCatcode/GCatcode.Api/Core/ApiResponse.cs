using System.Net;
using System.Text.Json.Serialization;

namespace GCatcode.Api.Core
{
    public class ApiResponse<T> where T : class
    {
        public bool IsSuccess { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; } = null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int StatusCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Errors { get; set; } = null;

        public static ApiResponse<T> SuccessResult(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Data = data,
                Message = message
            };
        }

        public static ApiResponse<T> ErrorResult(StatusResponse statusResponse)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = (int)statusResponse.StatusCode,
                Errors = new List<string>() { statusResponse.Message }
            };
        }

        public static ApiResponse<T> Result(StatusResponse statusResponse, T data)
        {
            return new ApiResponse<T>
            {
                IsSuccess = statusResponse.isSuccess,
                StatusCode = (int)statusResponse.StatusCode,
                Data = data,
                Errors = new List<string>() { statusResponse.Message }
            };
        }
    }
}