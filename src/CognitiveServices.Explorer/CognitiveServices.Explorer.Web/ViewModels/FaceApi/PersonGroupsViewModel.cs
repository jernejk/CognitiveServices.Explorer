using CognitiveServices.Explorer.Application;
using CognitiveServices.Explorer.Application.Curl;
using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Domain.Face;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.ViewModels.FaceApi
{
    public class PersonGroupsViewModel
    {
        private readonly ICognitiveServicesConfigService _csConfigService;
        private readonly PersonGroupCurlGenerator _generator = new PersonGroupCurlGenerator();
        private readonly HttpRequestService _httpRequestService = new HttpRequestService();
        private readonly CurlGenerator _curlGenerator = new CurlGenerator();

        public PersonGroupsViewModel(ICognitiveServicesConfigService csConfigService)
        {
            RequestData = new CurlRequestData
            {
                RequestName = "GET PersonGroup list"
            };
            _csConfigService = csConfigService;
        }

        public string RawJson { get; set; } = string.Empty;
        public List<PersonGroupDto>? PersonGroups { get; set; }
        public CurlRequestData RequestData { get; set; }
        public string Error { get; set; } = string.Empty;

        public async Task OnInitializedAsync()
        {
            var request = _generator.List();
            var config = await _csConfigService.GetConfig("FaceApi");
            RequestData.Curl = _curlGenerator.Generate(request, config, false);
        }

        public async Task GetGroups()
        {
            Error = string.Empty;

            var request = _generator.List();
            var config = await _csConfigService.GetConfig("FaceApi");
            RequestData.Curl = _curlGenerator.Generate(request, config, false);

            try
            {
                RawJson = await _httpRequestService.Send(request, config) ?? string.Empty;
                PersonGroups = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonGroupDto>?>(RawJson);
            }
            catch (Exception e)
            {
                Error = e.ToString();
            }
        }
    }
}
