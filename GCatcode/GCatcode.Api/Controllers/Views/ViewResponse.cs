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
                Message = "Vista no encontrada"
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

        public static StatusResponse ViewAlreadyExists()
        {
            return new StatusResponse
            {
                Key = "ViewAlreadyExists",
                StatusCode = System.Net.HttpStatusCode.Conflict,
                Message = "Ya existe una vista con ese nombre o ruta"
            };
        }

        public static StatusResponse ViewCreated()
        {
            return new StatusResponse
            {
                Key = "ViewCreated",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.Created,
                Message = "Vista creada exitosamente"
            };
        }

        public static StatusResponse ViewUpdated()
        {
            return new StatusResponse
            {
                Key = "ViewUpdated",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Vista actualizada exitosamente"
            };
        }

        public static StatusResponse ViewDeleted()
        {
            return new StatusResponse
            {
                Key = "ViewDeleted",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Vista eliminada exitosamente"
            };
        }

        public static StatusResponse RoleAssigned()
        {
            return new StatusResponse
            {
                Key = "RoleAssigned",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Rol asignado a la vista exitosamente"
            };
        }

        public static StatusResponse RoleRemoved()
        {
            return new StatusResponse
            {
                Key = "RoleRemoved",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Rol removido de la vista exitosamente"
            };
        }

        public static StatusResponse RolesAssigned()
        {
            return new StatusResponse
            {
                Key = "RolesAssigned",
                isSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Roles asignados a la vista exitosamente"
            };
        }

        public static StatusResponse Forbidden(string message = null)
        {
            return new StatusResponse
            {
                Key = "Forbidden",
                StatusCode = System.Net.HttpStatusCode.Forbidden,
                Message = string.IsNullOrEmpty(message) ? "No tienes permisos para realizar esta acción" : message
            };
        }
    }
}
