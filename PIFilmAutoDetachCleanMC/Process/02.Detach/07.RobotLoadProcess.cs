using EQX.Core.InOut;
using EQX.Core.Robot;
using EQX.Core.Sequence;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
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
    public class RobotLoadProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly IRobot _robotLoad;
        private readonly CommonRecipe _commonRecipe;
        private readonly Devices _devices;
        private readonly VirtualIO<EFlags> _virtualIO;

        private ICylinder ClampCyl => _devices.Cylinders.RobotFixtureClampUnclamp;
        private ICylinder AlignCyl => _devices.Cylinders.RobotFixtureAlignFwBw;

        private bool IsFixtureDetect => !ClampCyl.IsBackward && !ClampCyl.IsForward;
        #endregion

        #region Flags
        private bool FlagVinylCleanRequestFixture
        {
            get
            {
                return _virtualIO.GetFlag(EFlags.VinylCleanRequestFixture);
            }
            set
            {
                _virtualIO.SetFlag(EFlags.VinylCleanRequestFixture, value);
            }
        }

        private bool FlagRemoveFilmRequestUnload
        {
            get
            {
                return _virtualIO.GetFlag(EFlags.RemoveFilmRequestUnload);
            }
            set
            {
                _virtualIO.SetFlag(EFlags.RemoveFilmRequestUnload, value);
            }
        }

        private bool FlagVinylCleanRequestUnload
        {
            get
            {
                return _virtualIO.GetFlag(EFlags.VinylCleanRequestUnload);
            }
            set
            {
                _virtualIO.SetFlag(EFlags.VinylCleanRequestUnload, value);
            }
        }

        private bool FlagInCSTReady
        {
            get
            {
                return _virtualIO.GetFlag(EFlags.InCSTReady);
            }
        }

        private bool FlagVinylCleanLoadDone
        {
            set
            {
                _virtualIO.SetFlag(EFlags.VinylCleanLoadDone, value);
            }
        }
        #endregion

        #region Constructor
        public RobotLoadProcess([FromKeyedServices("RobotLoad")] IRobot robotLoad,
            CommonRecipe commonRecipe,
            Devices devices,
            VirtualIO<EFlags> virtualIO)
        {
            _robotLoad = robotLoad;
            _commonRecipe = commonRecipe;
            _devices = devices;
            _virtualIO = virtualIO;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((ERobotLoadOriginStep)Step.OriginStep)
            {
                case ERobotLoadOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.Fixture_Detect_Check:
                    if(IsFixtureDetect)
                    {
                        RaiseWarning((int)EWarning.RobotLoadOriginFixtureDetect);
                        break;
                    }
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.Cyl_Backward:
                    Log.Debug("Cylinders Backward");
                    ClampCyl.Backward();
                    AlignCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => ClampCyl.IsBackward && AlignCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.Cyl_BackwardWait:
                    if(WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinders Backward Done");
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.Robot_Origin:
                    Log.Debug("Robot Origin");
                    Wait(1000);
                    Step.OriginStep++;
                    break;
                case ERobotLoadOriginStep.End:
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    break;
            }

            return true;
        }

        public override bool ProcessRun()
        {
            switch (Sequence)
            {
                case ESequence.Stop:
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    break;
                case ESequence.InWorkCSTLoad:
                    break;
                case ESequence.InWorkCSTUnLoad:
                    break;
                case ESequence.CSTTilt:
                    break;
                case ESequence.CSTUnTilt:
                    break;
                case ESequence.OutWorkCSTLoad:
                    break;
                case ESequence.OutWorkCSTUnLoad:
                    break;
                case ESequence.RobotPickFixtureFromCST:
                    Sequence_RobotPickFixtureFromCST();
                    break;
                case ESequence.RobotPlaceFixtureToVinylClean:
                    Sequence_RobotPickPlaceVinylClean(false);
                    break;
                case ESequence.VinylClean:
                    break;
                case ESequence.RobotPickFixtureFromVinylClean:
                    Sequence_RobotPickPlaceVinylClean(true);
                    break;
                case ESequence.RobotPlaceFixtureToAlign:
                    break;
                case ESequence.FixtureAlign:
                    break;
                case ESequence.RobotPickFixtureFromRemoveZone:
                    break;
                case ESequence.RobotPlaceFixtureToOutWorkCST:
                    break;
                case ESequence.TransferFixtureLoad:
                    break;
                case ESequence.Detach:
                    break;
                case ESequence.TransferFixtureUnload:
                    break;
                case ESequence.DetachUnload:
                    break;
                case ESequence.RemoveFilm:
                    break;
                case ESequence.GlassTransferPick:
                    break;
                case ESequence.GlassTransferPlace:
                    break;
                case ESequence.AlignGlass:
                    break;
                case ESequence.TransferInShuttlePick:
                    break;
                case ESequence.TransferInShuttlePlace:
                    break;
                case ESequence.WETCleanLoad:
                    break;
                case ESequence.WETClean:
                    break;
                case ESequence.WETCleanUnload:
                    break;
                case ESequence.TransferRotationPick:
                    break;
                case ESequence.TransferRotationPlace:
                    break;
                case ESequence.AFCleanLoad:
                    break;
                case ESequence.AFClean:
                    break;
                case ESequence.AFCleanUnload:
                    break;
                case ESequence.UnloadTransferPick:
                    break;
                case ESequence.UnloadTransferPlace:
                    break;
                case ESequence.UnloadAlignGlass:
                    break;
                case ESequence.UnloadRobotPick:
                    break;
                case ESequence.UnloadRobotPlasma:
                    break;
                case ESequence.UnloadRobotPlace:
                    break;
            }

            return true;
        }
        #endregion

        #region Private Methods
        private void Sequence_AutoRun()
        {
            switch ((ERobotLoadAutoRunStep)Step.RunStep)
            {
                case ERobotLoadAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    break;
                case ERobotLoadAutoRunStep.Check_Flag_VinylCleanRequestFixture:
                    if(FlagVinylCleanRequestFixture)
                    {
                        FlagVinylCleanRequestFixture = false;
                        Log.Info("Sequence Robot Pick Fixture From CST");
                        Sequence = ESequence.RobotPickFixtureFromCST;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotLoadAutoRunStep.Check_Flag_RemoveFilm:
                    if(FlagRemoveFilmRequestUnload)
                    {
                        FlagRemoveFilmRequestUnload = false;
                        Log.Info("Sequence Robot Pick Fixture From Remove Zone");
                        Sequence = ESequence.RobotPickFixtureFromRemoveZone;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotLoadAutoRunStep.Check_Flag_VinylCleanRequestUnload:
                    if(FlagVinylCleanRequestUnload)
                    {
                        FlagVinylCleanRequestUnload = false;
                        Log.Info("Sequence Robot Pick Fixture From Vinyl Clean");
                        Sequence = ESequence.RobotPickFixtureFromVinylClean;
                        break;
                    }
                    Step.RunStep = (int)ERobotLoadAutoRunStep.Check_Flag_VinylCleanRequestFixture;
                    break;
                case ERobotLoadAutoRunStep.End:
                    break;
            }
        }

        private void Sequence_RobotPickFixtureFromCST()
        {
            switch ((ERobotLoadPickFixtureFromCSTStep)Step.RunStep)
            {
                case ERobotLoadPickFixtureFromCSTStep.Start:
                    Log.Debug("Robot Pick Fixture From CST Start");
                    Step.RunStep++;
                    Log.Debug("Wait In Cassette Ready");
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Wait_InCST_Ready:
                    if(FlagInCSTReady == false)
                    {
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Move_InCST_PickPositon:
                    Log.Debug("Move In Cassette Pick Position");
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Move_InCST_PickPosition_Wait:
                    Log.Debug("Move In Cassette Pick Position Wait");
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Clamp:
                    Log.Debug("Cylinder Clamp");
                    ClampCyl.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => ClampCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Clamp_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        break;
                    }
                    Log.Debug("Cylinder Clamp Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Align:
                    Log.Debug("Cylinder Align");
                    AlignCyl.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => AlignCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Cyl_Align_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        break;
                    }
                    Log.Debug("Cylinder Align Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Move_InCST_ReadyPositon:
                    Log.Debug("Move In Cassette Ready Position");
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.Move_InCST_ReadyPositon_Wait:
                    Log.Debug("Move In Cassette Ready Position Wait");
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case ERobotLoadPickFixtureFromCSTStep.End:
                    Log.Debug("Robot Pick Fixture From CST End");
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }

                    Log.Info("Sequence Place Fixture To Vinyl Clean");
                    Sequence = ESequence.RobotPlaceFixtureToVinylClean;
                    break;
            }
        }

        private void Sequence_RobotPickPlaceVinylClean(bool bPick)
        {
            switch ((ERobotLoadPlaceFixtureToVinylCleanStep)Step.RunStep)
            {
                case ERobotLoadPlaceFixtureToVinylCleanStep.Start:
                    Log.Debug("Robot Place Fixture To Vinyl Clean Start");
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToVinylCleanStep.Move_VinylClean_PickPlacePosition:
                    Log.Debug("Move Vinyl Clean Place Position");
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToVinylCleanStep.Move_VinylClean_PickPlacePosition_Wait:
                    Log.Debug("Move Vinyl Clean Place Position Wait");
                    Wait(1000);
                    if(bPick)
                    {
                        Step.RunStep++;
                        break;
                    }

                    Step.RunStep = (int)ERobotLoadPlaceFixtureToVinylCleanStep.Cyl_UnContact;
                    break;
                case ERobotLoadPlaceFixtureToVinylCleanStep.CylConntact:
                    Log.Debug("Cylinder Contact");
                    ClampCyl.Forward();
                    AlignCyl.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => ClampCyl.IsForward && AlignCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToVinylCleanStep.CylConntact_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        break;
                    }
                    Log.Debug("Cylinder Contact Done");
                    Step.RunStep = (int)ERobotLoadPlaceFixtureToVinylCleanStep.Move_VinylClean_ReadyPosition;
                    break;
                case ERobotLoadPlaceFixtureToVinylCleanStep.Cyl_UnContact:
                    Log.Debug("Cylinder UnContact");
                    ClampCyl.Backward();
                    AlignCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => ClampCyl.IsBackward && AlignCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToVinylCleanStep.Cyl_UnContact_Wait:
                    if(WaitTimeOutOccurred)
                    {
                        break;
                    }
                    Log.Debug("Cylinder UnContact Done");
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToVinylCleanStep.Move_VinylClean_ReadyPosition:
                    Log.Debug("Move Vinyl Clean Ready Position");
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToVinylCleanStep.Move_VinylClean_ReadyPosition_Wait:
                    Log.Debug("Move Vinyl Clean Ready Position Wait");
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToVinylCleanStep.SetFlag_VinylCleanLoadDone:
                    Log.Debug("Set Flag Vinyl Clean Load Done");
                    FlagVinylCleanLoadDone = true;
                    Step.RunStep++;
                    break;
                case ERobotLoadPlaceFixtureToVinylCleanStep.End:
                    Log.Debug("Robot Place Fixture To Vinyl Clean Done");
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }

                    Log.Info("Sequence Auto Run");
                    Sequence = ESequence.AutoRun;
                    break;
            }
        }
      
        #endregion
    }
}
