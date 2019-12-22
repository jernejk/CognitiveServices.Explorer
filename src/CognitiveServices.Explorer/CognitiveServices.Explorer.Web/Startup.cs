using Blazor.FileReader;
using Blazored.LocalStorage;
using CognitiveServices.Explorer.Application;
using CognitiveServices.Explorer.Application.Commands;
using CognitiveServices.Explorer.Application.ViewModels.FaceApi;
using CognitiveServices.Explorer.Web.Infrastructure;
using Flurl.Http;
using MatBlazor;
using MediatR;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

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

            services.AddMediatR(typeof(ExecuteCognitiveServicesCommand));

            // MatBlazor
            services.AddScoped<AppState>();
            services.AddLoadingBar();
            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });
            
            // 3rd party
            FlurlHttp.Configure(settings =>
            {
                settings.HttpClientFactory = new HttpClientFactoryForBlazor();
            });

            services.AddFileReaderService(options => options.UseWasmSharedBuffer = true);
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
