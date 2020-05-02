using Blazor.FileReader;
using Blazored.LocalStorage;
using CognitiveServices.Explorer.Application.Commands;
using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Application.Persistence.Profiles.Migrations;
using CognitiveServices.Explorer.Application.ViewModels.FaceApi;
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
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace CognitiveServices.Explorer.Web
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IWebAssemblyHostEnvironment hostEnvironment)
        {
            Console.WriteLine("Host env: " + hostEnvironment.Environment);
            services.AddSingleton<AppConfiguration>(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                return config.GetSection("App").Get<AppConfiguration>() ?? new AppConfiguration();
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
            services.AddSingleton(new HttpClient { BaseAddress = new Uri(hostEnvironment.BaseAddress) });

            services.AddFileReaderService(options => options.UseWasmSharedBuffer = true);
        }
    }
}
