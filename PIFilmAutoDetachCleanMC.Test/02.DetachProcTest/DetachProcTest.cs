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
    public class DetachProcTest
    {
        private async Task WaitUntilAsync(Func<bool> condition, int checkIntervalMs = 100)
        {
            while (!condition())
            {
                await Task.Delay(checkIntervalMs); // tránh CPU 100%
            }
        }

        [Fact]
        public async Task DetachProc_SeqOrigin_Test()
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
            devices.Motions.InovanceMaster.Connect();
            devices.Motions.AjinMaster.Connect();

            foreach (var motion in devices.Motions.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.Detach.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Origin;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.ProcessStatus == EProcessStatus.OriginDone), Task.Delay(5000));
            Assert.Equal(EProcessStatus.OriginDone, process.ProcessStatus);
        }

        [Fact]
        public async Task DetachProc_SeqDetach_Test()
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
            devices.Motions.InovanceMaster.Connect();
            devices.Motions.AjinMaster.Connect();

            foreach (var motion in devices.Motions.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.Detach.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.Detach;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            Assert.Equal(ESequence.Stop, process.Sequence);

        }

        [Fact]
        public async Task DetachProc_SeqTransferFixtureUnload_Test()
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
            devices.Motions.InovanceMaster.Connect();
            devices.Motions.AjinMaster.Connect();

            foreach (var motion in devices.Motions.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.Detach.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.TransferFixtureUnload;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            Assert.Equal(ESequence.Stop, process.Sequence);

        }

        [Fact]
        public async Task DetachProc_SeqDetachUnload_Test()
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
            devices.Motions.InovanceMaster.Connect();
            devices.Motions.AjinMaster.Connect();

            foreach (var motion in devices.Motions.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.Detach.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.DetachUnload;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            Assert.Equal(ESequence.Stop, process.Sequence);

        }

        [Fact]
        public async Task DetachProc_SeqGlassTransferPick_Test()
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
            devices.Motions.InovanceMaster.Connect();
            devices.Motions.AjinMaster.Connect();

            foreach (var motion in devices.Motions.All)
            {
                motion.Connect();
                motion.Initialization();
                motion.MotionOn();
            }

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.Detach.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.GlassTransferPick;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            Assert.Equal(ESequence.Stop, process.Sequence);

        }
    }
}
