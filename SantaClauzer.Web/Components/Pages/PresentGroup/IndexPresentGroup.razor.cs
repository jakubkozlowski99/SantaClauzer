using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using SantaClauzer.Model.Entities;
using SantaClauzer.Model.Models;
using SantaClauzer.Web.Components.BaseComponents;
using SantaClauzer.Web.Services;

namespace SantaClauzer.Web.Components.Pages.PresentGroup
{
    public partial class IndexPresentGroup : ComponentBase
    {
        [Inject]
        public ApiClient apiClient { get; set; } = default!;
        [Inject]
        private NotificationState NotificationState { get; set; } = default!;
        public List<PresentGroupModel> PresentGroupModels { get; set; } = new List<PresentGroupModel>();
        public AppModal AppModal { get; set; }
        public int DeleteId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadPresentGroups();
            await base.OnInitializedAsync();
        }

        protected async Task LoadPresentGroups()
        {
            var res = await apiClient.GetFromJsonAsync<BaseResponseModel>("/api/PresentGroup");
            if (res != null && res.Success)
            {
                PresentGroupModels = JsonConvert.DeserializeObject<List<PresentGroupModel>>(res.Data.ToString()) ?? new List<PresentGroupModel>();
            }
        }

        protected async Task HandleDelete()
        {
            var res = await apiClient.DeleteAsync<BaseResponseModel>($"/api/PresentGroup/{DeleteId}");
            if (res != null && res.Success)
            {
                NotificationState.SetSuccess("Present group deleted successfully.");
                AppModal.CloseModal();
                await LoadPresentGroups();
                StateHasChanged();
            }
        }
    }
}
