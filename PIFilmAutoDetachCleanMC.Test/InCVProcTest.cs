using EQX.Core.Process;
using EQX.Core.Sequence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Extensions;
using PIFilmAutoDetachCleanMC.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Test
{
    public class TestApp
    {
        public static IHost? AppHost { get; internal set; }

        public static IHost BuildHost()
        {
            return Host.CreateDefaultBuilder()
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
                .AddRobotDevices()
                .AddSpeedControllerDevices()
                .AddTorqueControllerDevices()
                .AddRecipes()
                .AddProcesses()
                .AddCassette()
                .Build();
        }
    }

    public class InCVProcTest
    {
        [Fact]
        public async Task InCVProc_SeqInCVLoad_Test()
        {
            // Arrange
            TestApp.AppHost = TestApp.BuildHost();
            await TestApp.AppHost!.StartAsync();

            var recipeSelector = TestApp.AppHost.Services.GetRequiredService<PIFilmAutoDetachCleanMC.Recipe.RecipeSelector>();
            recipeSelector.Load();

            var devices = TestApp.AppHost.Services.GetRequiredService<Devices>();
            devices.Inputs.Initialize();
            devices.Outputs.Initialize();
            devices.Inputs.Connect();
            devices.Outputs.Connect();

            var process = TestApp.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.InConveyor.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Origin;

            // Assert
            await Task.Delay(5000);
            Assert.Equal(EProcessStatus.OriginDone, process.ProcessStatus);
        }
    }
}
