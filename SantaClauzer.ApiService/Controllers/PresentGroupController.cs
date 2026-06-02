using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SantaClauzer.BL.Services;
using SantaClauzer.Model.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace SantaClauzer.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresentGroupController : ControllerBase
    {
        private readonly IPresentGroupService _presentGroupService;

        public PresentGroupController(IPresentGroupService presentGroupService)
        {
            _presentGroupService = presentGroupService;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModel>> GetPresentGroups()
        {
            var presentGroups = await _presentGroupService.GetPresentGroups();
            return Ok(new BaseResponseModel
            {
                Success = true,
                Data = presentGroups
            });
        }
    }
}
