using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using SantaClauzer.Model.Entities;
using SantaClauzer.Model.Models;

namespace SantaClauzer.Web.Components.Pages.PresentGroup
{
    public partial class IndexPresentGroup : ComponentBase
    {
        [Inject]
        public ApiClient apiClient { get; set; } = default!;
        public List<PresentGroupModel> PresentGroupModels { get; set; } = new List<PresentGroupModel>();

        protected override async Task OnInitializedAsync()
        {
            var res = await apiClient.GetFromJsonAsync<BaseResponseModel>("/api/PresentGroup");
            if (res != null && res.Success)
            {
                PresentGroupModels = JsonConvert.DeserializeObject<List<PresentGroupModel>>(res.Data.ToString()) ?? new List<PresentGroupModel>();
            }

            await base.OnInitializedAsync();
        }
    }
}
