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
    public class OutConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        #endregion

        #region Constructor
        public OutConveyorProcess(Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
        }
        #endregion

        #region Inputs
        private IDInput CST_Det1 => _devices.Inputs.OutCstDetect1;
        private IDInput CST_Det2 => _devices.Inputs.OutCstDetect2;
        private IDInput OutButton1 => _devices.Inputs.OutButton1;
        private IDInput OutButton2 => _devices.Inputs.OutButton2;
        private IDInput OutCSTLightCurtain => _devices.Inputs.OutCstLightCurtainAlarmDetect;
        #endregion

        #region Outputs
        private IDOutput OutButton1Lamp => _devices.Outputs.OutButtonLamp1;
        private IDOutput OutButton2Lamp => _devices.Outputs.OutButtonLamp2;
        private IDOutput OutCstMutingLightCurtain1 => _devices.Outputs.OutCstLightCurtainMuting1;
        private IDOutput OutCstMutingLightCurtain2 => _devices.Outputs.OutCstLightCurtainMuting2;
        #endregion

        #region Cylinders
        private ICylinder CstStopper => _devices.Cylinders.OutCstStopperUpDown;
        #endregion

        #region Rollers
        private ISpeedController Roller1 => _devices.SpeedControllerList.OutConveyorRoller1;
        private ISpeedController Roller2 => _devices.SpeedControllerList.OutConveyorRoller2;
        private ISpeedController Roller3 => _devices.SpeedControllerList.OutConveyorRoller3;

        #endregion
    }
}
