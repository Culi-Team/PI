using EQX.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Factories;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using PIFilmAutoDetachCleanMC.MVVM.Views;

namespace PIFilmAutoDetachCleanMC.Extensions
{
    public static class AddViewViewModelExtension
    {
        public static void AddViewModel<TViewModel>(this IServiceCollection services) where TViewModel : ViewModelBase
        {
            services.AddSingleton<TViewModel>();
        }

        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedScoped<MainWindowViewModel>(EScreen.RightScreen);
                services.AddKeyedScoped<MainWindowViewModel>(EScreen.LeftScreen);

                services.AddViewModel<NavigateMenuViewModel>();
                services.AddViewModel<HeaderViewModel>();
                services.AddViewModel<FooterViewModel>();
                services.AddViewModel<MaintenanceViewModel>();

                services.AddViewModel<InitDeinitViewModel>();
                services.AddViewModel<InitializeViewModel>();
                services.AddViewModel<OriginViewModel>();
                services.AddViewModel<AutoViewModel>();
                services.AddViewModel<ManualViewModel>();
                services.AddViewModel<DataViewModel>();
                services.AddViewModel<TeachViewModel>();
                services.AddViewModel<VisionViewModel>();
                services.AddViewModel<IOMonitoringViewModel>();

                services.AddViewModel<LogViewModel>();
                services.AddViewModel<LoginViewModel>();
                services.AddViewModel<DevViewModel>();

                services.AddSingleton<ViewModelNavigationStore>();
                services.AddTransient<ViewModelProvider>();
                services.AddTransient<INavigationService, NavigationService>();
            });

            return hostBuilder;
        }

        public static IHostBuilder AddViews(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                services.AddKeyedScoped<MainWindowView>(EScreen.RightScreen);
                services.AddKeyedScoped<MainWindowView>(EScreen.LeftScreen);
            });

            return hostBuilder;
        }
    }
}
