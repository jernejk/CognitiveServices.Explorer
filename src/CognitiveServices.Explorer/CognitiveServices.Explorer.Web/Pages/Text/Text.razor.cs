using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.Pages.Text
{
    public partial class Text
    {
        protected override async Task OnInitializedAsync() => await viewModel.OnInitializedAsync();

        public async Task UpdatedProfile()
        {
            // NOTE: This will update a few variables that are not caught but Blazor.
            await viewModel.LoadLatestConfig();
            StateHasChanged();
        }
    }
}
