using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Process;
using EQX.Core.Sequence;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Process;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Test.DetachTest
{
    public class DetachProcessTest
    {
        private async Task WaitUntilAsync(Func<bool> condition, int checkIntervalMs = 100)
        {
            while (!condition())
            {
                await Task.Delay(checkIntervalMs); // tránh CPU 100%
            }
        }

        [Fact]
        public async Task DetachProcess_Test()
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

            var currentRecipe = recipeSelector.CurrentRecipe;
            var detachRecipe = currentRecipe.DetachRecipe;

            IMotion Detach_ZAxis = devices.Motions.DetachGlassZAxis;
            IMotion Shuttle_XAxis = devices.Motions.ShuttleTransferXAxis;
            IMotion Shuttle_ZAxis = devices.Motions.ShuttleTransferZAxis;

            ICylinder Detach_Cyl1 = devices.Cylinders.Detach_UpDownCyl1;
            ICylinder Detach_Cyl2 = devices.Cylinders.Detach_UpDownCyl2;
            ICylinder Detach_Clamp1 = devices.Cylinders.Detach_ClampCyl1;
            ICylinder Detach_Clamp2 = devices.Cylinders.Detach_ClampCyl2;
            ICylinder Detach_Clamp3 = devices.Cylinders.Detach_ClampCyl3;
            ICylinder Detach_Clamp4 = devices.Cylinders.Detach_ClampCyl4;

            // 1. Move to Ready Position
            Detach_ZAxis.MoveAbs(detachRecipe.DetachZAxisReadyPosition);    // 1
            Shuttle_ZAxis.MoveAbs(detachRecipe.ShuttleTransferZAxisReadyPosition);  // 1

            Detach_Cyl1.Backward();
            Detach_Cyl2.Backward();

            await Task.WhenAny(WaitUntilAsync(() => Detach_ZAxis.IsOnPosition(detachRecipe.DetachZAxisReadyPosition) && Shuttle_ZAxis.IsOnPosition(detachRecipe.ShuttleTransferZAxisReadyPosition)),
                Task.Delay(20000)
                );

            Detach_Clamp1.Forward();
            Detach_Clamp2.Forward();
            Detach_Clamp3.Forward();
            Detach_Clamp4.Forward();

            // 2. Move X to Ready Position
            Shuttle_XAxis.MoveAbs(detachRecipe.ShuttleTransferXAxisDetachPosition);
            await Task.WhenAny(WaitUntilAsync(() => Shuttle_XAxis.IsOnPosition(detachRecipe.ShuttleTransferXAxisDetachPosition)),
                Task.Delay(20000)
                );

            // 2. Move to Detach Ready Position
            Detach_ZAxis.MoveAbs(detachRecipe.DetachZAxisDetachReadyPosition1); // 123
            Shuttle_ZAxis.MoveAbs(detachRecipe.ShuttleTransferZAxisDetachReadyPosition); // 34

            Detach_Cyl2.Backward();
            Detach_Cyl1.Forward();

            // Vac on
            devices.Outputs.DetachGlassShtVac1OnOff.Value = true;
            devices.Outputs.DetachGlassShtVac2OnOff.Value = true;
            devices.Outputs.DetachGlassShtVac3OnOff.Value = true;

            await Task.WhenAny(WaitUntilAsync(() => Detach_ZAxis.IsOnPosition(detachRecipe.DetachZAxisDetachReadyPosition1) && Shuttle_ZAxis.IsOnPosition(detachRecipe.ShuttleTransferZAxisDetachReadyPosition)),
                Task.Delay(20000)
                );

            // 3. Detach 1
            Detach_ZAxis.MoveAbs(detachRecipe.DetachZAxisDetach1Position, 10);  // 136 (+13)
            Shuttle_ZAxis.MoveAbs(detachRecipe.ShuttleTransferZAxisDetach1Position, 10);    // 26 (-8)

            // 4. Detach 1 Up
            Detach_ZAxis.MoveAbs(detachRecipe.DetachZAxisDetachReadyPosition2); // 115 (-21)
            Shuttle_ZAxis.MoveAbs(detachRecipe.ShuttleTransferZAxisDetach1Position + 6);    // 32 (+6)

            // 5.1. Detach 2
            Detach_Cyl2.Forward();

            // 5.2. Detach 2
            Detach_ZAxis.MoveAbs(detachRecipe.DetachZAxisDetach2Position, 10);  // 136 (+8)
            Shuttle_ZAxis.MoveAbs(detachRecipe.ShuttleTransferZAxisDetach2Position, 10);    // 10 (-22)

            // 6. Detach Finishing
            Detach_Cyl1.Backward();
            Detach_Cyl2.Backward();
            Detach_ZAxis.MoveAbs(detachRecipe.DetachZAxisReadyPosition, 50);        // 1
            Shuttle_ZAxis.MoveAbs(detachRecipe.ShuttleTransferZAxisUnloadPosition, 50); // 1

            // 7. Finishing
            Detach_Clamp1.Backward();
            Detach_Clamp2.Backward();
            Detach_Clamp3.Backward();
            Detach_Clamp4.Backward();

            Detach_Cyl1.Backward();
            Detach_Cyl2.Backward();

            Shuttle_XAxis.MoveAbs(detachRecipe.ShuttleTransferXAxisDetachCheckPosition, 50);

            recipeSelector.Save();


            recipeSelector.Save();
        }
    }
}
