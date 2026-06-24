using Microsoft.AspNetCore.Components;
using SantaClauzer.Model.Entities;
using SantaClauzer.Model.Models;
using SantaClauzer.Web.Services;

namespace SantaClauzer.Web.Components.Pages.PresentGroup
{
    public partial class CreatePresentGroup : ComponentBase
    {
        public PresentGroupModel Model { get; set; } = new();

        [Inject]
        public ApiClient apiClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private NotificationState NotificationState { get; set; }

        public async Task Submit()
        {
            var res = await apiClient.PostAsync<BaseResponseModel, PresentGroupModel>("api/PresentGroup", Model);

            if (res != null && res.Success)
            {
                Console.WriteLine("CreatePresentGroup: success, setting toast and navigating.");
                NotificationState.SetSuccess("Present group created successfully.");
                NavigationManager.NavigateTo("/present-groups");
            }
            else
            {
                Console.WriteLine("CreatePresentGroup: failed, setting error toast.");
                NotificationState.SetInfo("Failed to create present group.");
            }
            
        }
    }
}
