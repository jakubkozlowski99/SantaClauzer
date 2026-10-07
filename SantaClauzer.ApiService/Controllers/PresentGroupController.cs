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

        [HttpPost("{presentGroupId}/users")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel>> InviteUserToPresentGroup(int presentGroupId, [FromBody] InviteUserRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username))
                return BadRequest(new BaseResponseModel { Success = false, ErrorMessage = "Username is required." });

            var presentGroup = await _presentGroupService.GetPresentGroup(presentGroupId);
            if (presentGroup == null)
                return NotFound(new BaseResponseModel { Success = false, ErrorMessage = "Present group not found." });

            // ensure caller is the creator
            var callerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (!int.TryParse(callerIdClaim, out var callerId))
                return Unauthorized(new BaseResponseModel { Success = false, ErrorMessage = "Unauthorized" });

            if (presentGroup.CreatorId != callerId)
                return Forbid();

            var userToInvite = await _authService.GetUserByUserNameNoPassword(request.Username);
            if (userToInvite == null)
                return NotFound(new BaseResponseModel { Success = false, ErrorMessage = "User not found." });

            var ifUserExistsInGroup = await _presentGroupUserService.CheckIfUserInPresentGroup(userToInvite.Id, presentGroupId);
            if (ifUserExistsInGroup)
                return BadRequest(new BaseResponseModel { Success = false, ErrorMessage = "User is already in the present group or invited." });

            var added = await _presentGroupUserService.AddPresentGroupUser(new PresentGroupUserModel
            {
                PresentGroupId = presentGroupId,
                UserId = userToInvite.Id,
                InvitationAccepted = false
            });

            return Ok(new BaseResponseModel { Success = true, Data = added });
        }

        [HttpGet("groups-by-user/{userId}")]
        public async Task<ActionResult<BaseResponseModel>> GetPresentGroupsByUser(int userId)
        {
            var presentGroups = await _presentGroupService.GetPresentGroupsByUser(userId);
            return Ok(new BaseResponseModel { Success = true, Data = presentGroups });
        }

        [HttpGet("invitations/{userId}")]
        public async Task<ActionResult<BaseResponseModel>> GetActiveInvitationsForUser(int userId)
        {
            var invitations = await _presentGroupUserService.GetActiveInvitationsForUser(userId);
            return Ok(new BaseResponseModel { Success = true, Data = invitations });
        }

        // Accept an invitation (only the invited user may accept)
        [HttpPost("{presentGroupId}/users/{userId}/accept")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel>> AcceptInvitation(int presentGroupId, int userId)
        {
            var callerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (!int.TryParse(callerIdClaim, out var callerId))
                return Unauthorized(new BaseResponseModel { Success = false, ErrorMessage = "Unauthorized" });

            if (callerId != userId)
                return Forbid();

            var success = await _presentGroupUserService.AcceptInvitation(presentGroupId, userId);
            if (success == null)
                return NotFound(new BaseResponseModel { Success = false, ErrorMessage = "Invitation not found." });

            return Ok(new BaseResponseModel { Success = true, Data = success });
        }

        // Decline invitation or remove membership (invited user can decline; creator or admin can remove)
        [HttpDelete("{presentGroupId}/users/{userId}")]
        [Authorize]
        public async Task<ActionResult<BaseResponseModel>> RemovePresentGroupUser(int presentGroupId, int userId)
        {
            var callerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (!int.TryParse(callerIdClaim, out var callerId))
                return Unauthorized(new BaseResponseModel { Success = false, ErrorMessage = "Unauthorized" });

            var presentGroup = await _presentGroupService.GetPresentGroup(presentGroupId);
            if (presentGroup == null)
                return NotFound(new BaseResponseModel { Success = false, ErrorMessage = "Present group not found." });

            // allow invited user to decline (caller == userId) or group creator to remove
            if (callerId != userId && presentGroup.CreatorId != callerId)
                return Forbid();

            var removed = await _presentGroupUserService.RemovePresentGroupUser(presentGroupId, userId);
            if (!removed)
                return NotFound(new BaseResponseModel { Success = false, ErrorMessage = "Invitation/membership not found." });

            return Ok(new BaseResponseModel { Success = true });
        }
    }
}
