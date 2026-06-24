using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SantaClauzer.BL.Services;
using SantaClauzer.Model.Entities;
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

        [HttpPost]
        public async Task<ActionResult<BaseResponseModel>> CreatePresentGroup(PresentGroupModel model)
        {
            await _presentGroupService.CreatePresentGroup(model);
            return Ok(new BaseResponseModel { Success = true });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModel>> GetPresentGroup(int id)
        {
            var presentGroup = await _presentGroupService.GetPresentGroup(id);
            if (presentGroup == null)
            {
                return NotFound(new BaseResponseModel { Success = false, ErrorMessage = "Present group not found." });
            }
            return Ok(new BaseResponseModel { Success = true, Data = presentGroup });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> UpdatePresentGroup(int id, PresentGroupModel model)
        {
            var existingPresentGroup = await _presentGroupService.GetPresentGroup(id);

            if (existingPresentGroup == null)
            {
                return NotFound(new BaseResponseModel { Success = false, ErrorMessage = "Present group not found." });
            }

            // Update the properties of the existing present group
            existingPresentGroup.Name = model.Name;
            existingPresentGroup.Description = model.Description;
            await _presentGroupService.UpdatePresentGroup(id, existingPresentGroup);
            return Ok(new BaseResponseModel { Success = true });
        }
    }
}
