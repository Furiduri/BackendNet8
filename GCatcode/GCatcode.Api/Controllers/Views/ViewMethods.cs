using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Services.DB.RolServices;
using GCatcode.Services.DB.ViewServices;
using GCatcode.Services.DB.ViewServices.Models;
using GCatcode.Utils.GenericModels;
using Microsoft.Data.SqlClient;

namespace GCatcode.Api.Controllers.Views
{
    public class ViewMethods
    {
        private readonly AppSettings _configuration;

        public ViewMethods(AppSettings configuration)
        {
            _configuration = configuration;
        }

        public ApiResponse<PagesList<ViewDTO>> GetAll(int page, int pageSize)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var viewService = new ViewService(connection);
                var views = viewService.GetAll(page, pageSize);
                return ApiResponse<PagesList<ViewDTO>>.SuccessResult(views);
            }
        }

        public ApiResponse<ViewDTO> GetById(int id)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var viewService = new ViewService(connection);
                var view = viewService.GetById(id);

                if (view == null)
                {
                    return ApiResponse<ViewDTO>.ErrorResult(ViewResponse.ViewNotFound());
                }

                return ApiResponse<ViewDTO>.SuccessResult(view);
            }
        }

        public ApiResponse<IEnumerable<ViewDTO>> GetByUserId(int userId)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var viewService = new ViewService(connection);
                var views = viewService.GetByUserId(userId);
                return ApiResponse<IEnumerable<ViewDTO>>.SuccessResult(views);
            }
        }

        public ApiResponse<IEnumerable<ViewDTO>> GetByRoleId(int roleId)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var viewService = new ViewService(connection);
                var views = viewService.GetByRoleId(roleId);
                return ApiResponse<IEnumerable<ViewDTO>>.SuccessResult(views);
            }
        }

        public ApiResponse<ViewDTO> Post(ViewInsert data)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var viewService = new ViewService(connection, transaction);
                var existingView = viewService.GetByRoute(data.Route);
                if (existingView != null)
                {
                    return ApiResponse<ViewDTO>.ErrorResult(ViewResponse.ViewAlreadyExists());
                }
                var createdView = viewService.Add(data);

                if (createdView == null)
                {
                    transaction.Rollback();
                    return ApiResponse<ViewDTO>.ErrorResult(ViewResponse.InvalidData());
                }

                transaction.Commit();
                return ApiResponse<ViewDTO>.Result(ViewResponse.ViewCreated(), createdView);
            }
        }

        public ApiResponse<ViewDTO> Put(ViewUpdate data)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var viewService = new ViewService(connection, transaction);

                var existingView = viewService.GetById(data.ViewId);
                if (existingView == null)
                {
                    transaction.Rollback();
                    return ApiResponse<ViewDTO>.ErrorResult(ViewResponse.ViewNotFound());
                }

                var result = viewService.Update(data);

                if (!result)
                {
                    transaction.Rollback();
                    return ApiResponse<ViewDTO>.ErrorResult(ViewResponse.InvalidData());
                }

                transaction.Commit();
                var updatedView = viewService.GetById(data.ViewId);
                return ApiResponse<ViewDTO>.Result(ViewResponse.ViewUpdated(), updatedView!);
            }
        }

        public ApiResponse<ViewDTO> Delete(int id)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var viewService = new ViewService(connection, transaction);

                var existingView = viewService.GetById(id);
                if (existingView == null)
                {
                    transaction.Rollback();
                    return ApiResponse<ViewDTO>.ErrorResult(ViewResponse.ViewNotFound());
                }

                var result = viewService.Delete(id);

                if (!result)
                {
                    transaction.Rollback();
                    return ApiResponse<ViewDTO>.ErrorResult(ViewResponse.InvalidData());
                }

                transaction.Commit();
                return ApiResponse<ViewDTO>.Result(ViewResponse.ViewDeleted(), existingView);
            }
        }

        public ApiResponse<StatusResponse> AssignRoleToView(ViewRoleAssignment viewToRole)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var viewService = new ViewService(connection, transaction);
                var roleService = new RolesService(connection, transaction);

                var existingView = viewService.GetById(viewToRole.ViewId);
                if (existingView == null)
                {
                    transaction.Rollback();
                    return ApiResponse<StatusResponse>.ErrorResult(ViewResponse.ViewNotFound());
                }

                var existingRoles = roleService.GetById(viewToRole.RoleId);
                if (existingRoles == null)
                {
                    transaction.Rollback();
                    return ApiResponse<StatusResponse>.ErrorResult(ViewResponse.RoleNotFound());
                }

                viewService.AssignViewToRole(viewToRole);
                transaction.Commit();

                return ApiResponse<StatusResponse>.Result(ViewResponse.RoleAssigned());
            }
        }

        public ApiResponse<StatusResponse> RemoveRoleFromView(ViewRoleAssignment viewToRole)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var viewService = new ViewService(connection, transaction);

                var existingView = viewService.GetById(viewToRole.ViewId);
                if (existingView == null)
                {
                    transaction.Rollback();
                    return ApiResponse<StatusResponse>.ErrorResult(ViewResponse.ViewNotFound());
                }

                viewService.RemoveViewFromRole(viewToRole);
                transaction.Commit();

                return ApiResponse<StatusResponse>.Result(ViewResponse.RoleRemoved());
            }
        }

        public ApiResponse<StatusResponse> AssignRolesToView(ViewAssignmentRoleList assignmentRoleList)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var viewService = new ViewService(connection, transaction);
                var roleService = new RolesService(connection, transaction);

                var existingView = viewService.GetById(assignmentRoleList.ViewId);
                if (existingView == null)
                {
                    transaction.Rollback();
                    return ApiResponse<StatusResponse>.ErrorResult(ViewResponse.ViewNotFound());
                }

                foreach (var rol in assignmentRoleList.RoleIds)
                {
                    var existingRol = roleService.GetById(rol);
                    if (existingRol == null)
                    {
                        transaction.Rollback();
                        return ApiResponse<StatusResponse>.ErrorResult(ViewResponse.RoleNotFound());
                    }
                }

                viewService.AssignRolesToView(assignmentRoleList);
                transaction.Commit();

                return ApiResponse<StatusResponse>.Result(ViewResponse.RoleAssigned());
            }
        }
    }
}