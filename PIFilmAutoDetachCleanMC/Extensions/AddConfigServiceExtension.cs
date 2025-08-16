using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddConfigServiceExtension
    {
        public static IHostBuilder AddConfigs(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
                services.AddSingleton<IConfiguration>(configuration);
            });
            return hostBuilder;
        }
    }
}
