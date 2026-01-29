using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using Microsoft.Data.SqlClient;

namespace GCatcode.Api.Controllers.Users
{
    public class UsersMethods
    {
        private AppSettings _configuration;

        public UsersMethods(AppSettings configuration)
        { _configuration = configuration; }

        public ApiResponse GetListUsers(int page, bool available)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var users = new UserService(context).Get(page, filters: new UserFilter { Available = available });
                return ApiResponse.SuccessResult(users);
            }
        }

        public ApiResponse GetById(int id)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                UserDTO userDto = new UserService(context).GetById(id);
                if (userDto == null)
                {
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.NotFound, UserResponseMenssage.UserNotFound.ToMsgString());
                }
                UserAndRols userAndRols = new UserAndRols
                {
                    UserId = userDto.UserId,
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    Roles = new UserRolService(context).GetRolsByUserId(id)
                };
                return ApiResponse.SuccessResult(userAndRols);
            }
        }

        internal ApiResponse SearchUsers(string userName, int page)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                if (userName.Length < 3)
                {
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.NotFound, UserResponseMenssage.SearchTermTooShort.ToMsgString());
                }
                var users = new UserService(context).Search(userName, page);
                return ApiResponse.SuccessResult(users);
            }
        }

        public ApiResponse CreateUser(UserInsert data)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var userService = new UserService(context, transaction);
                var userDto = userService.Add(data);
                if (userDto == null)
                {
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.BadRequest, UserResponseMenssage.UserNotCreate.ToMsgString());
                }
                transaction.Commit();
                return ApiResponse.SuccessResult(userDto);
            }
        }

        public ApiResponse Update(UserUpdate data)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var userService = new UserService(context, transaction);
                var existingUser = userService.GetById(data.UserId);
                if (existingUser == null)
                {
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.NotFound, UserResponseMenssage.UserNotFound.ToMsgString());
                }
                var updatedUser = userService.Update(data);
                transaction.Commit();
                return ApiResponse.SuccessResult(updatedUser);
            }
        }

        public ApiResponse ChangePassword(UserChangePassword data)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var userService = new UserService(context, transaction);
                var existingUser = userService.GetById(data.UserId);
                if (existingUser == null)
                {
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.NotFound, UserResponseMenssage.UserNotFound.ToMsgString());
                }
                var response = userService.ChangePassword(data);
                transaction.Commit();
                return ApiResponse.SuccessResult(response);
            }
        }

        public ApiResponse Delete(int userId)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var userService = new UserService(context, transaction);
                var existingUser = userService.GetById(userId);
                if (existingUser == null)
                {
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.NotFound, UserResponseMenssage.UserNotFound.ToMsgString());
                }
                var response = userService.Delete(userId);
                transaction.Commit();
                return ApiResponse.SuccessResult(response);
            }
        }
    }
}