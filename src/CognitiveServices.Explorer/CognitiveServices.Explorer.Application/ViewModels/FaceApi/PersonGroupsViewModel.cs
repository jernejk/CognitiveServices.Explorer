using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Domain.Face;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.ViewModels.FaceApi
{
    public class PersonGroupsViewModel : BaseFaceApiViewModel
    {
        public PersonGroupsViewModel(ICognitiveServicesConfigService csConfigService, IMediator mediator)
            : base(csConfigService, mediator)
        {
            PersonGroupListRequest = PersonGroupRequestGenerator.List();
            CreatePersonGroupRequest = PersonGroupRequestGenerator.Create("group-id", "Group name");
            UpdatePersonGroupRequest = PersonGroupRequestGenerator.Update("group-id", "Group name");
            DeletePersonGroupRequest = PersonGroupRequestGenerator.Delete("group-id");
        }

        public List<PersonGroupDto>? PersonGroups { get; private set; }
        public HttpRequest PersonGroupListRequest { get; set; }
        public HttpRequest CreatePersonGroupRequest { get; set; }
        public HttpRequest UpdatePersonGroupRequest { get; set; }
        public HttpRequest DeletePersonGroupRequest { get; set; }
        public PersonGroupDto EditGroup { get; set; } = new PersonGroupDto();
        public List<HttpRequest> Requests { get; } = new List<HttpRequest>();

        public override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            UpdateRequestList();

            await GetGroups();
        }

        public async Task GetGroups()
        {
            PersonGroups = await MakeRequest<List<PersonGroupDto>>(PersonGroupListRequest).ConfigureAwait(false);
        }

        public async Task CreateGroup()
        {
            CreatePersonGroupRequest = PersonGroupRequestGenerator.Create(EditGroup.PersonGroupId, EditGroup.Name, EditGroup.UserData, EditGroup.RecognitionModel);
            UpdateRequestList();

            _ = await MakeRequest<string>(CreatePersonGroupRequest).ConfigureAwait(false);

            EditGroup = new PersonGroupDto();

            // Refresh list.
            PersonGroups = await MakeRequest<List<PersonGroupDto>>(PersonGroupListRequest).ConfigureAwait(false);
        }

        public async Task UpdateGroup()
        {
            UpdatePersonGroupRequest = PersonGroupRequestGenerator.Update(EditGroup.PersonGroupId, EditGroup.Name, EditGroup.UserData);
            UpdateRequestList();

            _ = await MakeRequest<string>(UpdatePersonGroupRequest).ConfigureAwait(false);

            EditGroup = new PersonGroupDto();

            // Refresh list.
            PersonGroups = await MakeRequest<List<PersonGroupDto>>(PersonGroupListRequest).ConfigureAwait(false);
        }

        public async Task DeleteGroup(string groupId)
        {
            DeletePersonGroupRequest = PersonGroupRequestGenerator.Delete(groupId);
            UpdateRequestList();

            _ = await MakeRequest<string>(DeletePersonGroupRequest).ConfigureAwait(false);

            // Refresh list.
            PersonGroups = await MakeRequest<List<PersonGroupDto>>(PersonGroupListRequest).ConfigureAwait(false);
        }

        private void UpdateRequestList()
        {
            // Both requests are changing every time we do a request and because the reference is changing, we need to rebuild the list.
            Requests.Clear();
            Requests.Add(PersonGroupListRequest);
            Requests.Add(CreatePersonGroupRequest);
            Requests.Add(UpdatePersonGroupRequest);
            Requests.Add(DeletePersonGroupRequest);
        }
    }
}
