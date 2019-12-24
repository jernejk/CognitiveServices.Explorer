using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Application.Profiles.Shared;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.Profiles.Commands
{
    public class DeleteProfileCommand
    {
        public DeleteProfileCommand(Guid profileId)
        {
            ProfileId = profileId;
        }

        public Guid ProfileId { get; }
    }

    public class DeleteProfileCommandHandler
    {
        private readonly IProfilesRepository _profilesRepository;

        public DeleteProfileCommandHandler(IProfilesRepository profilesRepository)
        {
            _profilesRepository = profilesRepository;
        }

        public async Task Handle(DeleteProfileCommand request, CancellationToken ct = default)
        {
            var profiles = await _profilesRepository.GetProfiles();

            var profile = profiles.FirstOrDefault(p => p.Id == request.ProfileId);
            if (profile == null)
            {
                return;
            }

            profiles.Remove(profile);
            profiles.UpdateIsSelected();

            await _profilesRepository.SaveProfiles(profiles);
        }
    }
}
