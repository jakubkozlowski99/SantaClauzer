using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using SantaClauzer.Model.Entities;
using SantaClauzer.Model.Models;
using SantaClauzer.Web.Components.BaseComponents;
using SantaClauzer.Web.Services;

namespace SantaClauzer.Web.Components.Pages.PresentGroup
{
    public partial class DetailsPresentGroup : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        private PresentGroupModel Model { get; set; } = new();
        private bool isLoading = true;
        private bool notFound = false;

        private List<UserModel> members { get; set; } = new List<UserModel>();

        [Inject]
        public ApiClient apiClient { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private NotificationState NotificationState { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        public AppModal DeleteModal { get; set; } = default!;
        public AppModal InviteModal { get; set; } = default!;

        private string inviteUsername = string.Empty;
        private bool isCreator = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadDetails();
            await base.OnInitializedAsync();
        }

        private async Task LoadDetails()
        {
            isLoading = true;
            notFound = false;

            try
            {
                var presentGroupResult = await apiClient.GetFromJsonAsync<BaseResponseModel>($"/api/PresentGroup/{Id}");
                if (presentGroupResult != null && presentGroupResult.Success)
                {
                    Model = JsonConvert.DeserializeObject<PresentGroupModel>(presentGroupResult.Data.ToString()) ?? new PresentGroupModel();

                    // set creator object
                    if (Model != null)
                    {
                        var creatorResult = await apiClient.GetFromJsonAsync<BaseResponseModel>($"/api/User/{Model.CreatorId}");
                        if (creatorResult != null && creatorResult.Success)
                        {
                            Model.Creator = JsonConvert.DeserializeObject<UserModel>(creatorResult.Data.ToString()) ?? new UserModel();
                        }
                    }
                }
                else
                {
                    notFound = true;
                }

                var presentGroupUsersResult = await apiClient.GetFromJsonAsync<BaseResponseModel>($"/api/PresentGroup/{Id}/users");
                if (presentGroupUsersResult != null && presentGroupUsersResult.Success)
                {
                    members = JsonConvert.DeserializeObject<List<UserModel>>(presentGroupUsersResult.Data.ToString()) ?? new List<UserModel>();
                }

                // determine if current user is creator
                try
                {
                    var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    var user = authState.User;
                    if (user?.Identity?.IsAuthenticated == true)
                    {
                        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;
                        if (int.TryParse(userIdClaim, out var currentUserId))
                        {
                            isCreator = (Model != null && Model.CreatorId == currentUserId);
                        }
                    }
                }
                catch
                {
                    isCreator = false;
                }
            }
            catch
            {
                notFound = true;
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private void ShowDeleteModal()
        {
            DeleteModal.OpenModal();
        }

        private async Task ConfirmDelete()
        {
            var res = await apiClient.DeleteAsync<BaseResponseModel>($"/api/PresentGroup/{Id}");
            if (res != null && res.Success)
            {
                NotificationState.SetSuccess("Present group deleted successfully.");
                DeleteModal.CloseModal();
                NavigationManager.NavigateTo("/present-groups");
            }
            else
            {
                NotificationState.SetError(res?.ErrorMessage ?? "Failed to delete present group.");
            }
        }

        private void ShowInviteModal()
        {
            inviteUsername = string.Empty;
            InviteModal.OpenModal();
        }

        private async Task SendInvite()
        {
            if (string.IsNullOrWhiteSpace(inviteUsername))
            {
                NotificationState.SetError("Please enter a username.");
                return;
            }

            var req = new { Username = inviteUsername };
            var res = await apiClient.PostAsync<BaseResponseModel, object>($"/api/PresentGroup/{Id}/users", req);

            if (res != null && res.Success)
            {
                NotificationState.SetSuccess("Invitation sent.");
                InviteModal.CloseModal();
                // refresh members
                await LoadDetails();
            }
            else
            {
                NotificationState.SetError(res?.ErrorMessage ?? "User with this username couldn't be found.");
            }
        }
    }
}


