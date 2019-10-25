using CognitiveServices.Explorer.Application;
using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Domain.Face;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.ViewModels.FaceApi
{
    public class PersonGroupViewModel : BaseFaceApiViewModel
    {
        private readonly PersonGroupCurlGenerator _generator = new PersonGroupCurlGenerator();
        private readonly CurlRequestData _getPeopleRequest = new CurlRequestData();
        private readonly CurlRequestData _getTrainingStatusRequest = new CurlRequestData();
        private readonly CurlRequestData _getTrainRequest = new CurlRequestData();

        public PersonGroupViewModel(ICognitiveServicesConfigService csConfigService) : base(csConfigService)
        {
            _getPeopleRequest.RequestName = "GET PersonGroup Person list";
            _getTrainingStatusRequest.RequestName = "GET PersonGroup Training status";
            _getTrainRequest.RequestName = "POST PersonGroup Start training";

            RequestsData.Add(_getPeopleRequest);
            RequestsData.Add(_getTrainingStatusRequest);
            RequestsData.Add(_getTrainRequest);
        }

        public string? PersonGroupId { get; set; }
        public List<PersonDto>? People { get; private set; }
        public TrainingStatus? TrainingStatus { get; set; }
        public List<CurlRequestData> RequestsData { get; } = new List<CurlRequestData>();

        public async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        public async Task GetPeople()
        {
            //faceApiKey = await localStorage.GetItemAsync<string>(nameof(faceApiKey));
            //faceApiBaseUrl = await localStorage.GetItemAsync<string>(nameof(faceApiBaseUrl));

            //people = await MakeRequest<List<PersonDto>>($"/face/v1.0/persongroups/{PersonGroupId}/persons");

            var request = _generator.List();
            People = await MakeRequest<List<PersonDto>>(request).ConfigureAwait(false);
        }

        public async Task RefreshTrainingStatus()
        {
            var request = _generator.CheckTraining(PersonGroupId);
            TrainingStatus = await MakeRequest<TrainingStatus>(request).ConfigureAwait(false);
        }

        public async Task Train()
        {
            var request = _generator.Train(PersonGroupId);
            TrainingStatus = await MakeRequest<TrainingStatus>(request).ConfigureAwait(false);
        }
    }
}
