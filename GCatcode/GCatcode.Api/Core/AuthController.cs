using GCatcode.Repository.DB.UserRolServices;
using GCatcode.Repository.DB.UserServices;
using GCatcode.Utils;
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
        private readonly UserService service;

        public AuthController(IConfiguration configuration)
            : base(configuration)
        {
            service = new UserService(_connection);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            UserDTO userDto = service.GetUserByUserName(user.Username);
            if (userDto == null)
            {
                return Unauthorized();
            }
            if (service.ValidPassword(user))
            {
                
                var token = GenerateJwtToken(userDto);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserInsert user)
        {            
            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    var userCreated = service.Insert(user);
                    if(userCreated == null)
                    {
                        return BadRequest("User creation failed.");
                    }

                    var token = GenerateJwtToken(userCreated);
                    return Ok(new { token });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
        }  

        private string GenerateJwtToken(UserDTO user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

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