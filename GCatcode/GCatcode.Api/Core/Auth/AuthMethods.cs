using GCatcode.Api.Configuration;
using GCatcode.Api.Controllers.Admin.Users;
using GCatcode.Api.Core.Auth.Models;
using GCatcode.Repository.DB.RefreshTokenServices;
using GCatcode.Repository.DB.RefreshTokenServices.Models;
using GCatcode.Repository.DB.RolServices;
using GCatcode.Repository.DB.RolServices.Models;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using GCatcode.Utils.GenericModels;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
                    UserId = userId,
                    Email = userInfo.Email,
                    UserName = userInfo.UserName,
                    Roles = roles
                });
            }
        }

        public ApiResponse<LoginInfo> Login(UserLogin user, string ipAddress)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();
                var _userService = new UserService(context, transaction);
                var userinfo = _userService.GetByEmail(user.Email);
                if (userinfo == null)
                {
                    return ApiResponse<LoginInfo>.ErrorResult(AuthResponse.InvalidCredentials());
                }

                if (_userService.ValidPassword(user))
                {
                    var _userRolService = new UserRolService(context, transaction);
                    var roles = _userRolService.GetRolsByUserId(userinfo.UserId).ToList();

                    var tokenExpiryDate = DateTime.UtcNow.AddMinutes(_configuration.JwtSettings.AccessTokenExpirationMinutes);
                    var refreshExpiryDate = DateTime.UtcNow.AddDays(_configuration.JwtSettings.RefreshTokenExpirationDays);

                    var token = GenerateJwtToken(userinfo, roles, tokenExpiryDate);
                    var refreshToken = GenerateRefreshToken(ipAddress);

                    var _refreshTokenService = new RefreshTokenService(context, transaction);
                    _refreshTokenService.RemoveExpiredTokens();
                    _refreshTokenService.Add(new RefreshTokenInsert
                    {
                        UserId = userinfo.UserId,
                        Token = refreshToken,
                        ExpiryDate = refreshExpiryDate,
                        CreatedByIp = ipAddress
                    });
                    transaction.Commit();
                    return ApiResponse<LoginInfo>.SuccessResult(new LoginInfo
                    {
                        Token = token,
                        RefreshToken = refreshToken,
                        TokenExpiryDate = tokenExpiryDate,
                        RefreshExpiryDate = refreshExpiryDate,
                        User = userinfo,
                    }, "Login Succes");
                }
                return ApiResponse<LoginInfo>.ErrorResult(AuthResponse.InvalidCredentials());
            }
        }

        public ApiResponse<LoginInfo> RefreshToken(string token, string ipAddress)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                context.Open();
                var transaction = context.BeginTransaction();

                var _refreshTokenService = new RefreshTokenService(context, transaction);
                _refreshTokenService.RemoveExpiredTokens();
                var refreshToken = _refreshTokenService.GetByToken(token);

                if (refreshToken == null || !refreshToken.IsActive)
                {
                    transaction.Rollback();
                    return ApiResponse<LoginInfo>.ErrorResult(AuthResponse.InvalidRefreshToken());
                }

                var tokenExpiryDate = DateTime.UtcNow.AddMinutes(_configuration.JwtSettings.AccessTokenExpirationMinutes);
                var refreshExpiryDate = DateTime.UtcNow.AddDays(_configuration.JwtSettings.RefreshTokenExpirationDays);

                var newRefreshToken = GenerateRefreshToken(ipAddress);

                _refreshTokenService.RevokeTokenAndReplace(token, newRefreshToken, ipAddress);
                _refreshTokenService.Add(new RefreshTokenInsert
                {
                    UserId = refreshToken.UserId,
                    Token = newRefreshToken,
                    ExpiryDate = refreshExpiryDate,
                    CreatedByIp = ipAddress
                });

                var _userService = new UserService(context, transaction);
                var user = _userService.GetById(refreshToken.UserId);

                if (user == null)
                {
                    transaction.Rollback();
                    return ApiResponse<LoginInfo>.ErrorResult(AuthResponse.UserNotFound());
                }

                var _userRolService = new UserRolService(context, transaction);
                var roles = _userRolService.GetRolsByUserId(user.UserId).ToList();

                var newJwtToken = GenerateJwtToken(user, roles, tokenExpiryDate);

                transaction.Commit();

                return ApiResponse<LoginInfo>.SuccessResult(new LoginInfo
                {
                    Token = newJwtToken,
                    TokenExpiryDate = tokenExpiryDate,
                    RefreshToken = newRefreshToken,
                    RefreshExpiryDate = refreshExpiryDate,
                    User = user
                }, "Token refreshed successfully");
            }
        }

        public ApiResponse<LoginInfo> Register(UserInsert data, string ipAddress)
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

                var tokenExpiryDate = DateTime.UtcNow.AddMinutes(_configuration.JwtSettings.AccessTokenExpirationMinutes);
                var refreshExpiryDate = DateTime.UtcNow.AddDays(_configuration.JwtSettings.RefreshTokenExpirationDays);

                var token = GenerateJwtToken(userCreated, roles, tokenExpiryDate);
                var refreshToken = GenerateRefreshToken(ipAddress);

                var _refreshTokenService = new RefreshTokenService(context, transaction);
                _refreshTokenService.RemoveExpiredTokens();
                _refreshTokenService.Add(new RefreshTokenInsert
                {
                    UserId = userCreated.UserId,
                    Token = refreshToken,
                    ExpiryDate = refreshExpiryDate,
                    CreatedByIp = ipAddress
                });

                transaction.Commit();
                return ApiResponse<LoginInfo>.SuccessResult(new LoginInfo
                {
                    Token = token,
                    TokenExpiryDate = tokenExpiryDate,
                    RefreshToken = refreshToken,
                    RefreshExpiryDate = refreshExpiryDate,
                    User = userCreated,
                }, "Login Succes");
            }
        }

        public ApiResponse<GenericMessage> RevokeToken(string token, string ipAddress)
        {
            using (var context = new SqlConnection(_configuration.DB.DefaultConnection))
            {
                var _refreshTokenService = new RefreshTokenService(context);
                _refreshTokenService.RemoveExpiredTokens();
                var refreshToken = _refreshTokenService.GetByToken(token);

                if (refreshToken == null || !refreshToken.IsActive)
                {
                    return ApiResponse<GenericMessage>.ErrorResult(AuthResponse.InvalidRefreshToken());
                }

                var result = _refreshTokenService.RevokeToken(token, ipAddress);

                if (result)
                {
                    return ApiResponse<GenericMessage>.SuccessResult(new GenericMessage { Message = "Token revoked successfully" });
                }

                return ApiResponse<GenericMessage>.ErrorResult(AuthResponse.RevokeFailed());
            }
        }

        private string GenerateJwtToken(UserDTO user, IEnumerable<RolItem> roles, DateTime expires)
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
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(string ipAddress)
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }
}