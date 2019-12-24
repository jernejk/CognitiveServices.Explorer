using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Application.Profiles.Shared;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.Profiles.Commands
{
    public class SelectProfileCommand
    {
        public SelectProfileCommand(Guid profileId)
        {
            ProfileId = profileId;
        }

        public Guid ProfileId { get; }
    }

    public class SelectProfileCommandHandler
    {
        private readonly IProfilesRepository _profilesRepository;

        public SelectProfileCommandHandler(IProfilesRepository profilesRepository)
        {
            _profilesRepository = profilesRepository;
        }

        public async Task Handle(SelectProfileCommand request, CancellationToken ct = default)
        {
            var profiles = await _profilesRepository.GetProfiles();

            var profile = profiles.FirstOrDefault(p => p.Id == request.ProfileId);
            if (profile == null)
            {
                return;
            }

            profile.IsSelected = true;
            profiles.UpdateIsSelected(profile);

            await _profilesRepository.SaveProfiles(profiles);
        }
    }
}
