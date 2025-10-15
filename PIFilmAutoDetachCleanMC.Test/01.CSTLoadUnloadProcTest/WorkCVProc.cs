using EQX.Core.InOut;
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
    public class WorkCVProc
    {
        private async Task WaitUntilAsync(Func<bool> condition, int checkIntervalMs = 100)
        {
            while (!condition())
            {
                await Task.Delay(checkIntervalMs); // tránh CPU 100%
            }
        }

        [Theory]
        [InlineData(EProcess.InWorkConveyor)]

        public async Task InWorkCVProc_SeqOrigin_Test(EProcess processKey)
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

            _ = TestAppCommon.AppHost.Services.GetRequiredService<Processes>();
            var port = processKey == EProcess.InWorkConveyor
                ? EPort.Right
                : EPort.Left;
            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.InWorkConveyor.ToString());

            Assert.NotNull(process);
            Assert.Equal(processKey.ToString(), process!.Name);
            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Origin;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.ProcessStatus == EProcessStatus.OriginDone), Task.Delay(5000));
            Assert.Equal(EProcessStatus.OriginDone, process.ProcessStatus);
        }

        [Theory]
        [InlineData(EProcess.OutWorkConveyor)]

        public async Task OutWorkCVProc_SeqOrigin_Test(EProcess processKey)
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

            _ = TestAppCommon.AppHost.Services.GetRequiredService<Processes>();
            var port = processKey == EProcess.InWorkConveyor
                ? EPort.Right
                : EPort.Left;
            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.OutWorkConveyor.ToString());

            Assert.NotNull(process);
            Assert.Equal(processKey.ToString(), process!.Name);
            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Origin;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.ProcessStatus == EProcessStatus.OriginDone), Task.Delay(5000));
            Assert.Equal(EProcessStatus.OriginDone, process.ProcessStatus);
        }

        [Theory]
        [InlineData(EProcess.InWorkConveyor)]

        public async Task InWorkCVProc_SeqLoad_Test(EProcess processKey)
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

            _ = TestAppCommon.AppHost.Services.GetRequiredService<Processes>();
            var port = processKey == EProcess.InWorkConveyor
                ? EPort.Right
                : EPort.Left;
            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.InWorkConveyor.ToString());

            Assert.NotNull(process);
            Assert.Equal(processKey.ToString(), process!.Name);
            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.InWorkCSTLoad;

            // Assert
            await Task.Delay(600000);
            //await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            //Assert.Equal(ESequence.Stop, process.Sequence);
        }

        [Theory]
        [InlineData(EProcess.OutWorkConveyor)]

        public async Task OutWorkCVProc_SeqLoad_Test(EProcess processKey)
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

            _ = TestAppCommon.AppHost.Services.GetRequiredService<Processes>();
            var port = processKey == EProcess.InWorkConveyor
                ? EPort.Right
                : EPort.Left;
            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.OutWorkConveyor.ToString());

            Assert.NotNull(process);
            Assert.Equal(processKey.ToString(), process!.Name);
            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.OutWorkCSTLoad;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            Assert.Equal(ESequence.Stop, process.Sequence);
        }

        [Theory]
        [InlineData(EProcess.InWorkConveyor)]

        public async Task InWorkCVProc_SeqUnLoad_Test(EProcess processKey)
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

            _ = TestAppCommon.AppHost.Services.GetRequiredService<Processes>();
            var port = processKey == EProcess.InWorkConveyor
                ? EPort.Right
                : EPort.Left;
            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.InWorkConveyor.ToString());


            Assert.NotNull(process);
            Assert.Equal(processKey.ToString(), process!.Name);
            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.InWorkCSTUnLoad;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            Assert.Equal(ESequence.Stop, process.Sequence);
        }

        [Theory]
        [InlineData(EProcess.OutWorkConveyor)]

        public async Task OutWorkCVProc_SeqUnLoad_Test(EProcess processKey)
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

            foreach(var motion in devices.MotionsAjin.All)
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

            _ = TestAppCommon.AppHost.Services.GetRequiredService<Processes>();
            var port = processKey == EProcess.InWorkConveyor
                ? EPort.Right
                : EPort.Left;
            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.OutWorkConveyor.ToString());

            Assert.NotNull(process);
            Assert.Equal(processKey.ToString(), process!.Name);
            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.OutWorkCSTUnLoad;

            // Assert
            //await Task.Delay(600000);
            await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            Assert.Equal(ESequence.Stop, process.Sequence);
        }

        [Theory]
        [InlineData(EProcess.InWorkConveyor)]

        public async Task InWorkCVProc_SeqTilt_Test(EProcess processKey)
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

            _ = TestAppCommon.AppHost.Services.GetRequiredService<Processes>();
            var port = processKey == EProcess.InWorkConveyor
                ? EPort.Right
                : EPort.Left;
            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.InWorkConveyor.ToString());

            Assert.NotNull(process);
            Assert.Equal(processKey.ToString(), process!.Name);
            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.CSTTilt;

            // Assert
            await Task.Delay(600000);
            //await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            //Assert.Equal(ESequence.Stop, process.Sequence);
        }

        [Theory]
        [InlineData(EProcess.OutWorkConveyor)]

        public async Task OutWorkCVProc_SeqTilt_Test(EProcess processKey)
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

            _ = TestAppCommon.AppHost.Services.GetRequiredService<Processes>();
            var port = processKey == EProcess.InWorkConveyor
                ? EPort.Right
                : EPort.Left;
            var process = TestAppCommon.AppHost.Services.GetKeyedService<IProcess<ESequence>>(EProcess.OutWorkConveyor.ToString());

            Assert.NotNull(process);
            Assert.Equal(processKey.ToString(), process!.Name);
            // Act
            process.Start();
            process.ProcessMode = EProcessMode.Run;
            process.Sequence = ESequence.CSTTilt;

            // Assert
            await Task.Delay(600000);
            //await Task.WhenAny(WaitUntilAsync(() => process.Sequence == ESequence.Stop), Task.Delay(5000));
            //Assert.Equal(ESequence.Stop, process.Sequence);
        }
    }
}
