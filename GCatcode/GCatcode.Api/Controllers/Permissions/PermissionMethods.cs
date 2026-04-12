using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Services.DB.PermissionServices;
using GCatcode.Services.DB.PermissionServices.Models;
using GCatcode.Utils.GenericModels;
using Microsoft.Data.SqlClient;

namespace GCatcode.Api.Controllers.Permissions
{
    public class PermissionMethods
    {
        private readonly AppSettings _configuration;

        public PermissionMethods(AppSettings configuration)
        {
            _configuration = configuration;
        }

        public ApiResponse<PagesList<PermissionDTO>> GetAll(int page, int pageSize)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var permissionService = new PermissionService(connection);
                var permissions = permissionService.GetAll(page, pageSize);
                return ApiResponse<PagesList<PermissionDTO>>.SuccessResult(permissions);
            }
        }

        public ApiResponse<PermissionDTO> GetById(int id)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var permissionService = new PermissionService(connection);
                var permission = permissionService.GetById(id);

                if (permission == null)
                {
                    return ApiResponse<PermissionDTO>.ErrorResult(PermissionResponse.PermissionNotFound());
                }

                return ApiResponse<PermissionDTO>.SuccessResult(permission);
            }
        }

        public ApiResponse<IEnumerable<UserPermissionResult>> GetUserPermissions(int userId)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var permissionService = new PermissionService(connection);
                var permissions = permissionService.GetUserPermissions(userId);
                return ApiResponse<IEnumerable<UserPermissionResult>>.SuccessResult(permissions);
            }
        }

        public ApiResponse<IEnumerable<PermissionDTO>> GetRolePermissions(int roleId)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var permissionService = new PermissionService(connection);
                var permissions = permissionService.GetRolePermissions(roleId);
                return ApiResponse<IEnumerable<PermissionDTO>>.SuccessResult(permissions);
            }
        }

        public ApiResponse<StatusResponse> GrantPermissionToUser(UserPermissionInsert data)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var permissionService = new PermissionService(connection, transaction);

                var permission = permissionService.GetById(data.PermissionId);
                if (permission == null)
                {
                    transaction.Rollback();
                    return ApiResponse<StatusResponse>.ErrorResult(PermissionResponse.PermissionNotFound());
                }

                permissionService.GrantPermissionToUser(data.UserId, data.PermissionId, data.IsGranted, data.Conditions);
                transaction.Commit();

                return ApiResponse<StatusResponse>.Result(PermissionResponse.PermissionGranted());
            }
        }

        public ApiResponse<StatusResponse> RevokePermissionFromUser(int userId, int permissionId)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var permissionService = new PermissionService(connection, transaction);

                permissionService.RevokePermissionFromUser(userId, permissionId);
                transaction.Commit();

                return ApiResponse<StatusResponse>.Result(PermissionResponse.PermissionRevoked());
            }
        }

        public ApiResponse<StatusResponse> GrantPermissionToRole(RolePermissionInsert data)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var permissionService = new PermissionService(connection, transaction);

                var permission = permissionService.GetById(data.PermissionId);
                if (permission == null)
                {
                    transaction.Rollback();
                    return ApiResponse<StatusResponse>.ErrorResult(PermissionResponse.PermissionNotFound());
                }

                permissionService.GrantPermissionToRole(data.RoleId, data.PermissionId);
                transaction.Commit();

                return ApiResponse<StatusResponse>.Result(PermissionResponse.PermissionAssignedToRole());
            }
        }

        public ApiResponse<StatusResponse> RevokePermissionFromRole(int roleId, int permissionId)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var permissionService = new PermissionService(connection, transaction);

                permissionService.RevokePermissionFromRole(roleId, permissionId);
                transaction.Commit();

                return ApiResponse<StatusResponse>.Result(PermissionResponse.PermissionRemovedFromRole());
            }
        }
    }
}
