using Blazor.FileReader;
using Microsoft.AspNetCore.Components;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.Pages.Forms
{
    public partial class Analyze
    {
        protected override async Task OnInitializedAsync() => await viewModel.OnInitializedAsync();

        public ElementReference InputTypeFileElement { get; set; }
        public string? ImageUrl { get; set; }
        public string OpenedTab { get; set; } = string.Empty;

        public bool ImageUrlTabOpened
        {
            get { return OpenedTab == "imageUrl"; }
            set
            {
                if (value)
                {
                    OpenedTab = "imageUrl";
                    this.StateHasChanged();
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
                    this.StateHasChanged();
                }
            }
        }

        public async Task UpdatedProfile()
        {
            // NOTE: This will update a few variables that are not caught but Blazor.
            await viewModel.LoadLatestConfig();
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

                await viewModel.Analyze(ImageUrl);
            }
            else if (OpenedTab == "imageUpload")
            {
                var imageData = await ReadFile();
                if (imageData == null)
                {
                    viewModel.Error = "Unable to load the image.";
                    return;
                }

                await viewModel.Analyze(imageData);
            }
        }

        private async Task<byte[]?> ReadFile()
        {
            try
            {
                foreach (var file in await fileReaderService.CreateReference(InputTypeFileElement).EnumerateFilesAsync())
                {
                    int offset = 0;
                    var buffer = new byte[4096];

                    // Read into memory and act
                    using (MemoryStream memoryStream = await file.CreateMemoryStreamAsync(4096))
                    {
                        // Read into buffer and act (uses less memory)
                        using (Stream stream = await file.OpenReadAsync())
                        {
                            // Do (async) stuff with stream...
                            await stream.ReadAsync(buffer, 0, buffer.Length);
                            memoryStream.Write(buffer, offset, buffer.Length);

                            offset += buffer.Length;
                        }

                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}
