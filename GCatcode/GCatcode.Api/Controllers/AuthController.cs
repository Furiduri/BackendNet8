using GCatcode.Api.Core;
using GCatcode.Repository.DB.UserRolService;
using GCatcode.Repository.DB.UserService;
using GCatcode.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GCatcode.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly UserService service;
        private readonly UserRolService userRolService;

        public AuthController(IConfiguration configuration)
            : base(configuration)
        {
            service = new UserService(Settings.GetBaseDBConnection(_configuration));
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