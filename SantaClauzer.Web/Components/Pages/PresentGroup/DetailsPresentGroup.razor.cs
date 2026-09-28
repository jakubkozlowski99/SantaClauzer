using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using SantaClauzer.Model.Entities;
using SantaClauzer.Model.Models;
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

        [Inject]
        public ApiClient apiClient { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

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
                var res = await apiClient.GetFromJsonAsync<BaseResponseModel>($"/api/PresentGroup/{Id}");
                if (res != null && res.Success)
                {
                    Model = JsonConvert.DeserializeObject<PresentGroupModel>(res.Data.ToString()) ?? new PresentGroupModel();

                    if (Model != null && Model.CreatorId != null)
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
    }
}
