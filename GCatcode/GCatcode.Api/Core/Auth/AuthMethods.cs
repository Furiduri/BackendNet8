using GCatcode.Api.Configuration;
using GCatcode.Api.Controllers.Users;
using GCatcode.Api.Core.Auth.Models;
using GCatcode.Repository.DB.RolServices;
using GCatcode.Repository.DB.RolServices.Models;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GCatcode.Api.Core.Auth
{
    public partial class AuthMethods
    {
        private AppSettings _configuration;

        public AuthMethods(AppSettings configuration)
        {
            _configuration = configuration;
        }

        public ApiResponse<UserInfo> GetUserInfo(int userId)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var userInfo = new UserService(context).GetById(userId);
                if (userInfo == null)
                {
                    return ApiResponse<UserInfo>.ErrorResult(AuthResponse.UserNotFound());
                }
                var roles = new RolesService(context).GetRolesByUserId(userId);
                return ApiResponse<UserInfo>.SuccessResult(new UserInfo
                {
                    Email = userInfo.Email,
                    UserName = userInfo.UserName,
                    UserId = userId,
                    Roles = roles
                });
            }
        }

        public ApiResponse<LoginInfo> Login(UserLogin user)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var _userService = new UserService(context);
                var userinfo = _userService.GetByEmail(user.Email);
                if (userinfo == null)
                {
                    return ApiResponse<LoginInfo>.ErrorResult(AuthResponse.InvalidCredentials());
                }

                if (_userService.ValidPassword(user))
                {
                    var _userRolService = new UserRolService(context);
                    var roles = _userRolService.GetRolsByUserId(userinfo.UserId).ToList();
                    var token = GenerateJwtToken(userinfo, roles);
                    return ApiResponse<LoginInfo>.SuccessResult(new LoginInfo
                    {
                        Token = token,
                        User = userinfo,
                    }, "Login Succes");
                }
                return ApiResponse<LoginInfo>.ErrorResult(AuthResponse.InvalidCredentials());
            }
        }

        public ApiResponse<LoginInfo> Register(UserInsert data)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var userService = new UserService(context, transaction);
                var userExists = userService.GetByEmail(data.Email);
                if (userExists != null)
                {
                    return ApiResponse<LoginInfo>.ErrorResult(UserResponse.UserAlreadyExists());
                }
                var userCreated = userService.Add(data);
                if (userCreated == null)
                {
                    return ApiResponse<LoginInfo>.ErrorResult(AuthResponse.FailedUserCreation());
                }
                var _userRolService = new UserRolService(context, transaction);
                var roles = _userRolService.GetRolsByUserId(userCreated.UserId).ToList();
                transaction.Commit();
                var token = GenerateJwtToken(userCreated, roles);
                return ApiResponse<LoginInfo>.SuccessResult(new LoginInfo
                {
                    Token = token,
                    User = userCreated,
                }, "Login Succes");
            }
        }

        private string GenerateJwtToken(UserDTO user, IEnumerable<RolItem> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, $"{user.UserId}"),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.JwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.JwtSettings.Issuer,
                audience: _configuration.JwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}