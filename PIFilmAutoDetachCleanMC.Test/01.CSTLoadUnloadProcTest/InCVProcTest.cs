using EQX.Core.InOut;
using EQX.Core.Motion;
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
            devices.Motions.InovanceMaster.Connect();
            devices.Motions.AjinMaster.Connect();

            foreach (var motion in devices.Motions.All)
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
            devices.Motions.InovanceMaster.Connect();
            devices.Motions.AjinMaster.Connect();

            foreach (var motion in devices.Motions.All)
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
            devices.Motions.InovanceMaster.Connect();
            devices.Motions.AjinMaster.Connect();

            foreach (var motion in devices.Motions.All)
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

        [Fact]
        public async Task Detach_Test()
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

            IMotion Shuttle_XAxis = devices.Motions.ShuttleTransferXAxis;
            IMotion Shuttle_ZAxis = devices.Motions.ShuttleTransferZAxis;
            IMotion Detach_ZAxis = devices.Motions.DetachGlassZAxis;

            ICylinder Detach_Clamp1 = devices.Cylinders.Detach_ClampCyl1;
            ICylinder Detach_Clamp2 = devices.Cylinders.Detach_ClampCyl2;
            ICylinder Detach_Clamp3 = devices.Cylinders.Detach_ClampCyl3;
            ICylinder Detach_Clamp4 = devices.Cylinders.Detach_ClampCyl4;

            ICylinder Detach_Cyl1 = devices.Cylinders.Detach_UpDownCyl1;
            ICylinder Detach_Cyl2 = devices.Cylinders.Detach_UpDownCyl2;

            var currentRecipe = recipeSelector.CurrentRecipe;

            Detach_ZAxis.MoveAbs(currentRecipe.DetachRecipe.DetachZAxisReadyPosition);
            Shuttle_ZAxis.MoveAbs(currentRecipe.DetachRecipe.ShuttleTransferZAxisReadyPosition);

            await Task.Delay(5000);


        }
    }
}
