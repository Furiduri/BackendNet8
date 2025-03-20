using GCatcode.Api.Core;
using GCatcode.Repository.DB.UserService;
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

        public AuthController(IConfiguration configuration)
            : base(configuration)
        {
            service = new UserService(configuration.GetConnectionString("BaseLine"));
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
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("KeyJWT")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}