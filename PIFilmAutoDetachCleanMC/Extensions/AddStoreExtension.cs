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
using PIFilmAutoDetachCleanMC.Services.Factories;
using PIFilmAutoDetachCleanMC.Services.User;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddStoreExtension
    {
        public static IHostBuilder AddStores(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<UserStore>();
                services.AddSingleton<ICellColorRepository,CellColorRepository>();
                services.AddSingleton<CellStatusToColorConverter>();

                services.AddSingleton<TeachingViewModelFactory>();
                services.AddSingleton<ManualViewModelFactory>();

                services.AddSingleton<CCountData>();
                services.AddSingleton<CTaktTime>();
                services.AddSingleton<CWorkData>();

                services.AddKeyedScoped<IAlertService, AlarmService>("AlarmService");
                services.AddKeyedScoped<IAlertService, WarningService>("WarningService");

                services.AddKeyedScoped("BlinkTimer", (s, o) => { return new ActionAssignableTimer(500); });
            });

            return hostBuilder;
        }
    }
}
