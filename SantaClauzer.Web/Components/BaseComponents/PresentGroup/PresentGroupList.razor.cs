using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SantaClauzer.Model.Entities;

namespace SantaClauzer.Web.Components.BaseComponents.PresentGroup
{
    public partial class PresentGroupList : ComponentBase, IDisposable
    {
        [Parameter]
        public IEnumerable<PresentGroupModel> Groups { get; set; } = Enumerable.Empty<PresentGroupModel>();

        [Parameter]
        public EventCallback<int> OnDeleteRequested { get; set; }

        [Parameter]
        public string CreateUrl { get; set; } = "/present-groups/create";

        [Parameter]
        public bool ShowCreateButton { get; set; } = true;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        // exposes current user id for the razor view to decide creator-specific UI
        public int? CurrentUserId { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            // subscribe to auth changes so the list updates when user signs in/out
            AuthenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
            await UpdateCurrentUserAsync();
            await base.OnInitializedAsync();
        }

        private async Task UpdateCurrentUserAsync()
        {
            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user?.Identity?.IsAuthenticated == true)
                {
                    var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;
                    if (int.TryParse(idClaim, out var id))
                        CurrentUserId = id;
                    else
                        CurrentUserId = null;
                }
                else
                {
                    CurrentUserId = null;
                }
            }
            catch
            {
                CurrentUserId = null;
            }
        }

        private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            try
            {
                var authState = await task;
                var user = authState.User;
                if (user?.Identity?.IsAuthenticated == true)
                {
                    var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;
                    if (int.TryParse(idClaim, out var id))
                        CurrentUserId = id;
                    else
                        CurrentUserId = null;
                }
                else
                {
                    CurrentUserId = null;
                }
            }
            catch
            {
                CurrentUserId = null;
            }

            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            AuthenticationStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}
