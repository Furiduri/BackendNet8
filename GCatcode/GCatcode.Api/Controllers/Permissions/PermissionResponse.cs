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
                Message = "Permiso no encontrado"
            };
        }

        public static StatusResponse InvalidData()
        {
            return new StatusResponse
            {
                Key = "InvalidData",
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = "Datos inválidos"
            };
        }

        public static StatusResponse PermissionGranted()
        {
            return new StatusResponse
            {
                Key = "PermissionGranted",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Permiso otorgado exitosamente"
            };
        }

        public static StatusResponse PermissionRevoked()
        {
            return new StatusResponse
            {
                Key = "PermissionRevoked",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Permiso revocado exitosamente"
            };
        }

        public static StatusResponse PermissionAssignedToRole()
        {
            return new StatusResponse
            {
                Key = "PermissionAssignedToRole",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Permiso asignado al rol exitosamente"
            };
        }

        public static StatusResponse PermissionRemovedFromRole()
        {
            return new StatusResponse
            {
                Key = "PermissionRemovedFromRole",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Permiso removido del rol exitosamente"
            };
        }
    }
}
