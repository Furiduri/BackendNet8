using GCatcode.Api.Configuration;
using GCatcode.Api.Core;
using GCatcode.Services.DB.UserRolServices;
using GCatcode.Services.DB.UserRolServices.Models;
using GCatcode.Services.DB.UserServices;
using GCatcode.Services.DB.UserServices.Models;
using GCatcode.Utils.GenericModels;
using Microsoft.Data.SqlClient;

namespace GCatcode.Api.Controllers.Admin.Users
{
    public class UsersMethods
    {
        private AppSettings _configuration;

        public UsersMethods(AppSettings configuration)
        { _configuration = configuration; }

        public ApiResponse<IEnumerable<UserDTO>> GetListUsers(int page, bool available)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var users = new UserService(context).Get(page: page, filters: new UserFilter { Available = available });
                return ApiResponse<IEnumerable<UserDTO>>.SuccessResult(users);
            }
        }

        public ApiResponse<UserAndRols> GetById(int id)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                UserDTO userDto = new UserService(context).GetById(id);
                if (userDto == null)
                {
                    return ApiResponse<UserAndRols>.ErrorResult(UserResponse.UserNotFound());
                }
                UserAndRols userAndRols = new UserAndRols
                {
                    UserId = userDto.UserId,
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    Roles = new UserRolService(context).GetRolsByUserId(id)
                };
                return ApiResponse<UserAndRols>.SuccessResult(userAndRols);
            }
        }

        public ApiResponse<IEnumerable<UserItem>> SearchUsers(string userName, int page)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                if (userName.Length < 3)
                {
                    return ApiResponse<IEnumerable<UserItem>>.ErrorResult(UserResponse.SearchTermTooShort());
                }
                var users = new UserService(context).Search(userName, page);
                return ApiResponse<IEnumerable<UserItem>>.SuccessResult(users);
            }
        }

        public ApiResponse<UserDTO> CreateUser(UserInsert data)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var userService = new UserService(context, transaction);
                var userExists = userService.GetByEmail(data.Email);
                if (userExists != null)
                {
                    return ApiResponse<UserDTO>.ErrorResult(UserResponse.UserAlreadyExists());
                }
                var userDto = userService.Add(data);
                if (userDto == null)
                {
                    return ApiResponse<UserDTO>.ErrorResult(UserResponse.UserNotCreate());
                }
                transaction.Commit();
                return ApiResponse<UserDTO>.SuccessResult(userDto);
            }
        }

        public ApiResponse<UserDTO> Update(UserUpdate data)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var userService = new UserService(context, transaction);
                var existingUser = userService.GetById(data.UserId);
                if (existingUser == null)
                {
                    return ApiResponse<UserDTO>.ErrorResult(UserResponse.UserNotFound());
                }
                var updatedUser = userService.Update(data);
                transaction.Commit();
                return ApiResponse<UserDTO>.SuccessResult(updatedUser);
            }
        }

        public ApiResponse<GenericMessage> ChangePassword(UserChangePassword data)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var userService = new UserService(context, transaction);
                var existingUser = userService.GetById(data.UserId);
                if (existingUser == null)
                {
                    return ApiResponse<GenericMessage>.ErrorResult(UserResponse.UserNotFound());
                }
                var response = userService.ChangePassword(data);
                transaction.Commit();
                return ApiResponse<GenericMessage>.SuccessResult(response);
            }
        }

        public ApiResponse<UserDTO> Delete(int userId)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var userService = new UserService(context, transaction);
                var existingUser = userService.GetById(userId);
                if (existingUser == null)
                {
                    return ApiResponse<UserDTO>.ErrorResult(UserResponse.UserNotFound());
                }
                var response = userService.Delete(userId);
                transaction.Commit();
                return ApiResponse<UserDTO>.SuccessResult(response);
            }
        }
    }
}