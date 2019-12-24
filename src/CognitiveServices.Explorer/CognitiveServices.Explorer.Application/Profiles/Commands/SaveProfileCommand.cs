using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Application.Profiles.Shared;
using CognitiveServices.Explorer.Domain.Profiles;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.Profiles.Commands
{
    public class SaveProfileCommand
    {
        public SaveProfileCommand(Profile profile)
        {
            Profile = profile;
        }

        public Profile Profile { get; }
    }

    public class SaveProfileCommandHandler
    {
        private readonly IProfilesRepository _profilesRepository;

        public SaveProfileCommandHandler(IProfilesRepository profilesRepository)
        {
            _profilesRepository = profilesRepository;
        }

        public async Task Handle(SaveProfileCommand request, CancellationToken ct = default)
        {
            var profiles = await _profilesRepository.GetProfiles();

            var profile = profiles.FirstOrDefault(p => p.Id == request.Profile.Id);
            if (profile == null)
            {
                profile = request.Profile;
                profile.Id = Guid.NewGuid();

                profiles.Add(profile);
            }
            else
            {
                Console.WriteLine("Mapping");
                profile.Map(request.Profile);
            }

            profiles.UpdateIsSelected(profile);

            Console.WriteLine("Save profiles");
            await _profilesRepository.SaveProfiles(profiles);
        }
    }
}
