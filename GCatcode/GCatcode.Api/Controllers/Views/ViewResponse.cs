using GCatcode.Api.Core;

namespace GCatcode.Api.Controllers.Views
{
    public class ViewResponse : BaseResponse
    {
        public static StatusResponse ViewNotFound()
        {
            return new StatusResponse
            {
                Key = "ViewNotFound",
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = "View not found"
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

        public static StatusResponse ViewAlreadyExists()
        {
            return new StatusResponse
            {
                Key = "ViewAlreadyExists",
                StatusCode = System.Net.HttpStatusCode.Conflict,
                Message = "A view with that name or route already exists"
            };
        }

        public static StatusResponse ViewCreated()
        {
            return new StatusResponse
            {
                Key = "ViewCreated",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.Created,
                Message = "View created successfully"
            };
        }

        public static StatusResponse ViewUpdated()
        {
            return new StatusResponse
            {
                Key = "ViewUpdated",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "View updated successfully"
            };
        }

        public static StatusResponse ViewDeleted()
        {
            return new StatusResponse
            {
                Key = "ViewDeleted",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "View deleted successfully"
            };
        }

        public static StatusResponse RoleAssigned()
        {
            return new StatusResponse
            {
                Key = "RoleAssigned",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Role assigned to view successfully"
            };
        }

        public static StatusResponse RoleRemoved()
        {
            return new StatusResponse
            {
                Key = "RoleRemoved",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Role removed from view successfully"
            };
        }

        public static StatusResponse RoleNotFound()
        {
            return new StatusResponse
            {
                Key = "RoleNotFound",
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = "Role not found"
            };
        }

        public static StatusResponse Forbidden(string message = null)
        {
            return new StatusResponse
            {
                Key = "Forbidden",
                StatusCode = System.Net.HttpStatusCode.Forbidden,
                Message = string.IsNullOrEmpty(message) ? "You don't have permissions to perform this action" : message
            };
        }
    }
}
