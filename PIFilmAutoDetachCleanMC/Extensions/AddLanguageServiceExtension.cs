using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Converters;
using PIFilmAutoDetachCleanMC.Language;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddLanguageServiceExtension
    {
        public static IHostBuilder AddLanguageService(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<LanguageService>();
                services.AddSingleton<LanguageToDisplayNameConverter>();
                services.AddSingleton<LanguageToFlagImageConverter>();
            });

            return hostBuilder;
        }
    }
}
