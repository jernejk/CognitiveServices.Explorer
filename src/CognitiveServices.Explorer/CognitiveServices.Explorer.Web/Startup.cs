using Blazored.LocalStorage;
using CognitiveServices.Explorer.Application;
using CognitiveServices.Explorer.Web.Infrastructure;
using CognitiveServices.Explorer.Web.ViewModels.FaceApi;
using Flurl.Http;
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
            services.AddTransient<ICognitiveServicesConfigService, CognitiveServicesConfigService>();
            services.AddTransient<PersonGroupsViewModel>();
            services.AddTransient<PersonGroupsPersonViewModel>();
            services.AddTransient<PersonViewModel>();
            services.AddTransient<DetectViewModel>();

            FlurlHttp.Configure(settings =>
            {
                settings.HttpClientFactory = new HttpClientFactoryForBlazor();
            });
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
