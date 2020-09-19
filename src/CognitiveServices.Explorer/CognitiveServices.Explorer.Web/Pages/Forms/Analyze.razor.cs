using CognitiveServices.Explorer.Web.Models;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.Pages.Forms
{
    public partial class Analyze
    {
        protected override async Task OnInitializedAsync() => await viewModel.OnInitializedAsync();
        public bool IsAnalyzing { get; set; }

        public string? ImageUrl { get; set; }
        public string OpenedTab { get; set; } = string.Empty;
        public FileUploadViewModel? UploadedFile { get; set; }

        public bool ImageUrlTabOpened
        {
            get { return OpenedTab == "imageUrl"; }
            set
            {
                if (value)
                {
                    OpenedTab = "imageUrl";
                    StateHasChanged();
                }
            }
        }

        public bool ImageUploadTabOpened
        {
            get { return OpenedTab == "imageUpload"; }
            set
            {
                if (value)
                {
                    OpenedTab = "imageUpload";
                    StateHasChanged();
                }
            }
        }

        public async Task UpdatedProfile()
        {
            // NOTE: This will update a few variables that are not caught but Blazor.
            await viewModel.OnInitializedAsync();

            StateHasChanged();
        }

        public async Task AnalyzeForm()
        {
            if (OpenedTab == "imageUrl")
            {
                if (string.IsNullOrWhiteSpace(ImageUrl))
                {
                    viewModel.Error = "Image URL can't be empty.";
                    return;
                }

                IsAnalyzing = true;
                StateHasChanged();

                await viewModel.Analyze(ImageUrl);
            }
            else if (OpenedTab == "imageUpload")
            {
                if (UploadedFile?.Data == null)
                {
                    viewModel.Error = "Unable to load the image.";
                    return;
                }

                IsAnalyzing = true;
                StateHasChanged();

                await viewModel.Analyze(UploadedFile.Data);
            }

            IsAnalyzing = false;
        }
    }
}
