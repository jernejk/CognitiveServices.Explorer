using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Domain.Profiles;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.Profiles.Queries
{
    public class GetAllProfilesQuery
    {
    }

    public class GetAllProfilesQueryHandler
    {
        private readonly IProfilesRepository _profilesRepository;

        public GetAllProfilesQueryHandler(IProfilesRepository profilesRepository)
        {
            _profilesRepository = profilesRepository;
        }

        public Task<List<Profile>> Handle(GetAllProfilesQuery request, CancellationToken ct = default)
        {
            return _profilesRepository.GetProfiles();
        }
    }
}
