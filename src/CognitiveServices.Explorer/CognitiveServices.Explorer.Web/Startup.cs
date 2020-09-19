using Blazored.LocalStorage;
using CognitiveServices.Explorer.Application.Commands;
using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Application.Persistence.Profiles.Migrations;
using CognitiveServices.Explorer.Application.ViewModels.FaceApi;
using CognitiveServices.Explorer.Application.ViewModels.FormApi;
using CognitiveServices.Explorer.Application.ViewModels.TextApi;
using MatBlazor;
using MediatR;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace CognitiveServices.Explorer.Web
{
    public static class Startup
    {
        public static void ConfigureConfiguration(IConfigurationBuilder configurationBuilder, IWebAssemblyHostEnvironment hostEnvironment)
        {
            Assembly assembly = typeof(Startup).Assembly;

            var appsettingsFiles = assembly.GetManifestResourceNames()
                .Where(f => f.Contains("appsettings.", StringComparison.OrdinalIgnoreCase) && f.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var resourceName in appsettingsFiles)
            {
                configurationBuilder.AddJsonStream(assembly.GetManifestResourceStream(resourceName));
            }
        }

        public static void ConfigureServices(IServiceCollection services, IWebAssemblyHostEnvironment hostEnvironment)
        {
            services.AddSingleton(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                return config.GetSection("App").Get<AppConfiguration>();
            });

            // Temporary cache
            services.AddMemoryCache();

            // More permanent cache
            services.AddBlazoredLocalStorage();
            services.AddTransient<PersonGroupsViewModel>();
            services.AddTransient<PersonGroupsPersonViewModel>();
            services.AddTransient<PersonViewModel>();
            services.AddTransient<DetectViewModel>();
            services.AddTransient<TextViewModel>();
            services.AddTransient<CustomFormsViewModel>();

            // Repos
            services.AddTransient<IProfilesRepository, ProfilesRepository>();
            services.AddTransient<IProfileMigrations, ProfileMigrations>();

            services.AddMediatR(typeof(ExecuteCognitiveServicesCommand));

            // MatBlazor
            services.AddScoped<AppState>();
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
            services.AddSingleton(new HttpClient { BaseAddress = new Uri(hostEnvironment.BaseAddress) });
        }
    }
}
