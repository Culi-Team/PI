using EQX.Core.Device.SpeedController;
using EQX.Core.InOut;
using EQX.Core.Motion;
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
    public class WorkConveyorProcess : ProcessBase<ESequence>
    {
        #region Private
        private EPort port => Name == EProcess.InWorkConveyor.ToString() ? EPort.Left : EPort.Right;
        private readonly Devices _devices;
        private readonly CSTLoadUnloadRecipe _cstLoadUnloadRecipe;
        private readonly CommonRecipe _commonRecipe;
        #endregion

        #region Constructor
        public WorkConveyorProcess(Devices devices, CSTLoadUnloadRecipe cstLoadUnloadRecipe,CommonRecipe commonRecipe)
        {
            _devices = devices;
            _cstLoadUnloadRecipe = cstLoadUnloadRecipe;
            _commonRecipe = commonRecipe;
        }
        #endregion

        #region Inputs
        private IDInput Detect1 => port == EPort.Left ?  _devices.Inputs.InCstWorkDetect1 :
                                                         _devices.Inputs.OutCstWorkDetect1;
        private IDInput Detect2 => port == EPort.Left ?  _devices.Inputs.InCstWorkDetect2 :
                                                         _devices.Inputs.OutCstWorkDetect2;
        private IDInput Detect3 => port == EPort.Left ? _devices.Inputs.InCstWorkDetect3 :
                                                        _devices.Inputs.OutCstWorkDetect3;
        private IDInput Detect4 => _devices.Inputs.InCstWorkDetect4;
        #endregion

        #region Outputs
        #endregion

        #region Cylinders
        private ICylinder FixCylinder => port == EPort.Left ? _devices.Cylinders.InCstFixCylFwBw :
                                                              _devices.Cylinders.OutCstFixCylFwBw;
        private ICylinder TiltCylinder => port == EPort.Left ? _devices.Cylinders.InCstTiltCylUpDown :
                                                               _devices.Cylinders.OutCstTiltCylUpDown;
        private ICylinder CVSupportCyl1 => port == EPort.Left ? _devices.Cylinders.InCvSupportUpDown :
                                                                _devices.Cylinders.OutCvSupportBufferUpDown;
        private ICylinder CVSupportCyl2 => port == EPort.Left ? _devices.Cylinders.InCvSupportBufferUpDown :
                                                                _devices.Cylinders.OutCvSupportUpDown;
        #endregion

        #region Motions
        private IMotion InCSTTAxis => port == EPort.Left ? _devices.MotionsInovance.InCassetteTAxis :
                                                           _devices.MotionsInovance.OutCassetteTAxis;
        #endregion

        #region Rollers
        private ISpeedController RollerSupport1 => port == EPort.Left ? _devices.SpeedControllerList.SupportConveyor1Roller : 
                                                                        _devices.SpeedControllerList.SupportConveyor3Roller;

        private ISpeedController RollerSupport2 => port == EPort.Left ? _devices.SpeedControllerList.SupportConveyor2Roller :
                                                                        _devices.SpeedControllerList.SupportConveyor4Roller;

        private ISpeedController Roller1 => port == EPort.Left ? _devices.SpeedControllerList.InWorkConveyorRoller1 : 
                                                                 _devices.SpeedControllerList.OutWorkConveyorRoller1;
        private ISpeedController Roller2 => port == EPort.Left ? _devices.SpeedControllerList.InWorkConveyorRoller2 : 
                                                                 _devices.SpeedControllerList.OutWorkConveyorRoller2;
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch((EWorkConveyorOriginStep)Step.OriginStep)
            {
                case EWorkConveyorOriginStep.Start:
                    Log.Debug("Start");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_UnTilt:
                    Log.Debug("Cyl_UnTilt");
                    TiltCylinder.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => TiltCylinder.IsBackward);
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_UnTilt_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UnTilt Done");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.SupportCV_Down:
                    Log.Debug("Support CV Down");
                    CVSupportCyl1.Backward();
                    CVSupportCyl2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => CVSupportCyl1.IsBackward && CVSupportCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.SupportCV_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Support CV Down Done");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.TAxis_Origin:
                    Log.Debug("T Axis Origin");
                    InCSTTAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => InCSTTAxis.Status.IsHomeDone);
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.TAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("T Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_UnAlign:
                    Log.Debug("Cylinder UnAlign");
                    FixCylinder.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => FixCylinder.IsBackward);
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.Cyl_UnAlign_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder UnAlign Done");
                    Step.OriginStep++;
                    break;
                case EWorkConveyorOriginStep.End:
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    break;
                default:
                    Wait(20);
                    break;
            }

            return true;
        }
        #endregion
    }
}
