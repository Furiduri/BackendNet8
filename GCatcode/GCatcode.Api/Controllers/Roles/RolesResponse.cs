using GCatcode.Api.Core;

namespace GCatcode.Api.Controllers.Roles
{
    public class RolesResponse : BaseResponse
    {
        private RolesResponse() : base()
        {
        }

        public static StatusResponse RoleNotFound() => new StatusResponse
        {
            Key = "RoleNotFound",
            StatusCode = System.Net.HttpStatusCode.NotFound,
            Message = "The specified role was not found."
        };
    }
}