using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Domain.Face;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.ViewModels.FaceApi
{
    public class PersonGroupsPersonViewModel : BaseFaceApiViewModel
    {
        private HttpRequest? _getPeopleRequest;
        private HttpRequest? _createPersonRequest;
        private HttpRequest? _updatePersonRequest;
        private HttpRequest? _deletePersonRequest;
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

            if (string.IsNullOrWhiteSpace(PersonGroupId))
            {
                Error = "Person group ID not set!";

                // Generate generic requests for demo purposes.
                _getPeopleRequest = PersonGroupPersonRequestGenerator.List("group-id");
                _createPersonRequest = PersonGroupPersonRequestGenerator.Create("group-id", "Person's name", "{\"custom\":\"data\"}");
                _updatePersonRequest = PersonGroupPersonRequestGenerator.Update("group-id", Guid.Empty.ToString(), "Person's name", "{\"custom\":\"data\"}");
                _deletePersonRequest = PersonGroupPersonRequestGenerator.Delete("group-id", Guid.Empty.ToString());
                _getGroupTrainStatusRequest = PersonGroupRequestGenerator.CheckTraining("group-id");
                _trainRequest = PersonGroupRequestGenerator.Train("group-id");

                RefreshRequests();
                return;
            }

            string groupId = PersonGroupId!;
            _getPeopleRequest = PersonGroupPersonRequestGenerator.List(groupId);
            _createPersonRequest = PersonGroupPersonRequestGenerator.Create(groupId, "Person's name", "{\"custom\":\"data\"}");
            _updatePersonRequest = PersonGroupPersonRequestGenerator.Update(groupId, Guid.Empty.ToString(), "Person's name", "{\"custom\":\"data\"}");
            _deletePersonRequest = PersonGroupPersonRequestGenerator.Delete(groupId, Guid.Empty.ToString());
            _getGroupTrainStatusRequest = PersonGroupRequestGenerator.CheckTraining(groupId);
            _trainRequest = PersonGroupRequestGenerator.Train(groupId);

            RefreshRequests();
        }

        public async Task GetPeople()
        {
            People = await MakeRequest<List<PersonDto>>(_getPeopleRequest).ConfigureAwait(false);
        }

        public async Task RefreshTrainingStatus()
        {
            TrainingStatus = await MakeRequest<TrainingStatus>(_getGroupTrainStatusRequest).ConfigureAwait(false);
        }

        public async Task Train()
        {
            TrainingStatus = await MakeRequest<TrainingStatus>(_trainRequest).ConfigureAwait(false);
        }

        public async Task CreatePerson(PersonDto person)
        {
            _createPersonRequest = PersonGroupPersonRequestGenerator.Create(PersonGroupId!, person.Name, person.UserData);
            RefreshRequests();

            // TODO: Parse PersonId
            _ = await MakeRequest<string>(_createPersonRequest).ConfigureAwait(false);

            await GetPeople();
        }

        public async Task UpdatePerson(PersonDto person)
        {
            _updatePersonRequest = PersonGroupPersonRequestGenerator.Update(PersonGroupId!, person.PersonId, person.Name, person.UserData);
            RefreshRequests();

            _ = await MakeRequest<string>(_updatePersonRequest).ConfigureAwait(false);

            await GetPeople();
        }

        public async Task DeletePerson(string personId)
        {
            _deletePersonRequest = PersonGroupPersonRequestGenerator.Delete(PersonGroupId!, personId);
            RefreshRequests();

            _ = await MakeRequest<string>(_deletePersonRequest).ConfigureAwait(false);

            await GetPeople();
        }

        private void RefreshRequests()
        {
            Requests.Clear();
            Requests.Add(_getPeopleRequest!);
            Requests.Add(_createPersonRequest!);
            Requests.Add(_updatePersonRequest!);
            Requests.Add(_deletePersonRequest!);
            Requests.Add(_getGroupTrainStatusRequest!);
            Requests.Add(_trainRequest!);
        }
    }
}
