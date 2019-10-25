using CognitiveServices.Explorer.Application;
using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Domain.Face;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.ViewModels.FaceApi
{
    public class PersonGroupsViewModel : BaseFaceApiViewModel
    {
        private readonly PersonGroupCurlGenerator _generator = new PersonGroupCurlGenerator();

        public PersonGroupsViewModel(ICognitiveServicesConfigService csConfigService)
            : base(csConfigService)
        {
            PersonGroupListRequest = _generator.List();
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
