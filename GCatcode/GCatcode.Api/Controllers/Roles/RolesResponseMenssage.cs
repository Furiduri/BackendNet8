namespace GCatcode.Api.Controllers.Roles
{
    public enum RolesResponseMenssage
    {
        RoleNotFound = 1,
    }

    public static class RolesResponseMenssageExtensions
    {
        public static string ToMsgString(this RolesResponseMenssage responseMessage)
        {
            return responseMessage switch
            {
                RolesResponseMenssage.RoleNotFound => "The specified role was not found.",
                _ => "An unknown error occurred."
            };
        }
    }
}