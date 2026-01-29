namespace GCatcode.Api.Core.Auth
{
    public enum AuthResponseMessage
    {
        FailedUserCreation,
        UserNotFound,
    }

    public static class AuthResponseMessageExtensions
    {
        public static string ToMsgString(this AuthResponseMessage responseMessage)
        {
            return responseMessage switch
            {
                AuthResponseMessage.FailedUserCreation => "User creation failed.",
                AuthResponseMessage.UserNotFound => "User not found.",
                _ => "Unknown"
            };
        }
    }
}