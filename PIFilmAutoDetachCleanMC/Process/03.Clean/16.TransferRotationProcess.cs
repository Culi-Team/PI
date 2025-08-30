using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class TransferRotationProcess : ProcessBase<ESequence>
    {
        #region Privates
        private EPort port => Name == EProcess.TransferRotationLeft.ToString() ? EPort.Left : EPort.Right;

        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;

        private IMotion ZAxis => port == EPort.Left ? _devices.MotionsInovance.TransferRotationLZAxis :
                                                      _devices.MotionsInovance.TransferRotationRZAxis;

        private ICylinder ClampCyl => port == EPort.Left ? _devices.Cylinders.TrRotateLeftClampUnclamp :
                                                      _devices.Cylinders.TrRotateRightClampUnclamp;

        private ICylinder RotateCyl => port == EPort.Left ? _devices.Cylinders.TrRotateLeftRotate :
                                                      _devices.Cylinders.TrRotateRightRotate;

        private ICylinder TransferCyl => port == EPort.Left ? _devices.Cylinders.TrRotateLeftFwBw :
                                                      _devices.Cylinders.TrRotateRightFwBw;

        public IDOutput GlassVac => port == EPort.Left ? _devices.Outputs.TrRotateLeftVacOnOff :
                                                      _devices.Outputs.TrRotateRightVacOnOff;

        private bool IsVacDetect => port == EPort.Left ? _devices.Inputs.TrRotateLeftVac.Value :
                                                         _devices.Inputs.TrRotateRightVac.Value;
        #endregion

        #region Constructor
        public TransferRotationProcess(Devices devices,CommonRecipe commonRecipe)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
        }
        #endregion

        #region Override Methods

        public override bool ProcessOrigin()
        {
            switch((ETransferRotationOriginStep)Step.OriginStep)
            {
                case ETransferRotationOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.ZAxis_Origin:
                    Log.Debug("Z Axis Origin Start");
                    ZAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return ZAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.ZAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Z Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.TransferRotation_Cyl_Backward:
                    Log.Debug("Transfer Rotation Cylinder Backward");
                    TransferCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return TransferCyl.IsBackward; });
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.TransferRotation_Cyl_Backward_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer Rotation Cylinder Backward Done");
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.TransferRotation_Cyl_Unclamp:
                    Log.Debug("Transfer Rotation Cylinder Unclamp");
                    ClampCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return ClampCyl.IsBackward; });
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.TransferRotation_Cyl_Unclamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer Rotation Cylinder Unclamp Done");
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.TransferRotation_0Degree:
                    Log.Debug("Transfer Rotation to 0 Degree");
                    RotateCyl.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return RotateCyl.IsForward; });
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.TransferRotation_0Degree_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Transfer Rotation to 0 Degree Done");
                    Step.OriginStep++;
                    break;
                case ETransferRotationOriginStep.End:
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
