using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using SantaClauzer.BL.Services;
using SantaClauzer.Model.Entities;
using SantaClauzer.Model.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using System.Security.Claims;

namespace SantaClauzer.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresentGroupController : ControllerBase
    {
        private readonly IPresentGroupService _presentGroupService;
        private readonly IAuthService _authService;
        private readonly IPresentGroupUserService _presentGroupUserService;

        public PresentGroupController(IPresentGroupService presentGroupService, IAuthService authService, IPresentGroupUserService presentGroupUserService)
        {
            _presentGroupService = presentGroupService;
            _authService = authService;
            _presentGroupUserService = presentGroupUserService;
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

        // require auth and populate CreatorId from JWT
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel>> CreatePresentGroup(PresentGroupModel model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                model.CreatorId = userId;
            }

            await _presentGroupService.CreatePresentGroup(model);
            await _presentGroupUserService.AddPresentGroupUser(new PresentGroupUserModel
            {
                UserId = model.CreatorId,
                PresentGroupId = model.Id,
                InvitationAccepted = true
            });

            return Ok(new BaseResponseModel { Success = true, Data = model });
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponseModel>> DeletePresentGroup(int id)
        {
            var existingPresentGroup = await _presentGroupService.GetPresentGroup(id);
            if (existingPresentGroup == null)
            {
                return NotFound(new BaseResponseModel { Success = false, ErrorMessage = "Present group not found." });
            }
            await _presentGroupService.DeletePresentGroup(id);
            return Ok(new BaseResponseModel { Success = true });
        }

        [HttpGet("{presentGroupId}/users")]
        public async Task<ActionResult<BaseResponseModel>> GetUsersInPresentGroup(int presentGroupId)
        {
            var users = await _presentGroupUserService.GetUsersInPresentGroup(presentGroupId);
            return Ok(new BaseResponseModel { Success = true, Data = users });
        }
    }
}
