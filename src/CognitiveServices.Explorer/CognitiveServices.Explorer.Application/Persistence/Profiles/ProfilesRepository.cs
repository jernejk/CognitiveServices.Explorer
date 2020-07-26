using Blazored.LocalStorage;
using CognitiveServices.Explorer.Application.Persistence.Profiles.Migrations;
using CognitiveServices.Explorer.Domain.Profiles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.Persistence.Profiles
{
    public class ProfilesRepository : IProfilesRepository
    {
        public const string ProfileStorageKey = "Profiles";

        private readonly ILocalStorageService _localStorageService;
        private readonly IProfileMigrations _profileMigrations;

        public ProfilesRepository(ILocalStorageService localStorageService, IProfileMigrations profileMigrations)
        {
            _localStorageService = localStorageService;
            _profileMigrations = profileMigrations;
        }

        public async Task<List<Profile>> GetProfiles()
        {
            var container = await _localStorageService.GetItemAsync<ProfileStorageContainer?>(ProfileStorageKey);
            container = await MigrateIfNecessary(container);

            return container.Profiles;
        }

        public async Task SaveProfiles(List<Profile> profiles)
        {
            var container = new ProfileStorageContainer
            {
                Profiles = profiles
            };

            await _localStorageService.SetItemAsync(ProfileStorageKey, container);
        }

        private async Task<ProfileStorageContainer> MigrateIfNecessary(ProfileStorageContainer? container)
        {
            if (container == null || container.Version < ProfileStorageContainer.CurrentVersion)
            {
                // This will also initialize the container if null.
                container = await _profileMigrations.StartMigration(container);
            }

            return container;
        }
    }

    public interface IProfilesRepository
    {
        Task<List<Profile>> GetProfiles();
        Task SaveProfiles(List<Profile> profiles);
    }
}
