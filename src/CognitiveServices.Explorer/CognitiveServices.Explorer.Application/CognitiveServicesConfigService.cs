using Blazored.LocalStorage;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application
{
    public interface ICognitiveServicesConfigService
    {
        Task<CognitiveServiceConfig> GetConfig(string serviceName, string profileName = "default");
        Task SetConfig(CognitiveServiceConfig config, string profileName = "default");
    }

    public class CognitiveServicesConfigService : ICognitiveServicesConfigService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILocalStorageService _localStorageService;

        public CognitiveServicesConfigService(IMemoryCache memoryCache, ILocalStorageService localStorageService)
        {
            _memoryCache = memoryCache;
            _localStorageService = localStorageService;
        }

        public async Task SetConfig(CognitiveServiceConfig config, string profileName = "default")
        {
            string cacheKey = GetCacheKey(config.ServiceName, profileName);
            await _localStorageService.SetItemAsync(cacheKey, config);
            _memoryCache.Set(cacheKey, config);
        }

        public async Task<CognitiveServiceConfig> GetConfig(string serviceName, string profileName = "default")
        {
            string cacheKey = GetCacheKey(serviceName, profileName);
            if (!_memoryCache.TryGetValue(cacheKey, out CognitiveServiceConfig config))
            {
                config = await _localStorageService.GetItemAsync<CognitiveServiceConfig>(cacheKey);
            }

            return config;
        }

        private static string GetCacheKey(string serviceName, string profileName)
        {
            return $"cs-config-{serviceName}-{profileName}";
        }
    }
}
