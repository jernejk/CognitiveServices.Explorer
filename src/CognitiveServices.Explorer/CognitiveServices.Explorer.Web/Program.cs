using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Web
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            // I prefer this setup to be separate from Program.cs just like regular ASP.NET web app.
            Startup.ConfigureConfiguration(builder.Configuration);
            Startup.ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }
    }
}
