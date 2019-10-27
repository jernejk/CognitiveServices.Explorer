using CognitiveServices.Explorer.Application;
using CognitiveServices.Explorer.Application.Curl;
using CognitiveServices.Explorer.Domain.Face;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.ViewModels.FaceApi
{
    public abstract class BaseFaceApiViewModel
    {
        protected readonly ICognitiveServicesConfigService _csConfigService;
        protected readonly HttpRequestService _httpRequestService = new HttpRequestService();
        protected readonly CurlGenerator _curlGenerator = new CurlGenerator();
        protected CognitiveServiceConfig? _faceApiConfig = null;

        public BaseFaceApiViewModel(ICognitiveServicesConfigService csConfigService)
        {
            _csConfigService = csConfigService;
        }

        public string RawJson { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public CognitiveServiceConfig? FaceApiConfig => _faceApiConfig;

        public virtual async Task OnInitializedAsync()
        {
            await LoadLatestConfig().ConfigureAwait(false);
        }

        protected async Task LoadLatestConfig()
        {
            _faceApiConfig = await _csConfigService.GetConfig("FaceApi").ConfigureAwait(false);
        }

        protected async Task<T> MakeRequest<T>(HttpRequest? request)
            where T : class
        {
            Error = string.Empty;
            if (request == null)
            {
                Error = "Request is not set!";
                return default!;
            }

            if (_faceApiConfig == null)
            {
                Error = "Face API configuration is not set\n";
                return default!;
            }

            try
            {
                await LoadLatestConfig().ConfigureAwait(false);

                RawJson = await _httpRequestService.Send(request, _faceApiConfig).ConfigureAwait(false) ?? string.Empty;
                return JsonConvert.DeserializeObject<T>(RawJson);
            }
            catch (Exception e)
            {
                Error = e.ToString();
                return default!;
            }
        }
    }
}
