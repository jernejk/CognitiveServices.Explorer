using Blazor.FileReader;
using Blazored.LocalStorage;
using CognitiveServices.Explorer.Application.Commands;
using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Application.Persistence.Profiles.Migrations;
using CognitiveServices.Explorer.Application.ViewModels.FaceApi;
using CognitiveServices.Explorer.Application.ViewModels.TextApi;
using MatBlazor;
using MediatR;
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
            services.AddTransient<PersonGroupsViewModel>();
            services.AddTransient<PersonGroupsPersonViewModel>();
            services.AddTransient<PersonViewModel>();
            services.AddTransient<DetectViewModel>();
            services.AddTransient<TextViewModel>();

            // Repos
            services.AddTransient<IProfilesRepository, ProfilesRepository>();
            services.AddTransient<IProfileMigrations, ProfileMigrations>();

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

            services.AddHttpClient();
            services.AddBaseAddressHttpClient();

            services.AddFileReaderService(options => options.UseWasmSharedBuffer = true);
        }
    }
}
