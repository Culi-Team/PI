using EQX.Core.Helpers;
using EQX.Motion.ByVendor.Inovance;
using EQX.UI.Converters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PIFilmAutoDetachCleanMC.Converters;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Extensions;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using PIFilmAutoDetachCleanMC.MVVM.Views;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using static EQX.Core.Helpers.DisplayHelpers;

namespace PIFilmAutoDetachCleanMC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        private const string AppGuid = "2341E081-7BFC-4738-85A4-CF782120EDCC";
        private Mutex _mutex;

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .AddConfigs()
                .AddViews()
                .AddViewModels()
                .AddStores()
                .AddLanguageService()
                .AddMachineDescriptions()
                .AddDevices()
                .AddProcessIO()
                .AddRegulatorDevices()
                .AddRecipes()
                .AddProcesses()
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            bool isNewInstance = false;

            try
            {
                _mutex = new Mutex(true, AppGuid, out isNewInstance);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mutex create error: {ex.Message}");
                Shutdown();
                return;
            }

            if (!isNewInstance)
            {
                MessageBox.Show("Ứng dụng đã được khởi chạy trước đó");

                Shutdown();
                return;
            }

            ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
            ThreadPool.SetMinThreads(40, completionPortThreads);
            ThreadPool.GetMinThreads(out int newWorker, out _);

            await AppHost!.StartAsync();

            var converter = AppHost.Services.GetRequiredService<CellStatusToColorConverter>();
            Application.Current.Resources.Add(nameof(CellStatusToColorConverter), converter);

            var languageConverter = AppHost.Services.GetRequiredService<LanguageToDisplayNameConverter>();
            Application.Current.Resources.Add(nameof(LanguageToDisplayNameConverter), languageConverter);

            var flagConverter = AppHost.Services.GetRequiredService<LanguageToFlagImageConverter>();
            Application.Current.Resources.Add(nameof(LanguageToFlagImageConverter), flagConverter);

            ShowWindows();

            base.OnStartup(e);
        }

        private void ShowWindows()
        {
            if (AppHost == null) return;

            var validDisplays = DisplayHelpers.GetValidMonitors();

            if (validDisplays.Count == 2)
            {
                ShowSingleWindow(EScreen.RightScreen, validDisplays);

                ShowSingleWindow(EScreen.LeftScreen, validDisplays);

                return;
            }

            ShowSingleWindow(EScreen.RightScreen, validDisplays);
        }

        private void ShowSingleWindow(EScreen screen, IList<MonitorInfo> validDisplays)
        {
            var configuration = AppHost.Services.GetRequiredService<IConfiguration>();
            var displayOrder = JsonConvert.DeserializeObject<List<EScreen>>(
                File.ReadAllText(configuration["Files:DisplayConfigFile"] ?? ""));

            Window window = AppHost.Services.GetRequiredKeyedService<MainWindowView>(screen);
            var viewModel = AppHost.Services.GetRequiredKeyedService<MainWindowViewModel>(screen);
            viewModel.Screen = screen;
            window.DataContext = viewModel;

            window.Left = validDisplays[displayOrder.IndexOf(screen)].Left;
            window.Top = validDisplays[displayOrder.IndexOf(screen)].Top;

            window.Show();
            window.WindowState = WindowState.Maximized;
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();

            base.OnExit(e);
        }
    }
}
