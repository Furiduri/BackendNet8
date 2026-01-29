using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Repository.DB.RolServices;
using Microsoft.Data.SqlClient;

namespace GCatcode.Api.Controllers.Roles
{
    public class RolesMethods
    {
        private readonly AppSettings _configuration;

        public RolesMethods(AppSettings configuration)
        {
            _configuration = configuration;
        }

        public ApiResponse Get(int page)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var roles = new RolesService(context).Get(page, new RolFilter { Available = true });
                return ApiResponse.SuccessResult(roles);
            }
        }

        public ApiResponse Update(RolUpdate data)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var service = new RolesService(context, transaction);
                var role = service.GetById(data.RolId);
                if (role == null)
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.NotFound, RolesResponseMenssage.RoleNotFound.ToMsgString());

                var res = service.Update(data);
                transaction.Commit();
                return ApiResponse.SuccessResult(res);
            }
        }

        public ApiResponse GetById(int id)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var role = new RolesService(context).GetById(id);
                if (role == null)
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.NotFound, RolesResponseMenssage.RoleNotFound.ToMsgString());

                return ApiResponse.SuccessResult(role);
            }
        }

        public ApiResponse Delete(int id)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var service = new RolesService(context, transaction);
                var role = service.GetById(id);
                if (role == null)
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.NotFound, RolesResponseMenssage.RoleNotFound.ToMsgString());
                var res = service.Delete(id);
                transaction.Commit();
                return ApiResponse.SuccessResult(res);
            }
        }
    }
}