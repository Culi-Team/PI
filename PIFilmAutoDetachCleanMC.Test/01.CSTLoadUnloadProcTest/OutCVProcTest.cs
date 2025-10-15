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
    public class OutCVProcTest
    {
        private async Task WaitUntilAsync(Func<bool> condition, int checkIntervalMs = 100)
        {
            while (!condition())
            {
                await Task.Delay(checkIntervalMs); // tránh CPU 100%
            }
        }

        [Fact]
        public async Task OutCVProc_SeqOutWorkCSTUnLoad_Test()
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
            devices.MotionsInovance.MotionMaster.Connect();

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

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.OutConveyor.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.OutWorkCSTUnLoad;

            // Assert

            await Task.Delay(600000);
            //await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            //Assert.Equal(ESequence.Stop, process.Sequence);

        }

        [Fact]
        public async Task OutCVProc_SeqOutConveyorUnLoad_Test()
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
            devices.MotionsInovance.MotionMaster.Connect();

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

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.OutConveyor.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.OutConveyorUnload;

            // Assert
            await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            Assert.Equal(ESequence.Stop, process.Sequence);

        }
    }
}
