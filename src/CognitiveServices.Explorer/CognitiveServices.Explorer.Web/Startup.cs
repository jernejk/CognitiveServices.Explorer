using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CognitiveServices.Explorer.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Temporary cache
            services.AddMemoryCache();

            // More permanent cache
            services.AddBlazoredLocalStorage();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
