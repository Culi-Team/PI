using EQX.Core.Common;
using EQX.UI.Converters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.ProductDatas;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Services;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddStoreExtension
    {
        public static IHostBuilder AddStores(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<UserStore>();
                services.AddSingleton<ProcessInitSelect>();
                services.AddSingleton<ICellColorRepository,CellColorRepository>();
                services.AddSingleton<CellStatusToColorConverter>();
                
                services.AddSingleton<AppSettings>((serviceProvider) =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                    var appSettings = new AppSettings();
                    configuration.GetSection("Folders").Bind(appSettings.Folders);
                    configuration.GetSection("Files").Bind(appSettings.Files);
                    return appSettings;
                });

                services.AddSingleton<CCountData>();
                services.AddSingleton<CTaktTime>();
                services.AddSingleton<CWorkData>((serviceProvider) => 
                {
                    var countData = serviceProvider.GetRequiredService<CCountData>();
                    var taktTime = serviceProvider.GetRequiredService<CTaktTime>();
                    var appSettings = serviceProvider.GetRequiredService<AppSettings>();
                    return new CWorkData(countData, taktTime, appSettings);
                });

                services.AddKeyedScoped<IAlertService, AlarmService>("AlarmService");
                services.AddKeyedScoped<IAlertService, WarningService>("WarningService");

                services.AddKeyedScoped("BlinkTimer", (s, o) => { return new ActionAssignableTimer(500); });
            });

            return hostBuilder;
        }
    }
}
