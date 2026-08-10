using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SantaClauzer.BL.Services;
using SantaClauzer.Model.Entities;
using SantaClauzer.Model.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SantaClauzer.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration, IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginModel loginModel)
        {
            var user = await authService.GetUserByLogin(loginModel.Username, loginModel.Password);
            if (user != null)
            {
                var token = GenerateJwtToken(user, isRefreshToken:false);
                var refreshToken = GenerateJwtToken(user, isRefreshToken:true);

                await authService.AddRefreshTokenModel(new RefreshTokenModel
                {
                    RefreshToken = refreshToken,
                    UserId = user.Id
                });

                return Ok(new LoginResponseModel{
                    Token = token ,
                    RefreshToken = refreshToken,
                    TokenExpired = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()
                });
            }
            return null;
        }

        [HttpGet("loginByRefreshToken")]
        public async Task<ActionResult<LoginResponseModel>> LoginByRefreshToken([FromQuery] string refreshToken)
        {
            var refreshTokenModel = await authService.GetRefreshTokenModel(refreshToken);
            if (refreshTokenModel == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var newToken = GenerateJwtToken(refreshTokenModel.User, isRefreshToken:false);
            var newRefreshToken = GenerateJwtToken(refreshTokenModel.User, isRefreshToken:true);

            await authService.AddRefreshTokenModel(new RefreshTokenModel
            {
                RefreshToken = newRefreshToken,
                UserId = refreshTokenModel.UserId
            });

            return new LoginResponseModel
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                TokenExpired = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()
            };
        }

        private string GenerateJwtToken(UserModel user, bool isRefreshToken)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            claims.AddRange(user.UserRoles.Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name)));

            string secret = configuration.GetValue<string>($"Jwt:{(isRefreshToken ? "RefreshTokenSecret" : "Secret")}");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Jwt:Issuer"),
                audience: configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddHours(isRefreshToken ? 24 : 1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
