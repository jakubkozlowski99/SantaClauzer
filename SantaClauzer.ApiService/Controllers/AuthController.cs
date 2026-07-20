using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SantaClauzer.Model.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SantaClauzer.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        [HttpPost("login")]
        public ActionResult<LoginResponseModel> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel.Username == "Admin" && loginModel.Password == "Admin" || loginModel.Username == "User" && loginModel.Password == "User")
            {
                var token = GenerateJwtToken(loginModel.Username);
                return Ok(new LoginResponseModel{ Token = token });
            }
            return null;
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, username == "Admin" ? "Admin" : "User")
            };
            string secret = configuration.GetValue<string>("Jwt:Secret");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Jwt:Issuer"),
                audience: configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
