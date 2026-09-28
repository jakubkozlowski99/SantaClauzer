using Microsoft.AspNetCore.Mvc;
using SantaClauzer.BL.Services;
using SantaClauzer.Model.Models;

namespace SantaClauzer.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            var user = await _authService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { Success = false, ErrorMessage = "User not found." });
            }
            return Ok(new BaseResponseModel{ Success = true, Data = user });
        }
    }
}
