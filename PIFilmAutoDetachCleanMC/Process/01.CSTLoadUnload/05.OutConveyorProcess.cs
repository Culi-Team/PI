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
        private readonly Inputs _inputs;
        private readonly Outputs _outputs;
        private readonly Cylinders _cylinders;
        private readonly SpeedControllerList _speedControllerList;
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        #endregion
        #region Constructor
        public OutConveyorProcess(Inputs inputs, Outputs outputs, Cylinders cylinders, SpeedControllerList speedControllerList, Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe)
        {
            _inputs = inputs;
            _outputs = outputs;
            _cylinders = cylinders;
            _speedControllerList = speedControllerList;
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
        }
        #endregion
        #region Inputs
        private IDInput CST_Det1 => _inputs.OutCstDetect1;
        private IDInput CST_Det2 => _inputs.OutCstDetect2;
        private IDInput OutButton1 => _inputs.OutButton1;
        private IDInput OutButton2 => _inputs.OutButton2;
        private IDInput OutCSTLightCurtain => _inputs.OutCstLightCurtainSafetyDetect;
        #endregion
        #region Outputs
        private IDOutput OutButton1Lamp => _outputs.OutButtonLamp1;
        private IDOutput OutButton2Lamp => _outputs.OutButtonLamp2;
        private IDOutput OutCstMutingLightCurtain => _outputs.OutCstLightCurtainMuting;
        #endregion
        #region Cylinders
        private ICylinder CstStopper => _cylinders.OutCstStopperUpDown;
        #endregion
        #region Rollers
        private ISpeedController Roller1 => _speedControllerList.OutConveyorRoller1;
        private ISpeedController Roller2 => _speedControllerList.OutConveyorRoller2;
        private ISpeedController Roller3 => _speedControllerList.OutConveyorRoller3;

        #endregion
    }
}
