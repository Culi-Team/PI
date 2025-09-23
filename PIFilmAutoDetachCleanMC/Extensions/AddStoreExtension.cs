using EQX.Core.Common;
using EQX.UI.Converters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using PIFilmAutoDetachCleanMC.Process;

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
                services.AddKeyedScoped("BlinkTimer", (s, o) => { return new ActionAssignableTimer(500); });
            });

            return hostBuilder;
        }
    }
}
