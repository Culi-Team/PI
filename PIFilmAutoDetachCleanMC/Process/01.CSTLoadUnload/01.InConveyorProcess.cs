using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class InConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        #endregion

        #region Constructor
        public InConveyorProcess(Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
        }
        #endregion

        #region Inputs
        private IDInput CST_Det1 => _devices.Inputs.InCstDetect1;
        private IDInput CST_Det2 => _devices.Inputs.InCstDetect2;
        private IDInput InButton1 => _devices.Inputs.InButton1;
        private IDInput InButton2 => _devices.Inputs.InButton2;
        private IDInput InCSTLightCurtain => _devices.Inputs.InCstLightCurtainSafetyDetect;

        #endregion

        #region Outputs
        private IDOutput InButton1Lamp => _devices.Outputs.InButtonLamp1;
        private IDOutput InButton2Lamp => _devices.Outputs.InButtonLamp2;
        private IDOutput InCstMutingLightCurtain => _devices.Outputs.InCstLightCurtainMuting;
        #endregion

        #region Cylinders
        private ICylinder CstStopper => _devices.Cylinders.InCstStopperUpDown;
        #endregion

        #region Rollers
        private ISpeedController Roller1 => _devices.SpeedControllerList.InConveyorRoller1;
        private ISpeedController Roller2 => _devices.SpeedControllerList.InConveyorRoller2;
        private ISpeedController Roller3 => _devices.SpeedControllerList.InConveyorRoller3;

        #endregion
    }
}
