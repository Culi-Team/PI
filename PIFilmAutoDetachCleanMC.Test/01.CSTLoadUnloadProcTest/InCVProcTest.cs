using EQX.Core.InOut;
using EQX.Core.Process;
using EQX.Core.Sequence;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Process;

namespace PIFilmAutoDetachCleanMC.Test
{
    public class InCVProcTest
    {
        private async Task WaitUntilAsync(Func<bool> condition, int checkIntervalMs = 100)
        {
            while (!condition())
            {
                await Task.Delay(checkIntervalMs); // tránh CPU 100%
            }
        }

        [Fact]
        public async Task InCVProc_SeqOrigin_Test()
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

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.InConveyor.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Origin;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.ProcessStatus == EProcessStatus.OriginDone), Task.Delay(5000));
            Assert.Equal(EProcessStatus.OriginDone, process.ProcessStatus);
        }

        [Fact]
        public async Task InCVProc_SeqInCVLoad_Test()
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

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.InConveyor.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.InConveyorLoad;

            // Assert
            await Task.Delay(600000);
            //await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            //Assert.Equal(ESequence.Stop, process.Sequence);

        }

        [Fact]
        public async Task InCVProc_SeqInWorkCSTLoad_Test()
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

            var virtualIO = TestAppCommon.AppHost.Services.GetRequiredService<VirtualIO>();
            virtualIO.Initialize();
            virtualIO.Mappings();

            var inWCVYOut = TestAppCommon.AppHost.Services.GetKeyedService<IDOutputDevice>("InWorkConveyorOutput");
            inWCVYOut[(int)EWorkConveyorProcessOutput.REQUEST_CST_IN] = true;

            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.InConveyor.ToString());

            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.InWorkCSTLoad;

            // Assert
            await Task.Delay(600000);
            //await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            //Assert.Equal(ESequence.Stop, process.Sequence);
        }
    }
}
