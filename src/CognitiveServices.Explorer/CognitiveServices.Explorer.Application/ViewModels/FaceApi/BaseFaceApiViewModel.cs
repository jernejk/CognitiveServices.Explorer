using CognitiveServices.Explorer.Application.Commands;
using CognitiveServices.Explorer.Application.Curl;
using CognitiveServices.Explorer.Domain.Face;
using Flurl.Http;
using MediatR;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.ViewModels.FaceApi
{
    public abstract class BaseFaceApiViewModel
    {
        protected readonly ICognitiveServicesConfigService _csConfigService;
        private readonly IMediator _mediator;
        //protected readonly HttpRequestService _httpRequestService = new HttpRequestService();
        protected readonly CurlGenerator _curlGenerator = new CurlGenerator();
        protected CognitiveServiceConfig? _faceApiConfig = null;

        public BaseFaceApiViewModel(ICognitiveServicesConfigService csConfigService, IMediator mediator)
        {
            _csConfigService = csConfigService;
            _mediator = mediator;
        }

        public string RawJson { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public CognitiveServiceConfig? FaceApiConfig => _faceApiConfig;

        public virtual async Task OnInitializedAsync()
        {
            await LoadLatestConfig().ConfigureAwait(false);
        }

        public async Task LoadLatestConfig()
        {
            _faceApiConfig = await _csConfigService.GetConfig("FaceApi").ConfigureAwait(false);
        }

        protected async Task<T?> MakeRequest<T>(HttpRequest? request)
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

                RawJson = await _mediator.Send(new ExecuteCognitiveServicesCommand(request, _faceApiConfig)).ConfigureAwait(false) ?? string.Empty;
                //RawJson = await _httpRequestService.Send(request, _faceApiConfig).ConfigureAwait(false) ?? string.Empty;
                return JsonSerializer.Deserialize<T>(RawJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (FlurlHttpException fe)
            {
                try
                {
                    ErrorDto e = await fe.GetResponseJsonAsync<ErrorDto>();
                    if (e?.Error != null)
                    {
                        Error = $"Face API error code {e.Error.Code}: \n{e.Error.Message}";
                    }
                }
                catch
                {
                }

                if (string.IsNullOrWhiteSpace(Error))
                {
                    Error = fe.Message;
                }

                return default;
            }
            catch (Exception e)
            {
                Error = e.ToString();
                return default!;
            }
        }
    }
}
