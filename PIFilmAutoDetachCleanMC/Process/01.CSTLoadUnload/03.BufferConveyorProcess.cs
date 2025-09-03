using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Sequence;
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
    public class BufferConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        private readonly CommonRecipe _commonRecipe;
        #endregion

        #region Constructor
        public BufferConveyorProcess(Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe, CommonRecipe commonRecipe)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
            _commonRecipe = commonRecipe;
        }
        #endregion

        #region Inputs
        private IDInput BufferDect1 => _devices.Inputs.BufferCstDetect1;
        private IDInput BufferDect2 => _devices.Inputs.BufferCstDetect2;

        #endregion

        #region Cylinders
        private ICylinder BufferStopper1 => _devices.Cylinders.BufferCvStopper1UpDown;
        private ICylinder BufferStopper2 => _devices.Cylinders.BufferCvStopper2UpDown;
        private ICylinder InCvSupBuffer => _devices.Cylinders.InCvSupportBufferUpDown;
        private ICylinder OutCvSupBuffer => _devices.Cylinders.OutCvSupportBufferUpDown;
        #endregion

        #region Rollers
        private ISpeedController BufferRoller1 => _devices.SpeedControllerList.BufferConveyorRoller1;
        private ISpeedController BufferRoller2 => _devices.SpeedControllerList.BufferConveyorRoller2;
        #endregion

        #region Override Method
        public override bool ProcessOrigin()
        {
            switch ((EBufferConveyorOriginStep)Step.OriginStep)
            {
                case EBufferConveyorOriginStep.Start:
                    Log.Debug("Orign start");
                    Step.OriginStep++;
                    break;
                case EBufferConveyorOriginStep.Stopper_Cylinder_Down:
                    Log.Debug("Stopper_Cylinder_Down");
                    BufferStopper1.Backward();
                    BufferStopper2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => BufferStopper1.IsBackward && BufferStopper2.IsBackward);
                    Step.OriginStep++;
                    break;
                case EBufferConveyorOriginStep.Stopper_Cylinder_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Stopper Cylinder down done");
                    Step.OriginStep++;
                    break;
                case EBufferConveyorOriginStep.Roller_Stop:
                    BufferRoller1.Stop();
                    BufferRoller2.Stop();
                    Log.Debug("Roller Stop");
                    Step.OriginStep++;
                    break;
                case EBufferConveyorOriginStep.End:
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    break;
                default:
                    break;
            }
            return true;
        }
        #endregion
    }
}
