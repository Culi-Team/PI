using EQX.Core.InOut;
using EQX.Core.Sequence;
using EQX.InOut;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Recipe;
using PIFilmAutoDetachCleanMC.Services.DryRunServices;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class RobotUnloadProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly IDInputDevice _robotUnloadInput;
        private readonly IDOutputDevice _robotUnloadOutput;
        private readonly DieHardK180Plasma _plasma;
        private readonly MachineStatus _machineStatus;

        private bool IsPlasmaPrepare { get; set; } = false;

        private IDOutput GlassVacOnOff1 => _devices.Outputs.UnloadRobotVac1OnOff;
        private IDOutput GlassVacOnOff2 => _devices.Outputs.UnloadRobotVac2OnOff;
        private IDOutput GlassVacOnOff3 => _devices.Outputs.UnloadRobotVac3OnOff;
        private IDOutput GlassVacOnOff4 => _devices.Outputs.UnloadRobotVac4OnOff;

        private bool GlassVac1 => _machineStatus.IsSatisfied(_devices.Inputs.UnloadRobotVac1);
        private bool GlassVac2 => _machineStatus.IsSatisfied(_devices.Inputs.UnloadRobotVac2);
        private bool GlassVac3 => _machineStatus.IsSatisfied(_devices.Inputs.UnloadRobotVac3);
        private bool GlassVac4 => _machineStatus.IsSatisfied(_devices.Inputs.UnloadRobotVac4);

        private IEnumerable<IDInput> RobotVacuumInputs => new[]
        {
            _devices.Inputs.UnloadRobotVac1,
            _devices.Inputs.UnloadRobotVac2,
            _devices.Inputs.UnloadRobotVac3,
            _devices.Inputs.UnloadRobotVac4
        };

        private bool AreRobotVacuumsActive =>
            GlassVac1 == true &&
            GlassVac2 == true &&
            GlassVac3 == true &&
            GlassVac4 == true;

        private ICylinder Cyl1 => _devices.Cylinders.UnloadRobotCyl1UpDown;
        private ICylinder Cyl2 => _devices.Cylinders.UnloadRobotCyl2UpDown;
        private ICylinder Cyl3 => _devices.Cylinders.UnloadRobotCyl3UpDown;
        private ICylinder Cyl4 => _devices.Cylinders.UnloadRobotCyl4UpDown;

        private bool IsCylindersUp => Cyl1.IsBackward && Cyl2.IsBackward && Cyl3.IsBackward && Cyl4.IsBackward;
        private bool IsCylindersDown => Cyl1.IsForward && Cyl2.IsForward && Cyl3.IsForward && Cyl4.IsForward;

        private bool GlassDetect1 => _machineStatus.IsSatisfied(_devices.Inputs.UnloadRobotDetect1);
        private bool GlassDetect2 => _machineStatus.IsSatisfied(_devices.Inputs.UnloadRobotDetect2);
        private bool GlassDetect3 => _machineStatus.IsSatisfied(_devices.Inputs.UnloadRobotDetect3);
        private bool GlassDetect4 => _machineStatus.IsSatisfied(_devices.Inputs.UnloadRobotDetect4);
        #endregion

        #region Flags
        private bool FlagUnloadAlignRequestUnload
        {
            get
            {
                return _robotUnloadInput[(int)ERobotUnloadProcessInput.UNLOAD_ALIGN_REQ_ROBOT_UNLOAD];
            }
        }

        private bool FlagRobotUnloadDoneReceived
        {
            get
            {
                return _robotUnloadInput[(int)ERobotUnloadProcessInput.UNLOAD_ALIGN_UNLOAD_DONE_RECEIVED];
            }
        }

        private bool FlagRobotUnloadPickDone
        {
            set
            {
                _robotUnloadOutput[(int)ERobotUnloadProcessOutput.ROBOT_UNLOAD_PICK_DONE] = value;
            }
        }
        #endregion

        #region Constructor
        public RobotUnloadProcess(Devices devices,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
            [FromKeyedServices("RobotUnloadInput")] IDInputDevice robotUnloadInput,
            [FromKeyedServices("RobotUnloadOutput")] IDOutputDevice robotUnloadOutput,
            DieHardK180Plasma plasma)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _machineStatus = machineStatus;
            _robotUnloadInput = robotUnloadInput;
            _robotUnloadOutput = robotUnloadOutput;
            _plasma = plasma;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((ERobotUnloadOriginStep)Step.OriginStep)
            {
                case ERobotUnloadOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ERobotUnloadOriginStep.Cylinder_Up:
                    Log.Debug("Cylinders Up");
                    CylinderContact(false);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsCylindersUp);
                    Step.OriginStep++;
                    break;
                case ERobotUnloadOriginStep.Cylinder_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotUnload_Cylinder_Up_Fail);
                        break;
                    }
                    Log.Debug("Cylinders Up Done");
                    Step.OriginStep++;
                    break;
                case ERobotUnloadOriginStep.End:
                    Log.Debug("Origin End");
                    Step.OriginStep++;
                    ProcessStatus = EProcessStatus.OriginDone;
                    break;
                default:
                    Wait(10);
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
                    Sequence = ESequence.Stop;
                    break;
                case ESequence.InConveyorLoad:
                    break;
                case ESequence.InWorkCSTLoad:
                    break;
                case ESequence.InWorkCSTUnLoad:
                    break;
                case ESequence.CSTTilt:
                    break;
                case ESequence.OutWorkCSTLoad:
                    break;
                case ESequence.OutWorkCSTUnLoad:
                    break;
                case ESequence.OutConveyorUnload:
                    break;
                case ESequence.RobotPickFixtureFromCST:
                    break;
                case ESequence.RobotPlaceFixtureToVinylClean:
                    break;
                case ESequence.VinylClean:
                    break;
                case ESequence.RobotPickFixtureFromVinylClean:
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
                case ESequence.RemoveFilmThrow:
                    break;
                case ESequence.GlassTransferPick:
                    break;
                case ESequence.GlassTransferPlace:
                    break;
                case ESequence.AlignGlassLeft:
                    break;
                case ESequence.AlignGlassRight:
                    break;
                case ESequence.TransferInShuttleLeftPick:
                    break;
                case ESequence.TransferInShuttleRightPick:
                    break;
                case ESequence.WETCleanLeftLoad:
                    break;
                case ESequence.WETCleanRightLoad:
                    break;
                case ESequence.WETCleanLeft:
                    break;
                case ESequence.WETCleanRight:
                    break;
                case ESequence.WETCleanLeftUnload:
                    break;
                case ESequence.WETCleanRightUnload:
                    break;
                case ESequence.TransferRotationLeft:
                    break;
                case ESequence.TransferRotationRight:
                    break;
                case ESequence.AFCleanLeftLoad:
                    break;
                case ESequence.AFCleanRightLoad:
                    break;
                case ESequence.AFCleanLeft:
                    break;
                case ESequence.AFCleanRight:
                    break;
                case ESequence.AFCleanLeftUnload:
                    break;
                case ESequence.AFCleanRightUnload:
                    break;
                case ESequence.UnloadTransferLeftPlace:
                    break;
                case ESequence.UnloadTransferRightPlace:
                    break;
                case ESequence.UnloadAlignGlass:
                    break;
                case ESequence.UnloadRobotPick:
                    Sequence_UnloadRobotPick();
                    break;
                case ESequence.UnloadRobotPlasma:
                    Sequence_UnloadRobotPlasma();
                    break;
                case ESequence.UnloadRobotPlace:
                    Sequence_UnloadRobotPlace();
                    break;
            }

            return true;
        }
        #endregion

        #region Private Methods
        private void VacuumOnOff(bool bOnOff)
        {
            GlassVacOnOff1.Value = bOnOff;
            GlassVacOnOff2.Value = bOnOff;
            GlassVacOnOff3.Value = bOnOff;
            GlassVacOnOff4.Value = bOnOff;
        }

        private void CylinderContact(bool bContact)
        {
            //Cylinder Down
            if (bContact)
            {
                Cyl1.Forward();
                Cyl2.Forward();
                Cyl3.Forward();
                Cyl4.Forward();
            }
            //Cylinder Up
            else
            {
                Cyl1.Backward();
                Cyl2.Backward();
                Cyl3.Backward();
                Cyl4.Backward();
            }
        }

        private void Sequence_AutoRun()
        {
            switch ((ERobotUnloadAutoRunStep)Step.RunStep)
            {
                case ERobotUnloadAutoRunStep.Start:
                    Log.Debug("AutoRun Start");
                    Step.RunStep++;
                    break;
                case ERobotUnloadAutoRunStep.GlassVac_Check:
                    if (GlassVac1 || GlassVac2 || GlassVac3 || GlassVac4)
                    {
                        PlasmaPrepare();
                        Log.Info("Sequence Unload Robot Plasma");
                        Sequence = ESequence.UnloadRobotPlasma;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotUnloadAutoRunStep.End:
                    Log.Info("Unload Robot Pick");
                    Sequence = ESequence.UnloadRobotPick;
                    break;
            }
        }

        private void Sequence_UnloadRobotPick()
        {
            switch ((ERobotUnloadPickStep)Step.RunStep)
            {
                case ERobotUnloadPickStep.Start:
                    Log.Debug("Unload Robot Pick Start");
                    Step.RunStep++;
                    Log.Debug("Wait Unload Align Request Unload");
                    break;
                case ERobotUnloadPickStep.Wait_UnloadAlignRequestUnload:
                    if (FlagUnloadAlignRequestUnload == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Robot_Move_PickPosition:
                    Log.Debug("Robot Move Pick Position");
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Robot_Move_PickPosition_Wait:
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Cylinder_Down:
                    Log.Debug("Cylinders Down");
                    CylinderContact(true);
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => IsCylindersDown);
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Cylinder_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)EWarning.RobotUnload_Cylinder_Down_Fail);
                        break;
                    }
                    Log.Debug("Cylinders Down Done");
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    VacuumOnOff(true);
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac1, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac2, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac3, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac4, true);

                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect1, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect2, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect3, true);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect4, true);
#endif
                    Wait(_machineStatus.GetVacuumDelay(_commonRecipe.VacDelay, RobotVacuumInputs));
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Vacuum_On_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        break;
                    }

                    _machineStatus.ReleaseVacuumOutputsIfBypassed(RobotVacuumInputs,
                        GlassVacOnOff1,
                        GlassVacOnOff2,
                        GlassVacOnOff3,
                        GlassVacOnOff4);

                    if (!_machineStatus.ShouldBypassVacuum(RobotVacuumInputs) && !AreRobotVacuumsActive)
                    {
                        RaiseWarning((int)EWarning.RobotUnload_Vacuum_On_Fail);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.GlassDetect_Check:
                    Log.Debug("Glass Detect Check");
                    if (GlassDetect1 == false || GlassDetect2 == false || GlassDetect3 == false || GlassDetect4 == false)
                    {
                        RaiseWarning((int)EWarning.RobotUnload_Pick_Fail);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Set_FlagRobotPickDone:
                    Log.Debug("Set Flag Robot Pick Done");
                    FlagRobotUnloadPickDone = true;
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Wait_UnloadAlign_PickDoneReceived:
                    if (FlagRobotUnloadDoneReceived == false)
                    {
                        break;
                    }
                    Log.Debug("Clear Flag Robot Pick Done");
                    FlagRobotUnloadPickDone = false;
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Plasma_Prepare:
                    PlasmaPrepare();
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Robot_Move_ReadyPlasmaPosition:
                    Log.Debug("Robot Move Ready Plasma Position");
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.Robot_Move_ReadyPlasmaPosition_Wait:
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case ERobotUnloadPickStep.End:
                    Log.Debug("Unload Robot Pick End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Unload Robot Plasma");
                    Sequence = ESequence.UnloadRobotPlasma;
                    break;
            }
        }

        private void PlasmaPrepare()
        {
            int plasmaPrepareStep = 0;

            Task plasmaPrepareTask = Task.Run(async () =>
            {
                bool running = true;

                while (running)
                {
                    switch ((EPlasmaPrepareStep)plasmaPrepareStep)
                    {
                        case EPlasmaPrepareStep.Start:
                            Log.Debug("Plasma Prepare Start");
                            plasmaPrepareStep++;
                            break;

                        case EPlasmaPrepareStep.Air_Valve_Open:
                            _plasma.AirOpenClose(true);
                            await Task.Delay(500);
                            plasmaPrepareStep++;
                            break;

                        case EPlasmaPrepareStep.Plasma_On:
                            _plasma.PlasmaOnOff(true);
                            await Task.Delay(500);
                            plasmaPrepareStep++;
                            break;

                        case EPlasmaPrepareStep.End:
                            Log.Debug("Plasma Prepare End");
                            IsPlasmaPrepare = true;
                            running = false;
                            break;
                    }
                }
            });
        }

        private void Sequence_UnloadRobotPlasma()
        {
            switch ((ERobotUnloadPlasmaStep)Step.RunStep)
            {
                case ERobotUnloadPlasmaStep.Start:
                    Log.Debug("Unload Robot Plasma Start");
                    Step.RunStep++;
                    break;
                case ERobotUnloadPlasmaStep.Wait_PlasmaPrepareDone:
                    if (IsPlasmaPrepare == false)
                    {
                        Wait(20);
                        break;
                    }
                    IsPlasmaPrepare = false;
                    Step.RunStep++;
                    break;
                case ERobotUnloadPlasmaStep.Robot_Move_PlasmaPosition:
                    Log.Debug("Robot Move Plasma Position");
                    Step.RunStep++;
                    break;
                case ERobotUnloadPlasmaStep.Robot_Move_PlasmaPosition_Wait:
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case ERobotUnloadPlasmaStep.End:
                    Log.Debug("Plasma End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }

                    Log.Info("Unload Robot Place");
                    Sequence = ESequence.UnloadRobotPlace;
                    break;
            }
        }

        private void Sequence_UnloadRobotPlace()
        {
            switch ((EUnloadRobotPlaceStep)Step.RunStep)
            {
                case EUnloadRobotPlaceStep.Start:
                    Log.Debug("Unload Robot Place Start");
                    Step.RunStep++;
                    break;
                case EUnloadRobotPlaceStep.Wait_MachineRequestPlace:
                    if (_devices.Inputs.RobotUnload.Value == false)
                    {
                        Wait(20);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case EUnloadRobotPlaceStep.Robot_Move_PlacePosition:
                    Log.Debug("Robot Move Place Position");
                    Step.RunStep++;
                    break;
                case EUnloadRobotPlaceStep.Robot_Move_PlacePosition_Wait:
                    Wait(1000);
                    Step.RunStep++;
                    break;
                case EUnloadRobotPlaceStep.VacuumOff:
                    Log.Debug("Vacuum Off");
                    VacuumOnOff(false);
                    Step.RunStep++;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac1, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac2, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac3, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotVac4, false);

                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect1, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect2, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect3, false);
                    SimulationInputSetter.SetSimInput(_devices.Inputs.UnloadRobotDetect4, false);
#endif
                    break;
                case EUnloadRobotPlaceStep.End:
                    Log.Debug("Unload Robot Place End");
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    Log.Info("Sequence Unload Robot Pick");
                    Sequence = ESequence.UnloadRobotPick;
                    break;
            }
        }
        #endregion
    }
}
