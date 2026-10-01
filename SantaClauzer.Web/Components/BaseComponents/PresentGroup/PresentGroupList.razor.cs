using Microsoft.AspNetCore.Components;
using SantaClauzer.Model.Entities;

namespace SantaClauzer.Web.Components.BaseComponents.PresentGroup
{
    public partial class PresentGroupList : ComponentBase
    {
        [Parameter]
        public IEnumerable<PresentGroupModel> Groups { get; set; } = Enumerable.Empty<PresentGroupModel>();

        [Parameter]
        public EventCallback<int> OnDeleteRequested { get; set; }

        [Parameter]
        public string CreateUrl { get; set; } = "/present-groups/create";

        [Parameter]
        public bool ShowCreateButton { get; set; } = true;
    }
}
