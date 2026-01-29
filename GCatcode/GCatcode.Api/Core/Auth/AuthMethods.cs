using Azure;
using GCatcode.Api.Configuration;
using GCatcode.Repository.DB.RolServices;
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

        public ApiResponse GetUserInfo(int userId)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var userInfo = new UserService(context).GetById(userId);
                if (userInfo == null)
                {
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.NotFound, AuthResponseMessage.UserNotFound.ToMsgString());
                }
                var roles = new RolesService(context).GetRolesByUserId(userId);
                return ApiResponse.SuccessResult(new
                {
                    email = userInfo.Email,
                    username = userInfo.UserName,
                    id = userId,
                    roles = roles.Select(r => r.Name)
                });
            }
        }

        public ApiResponse Login(UserLogin user)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var _userService = new UserService(context);
                var userinfo = _userService.GetByUserName(user.Username);
                if (userinfo == null)
                {
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.Unauthorized, ResponseMessageCommon.InvalidCredentials.ToMsgString());
                }

                if (_userService.ValidPassword(user))
                {
                    var _userRolService = new UserRolService(context);
                    var roles = _userRolService.GetRolsByUserId(userinfo.UserId).ToList();
                    var token = GenerateJwtToken(userinfo, roles);
                    return ApiResponse.SuccessResult(new LoginResponse
                    {
                        Token = token,
                        User = userinfo,
                    });
                }
                return ApiResponse.ErrorResult(System.Net.HttpStatusCode.Unauthorized, ResponseMessageCommon.InvalidCredentials.ToMsgString());
            }
        }

        public ApiResponse Register(UserInsert user)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var transaction = context.BeginTransaction();
                var userCreated = new UserService(context, transaction).Add(user);
                if (userCreated == null)
                {
                    return ApiResponse.ErrorResult(System.Net.HttpStatusCode.BadRequest, AuthResponseMessage.FailedUserCreation.ToMsgString());
                }

                var roles = new RolesService(context).GetRolesByUserId(userCreated.UserId);
                var token = GenerateJwtToken(userCreated, roles);
                return ApiResponse.SuccessResult(new { token });
            }
        }

        private string GenerateJwtToken(UserDTO user, IEnumerable<RolItem> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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