using Blazored.LocalStorage;
using CognitiveServices.Explorer.Domain.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.Persistence.Profiles.Migrations
{
    public class ProfileMigrations : IProfileMigrations
    {
        private readonly ILocalStorageService _localStorageService;

        public ProfileMigrations(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public Task<ProfileStorageContainer> StartMigration(ProfileStorageContainer? container)
        {
            if (container == null)
            {
                // ATM, only null case can be migrated before Profile repository was created.
                return MigrateFromNull();
            }

            return Task.FromResult(container);
        }

        private async Task<ProfileStorageContainer> MigrateFromNull()
        {
            var container = new ProfileStorageContainer();
            
            await MigrateServiceConfigV1(container, "FaceApi", (p, c) => p.FaceApiConfig = c);
            await MigrateServiceConfigV1(container, "TextApi", (p, c) => p.TextApiConfig = c);
            await MigrateServiceConfigV1(container, "SpeechApi", (p, c) => p.SpeechApiConfig = c);

            Profile selectedProfile;
            var selectedFaceApi = await _localStorageService.GetItemAsync<string>("cs-config-profile-FaceApi-selected");
            if (selectedFaceApi != null)
            {
                selectedProfile = container.Profiles.FirstOrDefault(p => p.ProfileName == selectedFaceApi);
                if (selectedProfile != null)
                {
                    selectedProfile.IsSelected = true;
                }
            }

            if (!container.Profiles.Any(p => p.IsSelected) && container.Profiles.Any())
            {
                selectedProfile = container.Profiles.First();
                selectedProfile.IsSelected = true;
            }

            await _localStorageService.SetItemAsync(ProfilesRepository.ProfileStorageKey, container);

            await _localStorageService.RemoveItemAsync("faceApiKey");
            await _localStorageService.RemoveItemAsync("faceApiBaseUrl");

            await _localStorageService.RemoveItemAsync("cs-config-profile-FaceApi");
            await _localStorageService.RemoveItemAsync("cs-config-profile-FaceApi-selected");
            await _localStorageService.RemoveItemAsync("cs-config-profile-TextApi");
            await _localStorageService.RemoveItemAsync("cs-config-profile-TextApi-selected");
            await _localStorageService.RemoveItemAsync("cs-config-profile-SpeechApi");
            await _localStorageService.RemoveItemAsync("cs-config-profile-SpeechApi-selected");

            return container;
        }

        private async Task MigrateServiceConfigV1(ProfileStorageContainer container, string serviceName, Action<Profile, Domain.Profiles.CognitiveServiceConfig> setConfigProperty)
        {
            var faceApiConfig = await _localStorageService.GetItemAsync<Dictionary<string, CognitiveServiceConfig>>($"cs-config-profile-{serviceName}");

            // Migrate from old format where every service had it's own storage container.
            if (faceApiConfig != null)
            {
                foreach (var config in faceApiConfig)
                {
                    var profile = container.Profiles
                        .FirstOrDefault(c => c.ProfileName == config.Key);

                    if (profile == null)
                    {
                        profile = new Profile(config.Key);
                        container.Profiles.Add(profile);
                    }

                    var newConfig = new CognitiveServiceConfig
                    {
                        BaseUrl = config.Value.BaseUrl,
                        ServiceName = config.Value.ServiceName,
                        Token = config.Value.Token
                    };

                    setConfigProperty(profile, newConfig);
                }
            }
        }
    }

    public interface IProfileMigrations
    {
        Task<ProfileStorageContainer> StartMigration(ProfileStorageContainer? container);
    }
}
