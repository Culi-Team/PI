using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.Interlock;
using PIFilmAutoDetachCleanMC.Recipe;

namespace PIFilmAutoDetachCleanMC.Defines.Devices.Motion
{
    public static class MotionInterlockConfigurator
    {
        public static void Configure(Devices devices, TransferFixtureRecipe transferFixtureRecipe)
        {
            ConfigureConveyorTAxisOriginInterlock(devices, transferFixtureRecipe);
        }

        private static void ConfigureConveyorTAxisOriginInterlock(Devices devices, TransferFixtureRecipe transferFixtureRecipe)
        {
            var inCassetteTAxis = devices.Motions.InCassetteTAxis;
            var transferFixtureYAxis = devices.Motions.FixtureTransferYAxis;

            if (inCassetteTAxis == null || transferFixtureYAxis == null)
            {
                return;
            }

            var originInterlockKey = $"Motion.{inCassetteTAxis.Name}.Origin";

            inCassetteTAxis.ConfigureInterlock(
                originInterlockKey,
                () => transferFixtureYAxis.IsOnPosition(transferFixtureRecipe.TransferFixtureYAxisLoadPosition) == false,
                transferFixtureYAxis.Status);
        }
    }
}
