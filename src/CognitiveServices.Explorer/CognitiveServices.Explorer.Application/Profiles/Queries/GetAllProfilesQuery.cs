using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Domain.Profiles;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.Profiles.Queries
{
    public class GetAllProfilesQuery : IRequest<List<Profile>>
    {
        public class Handler : IRequestHandler<GetAllProfilesQuery, List<Profile>>
        {
            private readonly IProfilesRepository _profilesRepository;

            public Handler(IProfilesRepository profilesRepository)
            {
                _profilesRepository = profilesRepository;
            }

            public Task<List<Profile>> Handle(GetAllProfilesQuery request, CancellationToken cancellationToken)
            {
                return _profilesRepository.GetProfiles();
            }
        }
    }
}
