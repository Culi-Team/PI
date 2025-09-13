using EQX.Core.Device.Regulator;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.Core.TorqueController;
using EQX.InOut;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
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
    public class CleanProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;
        private readonly CleanRecipe _wetCleanLeftRecipe;
        private readonly CleanRecipe _wetCleanRightRecipe;
        private readonly CleanRecipe _afCleanLeftRecipe;
        private readonly CleanRecipe _afCleanRightRecipe;
        private readonly IDInputDevice _wetCleanLeftInput;
        private readonly IDOutputDevice _wetCleanLeftOutput;
        private readonly IDInputDevice _wetCleanRightInput;
        private readonly IDOutputDevice _wetCleanRightOutput;
        private readonly IDInputDevice _afCleanLeftInput;
        private readonly IDOutputDevice _afCleanLeftOutput;
        private readonly IDInputDevice _afCleanRightInput;
        private readonly IDOutputDevice _afCleanRightOutput;

        private bool Is3MPrepareDone { get; set; } = false;

        private EClean cleanType
        {
            get
            {
                return Name switch
                {
                    nameof(EProcess.WETCleanLeft) => EClean.WETCleanLeft,
                    nameof(EProcess.WETCleanRight) => EClean.WETCleanRight,
                    nameof(EProcess.AFCleanLeft) => EClean.AFCleanLeft,
                    nameof(EProcess.AFCleanRight) => EClean.AFCleanRight,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private ITorqueController UnWinder
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.TorqueControllers.WETCleanLeftUnWinder,
                    EClean.WETCleanRight => _devices.TorqueControllers.WETCleanRightUnWinder,
                    EClean.AFCleanLeft => _devices.TorqueControllers.AFCleanLeftUnWinder,
                    EClean.AFCleanRight => _devices.TorqueControllers.AFCleanRightUnWinder,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private ITorqueController Winder
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.TorqueControllers.WETCleanLeftWinder,
                    EClean.WETCleanRight => _devices.TorqueControllers.WETCleanRightWinder,
                    EClean.AFCleanLeft => _devices.TorqueControllers.AFCleanLeftWinder,
                    EClean.AFCleanRight => _devices.TorqueControllers.AFCleanRightWinder,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private IMotion FeedingAxis
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.MotionsInovance.WETCleanLFeedingAxis,
                    EClean.WETCleanRight => _devices.MotionsInovance.WETCleanRFeedingAxis,
                    EClean.AFCleanLeft => _devices.MotionsInovance.AFCleanLFeedingAxis,
                    EClean.AFCleanRight => _devices.MotionsInovance.AFCleanRFeedingAxis,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private IMotion XAxis
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.MotionsAjin.InShuttleLXAxis,
                    EClean.WETCleanRight => _devices.MotionsAjin.InShuttleRXAxis,
                    EClean.AFCleanLeft => _devices.MotionsAjin.OutShuttleLXAxis,
                    EClean.AFCleanRight => _devices.MotionsAjin.OutShuttleRXAxis,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private IMotion YAxis
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.MotionsAjin.InShuttleLYAxis,
                    EClean.WETCleanRight => _devices.MotionsAjin.InShuttleRYAxis,
                    EClean.AFCleanLeft => _devices.MotionsAjin.OutShuttleLYAxis,
                    EClean.AFCleanRight => _devices.MotionsAjin.OutShuttleRYAxis,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private IMotion TAxis
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.MotionsInovance.InShuttleLTAxis,
                    EClean.WETCleanRight => _devices.MotionsInovance.InShuttleRTAxis,
                    EClean.AFCleanLeft => _devices.MotionsInovance.OutShuttleLTAxis,
                    EClean.AFCleanRight => _devices.MotionsInovance.OutShuttleRTAxis,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private ICylinder PushCyl
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Cylinders.WetCleanPusherLeftUpDown,
                    EClean.WETCleanRight => _devices.Cylinders.WetCleanPusherRightUpDown,
                    EClean.AFCleanLeft => _devices.Cylinders.AFCleanPusherLeftUpDown,
                    EClean.AFCleanRight => _devices.Cylinders.AFCleanPusherRightUpDown,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private IRegulator Regulator
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Regulators.WetCleanLRegulator,
                    EClean.WETCleanRight => _devices.Regulators.WetCleanRRegulator,
                    EClean.AFCleanLeft => _devices.Regulators.AfCleanLRegulator,
                    EClean.AFCleanRight => _devices.Regulators.AfCleanRRegulator,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private IDOutput GlassVac
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Outputs.InShuttleLVacOnOff,
                    EClean.WETCleanRight => _devices.Outputs.InShuttleRVacOnOff,
                    EClean.AFCleanLeft => _devices.Outputs.OutShuttleLVacOnOff,
                    EClean.AFCleanRight => _devices.Outputs.OutShuttleRVacOnOff,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private IDInput VacDetect
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Inputs.InShuttleLVac,
                    EClean.WETCleanRight => _devices.Inputs.InShuttleRVac,
                    EClean.AFCleanLeft => _devices.Inputs.OutShuttleLVac,
                    EClean.AFCleanRight => _devices.Inputs.OutShuttleRVac,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private bool IsVacDetect => VacDetect.Value;

        private bool IsLeakDetect
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Inputs.WetCleanLeftPumpLeakDetect.Value || _devices.Inputs.WetCleanLeftAlcoholLeakDetect.Value,
                    EClean.WETCleanRight => _devices.Inputs.WetCleanRightPumpLeakDetect.Value || _devices.Inputs.WetCleanRightAlcoholLeakDetect.Value,
                    EClean.AFCleanLeft => _devices.Inputs.AfCleanLeftPumpLeakDetect.Value || _devices.Inputs.AfCleanLeftAlcoholLeakDetect.Value,
                    EClean.AFCleanRight => _devices.Inputs.AfCleanRightPumpLeakDetect.Value || _devices.Inputs.AfCleanRightAlcoholLeakDetect.Value,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private CleanRecipe cleanRecipe
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _wetCleanLeftRecipe,
                    EClean.WETCleanRight => _wetCleanRightRecipe,
                    EClean.AFCleanLeft => _afCleanLeftRecipe,
                    EClean.AFCleanRight => _afCleanRightRecipe,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private IDInputDevice Inputs
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _wetCleanLeftInput,
                    EClean.WETCleanRight => _wetCleanRightInput,
                    EClean.AFCleanLeft => _afCleanLeftInput,
                    EClean.AFCleanRight => _afCleanRightInput,
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private IDOutputDevice Outputs
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _wetCleanLeftOutput,
                    EClean.WETCleanRight => _wetCleanRightOutput,
                    EClean.AFCleanLeft => _afCleanLeftOutput,
                    EClean.AFCleanRight => _afCleanRightOutput,
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private double XAxisLoadPosition => cleanRecipe.XAxisLoadPosition;
        private double YAxisLoadPosition => cleanRecipe.YAxisLoadPosition;
        private double TAxisLoadPosition => cleanRecipe.TAxisLoadPosition;

        private double XAxisUnloadPosition => cleanRecipe.XAxisUnloadPosition;
        private double YAxisUnloadPosition => cleanRecipe.YAxisUnloadPosition;
        private double TAxisUnloadPosition => cleanRecipe.TAxisUnloadPosition;

        private double XAxisCleanHorizontalPosition => cleanRecipe.XAxisCleanHorizontalPosition;
        private double YAxisCleanHorizontalPosition => cleanRecipe.YAxisCleanHorizontalPosition;
        private double TAxisCleanHorizontalPosition => cleanRecipe.TAxisCleanHorizontalPosition;

        private double XAxisCleanVerticalPosition => cleanRecipe.XAxisCleanVerticalPosition;
        private double YAxisCleanVerticalPosition => cleanRecipe.YAxisCleanVerticalPosition;
        private double TAxisCleanVerticalPosition => cleanRecipe.TAxisCleanHorizontalPosition;
        #endregion

        #region Flags
        private bool FlagCleanRequestLoad
        {
            set
            {
                Outputs[(int)ECleanProcessOutput.REQ_LOAD] = value;
            }
        }

        private bool FlagCleanLoadDoneReceived
        {
            set
            {
                Outputs[(int)ECleanProcessOutput.LOAD_DONE_RECEIVED] = value;
            }
        }

        private bool FlagCleanLoadDone
        {
            get
            {
                return Inputs[(int)ECleanProcessInput.LOAD_DONE];
            }
        }

        private bool FlagCleanRequestUnload
        {
            set
            {
                Outputs[(int)ECleanProcessOutput.REQ_UNLOAD] = value;
            }
        }

        private bool FlagCleanUnloadDoneReceived
        {
            set
            {
                Outputs[(int)ECleanProcessOutput.UNLOAD_DONE_RECEIVED] = value;
            }
        }

        private bool FlagCleanUnloadDone
        {
            get
            {
                return Inputs[(int)ECleanProcessInput.UNLOAD_DONE];
            }
        }

        private bool FlagTransferRotationReadyPickPlace
        {
            get
            {
                return Inputs[(int)ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE];
            }
        }

        private bool FlagCleanLoading
        {
            set
            {
                if (cleanType == EClean.AFCleanLeft || cleanType == EClean.AFCleanRight)
                {
                    Outputs[(int)ECleanProcessOutput.AF_CLEAN_LOADING] = value;
                }
            }
        }

        private bool FlagCleanUnloading
        {
            set
            {
                if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                {
                    Outputs[(int)ECleanProcessOutput.WET_CLEAN_UNLOADING] = value;
                }
            }
        }
        #endregion

        #region Constructor
        public CleanProcess(Devices devices,
            CommonRecipe commonRecipe,
            [FromKeyedServices("WETCleanLeftRecipe")] CleanRecipe wetCleanLeftRecipe,
            [FromKeyedServices("WETCleanRightRecipe")] CleanRecipe wetCleanRightRecipe,
            [FromKeyedServices("AFCleanLeftRecipe")] CleanRecipe afCleanLeftRecipe,
            [FromKeyedServices("AFCleanRightRecipe")] CleanRecipe afCleanRightRecipe,
            [FromKeyedServices("WETCleanLeftInput")] IDInputDevice wetCleanLeftInput,
            [FromKeyedServices("WETCleanLeftOutput")] IDOutputDevice wetCleanLeftOutput,
            [FromKeyedServices("WETCleanRightInput")] IDInputDevice wetCleanRightInput,
            [FromKeyedServices("WETCleanRightOutput")] IDOutputDevice wetCleanRightOutput,
            [FromKeyedServices("AFCleanLeftInput")] IDInputDevice afCleanLeftInput,
            [FromKeyedServices("AFCleanLeftOutput")] IDOutputDevice afCleanLeftOutput,
            [FromKeyedServices("AFCleanRightInput")] IDInputDevice afCleanRightInput,
            [FromKeyedServices("AFCleanRightOutput")] IDOutputDevice afCleanRightOutput)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
            _wetCleanLeftRecipe = wetCleanLeftRecipe;
            _wetCleanRightRecipe = wetCleanRightRecipe;
            _afCleanLeftRecipe = afCleanLeftRecipe;
            _afCleanRightRecipe = afCleanRightRecipe;
            _wetCleanLeftInput = wetCleanLeftInput;
            _wetCleanLeftOutput = wetCleanLeftOutput;
            _wetCleanRightInput = wetCleanRightInput;
            _wetCleanRightOutput = wetCleanRightOutput;
            _afCleanLeftInput = afCleanLeftInput;
            _afCleanLeftOutput = afCleanLeftOutput;
            _afCleanRightInput = afCleanRightInput;
            _afCleanRightOutput = afCleanRightOutput;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch ((ECleanOriginStep)Step.OriginStep)
            {
                case ECleanOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.PushCyl_Up:
                    Log.Debug("Push Cylinder Up");
                    PushCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return PushCyl.IsBackward; });
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.PushCyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Push Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.AxisOrigin:
                    Log.Debug("X Axis Origin Start");
                    Log.Debug("Y Axis Origin Start");
                    Log.Debug("T Axis Origin Start");
                    Log.Debug("Feeding Axis Origin Start");
                    XAxis.SearchOrigin();
                    YAxis.SearchOrigin();
                    TAxis.SearchOrigin();
                    FeedingAxis.SearchOrigin();
                    Wait(_commonRecipe.MotionOriginTimeout, () => { return XAxis.Status.IsHomeDone && YAxis.Status.IsHomeDone && TAxis.Status.IsHomeDone && FeedingAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.AxisOrigin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("X Axis Origin Done");
                    Log.Debug("Y Axis Origin Done");
                    Log.Debug("T Axis Origin Done");
                    Log.Debug("Feeding Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.End:
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
                case ESequence.GlassTransferPick:
                    break;
                case ESequence.GlassTransferPlace:
                    break;
                case ESequence.AlignGlass:
                    break;
                case ESequence.TransferInShuttlePick:
                    break;
                case ESequence.WETCleanLoad:
                    Sequence_Load();
                    break;
                case ESequence.WETClean:
                    Sequence_Clean();
                    break;
                case ESequence.WETCleanUnload:
                    Sequence_Unload();
                    break;
                case ESequence.TransferRotation:
                    break;
                case ESequence.AFCleanLoad:
                    Sequence_Load();
                    break;
                case ESequence.AFClean:
                    Sequence_Clean();
                    break;
                case ESequence.AFCleanUnload:
                    Sequence_Unload();
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
            switch ((ECleanProcessAutoRunStep)Step.RunStep)
            {
                case ECleanProcessAutoRunStep.Start:
                    Log.Debug("Auto Run Start");
                    Step.RunStep++;
                    break;
                case ECleanProcessAutoRunStep.VacDetect_Check:
                    if (IsVacDetect)
                    {
                        Sequence_Prepare3M();
                        if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                        {
                            Log.Info("Sequence WET Clean");
                            Sequence = ESequence.WETClean;
                            break;
                        }

                        Log.Info("Sequence AF Clean");
                        Sequence = ESequence.AFClean;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessAutoRunStep.End:
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Log.Info("Sequence WET Clean Load");
                        Sequence = ESequence.WETCleanLoad;
                        break;
                    }
                    Log.Info("Sequence AF Clean Load");
                    Sequence = ESequence.AFCleanLoad;
                    break;
            }
        }

        private void Sequence_Load()
        {
            switch ((ECleanProcessLoadStep)Step.RunStep)
            {
                case ECleanProcessLoadStep.Start:
                    Log.Debug("Clean Load Start");

                    Log.Debug("Clear Flag Clean Load Done Received");

                    if (cleanType == EClean.AFCleanLeft || cleanType == EClean.AFCleanRight)
                    {
                        Log.Debug("Wait Transfer Rotation Ready Place");
                    }

                    FlagCleanLoadDoneReceived = false;
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.Wait_WETCleanUnloadDone:
                    if (cleanType == EClean.AFCleanLeft || cleanType == EClean.AFCleanRight)
                    {
                        if (FlagTransferRotationReadyPickPlace == false)
                        {
                            Wait(20);
                            break;
                        }
                    }
                    Log.Debug("Set Flag Loading");
                    FlagCleanLoading = true;
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.AxisMoveLoadPosition:
                    Log.Debug("X Y T Axis Move Load Position");
                    XAxis.MoveAbs(XAxisLoadPosition);
                    YAxis.MoveAbs(YAxisLoadPosition);
                    TAxis.MoveAbs(TAxisLoadPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => XAxis.IsOnPosition(XAxisLoadPosition) && YAxis.IsOnPosition(YAxisLoadPosition) && TAxis.IsOnPosition(TAxisLoadPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.AxisMoveLoadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("X Y T Axis Move Load Position Done");

                    Log.Debug("Clear Flag Unloading");
                    FlagCleanUnloading = false;
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.Set_FlagCleanRequestLoad:
                    Log.Debug("Set Flag Request Load");
                    FlagCleanRequestLoad = true;
                    Log.Debug("Wait Clean Load Glass Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.Wait_CleanLoadDone:
                    if (FlagCleanLoadDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Sequence_Prepare3M();
                    Log.Debug("Clear Flag Request Load");
                    FlagCleanRequestLoad = false;
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.Set_FlagCleanLoadDoneReceived:
                    Log.Debug("Set Flag Clean Load Done Received");
                    FlagCleanLoadDoneReceived = true;
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    GlassVac.Value = true;
#if SIMULATION
                    SimulationInputSetter.SetSimModbusInput(VacDetect, true);
#endif
                    Wait(_commonRecipe.VacDelay);
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Log.Info("Sequence WET Clean");
                        Sequence = ESequence.WETClean;
                        break;
                    }
                    Log.Info("Sequence AF Clean");
                    Sequence = ESequence.AFClean;
                    break;
            }
        }

        private void Sequence_Clean()
        {
            switch ((ECleanProcessCleanStep)Step.RunStep)
            {
                case ECleanProcessCleanStep.Start:
                    Log.Debug("Clean Start");
                    Log.Debug("Wait 3M Prepare Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Wait_3M_PrepareDone:
                    if (Is3MPrepareDone == false)
                    {
                        Wait(20);
                        break;
                    }
                    Is3MPrepareDone = false;
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Axis_MoveCleanHorizontalPosition:
                    Log.Debug("X Y T Axis Move Clean Horizontal Position");
                    XAxis.MoveAbs(XAxisCleanHorizontalPosition);
                    YAxis.MoveAbs(YAxisCleanHorizontalPosition);
                    TAxis.MoveAbs(TAxisCleanHorizontalPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => XAxis.IsOnPosition(XAxisCleanHorizontalPosition) &&
                                                               YAxis.IsOnPosition(YAxisCleanHorizontalPosition) &&
                                                               TAxis.IsOnPosition(TAxisCleanHorizontalPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Axis_MoveCleanHorizontalPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("X Y T Axis Move Clean Horizontal Position");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Down_CleanHorizontal:
                    Log.Debug("Cylinder Pusher Down");
                    PushCyl.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => PushCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Down_CleanHorizontal_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Pusher Down Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CleanHorizontal:
                    Log.Debug("Clean Horizontal");
                    FeedingAxis.MoveJog(cleanRecipe.RFeedingAxisCleaningSpeed, true);
#if !SIMULATION
                    _devices.MotionsAjin.CleanHorizontal(cleanType, XAxisCleanHorizontalPosition, YAxisCleanHorizontalPosition, 50, 10, cleanRecipe.CleanHorizontalCount);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => _devices.MotionsAjin.IsContiMotioning(cleanType) == false);
#endif
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CleanHorizontal_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    FeedingAxis.Stop();
                    Log.Debug("Clean Horizontal Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Up_CleanHorizontal:
                    Log.Debug("Cylinder Pusher Up");
                    PushCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => PushCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusherUp_CleanHorizontal_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Pusher Up Done");
                    if (cleanRecipe.IsCleanVertical)
                    {
                        Step.RunStep++;
                        break;
                    }
                    Step.RunStep = (int)ECleanProcessCleanStep.End;
                    break;
                case ECleanProcessCleanStep.Axis_Move_CleanVerticalPosition:
                    Log.Debug("X Y T Axis Move Clean Vertical Position");
                    XAxis.MoveAbs(XAxisCleanVerticalPosition);
                    YAxis.MoveAbs(YAxisCleanVerticalPosition);
                    TAxis.MoveAbs(TAxisCleanVerticalPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => XAxis.IsOnPosition(XAxisCleanVerticalPosition) &&
                                                               YAxis.IsOnPosition(YAxisCleanVerticalPosition) &&
                                                               TAxis.IsOnPosition(TAxisCleanVerticalPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Axis_Move_CleanVerticalPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("X Y T Axis Move Clean Vertical Position Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Down_CleanVertical:
                    Log.Debug("Cylinder Pusher Down");
                    PushCyl.Forward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => PushCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Down_CleanVertical_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Pusher Down Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CleanVertical:
                    Log.Debug("Clean Vertical");
#if !SIMULATION
                    _devices.MotionsAjin.CleanVertical(cleanType,XAxisCleanVerticalPosition,YAxisCleanVerticalPosition,cleanRecipe.CleanVerticalCount);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => _devices.MotionsAjin.IsContiMotioning(cleanType));
#endif
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CleanVertical_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Clean Vertical Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Up_CleanVertical:
                    Log.Debug("Cylinder Pusher Up");
                    PushCyl.Backward();
                    Wait(_commonRecipe.CylinderMoveTimeout, () => PushCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Up_CleanVertical_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("Cylinder Pusher Up Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Log.Info("Sequence WET Clean Unload");
                        Sequence = ESequence.WETCleanUnload;
                        break;
                    }
                    Log.Info("Sequence AF Clean Unload");
                    Sequence = ESequence.AFCleanUnload;
                    break;
            }
        }

        private void Sequence_Unload()
        {
            switch ((ECleanProcessUnloadStep)Step.RunStep)
            {
                case ECleanProcessUnloadStep.Start:
                    Log.Debug("Unload Start");
                    Log.Debug("Clear Flag Unload Done Received");
                    FlagCleanUnloadDoneReceived = false;
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Log.Debug("Wait Transfer Rotation Ready Pick");
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.Wait_AFCleanLoadDone:
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        if (FlagTransferRotationReadyPickPlace == false)
                        {
                            Wait(20);
                            break;
                        }
                    }

                    Log.Debug("Set Flag Unloading");
                    FlagCleanUnloading = true;
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.AxisMoveUnloadPosition:
                    Log.Debug("Axis Move Unload Position");
                    XAxis.MoveAbs(XAxisUnloadPosition);
                    YAxis.MoveAbs(YAxisUnloadPosition);
                    TAxis.MoveAbs(TAxisUnloadPosition);
                    Wait(_commonRecipe.MotionMoveTimeOut, () => XAxis.IsOnPosition(XAxisUnloadPosition) &&
                                                               YAxis.IsOnPosition(YAxisUnloadPosition) &&
                                                               TAxis.IsOnPosition(TAxisUnloadPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.AxisMoveUnloadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Log.Debug("X Y T Axis Move Unload Position Done");

                    Log.Debug("Clear Flag Loading");
                    FlagCleanLoading = false;
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    GlassVac.Value = false;
#if SIMULATION
                    SimulationInputSetter.SetSimModbusInput(VacDetect, false);
#endif
                    Wait(_commonRecipe.VacDelay);
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.Set_FlagRequestUnload:
                    Log.Debug("Set Flag Request Unload");
                    FlagCleanRequestUnload = true;
                    Log.Debug("Wait Clean Unload Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.Wait_CleanUnloadDone:
                    if (FlagCleanUnloadDone == false)
                    {
                        break;
                    }
                    Log.Debug("Set Flag Unload Done Received");
                    FlagCleanUnloadDoneReceived = true;

                    Log.Debug("Clear Flag Request Unload");
                    FlagCleanRequestUnload = false;
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.End:
                    if (Parent!.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Log.Info("Sequence WET Clean Load");
                        Sequence = ESequence.WETCleanLoad;
                        break;
                    }
                    Log.Info("Sequence AF Clean Load");
                    Sequence = ESequence.AFCleanLoad;
                    break;
            }
        }

        private void Sequence_Prepare3M()
        {
            Log.Debug("Prepare 3M");
            Is3MPrepareDone = true;
        }
        #endregion
    }
}
