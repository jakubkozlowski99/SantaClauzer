using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using SantaClauzer.Model.Entities;
using SantaClauzer.Model.Models;
using SantaClauzer.Web.Services;

namespace SantaClauzer.Web.Components.Pages.User
{
    public partial class Invitations : ComponentBase
    {
        [Inject]
        public ApiClient apiClient { get; set; } = default!;

        public List<PresentGroupUserModel> InvitationsList { get; set; } = new List<PresentGroupUserModel>();

        [Inject]
        private NotificationState NotificationState { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        private bool isLoading = true;
        private bool notFound = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadInvitations();
            await base.OnInitializedAsync();
        }

        protected async Task LoadInvitations()
        {
            isLoading = true;
            notFound = false;
            InvitationsList.Clear();

            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user?.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;
                    if (!int.TryParse(userIdClaim, out var userId))
                    {
                        notFound = true;
                        return;
                    }

                    // NOTE: controller exposes GET /api/PresentGroup/invitations/{userId}
                    var invitationResult = await apiClient.GetFromJsonAsync<BaseResponseModel>($"/api/PresentGroup/invitations/{userId}");
                    if (invitationResult != null && invitationResult.Success)
                    {
                        InvitationsList = JsonConvert.DeserializeObject<List<PresentGroupUserModel>>(invitationResult.Data.ToString()) ?? new List<PresentGroupUserModel>();
                    }
                    else
                    {
                        notFound = true;
                    }
                    return;
                }

                notFound = true;
            }
            catch (Exception ex)
            {
                notFound = true;
                Console.WriteLine($"Error loading invitations: {ex.Message}");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        protected async Task AcceptInvite(int presentGroupId, int userId)
        {
            try
            {
                // POST to accept invitation. Controller verifies caller matches userId.
                var res = await apiClient.PostAsync<BaseResponseModel, object>($"/api/PresentGroup/{presentGroupId}/users/{userId}/accept", null);
                if (res != null && res.Success)
                {
                    NotificationState.SetSuccess("Invitation accepted.");
                    await LoadInvitations();
                }
                else
                {
                    NotificationState.SetError(res?.ErrorMessage ?? "Failed to accept invitation.");
                }
            }
            catch (Exception ex)
            {
                NotificationState.SetError($"Failed to accept invitation: {ex.Message}");
            }
        }

        protected async Task DeclineInvite(int presentGroupId, int userId)
        {
            try
            {
                var res = await apiClient.DeleteAsync<BaseResponseModel>($"/api/PresentGroup/{presentGroupId}/users/{userId}");
                if (res != null && res.Success)
                {
                    NotificationState.SetSuccess("Invitation declined.");
                    await LoadInvitations();
                }
                else
                {
                    NotificationState.SetError(res?.ErrorMessage ?? "Failed to decline invitation.");
                }
            }
            catch (Exception ex)
            {
                NotificationState.SetError($"Failed to decline invitation: {ex.Message}");
            }
        }
    }
}
