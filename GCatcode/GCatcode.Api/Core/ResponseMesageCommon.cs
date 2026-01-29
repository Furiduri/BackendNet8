namespace GCatcode.Api.Core
{
    public enum ResponseMessageCommon
    {
        Success = 0,
        Error = 1,
        Warning = 2,
        InvalidCredentials = 401,
        InternalServerError = 5000,
    }

    public static class ResponseMessageCommonExtensions
    {
        public static string ToMsgString(this ResponseMessageCommon responseMessage)
        {
            return responseMessage switch
            {
                ResponseMessageCommon.Success => "Success",
                ResponseMessageCommon.Error => "Error",
                ResponseMessageCommon.Warning => "Warning",
                ResponseMessageCommon.InternalServerError => "Internal Server Error, contact support",

                ResponseMessageCommon.InvalidCredentials => "Invalid Credentials",
                _ => "Unknown"
            };
        }
    }
}