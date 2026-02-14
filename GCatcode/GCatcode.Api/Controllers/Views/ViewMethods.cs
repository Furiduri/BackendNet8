using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Repository.DB.ViewServices;
using GCatcode.Repository.DB.ViewServices.Models;
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

        public ApiResponse<IEnumerable<ViewDTO>> GetAll()
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var viewService = new ViewService(connection);
                var views = viewService.GetAll();
                return ApiResponse<IEnumerable<ViewDTO>>.SuccessResult(views, "Vistas obtenidas exitosamente");
            }
        }

        public ApiResponse<IEnumerable<ViewWithRoles>> GetAllWithRoles()
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var viewService = new ViewService(connection);
                var views = viewService.GetAllWithRoles();
                return ApiResponse<IEnumerable<ViewWithRoles>>.SuccessResult(views, "Vistas con roles obtenidas exitosamente");
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

                return ApiResponse<ViewDTO>.SuccessResult(view, "Vista obtenida exitosamente");
            }
        }

        public ApiResponse<IEnumerable<ViewDTO>> GetByUserId(int userId)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var viewService = new ViewService(connection);
                var views = viewService.GetByUserId(userId);
                return ApiResponse<IEnumerable<ViewDTO>>.SuccessResult(views, "Vistas del usuario obtenidas exitosamente");
            }
        }

        public ApiResponse<IEnumerable<ViewDTO>> GetByRoleId(int roleId)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var viewService = new ViewService(connection);
                var views = viewService.GetByRoleId(roleId);
                return ApiResponse<IEnumerable<ViewDTO>>.SuccessResult(views, "Vistas del rol obtenidas exitosamente");
            }
        }

        public ApiResponse<ViewDTO> Post(ViewInsert data)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var viewService = new ViewService(connection, transaction);

                var createdView = viewService.Add(data);

                if (createdView == null)
                {
                    transaction.Rollback();
                    return ApiResponse<ViewDTO>.ErrorResult(ViewResponse.InvalidData());
                }

                transaction.Commit();
                return ApiResponse<ViewDTO>.SuccessResult(createdView, ViewResponse.ViewCreated().Message);
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
                return ApiResponse<ViewDTO>.SuccessResult(updatedView!, ViewResponse.ViewUpdated().Message);
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
                return ApiResponse<ViewDTO>.SuccessResult(existingView, ViewResponse.ViewDeleted().Message);
            }
        }

        public ApiResponse<StatusResponse> AssignRoleToView(int viewId, int roleId)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var viewService = new ViewService(connection, transaction);

                var existingView = viewService.GetById(viewId);
                if (existingView == null)
                {
                    transaction.Rollback();
                    return ApiResponse<StatusResponse>.ErrorResult(ViewResponse.ViewNotFound());
                }

                viewService.AssignViewToRole(viewId, roleId);
                transaction.Commit();

                return ApiResponse<StatusResponse>.SuccessResult(ViewResponse.RoleAssigned(), ViewResponse.RoleAssigned().Message);
            }
        }

        public ApiResponse<StatusResponse> RemoveRoleFromView(int viewId, int roleId)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var viewService = new ViewService(connection, transaction);

                var existingView = viewService.GetById(viewId);
                if (existingView == null)
                {
                    transaction.Rollback();
                    return ApiResponse<StatusResponse>.ErrorResult(ViewResponse.ViewNotFound());
                }

                viewService.RemoveViewFromRole(viewId, roleId);
                transaction.Commit();

                return ApiResponse<StatusResponse>.SuccessResult(ViewResponse.RoleRemoved(), ViewResponse.RoleRemoved().Message);
            }
        }

        public ApiResponse<StatusResponse> AssignRolesToView(int viewId, IEnumerable<int> roleIds)
        {
            using (var connection = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var viewService = new ViewService(connection, transaction);

                var existingView = viewService.GetById(viewId);
                if (existingView == null)
                {
                    transaction.Rollback();
                    return ApiResponse<StatusResponse>.ErrorResult(ViewResponse.ViewNotFound());
                }

                viewService.AssignRolesToView(viewId, roleIds);
                transaction.Commit();

                return ApiResponse<StatusResponse>.SuccessResult(ViewResponse.RolesAssigned(), ViewResponse.RolesAssigned().Message);
            }
        }
    }
}
