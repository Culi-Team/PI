using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using PIFilmAutoDetachCleanMC.Extensions;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using PIFilmAutoDetachCleanMC.MVVM.Views;
using EQX.UI.Converters;

namespace PIFilmAutoDetachCleanMC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .AddConfigs()
                .AddViews()
                .AddViewModels()
                .AddStores()
                .AddMachineDescriptions()
                .AddIODevices()
                .AddProcessIO()
                .AddMotionDevices()
                .AddCylinderDevices()
                .AddRegulatorDevices()
                .AddSyringePumpDevices()
                .AddRobotDevices()
                .AddSpeedControllerDevices()
                .AddTorqueControllerDevices()
                .AddRecipes()
                .AddProcesses()
                .AddCassette()
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var converter = AppHost.Services.GetRequiredService<CellStatusToColorConverter>();
            Application.Current.Resources.Add(nameof(CellStatusToColorConverter), converter);

            Window window = AppHost.Services.GetRequiredService<MainWindowView>();
            window.DataContext = AppHost.Services.GetRequiredService<MainWindowViewModel>();
            window.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();

            base.OnExit(e);
        }
    }
}
