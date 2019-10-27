using CognitiveServices.Explorer.Application;
using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Domain.Face;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.ViewModels.FaceApi
{
    public class PersonGroupsPersonViewModel : BaseFaceApiViewModel
    {
        private HttpRequest? _getPersonsRequest;
        private HttpRequest? _getGroupTrainStatusRequest;
        private HttpRequest? _trainRequest;

        public PersonGroupsPersonViewModel(ICognitiveServicesConfigService csConfigService)
            : base(csConfigService)
        {
        }

        public string? PersonGroupId { get; set; }
        public List<PersonDto>? People { get; private set; }
        public TrainingStatus? TrainingStatus { get; set; }
        public List<HttpRequest> Requests { get; } = new List<HttpRequest>();

        public async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(PersonGroupId))
            {
                _getPersonsRequest = PersonGroupPersonRequestGenerator.List(PersonGroupId!);
                _getGroupTrainStatusRequest = PersonGroupRequestGenerator.CheckTraining(PersonGroupId!);
                _trainRequest = PersonGroupRequestGenerator.Train(PersonGroupId!);

                Requests.Add(_getPersonsRequest);
                Requests.Add(_getGroupTrainStatusRequest);
                Requests.Add(_trainRequest);
            }
            else
            {
                Error = "Person group ID not set!";
            }
        }

        public async Task GetPeople()
        {
            People = await MakeRequest<List<PersonDto>>(_getPersonsRequest).ConfigureAwait(false);
        }

        public async Task RefreshTrainingStatus()
        {
            TrainingStatus = await MakeRequest<TrainingStatus>(_getGroupTrainStatusRequest).ConfigureAwait(false);
        }

        public async Task Train()
        {
            TrainingStatus = await MakeRequest<TrainingStatus>(_trainRequest).ConfigureAwait(false);
        }
    }
}
