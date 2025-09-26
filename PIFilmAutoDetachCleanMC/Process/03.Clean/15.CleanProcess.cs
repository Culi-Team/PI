using EQX.Core.Device.Regulator;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.Core.TorqueController;
using EQX.InOut;
using EQX.InOut.Virtual;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Recipe;
using PIFilmAutoDetachCleanMC.Services.DryRunServices;
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
        private readonly MachineStatus _machineStatus;

        private bool Is3MPrepareDone { get; set; } = false;

        private EPort port => cleanType == EClean.WETCleanLeft || cleanType == EClean.AFCleanLeft ? EPort.Left : EPort.Right;
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

        private bool FeedingRollerDetect
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _machineStatus.IsSatisfied(_devices.Inputs.WetCleanLeftFeedingRollerDetect),
                    EClean.WETCleanRight => _machineStatus.IsSatisfied(_devices.Inputs.WetCleanRightFeedingRollerDetect),
                    EClean.AFCleanLeft => _machineStatus.IsSatisfied(_devices.Inputs.AfCleanLeftFeedingRollerDetect),
                    EClean.AFCleanRight => _machineStatus.IsSatisfied(_devices.Inputs.AfCleanRightFeedingRollerDetect),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private bool WiperCleanDetect1
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _machineStatus.IsSatisfied(_devices.Inputs.WetCleanLeftWiperCleanDetect1),
                    EClean.WETCleanRight => _machineStatus.IsSatisfied(_devices.Inputs.WetCleanRightWiperCleanDetect1),
                    EClean.AFCleanLeft => _machineStatus.IsSatisfied(_devices.Inputs.AfCleanLeftWiperCleanDetect1),
                    EClean.AFCleanRight => _machineStatus.IsSatisfied(_devices.Inputs.AfCleanRightWiperCleanDetect1),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private bool WiperCleanDetect2
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _machineStatus.IsSatisfied(_devices.Inputs.WetCleanLeftWiperCleanDetect2),
                    EClean.WETCleanRight => _machineStatus.IsSatisfied(_devices.Inputs.WetCleanRightWiperCleanDetect2),
                    EClean.AFCleanLeft => _machineStatus.IsSatisfied(_devices.Inputs.AfCleanLeftWiperCleanDetect2),
                    EClean.AFCleanRight => _machineStatus.IsSatisfied(_devices.Inputs.AfCleanRightWiperCleanDetect2),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private bool WiperCleanDetect3
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _machineStatus.IsSatisfied(_devices.Inputs.WetCleanLeftWiperCleanDetect3),
                    EClean.WETCleanRight => _machineStatus.IsSatisfied(_devices.Inputs.WetCleanRightWiperCleanDetect3),
                    EClean.AFCleanLeft => _machineStatus.IsSatisfied(_devices.Inputs.AfCleanLeftWiperCleanDetect3),
                    EClean.AFCleanRight => _machineStatus.IsSatisfied(_devices.Inputs.AfCleanRightWiperCleanDetect3),
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
                    EClean.WETCleanLeft => _machineStatus.IsSatisfied(_devices.Inputs.WetCleanLeftPumpLeakDetect) || _machineStatus.IsSatisfied(_devices.Inputs.WetCleanLeftAlcoholLeakDetect),
                    EClean.WETCleanRight => _machineStatus.IsSatisfied(_devices.Inputs.WetCleanRightPumpLeakDetect) || _machineStatus.IsSatisfied(_devices.Inputs.WetCleanRightAlcoholLeakDetect),
                    EClean.AFCleanLeft => _machineStatus.IsSatisfied(_devices.Inputs.AfCleanLeftPumpLeakDetect) || _machineStatus.IsSatisfied(_devices.Inputs.AfCleanLeftAlcoholLeakDetect),
                    EClean.AFCleanRight => _machineStatus.IsSatisfied(_devices.Inputs.AfCleanRightPumpLeakDetect) || _machineStatus.IsSatisfied(_devices.Inputs.AfCleanRightAlcoholLeakDetect),
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

        #endregion

        #region Constructor
        public CleanProcess(Devices devices,
            CommonRecipe commonRecipe,
            MachineStatus machineStatus,
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
            _machineStatus = machineStatus;
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
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => { return PushCyl.IsBackward; });
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.PushCyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_PusherCylinder_Up_Fail,
                            EClean.WETCleanRight => EWarning.WETCleanRight_PusherCylinder_Up_Fail,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_PusherCylinder_Up_Fail,
                            EClean.AFCleanRight => EWarning.AFCleanRight_PusherCylinder_Up_Fail,
                            _ => null
                        };

                        RaiseWarning((int)warning!);
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
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => { return XAxis.Status.IsHomeDone && YAxis.Status.IsHomeDone && TAxis.Status.IsHomeDone && FeedingAxis.Status.IsHomeDone; });
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.AxisOrigin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (XAxis.Status.IsHomeDone == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_XAxis_Origin_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_XAxis_Origin_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_XAxis_Origin_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_XAxis_Origin_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        if (YAxis.Status.IsHomeDone == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_YAxis_Origin_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_YAxis_Origin_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_YAxis_Origin_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_YAxis_Origin_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }

                        if (TAxis.Status.IsHomeDone == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_TAxis_Origin_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_TAxis_Origin_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_TAxis_Origin_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_TAxis_Origin_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }

                        if (FeedingAxis.Status.IsHomeDone == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_FeedingAxis_Origin_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_FeedingAxis_Origin_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_FeedingAxis_Origin_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_FeedingAxis_Origin_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }

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
                    Sequence = ESequence.Stop;
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
                case ESequence.AlignGlassLeft:
                    break;
                case ESequence.AlignGlassRight:
                    break;
                case ESequence.TransferInShuttleLeftPick:
                    break;
                case ESequence.TransferInShuttleRightPick:
                    break;
                case ESequence.WETCleanLeftLoad:
                    if (cleanType == EClean.WETCleanLeft)
                    {
                        Sequence_Load();
                    }
                    break;
                case ESequence.WETCleanRightLoad:
                    if (cleanType == EClean.WETCleanRight)
                    {
                        Sequence_Load();
                    }
                    break;
                case ESequence.WETCleanLeft:
                    if (cleanType == EClean.WETCleanLeft)
                    {
                        Sequence_Clean();
                    }
                    break;
                case ESequence.WETCleanRight:
                    if (cleanType == EClean.WETCleanRight)
                    {
                        Sequence_Clean();
                    }
                    break;
                case ESequence.WETCleanLeftUnload:
                    if (cleanType == EClean.WETCleanLeft)
                    {
                        Sequence_Unload();
                    }
                    break;
                case ESequence.WETCleanRightUnload:
                    if (cleanType == EClean.WETCleanRight)
                    {
                        Sequence_Unload();
                    }
                    break;
                case ESequence.TransferRotationLeft:
                    break;
                case ESequence.TransferRotationRight:
                    break;
                case ESequence.AFCleanLeftLoad:
                    if (cleanType == EClean.AFCleanLeft)
                    {
                        Sequence_Load();
                    }
                    break;
                case ESequence.AFCleanRightLoad:
                    if (cleanType == EClean.AFCleanRight)
                    {
                        Sequence_Load();
                    }
                    break;
                case ESequence.AFCleanLeft:
                    if (cleanType == EClean.AFCleanLeft)
                    {
                        Sequence_Clean();
                    }
                    break;
                case ESequence.AFCleanRight:
                    if (cleanType == EClean.AFCleanRight)
                    {
                        Sequence_Clean();
                    }
                    break;
                case ESequence.AFCleanLeftUnload:
                    if (cleanType == EClean.AFCleanLeft)
                    {
                        Sequence_Unload();
                    }
                    break;
                case ESequence.AFCleanRightUnload:
                    if (cleanType == EClean.AFCleanRight)
                    {
                        Sequence_Unload();
                    }
                    break;
                case ESequence.UnloadTransferLeftPlace:
                    break;
                case ESequence.UnloadTransferRightPlace:
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

        public override bool ProcessToRun()
        {
            switch ((ECleanProcessToRunStep)Step.ToRunStep)
            {
                case ECleanProcessToRunStep.Start:
                    Log.Debug("To Run Start");
                    Step.ToRunStep++;
                    break;
                case ECleanProcessToRunStep.FeedingRollerDetect_Check:
                    Log.Debug("Feeding Roller Detect Check");
                    if (FeedingRollerDetect == false)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_FeedingRoller_NotDetect,
                            EClean.WETCleanRight => EWarning.WETCleanRight_FeedingRoller_NotDetect,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_FeedingRoller_NotDetect,
                            EClean.AFCleanRight => EWarning.AFCleanRight_FeedingRoller_NotDetect,
                            _ => null
                        };

                        RaiseWarning((int)warning!);
                        break;
                    }
                    Step.ToRunStep++;
                    break;
                case ECleanProcessToRunStep.Wiper_Check:
                    Log.Debug("Wiper Clean Detect Check");
                    if (WiperCleanDetect1 == false || WiperCleanDetect2 == false || WiperCleanDetect3 == false)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_WiperClean_NotDetect,
                            EClean.WETCleanRight => EWarning.WETCleanRight_WiperClean_NotDetect,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_WiperClean_NotDetect,
                            EClean.AFCleanRight => EWarning.AFCleanRight_WiperClean_NotDetect,
                            _ => null
                        };

                        RaiseWarning((int)warning!);
                        break;
                    }
                    Step.ToRunStep++;
                    break;
                case ECleanProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((VirtualOutputDevice<ECleanProcessOutput>)Outputs).Clear();
                    Step.ToRunStep++;
                    break;
                case ECleanProcessToRunStep.End:
                    Log.Debug("To Run End");
                    Step.ToRunStep++;
                    ProcessStatus = EProcessStatus.ToRunDone;
                    break;
                default:
                    Thread.Sleep(20);
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
                            if (cleanType == EClean.WETCleanLeft)
                            {
                                Sequence = ESequence.WETCleanLeft;
                                break;
                            }
                            Sequence = ESequence.WETCleanRight;
                            break;
                        }

                        Log.Info("Sequence AF Clean");
                        if (cleanType == EClean.AFCleanLeft)
                        {
                            Sequence = ESequence.AFCleanLeft;
                            break;
                        }
                        Sequence = ESequence.AFCleanRight;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessAutoRunStep.End:
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Log.Info("Sequence WET Clean Load");
                        Sequence = port == EPort.Left ? ESequence.WETCleanLeftLoad : ESequence.WETCleanRightLoad;
                        break;
                    }
                    Log.Info("Sequence AF Clean Load");
                    Sequence = port == EPort.Left ? ESequence.AFCleanLeftLoad : ESequence.AFCleanRightLoad;
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
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.AxisMoveLoadPosition:
                    Log.Debug("X Y T Axis Move Load Position");
                    XAxis.MoveAbs(XAxisLoadPosition);
                    YAxis.MoveAbs(YAxisLoadPosition);
                    TAxis.MoveAbs(TAxisLoadPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => XAxis.IsOnPosition(XAxisLoadPosition) && YAxis.IsOnPosition(YAxisLoadPosition) && TAxis.IsOnPosition(TAxisLoadPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.AxisMoveLoadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (XAxis.IsOnPosition(XAxisLoadPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_XAxis_MoveLoadPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_XAxis_MoveLoadPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_XAxis_MoveLoadPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_XAxis_MoveLoadPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }

                        if (YAxis.IsOnPosition(YAxisLoadPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_YAxis_MoveLoadPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_YAxis_MoveLoadPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_YAxis_MoveLoadPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_YAxis_MoveLoadPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }

                        if (TAxis.IsOnPosition(TAxisLoadPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_TAxis_MoveLoadPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_TAxis_MoveLoadPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_TAxis_MoveLoadPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_TAxis_MoveLoadPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        break;
                    }
                    Log.Debug("X Y T Axis Move Load Position Done");
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
                    Wait((int)(_commonRecipe.VacDelay * 1000));
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Log.Info("Sequence WET Clean");
                        Sequence = port == EPort.Left ? ESequence.WETCleanLeft : ESequence.WETCleanRight;
                        break;
                    }
                    Log.Info("Sequence AF Clean");
                    Sequence = port == EPort.Left ? ESequence.AFCleanLeft : ESequence.AFCleanRight;
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
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => XAxis.IsOnPosition(XAxisCleanHorizontalPosition) &&
                                                               YAxis.IsOnPosition(YAxisCleanHorizontalPosition) &&
                                                               TAxis.IsOnPosition(TAxisCleanHorizontalPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Axis_MoveCleanHorizontalPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (XAxis.IsOnPosition(XAxisCleanHorizontalPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_XAxis_MoveCleanHorizontalPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_XAxis_MoveCleanHorizontalPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_XAxis_MoveCleanHorizontalPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_XAxis_MoveCleanHorizontalPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        if (YAxis.IsOnPosition(YAxisCleanHorizontalPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_YAxis_MoveCleanHorizontalPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_YAxis_MoveCleanHorizontalPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_YAxis_MoveCleanHorizontalPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_YAxis_MoveCleanHorizontalPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        if (TAxis.IsOnPosition(TAxisCleanHorizontalPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_TAxis_MoveCleanHorizontalPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_TAxis_MoveCleanHorizontalPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_TAxis_MoveCleanHorizontalPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_TAxis_MoveCleanHorizontalPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        break;
                    }
                    Log.Debug("X Y T Axis Move Clean Horizontal Position");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Down_CleanHorizontal:
                    Log.Debug("Cylinder Pusher Down");
                    PushCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PushCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Down_CleanHorizontal_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_PusherCylinder_Down_Fail,
                            EClean.WETCleanRight => EWarning.WETCleanRight_PusherCylinder_Down_Fail,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_PusherCylinder_Down_Fail,
                            EClean.AFCleanRight => EWarning.AFCleanRight_PusherCylinder_Down_Fail,
                            _ => null
                        };

                        RaiseWarning((int)warning!);
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
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _devices.MotionsAjin.IsContiMotioning(cleanType) == false);
#else
                    Thread.Sleep(100);
#endif
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CleanHorizontal_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EAlarm? alarm = cleanType switch
                        {
                            EClean.WETCleanLeft => EAlarm.WETCleanLeft_CleanHorizontal_Fail,
                            EClean.WETCleanRight => EAlarm.WETCleanRight_CleanHorizontal_Fail,
                            EClean.AFCleanLeft => EAlarm.AFCleanLeft_CleanHorizontal_Fail,
                            EClean.AFCleanRight => EAlarm.AFCleanRight_CleanHorizontal_Fail,
                            _ => null
                        };
                        RaiseAlarm((int)alarm!);
                        break;
                    }
                    FeedingAxis.Stop();
                    Log.Debug("Clean Horizontal Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Up_CleanHorizontal:
                    Log.Debug("Cylinder Pusher Up");
                    PushCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PushCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusherUp_CleanHorizontal_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_PusherCylinder_Up_Fail,
                            EClean.WETCleanRight => EWarning.WETCleanRight_PusherCylinder_Up_Fail,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_PusherCylinder_Up_Fail,
                            EClean.AFCleanRight => EWarning.AFCleanRight_PusherCylinder_Up_Fail,
                            _ => null
                        };

                        RaiseWarning((int)warning!);
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
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => XAxis.IsOnPosition(XAxisCleanVerticalPosition) &&
                                                               YAxis.IsOnPosition(YAxisCleanVerticalPosition) &&
                                                               TAxis.IsOnPosition(TAxisCleanVerticalPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Axis_Move_CleanVerticalPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (XAxis.IsOnPosition(XAxisCleanVerticalPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_XAxis_MoveCleanVerticalPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_XAxis_MoveCleanVerticalPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_XAxis_MoveCleanVerticalPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_XAxis_MoveCleanVerticalPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        if (YAxis.IsOnPosition(YAxisCleanVerticalPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_YAxis_MoveCleanVerticalPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_YAxis_MoveCleanVerticalPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_YAxis_MoveCleanVerticalPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_YAxis_MoveCleanVerticalPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        if (TAxis.IsOnPosition(TAxisCleanVerticalPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_TAxis_MoveCleanVerticalPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_TAxis_MoveCleanVerticalPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_TAxis_MoveCleanVerticalPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_TAxis_MoveCleanVerticalPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        break;
                    }
                    Log.Debug("X Y T Axis Move Clean Vertical Position Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Down_CleanVertical:
                    Log.Debug("Cylinder Pusher Down");
                    PushCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PushCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Down_CleanVertical_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_PusherCylinder_Down_Fail,
                            EClean.WETCleanRight => EWarning.WETCleanRight_PusherCylinder_Down_Fail,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_PusherCylinder_Down_Fail,
                            EClean.AFCleanRight => EWarning.AFCleanRight_PusherCylinder_Down_Fail,
                            _ => null
                        };

                        RaiseWarning((int)warning!);
                        break;
                    }
                    Log.Debug("Cylinder Pusher Down Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CleanVertical:
                    Log.Debug("Clean Vertical");
#if !SIMULATION
                    _devices.MotionsAjin.CleanVertical(cleanType,XAxisCleanVerticalPosition,YAxisCleanVerticalPosition,cleanRecipe.CleanVerticalCount);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _devices.MotionsAjin.IsContiMotioning(cleanType));
#else
                    Thread.Sleep(100);
#endif
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CleanVertical_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EAlarm? alarm = cleanType switch
                        {
                            EClean.WETCleanLeft => EAlarm.WETCleanLeft_CleanVertical_Fail,
                            EClean.WETCleanRight => EAlarm.WETCleanRight_CleanVertical_Fail,
                            EClean.AFCleanLeft => EAlarm.AFCleanLeft_CleanVertical_Fail,
                            EClean.AFCleanRight => EAlarm.AFCleanRight_CleanVertical_Fail,
                            _ => null
                        };
                        RaiseAlarm((int)alarm!);
                        break;
                    }
                    Log.Debug("Clean Vertical Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Up_CleanVertical:
                    Log.Debug("Cylinder Pusher Up");
                    PushCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PushCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.CylPusher_Up_CleanVertical_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_PusherCylinder_Up_Fail,
                            EClean.WETCleanRight => EWarning.WETCleanRight_PusherCylinder_Up_Fail,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_PusherCylinder_Up_Fail,
                            EClean.AFCleanRight => EWarning.AFCleanRight_PusherCylinder_Up_Fail,
                            _ => null
                        };

                        RaiseWarning((int)warning!);
                        break;
                    }
                    Log.Debug("Cylinder Pusher Up Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Log.Info("Sequence WET Clean Unload");
                        Sequence = port == EPort.Left ? ESequence.WETCleanLeftUnload : ESequence.WETCleanRightUnload;
                        break;
                    }
                    Log.Info("Sequence AF Clean Unload");
                    Sequence = port == EPort.Left ? ESequence.AFCleanLeftUnload : ESequence.AFCleanRightUnload;
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
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.AxisMoveUnloadPosition:
                    Log.Debug("Axis Move Unload Position");
                    XAxis.MoveAbs(XAxisUnloadPosition);
                    YAxis.MoveAbs(YAxisUnloadPosition);
                    TAxis.MoveAbs(TAxisUnloadPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => XAxis.IsOnPosition(XAxisUnloadPosition) &&
                                                               YAxis.IsOnPosition(YAxisUnloadPosition) &&
                                                               TAxis.IsOnPosition(TAxisUnloadPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.AxisMoveUnloadPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (XAxis.IsOnPosition(XAxisUnloadPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_XAxis_MoveUnloadPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_XAxis_MoveUnloadPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_XAxis_MoveUnloadPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_XAxis_MoveUnloadPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }

                        if (YAxis.IsOnPosition(YAxisUnloadPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_YAxis_MoveUnloadPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_YAxis_MoveUnloadPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_YAxis_MoveUnloadPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_YAxis_MoveUnloadPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }

                        if (TAxis.IsOnPosition(TAxisUnloadPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_TAxis_MoveUnloadPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_TAxis_MoveUnloadPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_TAxis_MoveUnloadPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_TAxis_MoveUnloadPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        break;
                    }
                    Log.Debug("X Y T Axis Move Unload Position Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    GlassVac.Value = false;
#if SIMULATION
                    SimulationInputSetter.SetSimModbusInput(VacDetect, false);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000));
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
                        Wait(20);
                        break;
                    }
                    Log.Debug("Set Flag Unload Done Received");
                    FlagCleanUnloadDoneReceived = true;

                    Log.Debug("Clear Flag Request Unload");
                    FlagCleanRequestUnload = false;
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        Parent.ProcessMode = EProcessMode.ToStop;
                        break;
                    }
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Log.Info("Sequence WET Clean Load");
                        Sequence = port == EPort.Left ? ESequence.WETCleanLeftLoad : ESequence.WETCleanRightLoad;
                        break;
                    }
                    Log.Info("Sequence AF Clean Load");
                    Sequence = port == EPort.Left ? ESequence.AFCleanLeftLoad : ESequence.AFCleanRightLoad;
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
