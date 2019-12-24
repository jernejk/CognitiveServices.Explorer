using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Domain.Profiles;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.Profiles.Queries
{
    public class GetCurrentProfileQuery
    {
    }

    public class GetCurrentProfileQueryHandler
    {
        private readonly IProfilesRepository _profilesRepository;

        public GetCurrentProfileQueryHandler(IProfilesRepository profilesRepository)
        {
            _profilesRepository = profilesRepository;
        }

        public async Task<Profile?> Handle(GetCurrentProfileQuery request, CancellationToken ct = default)
        {
            var profiles = await _profilesRepository.GetProfiles();
            return profiles.FirstOrDefault(p => p.IsSelected);
        }
    }
}
