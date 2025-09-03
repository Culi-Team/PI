using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.VirtualIO;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class TransferFixtrueProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly VirtualIO<EFlags> _virtualIO;

        private IMotion TransferFixtureYAxis => _devices.MotionsInovance.FixtureTransferYAxis;
        private Inputs Inputs => _devices.Inputs;
        private Outputs Outputs => _devices.Outputs;
        private ICylinder TransferFixtureUpDown => _devices.Cylinders.TransferFixtureUpDown;
        private ICylinder TransferFixtureClamp1 => _devices.Cylinders.TransferFixture1ClampUnclamp;
        private ICylinder TransferFixtureClamp2 => _devices.Cylinders.TransferFixture2ClampUnclamp;

        private bool IsFixtureDetect1 => !TransferFixtureClamp1.IsBackward && !TransferFixtureClamp1.IsForward;
        private bool IsFixtureDetect2 => !TransferFixtureClamp2.IsBackward && !TransferFixtureClamp2.IsForward;
        #endregion

        #region Flags
        private bool FlagDetachProcessOriginDone
        {
            get
            {
                return _virtualIO.GetFlag(EFlags.DetachProcessOriginDone);
            }
            set
            {
                _virtualIO.SetFlag(EFlags.DetachProcessOriginDone, value);
            }
        }
        #endregion
        public TransferFixtrueProcess(Devices devices, CommonRecipe commonRecipe, VirtualIO<EFlags> virtualIO)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _virtualIO = virtualIO;
        }

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch((ETransferFixtureOriginStep)Step.OriginStep)
            {
                case ETransferFixtureOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.FixtureDetectCheck:
                    Log.Debug("Fixture Detect Check");
                    if (IsFixtureDetect1 || IsFixtureDetect2)
                    {
                        RaiseWarning((int)EWarning.TransferFixtureOriginFixtureDetect);
                        break;
                    }
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.Unclamp:
                    Log.Debug("Unclamp");
                    TransferFixtureClamp1.Backward();
                    TransferFixtureClamp2.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return TransferFixtureClamp1.IsBackward && TransferFixtureClamp2.IsBackward; });
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.Unclamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Unclamp Done");

                    Step.OriginStep++;
                    Log.Debug("Wait Detach Origin");
                    break;
                case ETransferFixtureOriginStep.Wait_Detach_Origin:
                    if(!FlagDetachProcessOriginDone)
                    {
                        //Wait Detach Process Origin Done
                        break;
                    }
                    FlagDetachProcessOriginDone = false;

                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.CylUp:
                    Log.Debug("Cylinder Up");
                    TransferFixtureUpDown.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return TransferFixtureUpDown.IsBackward; });
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.CylUp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Up Done");

                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.YAxis_Origin:
                    Log.Debug("Fixture Transfer Y Axis Origin Start");
                    TransferFixtureYAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return TransferFixtureYAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.YAxis_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Fixture Transfer Y Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ETransferFixtureOriginStep.End:
                    Log.Debug("Origin End");
                    Step.OriginStep++;
                    ProcessStatus = EProcessStatus.OriginDone;
                    break;
            }
            return true;
        }
        #endregion
    }
}
