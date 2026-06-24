using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using SantaClauzer.Model.Entities;
using SantaClauzer.Model.Models;
using SantaClauzer.Web.Services;

namespace SantaClauzer.Web.Components.Pages.PresentGroup
{
    public partial class UpdatePresentGroup : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        public PresentGroupModel Model { get; set; } = new();

        [Inject]
        private ApiClient apiClient { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private NotificationState NotificationState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var res = await apiClient.GetFromJsonAsync<BaseResponseModel>($"/api/PresentGroup/{Id}");
            if (res != null && res.Success)
            {
                Model = JsonConvert.DeserializeObject<PresentGroupModel>(res.Data.ToString()) ?? new PresentGroupModel();
            }
            await base.OnInitializedAsync();
        }

        public async Task Submit()
        {
            var res = await apiClient.PutAsync<BaseResponseModel, PresentGroupModel>($"/api/PresentGroup/{Id}", Model);
            if (res != null && res.Success)
            {
                Console.WriteLine("UpdatePresentGroup: success, setting toast and navigating.");
                NotificationState.SetSuccess("Present group updated successfully.");
                NavigationManager.NavigateTo("/present-groups");
            }
            else
            {
                Console.WriteLine("UpdatePresentGroup: failed, setting error toast.");
                NotificationState.SetInfo("Failed to update present group.");
            }
        }
    }
}
