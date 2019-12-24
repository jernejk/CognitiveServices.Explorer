using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Application.Profiles.Shared;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.Profiles.Commands
{
    public class SelectProfileCommand : IRequest
    {
        public SelectProfileCommand(Guid profileId)
        {
            ProfileId = profileId;
        }

        public Guid ProfileId { get; }

        public class Handler : IRequestHandler<SelectProfileCommand>
        {
            private readonly IProfilesRepository _profilesRepository;

            public Handler(IProfilesRepository profilesRepository)
            {
                _profilesRepository = profilesRepository;
            }

            public async Task<Unit> Handle(SelectProfileCommand request, CancellationToken cancellationToken)
            {
                var profiles = await _profilesRepository.GetProfiles();

                var profile = profiles.FirstOrDefault(p => p.Id == request.ProfileId);
                if (profile == null)
                {
                    return Unit.Value;
                }

                profile.IsSelected = true;
                profiles.UpdateIsSelected(profile);

                await _profilesRepository.SaveProfiles(profiles);

                return Unit.Value;
            }
        }
    }
}
