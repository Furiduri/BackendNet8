using GCatcode.Api.Core;

namespace GCatcode.Api.Controllers.Permissions
{
    public class PermissionResponse : BaseResponse
    {
        public static StatusResponse PermissionNotFound()
        {
            return new StatusResponse
            {
                Key = "PermissionNotFound",
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = "Permission not found"
            };
        }

        public static StatusResponse InvalidData()
        {
            return new StatusResponse
            {
                Key = "InvalidData",
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = "Invalid data"
            };
        }

        public static StatusResponse PermissionGranted()
        {
            return new StatusResponse
            {
                Key = "PermissionGranted",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Permission granted successfully"
            };
        }

        public static StatusResponse PermissionRevoked()
        {
            return new StatusResponse
            {
                Key = "PermissionRevoked",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Permission revoked successfully"
            };
        }

        public static StatusResponse PermissionAssignedToRole()
        {
            return new StatusResponse
            {
                Key = "PermissionAssignedToRole",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Permission assigned to role successfully"
            };
        }

        public static StatusResponse PermissionRemovedFromRole()
        {
            return new StatusResponse
            {
                Key = "PermissionRemovedFromRole",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Permission removed from role successfully"
            };
        }
    }
}
