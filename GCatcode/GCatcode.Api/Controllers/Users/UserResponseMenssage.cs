namespace GCatcode.Api.Controllers.Users
{
    public enum UserResponseMenssage
    {
        SearchTermTooShort,
        UserNotFound,
        UserNotCreate,
        UserNotDeleteSelf
    }

    public static class UserResponseMenssageExtensions
    {
        public static string ToMsgString(this UserResponseMenssage msg)
        {
            return msg switch
            {
                UserResponseMenssage.SearchTermTooShort => "The search term must be at least 3 characters long.",
                UserResponseMenssage.UserNotFound => "User not found.",
                UserResponseMenssage.UserNotCreate => "User could not be created.",
                UserResponseMenssage.UserNotDeleteSelf => "You cannot delete your own user account.",
                _ => "Unknown message."
            };
        }
    }
}