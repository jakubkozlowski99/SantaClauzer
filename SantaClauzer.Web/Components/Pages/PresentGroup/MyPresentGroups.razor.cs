using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using SantaClauzer.Model.Entities;
using SantaClauzer.Model.Models;
using SantaClauzer.Web.Components.BaseComponents;
using SantaClauzer.Web.Services;

namespace SantaClauzer.Web.Components.Pages.PresentGroup
{
    public partial class MyPresentGroups : ComponentBase
    {
        [Inject]
        public ApiClient apiClient { get; set; } = default!;

        [Inject]
        private NotificationState NotificationState { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        public List<PresentGroupModel> PresentGroupModels { get; set; } = new List<PresentGroupModel>();
        public AppModal AppModal { get; set; } = default!;
        public int DeleteId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadMyPresentGroups();
            await base.OnInitializedAsync();
        }

        private async Task LoadMyPresentGroups()
        {
            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user?.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                      ?? user.FindFirst("sub")?.Value;

                    if (int.TryParse(userIdClaim, out var userId))
                    {
                        // call the controller endpoint that returns groups for a given user
                        var res = await apiClient.GetFromJsonAsync<BaseResponseModel>($"/api/PresentGroup/groups-by-user/{userId}");
                        if (res != null && res.Success)
                        {
                            PresentGroupModels = JsonConvert.DeserializeObject<List<PresentGroupModel>>(res.Data.ToString())
                                ?? new List<PresentGroupModel>();
                        }

                        return;
                    }
                }

                // Not authenticated or no id claim: show empty list (or adjust behavior as needed)
                PresentGroupModels = new List<PresentGroupModel>();
            }
            catch (Exception)
            {
                NotificationState.SetError("Failed to load your present groups.");
                PresentGroupModels = new List<PresentGroupModel>();
            }
            finally
            {
                StateHasChanged();
            }
        }

        protected void HandleDeleteRequested(int id)
        {
            DeleteId = id;
            AppModal.OpenModal();
        }

        protected async Task HandleDelete()
        {
            var res = await apiClient.DeleteAsync<BaseResponseModel>($"/api/PresentGroup/{DeleteId}");
            if (res != null && res.Success)
            {
                NotificationState.SetSuccess("Present group deleted successfully.");
                AppModal.CloseModal();
                await LoadMyPresentGroups();
                StateHasChanged();
            }
        }
    }
}