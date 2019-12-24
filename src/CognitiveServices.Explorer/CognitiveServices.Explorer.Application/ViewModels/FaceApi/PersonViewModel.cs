using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Application.Profiles.Queries;
using CognitiveServices.Explorer.Domain.Face;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.ViewModels.FaceApi
{
    public class PersonViewModel : BaseFaceApiViewModel
    {
        private HttpRequest? _personRequest;
        private HttpRequest? _addFaceRequest;
        private HttpRequest? _addBinaryFaceRequest;
        private HttpRequest? _getFaceRequest;
        private HttpRequest? _updateFaceRequest;
        private HttpRequest? _deleteFaceRequest;

        public PersonViewModel(ICognitiveServicesConfigService csConfigService, IMediator mediator)
            : base(csConfigService, mediator)
        {
        }

        public string? PersonId { get; set; }
        public string? PersonGroupId { get; set; }
        public PersonDto? Person { get; private set; }
        public List<HttpRequest> Requests { get; } = new List<HttpRequest>();

        public override async Task OnInitializedAsync()
        {
            Error = string.Empty;

            await base.OnInitializedAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(PersonGroupId))
            {
                Error = "Person group ID not set!";
            }

            if (string.IsNullOrWhiteSpace(PersonId))
            {
                Error = "Person ID not set!";
            }

            if (!string.IsNullOrWhiteSpace(Error))
            {
                _personRequest = PersonGroupPersonRequestGenerator.Get("group-id", Guid.Empty.ToString());
                _addFaceRequest = PersonGroupPersonFaceRequestGenerator.Add("group-id", Guid.Empty.ToString(), "http://image.url");
                _addFaceRequest = PersonGroupPersonFaceRequestGenerator.Add("group-id", Guid.Empty.ToString(), new byte[0]);
                _addFaceRequest = PersonGroupPersonFaceRequestGenerator.Get("group-id", Guid.Empty.ToString(), Guid.Empty.ToString());
                _addFaceRequest = PersonGroupPersonFaceRequestGenerator.Update("group-id", Guid.Empty.ToString(), Guid.Empty.ToString(), @"{""user"":""data""}");
                _addFaceRequest = PersonGroupPersonFaceRequestGenerator.Delete("group-id", Guid.Empty.ToString(), Guid.Empty.ToString());
                UpdateRequestList();

                return;
            }

            string groupId = PersonGroupId!;
            string personId = PersonId!;
            _personRequest = PersonGroupPersonRequestGenerator.Get(groupId, personId);
            _addFaceRequest = PersonGroupPersonFaceRequestGenerator.Add(groupId, personId, "http://image.url");
            _addBinaryFaceRequest = PersonGroupPersonFaceRequestGenerator.Add(groupId, personId, new byte[0]);
            _getFaceRequest = PersonGroupPersonFaceRequestGenerator.Get(groupId, personId, Guid.Empty.ToString());
            _updateFaceRequest = PersonGroupPersonFaceRequestGenerator.Update(groupId, personId, Guid.Empty.ToString(), @"{""user"":""data""}");
            _deleteFaceRequest = PersonGroupPersonFaceRequestGenerator.Delete(groupId, personId, Guid.Empty.ToString());
            UpdateRequestList();

            await GetPerson().ConfigureAwait(false);
        }

        public async Task GetPerson()
        {
            Person = await MakeRequest<PersonDto>(_personRequest).ConfigureAwait(false);
        }

        public async Task AddFace(FaceDto face, string imageUrl, string detectionModel)
        {
            _addFaceRequest = PersonGroupPersonFaceRequestGenerator.Add(
                PersonGroupId!,
                PersonId!,
                imageUrl,
                detectionModel,
                face.UserData);

            UpdateRequestList();

            _ = await MakeRequest<string>(_addFaceRequest).ConfigureAwait(false);

            // Refresh list.
            await GetPerson();
        }

        public async Task AddFace(FaceDto face, byte[] imageData, string detectionModel)
        {
            _addBinaryFaceRequest = PersonGroupPersonFaceRequestGenerator.Add(
                PersonGroupId!,
                PersonId!,
                imageData,
                detectionModel,
                face.UserData);

            UpdateRequestList();

            _ = await MakeRequest<string>(_addBinaryFaceRequest).ConfigureAwait(false);

            // Refresh list.
            await GetPerson();
        }

        public async Task UpdateFace(FaceDto face)
        {
            _getFaceRequest = PersonGroupPersonFaceRequestGenerator.Get(PersonGroupId!, PersonId!, face.PersistedFaceId);
            _updateFaceRequest = PersonGroupPersonFaceRequestGenerator.Update(PersonGroupId!, PersonId!, face.PersistedFaceId, face.UserData);
            _deleteFaceRequest = PersonGroupPersonFaceRequestGenerator.Delete(PersonGroupId!, PersonId!, face.PersistedFaceId);
            UpdateRequestList();

            _ = await MakeRequest<string>(_updateFaceRequest).ConfigureAwait(false);

            // Refresh list.
            await GetPerson();
        }

        public async Task DeleteFace(string faceId)
        {
            _getFaceRequest = PersonGroupPersonFaceRequestGenerator.Get(PersonGroupId!, PersonId!, faceId);
            _updateFaceRequest = PersonGroupPersonFaceRequestGenerator.Update(PersonGroupId!, PersonId!, faceId, @"{""user"":""data""}");
            _deleteFaceRequest = PersonGroupPersonFaceRequestGenerator.Delete(PersonGroupId!, PersonId!, faceId);
            UpdateRequestList();

            _ = await MakeRequest<string>(_deleteFaceRequest).ConfigureAwait(false);

            // Refresh list.
            await GetPerson();
        }

        private void UpdateRequestList()
        {
            // Both requests are changing every time we do a request and because the reference is changing, we need to rebuild the list.
            Requests.Clear();
            Requests.Add(_personRequest!);
            Requests.Add(_addFaceRequest!);
            Requests.Add(_addBinaryFaceRequest!);
            Requests.Add(_getFaceRequest!);
            Requests.Add(_updateFaceRequest!);
            Requests.Add(_deleteFaceRequest!);
        }
    }
}
