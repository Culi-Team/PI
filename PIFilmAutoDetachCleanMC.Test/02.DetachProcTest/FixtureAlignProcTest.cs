using EQX.Core.Process;
using EQX.Core.Sequence;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Test
{
    public class FixtureAlignProcTest
    {
        private async Task WaitUntilAsync(Func<bool> condition, int checkIntervalMs = 100)
        {
            while (!condition())
            {
                await Task.Delay(checkIntervalMs); // tránh CPU 100%
            }
        }

        [Fact]
        public async Task FixtureAlignProc_SeqOrigin_Test()
        {
            // Arrange
            TestAppCommon.AppHost = TestAppCommon.BuildHost();
            await TestAppCommon.AppHost!.StartAsync();

            var recipeSelector = TestAppCommon.AppHost.Services.GetRequiredService<PIFilmAutoDetachCleanMC.Recipe.RecipeSelector>();
            recipeSelector.Load();

            var devices = TestAppCommon.AppHost.Services.GetRequiredService<Devices>();
            devices.Inputs.Initialize();
            devices.Outputs.Initialize();
            devices.Inputs.Connect();
            devices.Outputs.Connect();
            devices.MotionsInovance.MotionController.Connect();

            foreach (var motion in devices.MotionsAjin.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            foreach (var motion in devices.MotionsInovance.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.FixtureAlign.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Origin;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.ProcessStatus == EProcessStatus.OriginDone), Task.Delay(5000));
            Assert.Equal(EProcessStatus.OriginDone, process.ProcessStatus);
        }

        [Fact]
        public async Task FixtureAlignProc_SeqRobotPlaceFixtureToAlign_Test()
        {
            // Arrange
            TestAppCommon.AppHost = TestAppCommon.BuildHost();
            await TestAppCommon.AppHost!.StartAsync();

            var recipeSelector = TestAppCommon.AppHost.Services.GetRequiredService<PIFilmAutoDetachCleanMC.Recipe.RecipeSelector>();
            recipeSelector.Load();

            var devices = TestAppCommon.AppHost.Services.GetRequiredService<Devices>();
            devices.Inputs.Initialize();
            devices.Outputs.Initialize();
            devices.Inputs.Connect();
            devices.Outputs.Connect();
            devices.MotionsInovance.MotionController.Connect();

            foreach (var motion in devices.MotionsAjin.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            foreach (var motion in devices.MotionsInovance.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.FixtureAlign.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.RobotPlaceFixtureToAlign;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            Assert.Equal(ESequence.Stop, process.Sequence);

        }

        [Fact]
        public async Task FixtureAlignProc_SeqFixtureAlign_Test()
        {
            // Arrange
            TestAppCommon.AppHost = TestAppCommon.BuildHost();
            await TestAppCommon.AppHost!.StartAsync();

            var recipeSelector = TestAppCommon.AppHost.Services.GetRequiredService<PIFilmAutoDetachCleanMC.Recipe.RecipeSelector>();
            recipeSelector.Load();

            var devices = TestAppCommon.AppHost.Services.GetRequiredService<Devices>();
            devices.Inputs.Initialize();
            devices.Outputs.Initialize();
            devices.Inputs.Connect();
            devices.Outputs.Connect();
            devices.MotionsInovance.MotionController.Connect();

            foreach (var motion in devices.MotionsAjin.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            foreach (var motion in devices.MotionsInovance.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.FixtureAlign.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.FixtureAlign;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            Assert.Equal(ESequence.Stop, process.Sequence);

        }

        [Fact]
        public async Task FixtureAlignProc_SeqTransferFixtureLoad_Test()
        {
            // Arrange
            TestAppCommon.AppHost = TestAppCommon.BuildHost();
            await TestAppCommon.AppHost!.StartAsync();

            var recipeSelector = TestAppCommon.AppHost.Services.GetRequiredService<PIFilmAutoDetachCleanMC.Recipe.RecipeSelector>();
            recipeSelector.Load();

            var devices = TestAppCommon.AppHost.Services.GetRequiredService<Devices>();
            devices.Inputs.Initialize();
            devices.Outputs.Initialize();
            devices.Inputs.Connect();
            devices.Outputs.Connect();
            devices.MotionsInovance.MotionController.Connect();

            foreach (var motion in devices.MotionsAjin.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            foreach (var motion in devices.MotionsInovance.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.FixtureAlign.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.TransferFixtureLoad;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            Assert.Equal(ESequence.Stop, process.Sequence);

        }
    }
}
