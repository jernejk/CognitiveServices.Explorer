using Blazored.LocalStorage;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application
{
    public interface ICognitiveServicesConfigService
    {
        Task SetConfig(CognitiveServiceConfig config);
        Task<CognitiveServiceConfig?> GetConfig(string serviceName, string? profileName = null);
        Task<Dictionary<string, CognitiveServiceConfig>> GetAllProfiles(string serviceName);
        Task<string> GetSelectedProfileName(string serviceName);
        Task SetSelectedProfile(string serviceName, string? profileName);
    }

    public class CognitiveServicesConfigService : ICognitiveServicesConfigService
    {
        private const string _defaultProfileName = "default";

        private readonly IMemoryCache _memoryCache;
        private readonly ILocalStorageService _localStorageService;

        public CognitiveServicesConfigService(IMemoryCache memoryCache, ILocalStorageService localStorageService)
        {
            _memoryCache = memoryCache;
            _localStorageService = localStorageService;
        }

        public async Task SetConfig(CognitiveServiceConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.ProfileName))
            {
                config.ProfileName = _defaultProfileName;
            }

            if (string.IsNullOrWhiteSpace(config.ServiceName))
            {
                throw new ArgumentException("Service name can't be empty.", nameof(config.ServiceName));
            }

            if (string.IsNullOrWhiteSpace(config.Token))
            {
                throw new ArgumentException("Subscription key/token can't be empty.", nameof(config.Token));
            }

            // Clean up the URL.
            string? baseUrl = GetBaseUrl(config.BaseUrl);
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("Base URL is invalid.", nameof(config.BaseUrl));
            }

            config.BaseUrl = baseUrl!;

            var profiles = await GetAllProfiles(config.ServiceName);
            profiles[config.ProfileName!] = config;

            string cacheKey = GetProfilesCacheKey(config.ServiceName);
            await _localStorageService.SetItemAsync(cacheKey, profiles);
            _memoryCache.Set(cacheKey, profiles);

            await SetSelectedProfile(config.ServiceName, config.ProfileName!);
        }

        public async Task<CognitiveServiceConfig?> GetConfig(string serviceName, string? profileName = null)
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                profileName = await GetSelectedProfileName(serviceName);
            }

            Console.WriteLine(profileName);
            var profiles = await GetAllProfiles(serviceName);
            profiles.TryGetValue(profileName!, out var profile);

            return profile;
        }

        public async Task<Dictionary<string, CognitiveServiceConfig>> GetAllProfiles(string serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ArgumentException($"Service can't be empty.", nameof(serviceName));
            }

            string cacheKey = GetProfilesCacheKey(serviceName);
            if (!_memoryCache.TryGetValue(cacheKey, out Dictionary<string, CognitiveServiceConfig> profiles))
            {
                profiles = await _localStorageService.GetItemAsync<Dictionary<string, CognitiveServiceConfig>>(cacheKey);
            }

            return profiles ?? new Dictionary<string, CognitiveServiceConfig>();
        }

        public async Task<string> GetSelectedProfileName(string serviceName)
        {
            string cacheKey = GetSelectedProfileCacheKey(serviceName);
            if (!_memoryCache.TryGetValue(cacheKey, out string profileName))
            {
                profileName = await _localStorageService.GetItemAsync<string>(cacheKey);
            }

            return !string.IsNullOrWhiteSpace(profileName) ? profileName : _defaultProfileName;
        }

        public async Task SetSelectedProfile(string serviceName, string? profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                throw new ArgumentException("Profile name can't be empty.", nameof(profileName));
            }

            var profiles = await GetAllProfiles(serviceName);
            if (!profiles.ContainsKey(profileName!))
            {
                throw new ArgumentException($"Profile {profileName} doesn't exists.", nameof(profileName));
            }

            string cacheKey = GetSelectedProfileCacheKey(serviceName);
            await _localStorageService.SetItemAsync(cacheKey, profileName);
            _memoryCache.Set(cacheKey, profileName);
        }

        private static string GetProfilesCacheKey(string serviceName)
        {
            return $"cs-config-profile-{serviceName}";
        }

        private static string GetSelectedProfileCacheKey(string serviceName)
        {
            return $"cs-config-profile-{serviceName}-selected";
        }

        private string? GetBaseUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                return null;
            }

            // The URL returned by Face API in ARM template is with /face/v1.0
            return $"{uri.Scheme}://{uri.Host}";
        }
    }
}
