using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Domain.Profiles;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.Profiles.Queries
{
    public class GetCurrentProfileQuery : IRequest<Profile?>
    {
    }

    public class GetCurrentProfileQueryHandler : IRequestHandler<GetCurrentProfileQuery, Profile?>
    {
        private readonly IProfilesRepository _profilesRepository;

        public GetCurrentProfileQueryHandler(IProfilesRepository profilesRepository)
        {
            _profilesRepository = profilesRepository;
        }

        public async Task<Profile?> Handle(GetCurrentProfileQuery request, CancellationToken cancellationToken)
        {
            var profiles = await _profilesRepository.GetProfiles();
            return profiles.FirstOrDefault(p => p.IsSelected);
        }
    }
}
