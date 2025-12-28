using GCatcode.Api.Core.Auth;
using GCatcode.Repository.DB.RolServices;
using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GCatcode.Api.Core
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        public AuthController(IConfiguration configuration, IUserService userService, IUserRolService userRolService)
            : base(configuration, userService, userRolService)
        {
        }

        [Authorize]
        [HttpGet("user_info")]
        public IActionResult GetUserInfo()
        {
            int userId = GetUserId();
            var user = _userService.GetById(userId);
            if (user == null)
                return NotFound();

            return Ok(new Response<object>
            {
                Data = new
                {
                    email = user.Email,
                    username = user.UserName,
                    id = userId,
                    roles = GetRoles().Select(r => r.Name)
                }
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            UserDTO userDto = _userService.GetByUserName(user.Username);
            if (userDto == null)
            {
                return Unauthorized();
            }

            if (_userService.ValidPassword(user))
            {
                var roles = _userRolService.GetRolsByUserId(userDto.UserId).ToList();
                var token = GenerateJwtToken(userDto, roles);
                return Ok(new Response<LoginResponse>
                {
                    Msg = "Ok",
                    Data = new LoginResponse
                    {
                        Token = token,
                        User = userDto
                    }
                });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserInsert user)
        {
            try
            {
                var userCreated = _userService.Add(user);
                if (userCreated == null)
                {
                    return BadRequest("User creation failed.");
                }

                var roles = _userRolService.GetRolsByUserId(userCreated.UserId).ToList();
                var token = GenerateJwtToken(userCreated, roles);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJwtToken(UserDTO user, List<RolItem> roles)
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}