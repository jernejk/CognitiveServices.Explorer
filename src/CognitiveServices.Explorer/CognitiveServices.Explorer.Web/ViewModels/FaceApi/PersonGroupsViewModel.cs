using CognitiveServices.Explorer.Application;
using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Domain.Face;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.ViewModels.FaceApi
{
    public class PersonGroupsViewModel : BaseFaceApiViewModel
    {
        public PersonGroupsViewModel(ICognitiveServicesConfigService csConfigService)
            : base(csConfigService)
        {
            PersonGroupListRequest = PersonGroupRequestGenerator.List();
        }

        public List<PersonGroupDto>? PersonGroups { get; private set; }
        public HttpRequest PersonGroupListRequest { get; set; }

        public override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        public async Task GetGroups()
        {
            PersonGroups = await MakeRequest<List<PersonGroupDto>>(PersonGroupListRequest).ConfigureAwait(false);
        }
    }
}
