using CognitiveServices.Explorer.Application;
using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Domain.Face;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web.ViewModels.FaceApi
{
    public class PersonViewModel : BaseFaceApiViewModel
    {
        public PersonViewModel(ICognitiveServicesConfigService csConfigService)
            : base(csConfigService)
        {
        }

        public string? PersonId { get; set; }
        public string? PersonGroupId { get; set; }
        public PersonDto? Person { get; private set; }
        public HttpRequest PersonRequest { get; set; }

        public override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            PersonRequest = PersonGroupPersonRequestGenerator.Get(PersonGroupId, PersonId);

            await GetPerson().ConfigureAwait(false);
        }

        public async Task GetPerson()
        {
            Person = await MakeRequest<PersonDto>(PersonRequest).ConfigureAwait(false);
        }
    }
}
