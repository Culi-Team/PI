using EQX.Core.Device.Regulator;
using EQX.Core.Device.SyringePump;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.Core.TorqueController;
using EQX.Device.Torque;
using EQX.InOut;
using EQX.InOut.Virtual;
using EQX.Process;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
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
        private readonly MachineStatus _machineStatus;
        private readonly CancellationTokenSource ctsPrepare3M = new CancellationTokenSource();

        private int GlassCleanCount { get; set; } = 0;

        private double RemainVolume { get; set; } = 0.0;

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

        private IDInput ShuttleAvoidNotCollision
        {
            get
            {
                return port == EPort.Left ? _devices.Inputs.ShuttleLAvoidNotCollision
                                          : _devices.Inputs.ShuttleRAvoidNotCollision;
            }
        }
        private DX3000TorqueController UnWinder
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

        private DX3000TorqueController Winder
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
                    EClean.WETCleanLeft => _devices.Motions.WETCleanLFeedingAxis,
                    EClean.WETCleanRight => _devices.Motions.WETCleanRFeedingAxis,
                    EClean.AFCleanLeft => _devices.Motions.AFCleanLFeedingAxis,
                    EClean.AFCleanRight => _devices.Motions.AFCleanRFeedingAxis,
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
                    EClean.WETCleanLeft => _devices.Motions.InShuttleLXAxis,
                    EClean.WETCleanRight => _devices.Motions.InShuttleRXAxis,
                    EClean.AFCleanLeft => _devices.Motions.OutShuttleLXAxis,
                    EClean.AFCleanRight => _devices.Motions.OutShuttleRXAxis,
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
                    EClean.WETCleanLeft => _devices.Motions.InShuttleLYAxis,
                    EClean.WETCleanRight => _devices.Motions.InShuttleRYAxis,
                    EClean.AFCleanLeft => _devices.Motions.OutShuttleLYAxis,
                    EClean.AFCleanRight => _devices.Motions.OutShuttleRYAxis,
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
                    EClean.WETCleanLeft => _devices.Motions.InShuttleLTAxis,
                    EClean.WETCleanRight => _devices.Motions.InShuttleRTAxis,
                    EClean.AFCleanLeft => _devices.Motions.OutShuttleLTAxis,
                    EClean.AFCleanRight => _devices.Motions.OutShuttleRTAxis,
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
                    EClean.WETCleanLeft => _devices.Cylinders.WetCleanL_PusherCyl,
                    EClean.WETCleanRight => _devices.Cylinders.WetCleanR_PusherCyl,
                    EClean.AFCleanLeft => _devices.Cylinders.AFCleanL_PusherCyl,
                    EClean.AFCleanRight => _devices.Cylinders.AFCleanR_PusherCyl,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private ICylinder BrushCyl
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Cylinders.WetCleanL_BrushCyl,
                    EClean.WETCleanRight => _devices.Cylinders.WetCleanR_BrushCyl,
                    EClean.AFCleanLeft => _devices.Cylinders.AFCleanL_BrushCyl,
                    EClean.AFCleanRight => _devices.Cylinders.AFCleanR_BrushCyl,
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

        private ISyringePump SyringePump
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.SyringePumps.WetCleanLeftSyringePump,
                    EClean.WETCleanRight => _devices.SyringePumps.WetCleanRightSyringePump,
                    EClean.AFCleanLeft => _devices.SyringePumps.AfCleanLeftSyringePump,
                    EClean.AFCleanRight => _devices.SyringePumps.AfCleanRightSyringePump,
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

        private bool FeedingRollerNotDetect
        {
            get
            {
                if (_machineStatus.IsDryRunMode || _machineStatus.MachineTestMode) return false;

                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Inputs.WetCleanLeftFeedingRollerDetect.Value,
                    EClean.WETCleanRight => _devices.Inputs.WetCleanRightFeedingRollerDetect.Value,
                    EClean.AFCleanLeft => _devices.Inputs.AfCleanLeftFeedingRollerDetect.Value,
                    EClean.AFCleanRight => _devices.Inputs.AfCleanRightFeedingRollerDetect.Value,
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
                    EClean.WETCleanLeft => _devices.Inputs.WetCleanLeftWiperCleanDetect1.Value,
                    EClean.WETCleanRight => _devices.Inputs.WetCleanRightWiperCleanDetect1.Value,
                    EClean.AFCleanLeft => _devices.Inputs.AfCleanLeftWiperCleanDetect1.Value,
                    EClean.AFCleanRight => _devices.Inputs.AfCleanRightWiperCleanDetect1.Value,
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
                    EClean.WETCleanLeft => _devices.Inputs.WetCleanLeftWiperCleanDetect2.Value,
                    EClean.WETCleanRight => _devices.Inputs.WetCleanRightWiperCleanDetect2.Value,
                    EClean.AFCleanLeft => _devices.Inputs.AfCleanLeftWiperCleanDetect2.Value,
                    EClean.AFCleanRight => _devices.Inputs.AfCleanRightWiperCleanDetect2.Value,
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
                    EClean.WETCleanLeft => _devices.Inputs.WetCleanLeftWiperCleanDetect3.Value,
                    EClean.WETCleanRight => _devices.Inputs.WetCleanRightWiperCleanDetect3.Value,
                    EClean.AFCleanLeft => _devices.Inputs.AfCleanLeftWiperCleanDetect3.Value,
                    EClean.AFCleanRight => _devices.Inputs.AfCleanRightWiperCleanDetect3.Value,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private bool IsVacDetect => VacDetect.Value;

        private bool IsPumpLeakDetect
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Inputs.WetCleanLeftPumpLeakNotDetect.Value == false,
                    EClean.WETCleanRight => _devices.Inputs.WetCleanRightPumpLeakNotDetect.Value == false,
                    EClean.AFCleanLeft => _devices.Inputs.AfCleanLeftPumpLeakNotDetect.Value == false,
                    EClean.AFCleanRight => _devices.Inputs.AfCleanRightPumpLeakNotDetect.Value == false,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private bool IsAlcoholLeakDetect
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Inputs.WetCleanLeftAlcoholLeakNotDetect.Value == false,
                    EClean.WETCleanRight => _devices.Inputs.WetCleanRightAlcoholLeakNotDetect.Value == false,
                    EClean.AFCleanLeft => _devices.Inputs.AfCleanLeftAlcoholLeakNotDetect.Value == false,
                    EClean.AFCleanRight => _devices.Inputs.AfCleanRightAlcoholLeakNotDetect.Value == false,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }


        private bool IsAlcoholDetect
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Inputs.WetCleanLeftAlcoholPumpDetect.Value,
                    EClean.WETCleanRight => _devices.Inputs.WetCleanRightAlcoholPumpDetect.Value,
                    EClean.AFCleanLeft => _devices.Inputs.AfCleanLeftAlcoholPumpDetect.Value,
                    EClean.AFCleanRight => _devices.Inputs.AfCleanRightAlcoholPumpDetect.Value,
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

        private ICylinder ClampCyl1
        {
            get
            {
                if (cleanType == EClean.WETCleanLeft)
                {
                    return _devices.Cylinders.WetCleanL_ClampCyl1;
                }
                if (cleanType == EClean.WETCleanRight)
                {
                    return _devices.Cylinders.WetCleanR_ClampCyl1;
                }

                throw new ArgumentOutOfRangeException();
            }
        }

        private ICylinder ClampCyl2
        {
            get
            {
                if (cleanType == EClean.WETCleanLeft)
                {
                    return _devices.Cylinders.WetCleanL_ClampCyl2;
                }
                if (cleanType == EClean.WETCleanRight)
                {
                    return _devices.Cylinders.WetCleanR_ClampCyl2;
                }

                throw new ArgumentOutOfRangeException();
            }
        }
        private double XAxisReadyPosition => cleanRecipe.XAxisReadyPosition;
        private double YAxisReadyPosition => cleanRecipe.YAxisReadyPosition;
        private double TAxisReadyPosition => cleanRecipe.TAxisReadyPosition;

        private double XAxisLoadPosition => cleanRecipe.XAxisLoadPosition;
        private double YAxisLoadPosition => cleanRecipe.YAxisLoadPosition;
        private double TAxisLoadPosition => cleanRecipe.TAxisLoadPosition;

        private double XAxisUnloadPosition => cleanRecipe.XAxisUnloadPosition;
        private double YAxisUnloadPosition => cleanRecipe.YAxisUnloadPosition;
        private double TAxisUnloadPosition => cleanRecipe.TAxisUnloadPosition;

        private double TAxisCleanHorizontalPosition => cleanRecipe.TAxisCleanHorizontalPosition;

        private double TAxisCleanVerticalPosition => cleanRecipe.TAxisCleanVerticalPosition;

        private double XAxisCleanShuttlePosition => cleanRecipe.XAxisCleanShuttlePosition;
        private double YAxisCleanShuttlePosition => cleanRecipe.YAxisCleanShuttlePosition;
        private double TAxisCleanShuttlePosition => cleanRecipe.TAxisCleanShuttlePosition;
        #endregion

        #region Flags
        private bool FlagCleanRequestLoad
        {
            set
            {
                Outputs[(int)ECleanProcessOutput.REQ_LOAD] = value;
            }
        }

        private bool FlagAFCleanCleaning
        {
            set
            {
                if (cleanType == EClean.AFCleanLeft || cleanType == EClean.AFCleanRight)
                {
                    Outputs[(int)ECleanProcessOutput.AF_CLEAN_CLEANING] = value;
                }
            }
        }

        private bool FlagAFCleanDisable
        {
            get
            {
                if (cleanType == EClean.AFCleanLeft || cleanType == EClean.AFCleanRight)
                {
                    return Inputs[(int)ECleanProcessInput.AF_CLEAN_DISABLE];
                }

                return false;
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

        private bool FlagTransferInShuttle_InSafePosition
        {
            get
            {
                return Inputs[(int)ECleanProcessInput.TRANSFER_IN_SHUTTLE_IN_SAFE_POS];
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
        public override bool PreProcess()
        {
            if (ProcessMode != EProcessMode.Run) return base.PreProcess();

            switch ((ECleanPreProcessStep)Step.PreProcessStep)
            {
                case ECleanPreProcessStep.Start:
                    Step.PreProcessStep++;
                    break;
                case ECleanPreProcessStep.Shuttle_XAxis_Collision_Check:
                    if (ShuttleAvoidNotCollision.Value == false)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.Shuttle_Left_XAxis_Collision_Detect :
                                                                EWarning.Shuttle_Right_XAxis_Collision_Detect));
                    }
                    Step.PreProcessStep++;
                    break;
                case ECleanPreProcessStep.Wiper_Detect_Check:
                    if (_commonRecipe.DisableLeftPort && port == EPort.Left)
                    {
                        Step.PreProcessStep++;
                        break;
                    }
                    if (_commonRecipe.DisableRightPort && port == EPort.Right)
                    {
                        Step.PreProcessStep++;
                        break;
                    }
                    if (_machineStatus.IsDryRunMode == false && _machineStatus.MachineTestMode == false && (WiperCleanDetect1 == true || WiperCleanDetect2 == true || WiperCleanDetect3 == true))
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
                    Step.PreProcessStep++;
                    break;
                case ECleanPreProcessStep.PumpLeak_Detect_Check:
                    if (IsPumpLeakDetect)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_PumpLeak_Detect,
                            EClean.WETCleanRight => EWarning.WETCleanRight_PumpLeak_Detect,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_PumpLeak_Detect,
                            EClean.AFCleanRight => EWarning.AFCleanRight_PumpLeak_Detect,
                            _ => null
                        };
                        RaiseWarning((int)warning!);
                        break;
                    }
                    Step.PreProcessStep++;
                    break;
                case ECleanPreProcessStep.AlcoholLeak_Detect_Check:
                    if (_machineStatus.IsDryRunMode == false && _machineStatus.MachineTestMode == false && IsAlcoholLeakDetect)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_AlcoholLeak_Detect,
                            EClean.WETCleanRight => EWarning.WETCleanRight_AlcoholLeak_Detect,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_AlcoholLeak_Detect,
                            EClean.AFCleanRight => EWarning.AFCleanRight_AlcoholLeak_Detect,
                            _ => null
                        };
                        RaiseWarning((int)warning!);
                        break;
                    }
                    Step.PreProcessStep++;
                    break;
                case ECleanPreProcessStep.End:
                    Step.PreProcessStep = (int)ECleanPreProcessStep.Start;
                    break;
                default:
                    break;
            }
            return base.PreProcess();
        }

        public override bool ProcessToOrigin()
        {

            switch ((ECleanToOriginStep)Step.OriginStep)
            {
                case ECleanToOriginStep.Start:
                    Step.OriginStep++;
                    break;
                case ECleanToOriginStep.Set_Pressure:
                    Log.Debug($"Set {Regulator.Name} Regulator Pressure to {cleanRecipe.CylinderPressure}Mpa");
                    Regulator.SetPressure(cleanRecipe.CylinderPressure);

                    Step.OriginStep++;
                    break;
                case ECleanToOriginStep.End:
                    ProcessStatus = EProcessStatus.ToOriginDone;
                    Step.OriginStep++;
                    break;
                default:
                    Wait(50);
                    break;
            }

            return true;
        }

        public override bool ProcessOrigin()
        {
            switch ((ECleanOriginStep)Step.OriginStep)
            {
                case ECleanOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    PushCyl.Backward();
                    BrushCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PushCyl.IsBackward && BrushCyl.IsBackward);
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (PushCyl.IsBackward == false)
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
                        if (BrushCyl.IsBackward)
                        {
                            EWarning? warning = cleanType switch
                            {
                                EClean.WETCleanLeft => EWarning.WETCleanLeft_BrushCylinder_Up_Fail,
                                EClean.WETCleanRight => EWarning.WETCleanRight_BrushCylinder_Up_Fail,
                                EClean.AFCleanLeft => EWarning.AFCleanLeft_BrushCylinder_Up_Fail,
                                EClean.AFCleanRight => EWarning.AFCleanRight_BrushCylinder_Up_Fail,
                                _ => null
                            };
                            RaiseWarning((int)warning!);
                            break;
                        }
                    }
                    Log.Debug("Cylinder Up Done");
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.AxisOrigin:
                    Log.Debug("X Axis Origin Start");
                    Log.Debug("Y Axis Origin Start");
                    Log.Debug("Feeding Axis Origin Start");
                    XAxis.SearchOrigin();
                    YAxis.SearchOrigin();
                    FeedingAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => XAxis.Status.IsHomeDone && YAxis.Status.IsHomeDone && FeedingAxis.Status.IsHomeDone);
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
                    Log.Debug("Feeding Axis Origin Done");
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.TAxisOrigin:
                    Log.Debug("T Axis Origin Start");
                    TAxis.SearchOrigin();
                    Wait((int)_commonRecipe.MotionOriginTimeout * 1000, () => TAxis.Status.IsHomeDone);
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.TAxisOrigin_Wait:
                    if (WaitTimeOutOccurred)
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
                    Log.Debug("T Axis Origin Done");
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Step.OriginStep++;
                        break;
                    }
                    Step.OriginStep = (int)ECleanOriginStep.SyringePump_Origin;
                    break;
                case ECleanOriginStep.Cyl_UnClamp:
                    Log.Debug("Cylinder UnClamp");
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => ClampCyl1.IsBackward && ClampCyl2.IsBackward);
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.WETCleanLeft_ClampCylinder_Unclamp_Fail
                                                                : EWarning.WETCleanRight_ClampCylinder_Unclamp_Fail));
                        break;
                    }
                    Log.Debug("Cylinder UnClamp Done");
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.SyringePump_Origin:
                    Log.Debug("Syringe Pump Oriign");
                    SyringePump.Initialize();
                    Wait((int)(_commonRecipe.MotionOriginTimeout * 1000), () => SyringePump.IsReady());
                    Step.OriginStep++;
                    break;
                case ECleanOriginStep.SyringePump_Origin_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EAlarm? alarm = cleanType switch
                        {
                            EClean.WETCleanLeft => EAlarm.WETCleanLeft_SyringePump_Origin_Fail,
                            EClean.WETCleanRight => EAlarm.WETCleanRight_SyringePump_Origin_Fail,
                            EClean.AFCleanLeft => EAlarm.AFCleanLeft_SyringePump_Origin_Fail,
                            EClean.AFCleanRight => EAlarm.AFCleanRight_SyringePump_Origin_Fail,
                            _ => null
                        };
                        RaiseAlarm((int)alarm!);
                        break;
                    }
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
                    Wait(50);
                    break;
                case ESequence.AutoRun:
                    Sequence_AutoRun();
                    break;
                case ESequence.Ready:
                    Sequence_Ready();
                    break;
                case ESequence.WETCleanLeftLoad:
                    if (cleanType == EClean.WETCleanLeft)
                    {
                        Sequence_Load();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.WETCleanRightLoad:
                    if (cleanType == EClean.WETCleanRight)
                    {
                        Sequence_Load();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.WETCleanLeft:
                    if (cleanType == EClean.WETCleanLeft)
                    {
                        Sequence_Clean();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.WETCleanRight:
                    if (cleanType == EClean.WETCleanRight)
                    {
                        Sequence_Clean();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.InShuttleCleanLeft:
                    if (cleanType == EClean.WETCleanLeft)
                    {
                        Sequence_CleanShuttle();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.InShuttleCleanRight:
                    if (cleanType == EClean.WETCleanRight)
                    {
                        Sequence_CleanShuttle();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.WETCleanLeftUnload:
                    if (cleanType == EClean.WETCleanLeft)
                    {
                        Sequence_Unload();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.WETCleanRightUnload:
                    if (cleanType == EClean.WETCleanRight)
                    {
                        Sequence_Unload();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.AFCleanLeftLoad:
                    if (cleanType == EClean.AFCleanLeft)
                    {
                        Sequence_Load();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.AFCleanRightLoad:
                    if (cleanType == EClean.AFCleanRight)
                    {
                        Sequence_Load();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.AFCleanLeft:
                    if (cleanType == EClean.AFCleanLeft)
                    {
                        Sequence_Clean();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.AFCleanRight:
                    if (cleanType == EClean.AFCleanRight)
                    {
                        Sequence_Clean();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.OutShuttleCleanLeft:
                    if (cleanType == EClean.AFCleanLeft)
                    {
                        Sequence_CleanShuttle();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.OutShuttleCleanRight:
                    if (cleanType == EClean.AFCleanRight)
                    {
                        Sequence_CleanShuttle();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.AFCleanLeftUnload:
                    if (cleanType == EClean.AFCleanLeft)
                    {
                        Sequence_Unload();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                case ESequence.AFCleanRightUnload:
                    if (cleanType == EClean.AFCleanRight)
                    {
                        Sequence_Unload();
                    }
                    else Sequence = ESequence.Stop;
                    break;
                default:
                    Sequence = ESequence.Stop;
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
                case ECleanProcessToRunStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    PushCyl.Backward();
                    BrushCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PushCyl.IsBackward && BrushCyl.IsBackward);
                    Step.ToRunStep++;
                    break;
                case ECleanProcessToRunStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (PushCyl.IsBackward == false)
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
                        if (BrushCyl.IsBackward == false)
                        {
                            EWarning? warning = cleanType switch
                            {
                                EClean.WETCleanLeft => EWarning.WETCleanLeft_BrushCylinder_Up_Fail,
                                EClean.WETCleanRight => EWarning.WETCleanRight_BrushCylinder_Up_Fail,
                                EClean.AFCleanLeft => EWarning.AFCleanLeft_BrushCylinder_Up_Fail,
                                EClean.AFCleanRight => EWarning.AFCleanRight_BrushCylinder_Up_Fail,
                                _ => null
                            };
                            RaiseWarning((int)warning!);
                            break;
                        }
                    }
                    Log.Debug("Cylinder Up Done");
                    Step.ToRunStep++;
                    break;
                case ECleanProcessToRunStep.FeedingRollerDetect_Check:
                    if (_commonRecipe.DisableLeftPort && port == EPort.Left)
                    {
                        Step.ToRunStep++;
                        break;
                    }
                    if (_commonRecipe.DisableRightPort && port == EPort.Right)
                    {
                        Step.ToRunStep++;
                        break;
                    }
                    Log.Debug("Feeding Roller Detect Check");
                    if (FeedingRollerNotDetect == true)
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
                    if (_commonRecipe.DisableLeftPort && port == EPort.Left)
                    {
                        Step.ToRunStep++;
                        break;
                    }
                    if (_commonRecipe.DisableRightPort && port == EPort.Right)
                    {
                        Step.ToRunStep++;
                        break;
                    }

                    Log.Debug("Wiper Clean Detect Check");
                    if (_machineStatus.IsDryRunMode == false && _machineStatus.MachineTestMode == false && (WiperCleanDetect1 == true || WiperCleanDetect2 == true || WiperCleanDetect3 == true))
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
                case ECleanProcessToRunStep.Set_Torque:
                    Winder.SetTorque(cleanRecipe.WinderTorque);
                    UnWinder.SetTorque(cleanRecipe.UnWinderTorque);
                    Step.ToRunStep++;
                    break;
                case ECleanProcessToRunStep.Winder_UnWinder_Run:
                    Winder.Run(true);
                    UnWinder.Run(false);
                    Step.ToRunStep++;
                    break;
                case ECleanProcessToRunStep.Set_Pressure:
                    Log.Debug($"Set Pressure : {cleanRecipe.CylinderPressure}");
                    Regulator.SetPressure(cleanRecipe.CylinderPressure);
                    Step.ToRunStep++;
                    break;
                case ECleanProcessToRunStep.Initialize_SyringePump_Parameter:
                    SyringePump.SetSpeed(1);
                    Thread.Sleep(100);
                    SyringePump.SetAcceleration(20);
                    Thread.Sleep(100);
                    SyringePump.SetDeccelation(20);
                    Thread.Sleep(100);
                    Step.ToRunStep++;
                    break;
                case ECleanProcessToRunStep.Clear_Flags:
                    Log.Debug("Clear Flags");
                    ((MappableOutputDevice<ECleanProcessOutput>)Outputs).ClearOutputs();
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
                case ECleanProcessAutoRunStep.Vacuum_On:
                    if (GlassVac.Value == false)
                    {
                        Log.Debug("Vacuum On");
                        GlassVac.Value = true;
                        Wait((int)(_commonRecipe.VacDelay * 1000), () => IsVacDetect);
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessAutoRunStep.Dispense_Remain:
                    SyringePump.Dispense(1.0, 7);
                    Thread.Sleep(100);
                    Step.RunStep++;
                    break;
                case ECleanProcessAutoRunStep.Dispense_Remain_Wait:
                    if (SyringePump.IsReady() == false)
                    {
                        Thread.Sleep(100);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessAutoRunStep.Fill:
                    SyringePump.Fill(1.0);
                    Thread.Sleep(100);
                    Step.RunStep++;
                    break;
                case ECleanProcessAutoRunStep.Fill_Wait:
                    if (SyringePump.IsReady() == false)
                    {
                        Thread.Sleep(100);
                        break;
                    }
                    RemainVolume = 1.0;
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
                    Log.Debug("Vacuum Off");
                    GlassVac.Value = false;
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

        private void Sequence_Ready()
        {
            switch ((ECleanProcessReadyStep)Step.RunStep)
            {
                case ECleanProcessReadyStep.Start:
                    Log.Debug("Ready Start");
                    if (GlassVac.Value == false)
                    {
                        //Vacuum On
                        GlassVac.Value = true;
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessReadyStep.TransInShuttle_SafePos_Wait:
                    if (FlagTransferInShuttle_InSafePosition == false &&
                        (cleanType == EClean.WETCleanRight || cleanType == EClean.WETCleanLeft))
                    {
                        Wait(20);
                        break;
                    }

                    Log.Debug("TransInShuttle_SafePos detect");
                    Step.RunStep++;
                    break;
                case ECleanProcessReadyStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    PushCyl.Backward();
                    BrushCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => PushCyl.IsBackward && BrushCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ECleanProcessReadyStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (PushCyl.IsBackward == false)
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
                        if (BrushCyl.IsBackward)
                        {
                            EWarning? warning = cleanType switch
                            {
                                EClean.WETCleanLeft => EWarning.WETCleanLeft_BrushCylinder_Up_Fail,
                                EClean.WETCleanRight => EWarning.WETCleanRight_BrushCylinder_Up_Fail,
                                EClean.AFCleanLeft => EWarning.AFCleanLeft_BrushCylinder_Up_Fail,
                                EClean.AFCleanRight => EWarning.AFCleanRight_BrushCylinder_Up_Fail,
                                _ => null
                            };
                            RaiseWarning((int)warning!);
                            break;
                        }
                    }
                    Log.Debug("Cylinder Up Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessReadyStep.YAxis_MoveReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(YAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessReadyStep.YAxis_MoveReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EAlarm? alarm = cleanType switch
                        {
                            EClean.WETCleanLeft => EAlarm.WETCleanLeft_YAxis_MoveReadyPosition_Fail,
                            EClean.WETCleanRight => EAlarm.WETCleanRight_YAxis_MoveReadyPosition_Fail,
                            EClean.AFCleanLeft => EAlarm.AFCleanLeft_YAxis_MoveReadyPosition_Fail,
                            EClean.AFCleanRight => EAlarm.AFCleanRight_YAxis_MoveReadyPosition_Fail,
                            _ => null
                        };
                        RaiseAlarm((int)alarm!);
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessReadyStep.XTAxis_MoveReadyPosition:
                    Log.Debug("X Axis Move Ready Position");
                    XAxis.MoveAbs(XAxisReadyPosition);
                    TAxis.MoveAbs(TAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => XAxis.IsOnPosition(XAxisReadyPosition) &&
                                                                              TAxis.IsOnPosition(TAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessReadyStep.XTAxis_MoveReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (XAxis.IsOnPosition(XAxisReadyPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_XAxis_MoveReadyPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_XAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_XAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_XAxis_MoveReadyPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        if (TAxis.IsOnPosition(TAxisReadyPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_TAxis_MoveReadyPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_TAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_TAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_TAxis_MoveReadyPosition_Fail,
                                _ => null
                            };
                        }
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessReadyStep.End:
                    Log.Debug("Ready End");
                    Sequence = ESequence.Stop;
                    break;
            }
        }

        private void Sequence_Load()
        {
            switch ((ECleanProcessLoadStep)Step.RunStep)
            {
                case ECleanProcessLoadStep.Start:
                    Log.Debug("Clean Load Start");

                    if (cleanType == EClean.AFCleanLeft || cleanType == EClean.AFCleanRight)
                    {
                        GlassVac.Value = true;
                        Log.Debug("Wait Transfer Rotation Ready Place");
                    }

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
                case ECleanProcessLoadStep.TAxisMoveLoadPosition:
                    Log.Debug("T Axis Move Load Position");
                    TAxis.MoveAbs(TAxisLoadPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => TAxis.IsOnPosition(TAxisLoadPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.TAxisMoveLoadPosition_Wait:
                    if (WaitTimeOutOccurred)
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

                    Log.Debug("T Axis Move Load Position Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.AxisMoveLoadPosition:
                    Log.Debug("X Y Axis Move Load Position");
                    XAxis.MoveAbs(XAxisLoadPosition);
                    YAxis.MoveAbs(YAxisLoadPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => XAxis.IsOnPosition(XAxisLoadPosition) && YAxis.IsOnPosition(YAxisLoadPosition));
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
                        break;
                    }
                    Log.Debug("X Y Axis Move Load Position Done");
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Step.RunStep++;
                        break;
                    }
                    Step.RunStep = (int)ECleanProcessLoadStep.Set_FlagCleanRequestLoad;
                    break;
                case ECleanProcessLoadStep.Cyl_UnClamp:
                    Log.Debug("Cylinder UnClamp");
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => ClampCyl1.IsBackward && ClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.WETCleanLeft_ClampCylinder_Unclamp_Fail
                                                                : EWarning.WETCleanRight_ClampCylinder_Unclamp_Fail));
                        break;
                    }
                    Log.Debug("Cylinder UnClamp Done");
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
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Log.Debug("Clear Flag Request Load");
                        FlagCleanRequestLoad = false;
                        Step.RunStep++;
                        break;
                    }
                    if (Parent.Sequence == ESequence.AutoRun)
                    {
                        Log.Debug("Set Flag AF Clean Cleaning");
                        FlagAFCleanCleaning = true;
                    }
                    Step.RunStep = (int)ECleanProcessLoadStep.Vacuum_On;
                    break;
                case ECleanProcessLoadStep.Cyl_Clamp:
                    Log.Debug("Cylinder Clamp");
                    ClampCyl1.Forward();
                    ClampCyl2.Forward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => ClampCyl1.IsForward && ClampCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.Cyl_Clamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.WETCleanLeft_ClampCylinder_Clamp_Fail :
                                                                EWarning.WETCleanRight_ClampCylinder_Clamp_Fail));
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.Vacuum_On:
                    Log.Debug("Vacuum On");
                    GlassVac.Value = true;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(VacDetect, true);
#endif
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => IsVacDetect || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.Vacuum_Check:
                    if (WaitTimeOutOccurred)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_Vacuum_Detect_Fail,
                            EClean.WETCleanRight => EWarning.WETCleanRight_Vacuum_Detect_Fail,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_Vacuum_Detect_Fail,
                            EClean.AFCleanRight => EWarning.AFCleanRight_Vacuum_Detect_Fail,
                            _ => null
                        };
                        RaiseWarning((int)warning!);
                        break;
                    }
                    Sequence_Prepare3M();
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Step.RunStep = (int)ECleanProcessLoadStep.End;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.Axis_MoveReadyPosition:
                    Log.Debug("X Y T Axis Move Ready Position");
                    XAxis.MoveAbs(XAxisReadyPosition);
                    YAxis.MoveAbs(YAxisReadyPosition);
                    TAxis.MoveAbs(TAxisReadyPosition);

                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () =>
                        XAxis.IsOnPosition(XAxisReadyPosition) &&
                        YAxis.IsOnPosition(YAxisReadyPosition) &&
                        TAxis.IsOnPosition(TAxisReadyPosition)
                        );

                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.Axis_MoveReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (XAxis.IsOnPosition(XAxisReadyPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_XAxis_MoveReadyPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_XAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_XAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_XAxis_MoveReadyPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }

                        if (YAxis.IsOnPosition(YAxisReadyPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_YAxis_MoveReadyPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_YAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_YAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_YAxis_MoveReadyPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        if (TAxis.IsOnPosition(TAxisReadyPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_TAxis_MoveReadyPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_TAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_TAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_TAxis_MoveReadyPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                    }

                    Log.Debug("Clear Flag Request Load");
                    FlagCleanRequestLoad = false;

                    Log.Debug("X Y T Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessLoadStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
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
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        GlassVac.Value = false;
                        Wait((int)(_commonRecipe.VacDelay * 1000), () => IsVacDetect);
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Wait_WETCleanUnloadDone:
                    if (FlagAFCleanDisable == true && Parent?.Sequence == ESequence.AutoRun)
                    {
                        Wait(20);
                        break;
                    }

                    FlagAFCleanCleaning = true;

                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence_Prepare3M();
                    }

                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Step.RunStep++;
                        break;
                    }
                    Step.RunStep = (int)ECleanProcessCleanStep.Vacuum_On;
                    break;
                case ECleanProcessCleanStep.Cyl_Clamp:
                    Log.Debug("Cylinder Clamp");
                    ClampCyl1.Forward();
                    ClampCyl2.Forward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => ClampCyl1.IsForward && ClampCyl2.IsForward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Cyl_Clamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.WETCleanLeft_ClampCylinder_Clamp_Fail :
                                                                EWarning.WETCleanRight_ClampCylinder_Clamp_Fail));
                        break;
                    }
                    Log.Debug("Cylinder Clamp Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Vacuum_On:
                    GlassVac.Value = true;
                    Wait((int)(_commonRecipe.VacDelay * 1000), () => IsVacDetect || _machineStatus.IsDryRunMode);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Vacuum_On_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_Vacuum_Detect_Fail,
                            EClean.WETCleanRight => EWarning.WETCleanRight_Vacuum_Detect_Fail,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_Vacuum_Detect_Fail,
                            EClean.AFCleanRight => EWarning.AFCleanRight_Vacuum_Detect_Fail,
                            _ => null
                        };

                        RaiseWarning((int)warning!);
                        break;
                    }

                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Axis_MoveCleanHorizontalPosition:
                    if (cleanRecipe.HorizontalPointData == null || cleanRecipe.HorizontalPointData.Count <= 0)
                    {
                        RaiseWarning((int)EWarning.CleanDataError);
                        break;
                    }

                    Log.Debug("X Y T Axis Move Clean Horizontal Position");
                    XAxis.MoveAbs(cleanRecipe.XAxisCleanCenterPosition + cleanRecipe.HorizontalCleanLength / 2.0);
                    YAxis.MoveAbs(cleanRecipe.YAxisCleanCenterPosition + cleanRecipe.HorizontalPointData[0].Point);
                    TAxis.MoveAbs(TAxisCleanHorizontalPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => XAxis.IsOnPosition(cleanRecipe.XAxisCleanCenterPosition + cleanRecipe.HorizontalCleanLength / 2.0) &&
                                                               YAxis.IsOnPosition(cleanRecipe.YAxisCleanCenterPosition + cleanRecipe.HorizontalPointData[0].Point) &&
                                                               TAxis.IsOnPosition(TAxisCleanHorizontalPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Axis_MoveCleanHorizontalPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (XAxis.IsOnPosition(cleanRecipe.XAxisCleanCenterPosition + cleanRecipe.HorizontalCleanLength / 2.0) == false)
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
                        if (YAxis.IsOnPosition(cleanRecipe.YAxisCleanCenterPosition + cleanRecipe.HorizontalPointData[0].Point) == false)
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
                    }
                    Log.Debug("X Y T Axis Move Clean Horizontal Position");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Wait_3M_PrepareDone:
                    // Stop Prepare3M Task if Timeout
                    if (ProcessTimer.StepElapsedTime > 5000)
                    {
                        Log.Error("Prepare3M Task Timeout");

                        ctsPrepare3M.Cancel();

                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_Prepare3M_Error,
                            EClean.WETCleanRight => EWarning.WETCleanRight_Prepare3M_Error,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_PrepareSumo_Error,
                            EClean.AFCleanRight => EWarning.AFCleanRight_PrepareSumo_Error,
                            _ => null
                        };

                        RaiseWarning((int)warning!);
                        break;
                    }
                    if (Is3MPrepareDone == false)
                    {
                        Wait(20);
                        break;
                    }

                    Is3MPrepareDone = false;
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
                    Clean(true);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _devices.Motions.IsContiMotioning(cleanType) == false);
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

                    Log.Debug("Clear Flag AF Clean Cleaning");
                    FlagAFCleanCleaning = false;
                    Step.RunStep = (int)ECleanProcessCleanStep.End;
                    break;
                case ECleanProcessCleanStep.Axis_Move_CleanVerticalPosition:
                    if (cleanRecipe.VerticalPointData == null || cleanRecipe.VerticalPointData.Count <= 0)
                    {
                        RaiseWarning((int)EWarning.CleanDataError);
                        break;
                    }
                    Log.Debug("X Y T Axis Move Clean Vertical Position");
                    XAxis.MoveAbs(cleanRecipe.XAxisCleanCenterPosition);
                    YAxis.MoveAbs(cleanRecipe.YAxisCleanCenterPosition + cleanRecipe.VerticalPointData[0].Point);
                    TAxis.MoveAbs(TAxisCleanVerticalPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => XAxis.IsOnPosition(cleanRecipe.XAxisCleanCenterPosition) &&
                                                               YAxis.IsOnPosition(cleanRecipe.YAxisCleanCenterPosition + cleanRecipe.VerticalPointData[0].Point) &&
                                                               TAxis.IsOnPosition(TAxisCleanVerticalPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.Axis_Move_CleanVerticalPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (XAxis.IsOnPosition(cleanRecipe.XAxisCleanCenterPosition) == false)
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
                        if (YAxis.IsOnPosition(cleanRecipe.YAxisCleanCenterPosition + cleanRecipe.VerticalPointData[0].Point) == false)
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
                    Clean(false);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _devices.Motions.IsContiMotioning(cleanType) == false);
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
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Step.RunStep = (int)ECleanProcessCleanStep.End;
                        break;
                    }
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.XAxis_Move_ReadyPosition:
                    Log.Debug("X Axis Move Ready Position");
                    XAxis.MoveAbs(XAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => XAxis.IsOnPosition(XAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.XAxis_Move_ReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EAlarm? alarm = cleanType switch
                        {
                            EClean.WETCleanLeft => EAlarm.WETCleanLeft_XAxis_MoveReadyPosition_Fail,
                            EClean.WETCleanRight => EAlarm.WETCleanRight_XAxis_MoveReadyPosition_Fail,
                            EClean.AFCleanLeft => EAlarm.AFCleanLeft_XAxis_MoveReadyPosition_Fail,
                            EClean.AFCleanRight => EAlarm.AFCleanRight_XAxis_MoveReadyPosition_Fail,
                            _ => null
                        };
                        RaiseAlarm((int)alarm!);
                        break;
                    }

                    Log.Debug("X Axis Move Ready Position Done");

                    Log.Debug("Clear Flag AF Clean Cleaning");
                    FlagAFCleanCleaning = false;
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    GlassCleanCount++;

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
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Step.RunStep++;
                        break;
                    }
                    Step.RunStep = (int)ECleanProcessUnloadStep.Vacuum_Off;
                    break;
                case ECleanProcessUnloadStep.Cyl_UnClamp:
                    Log.Debug("Cylinder UnClamp");
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => ClampCyl1.IsBackward && ClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.Cyl_UnClamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.WETCleanLeft_ClampCylinder_Unclamp_Fail :
                                                          EWarning.WETCleanRight_ClampCylinder_Unclamp_Fail));
                        break;
                    }
                    Log.Debug("Cylinder UnClamp Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.Vacuum_Off:
                    Log.Debug("Vacuum Off");
                    GlassVac.Value = false;
#if SIMULATION
                    SimulationInputSetter.SetSimInput(VacDetect, false);
#endif
                    Wait(300);
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
                    Log.Debug("Clear Flag Request Unload");
                    FlagCleanRequestUnload = false;
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.YAxis_MoveReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(YAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.YAxis_MoveReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EAlarm? alarm = cleanType switch
                        {
                            EClean.WETCleanLeft => EAlarm.WETCleanLeft_YAxis_MoveReadyPosition_Fail,
                            EClean.WETCleanRight => EAlarm.WETCleanRight_YAxis_MoveReadyPosition_Fail,
                            EClean.AFCleanLeft => EAlarm.AFCleanLeft_YAxis_MoveReadyPosition_Fail,
                            EClean.AFCleanRight => EAlarm.AFCleanRight_YAxis_MoveReadyPosition_Fail,
                            _ => null
                        };
                        RaiseAlarm((int)alarm!);
                        break;
                    }
                    Log.Debug("Y Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.XTAxis_MoveReadyPosition:
                    Log.Debug("X T Axis Move Ready Position");
                    XAxis.MoveAbs(XAxisReadyPosition);
                    TAxis.MoveAbs(TAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => XAxis.IsOnPosition(XAxisReadyPosition) &&
                                                               TAxis.IsOnPosition(TAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.XTAxis_MoveReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (XAxis.IsOnPosition(XAxisReadyPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_XAxis_MoveReadyPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_XAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_XAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_XAxis_MoveReadyPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        if (TAxis.IsOnPosition(TAxisReadyPosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_TAxis_MoveReadyPosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_TAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_TAxis_MoveReadyPosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_TAxis_MoveReadyPosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                    }

                    Log.Debug("X T Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessUnloadStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    if (cleanRecipe.IsCleanShuttle && cleanRecipe.CleanShuttleCycle >= GlassCleanCount)
                    {
                        if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                        {
                            Log.Info("Sequence In Shuttle Clean");
                            Sequence = port == EPort.Left ? ESequence.InShuttleCleanLeft : ESequence.InShuttleCleanRight;
                            break;
                        }

                        Log.Info("Sequence Out Shuttle Clean");
                        Sequence = port == EPort.Left ? ESequence.OutShuttleCleanLeft : ESequence.OutShuttleCleanRight;
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
            int prepare3MStep = 0;
            int StepWaitTime = 0;

            bool prepare3MRun = true;

            ctsPrepare3M.TryReset();

            Task prepare3MThread = new Task(async () =>
            {
                try
                {
                    while (prepare3MRun && ProcessMode == EProcessMode.Run && ctsPrepare3M.IsCancellationRequested == false)
                    {
                        switch ((ECleanProcessPrepare3MStep)prepare3MStep)
                        {
                            case ECleanProcessPrepare3MStep.Start:
                                Log.Debug("Prepare 3M Start");
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Feeding_Forward:
                                Log.Debug("R Feeding Axis Move Forward");
                                FeedingAxis.ClearPosition();
                                FeedingAxis.MoveAbs(cleanRecipe.RFeedingAxisForwardDistance * -1.0);
                                StepWaitTime = Environment.TickCount;
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Feeding_Forward_Wait:
                                if (Environment.TickCount - StepWaitTime > _commonRecipe.MotionMoveTimeOut * 1000)
                                {
                                    EAlarm? alarm = cleanType switch
                                    {
                                        EClean.WETCleanLeft => EAlarm.WETCleanLeft_RFeedingAxis_MoveForward_Fail,
                                        EClean.WETCleanRight => EAlarm.WETCleanRight_RFeedingAxis_MoveForward_Fail,
                                        EClean.AFCleanLeft => EAlarm.AFCleanLeft_RFeedingAxis_MoveForward_Fail,
                                        EClean.AFCleanRight => EAlarm.AFCleanRight_RFeedingAxis_MoveForward_Fail,
                                        _ => null
                                    };
                                    RaiseAlarm((int)alarm!);
                                    break;
                                }
                                if (FeedingAxis.IsOnPosition(-cleanRecipe.RFeedingAxisForwardDistance) == false)
                                {
                                    await Task.Delay(2, ctsPrepare3M.Token);
                                    break;
                                }
                                Log.Debug("R Feeding Axis Move Forward Done");
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port1:
                                if (cleanRecipe.UsePort1 == false)
                                {
                                    prepare3MStep = (int)ECleanProcessPrepare3MStep.Dispense_Port2;
                                    break;
                                }
                                Log.Debug("Dispense Port 1");
                                SyringePump.Dispense(cleanRecipe.CleanVolume, 1);
                                await Task.Delay(100, ctsPrepare3M.Token);
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port1_Wait:
                                if (SyringePump.IsReady() == false)
                                {
                                    await Task.Delay(100, ctsPrepare3M.Token);
                                    break;
                                }

                                RemainVolume -= cleanRecipe.CleanVolume;

                                Log.Debug("Dispense Port 1 Done");
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port2:
                                if (cleanRecipe.UsePort2 == false)
                                {
                                    prepare3MStep = (int)ECleanProcessPrepare3MStep.Dispense_Port3;
                                    break;
                                }
                                Log.Debug("Dispense Port 2");
                                SyringePump.Dispense(cleanRecipe.CleanVolume, 2);
                                await Task.Delay(100, ctsPrepare3M.Token);
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port2_Wait:
                                if (SyringePump.IsReady() == false)
                                {
                                    await Task.Delay(100, ctsPrepare3M.Token);
                                    break;
                                }

                                RemainVolume -= cleanRecipe.CleanVolume;

                                Log.Debug("Dispense Port 2 Done");
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port3:
                                if (cleanRecipe.UsePort3 == false)
                                {
                                    prepare3MStep = (int)ECleanProcessPrepare3MStep.Dispense_Port4;
                                    break;
                                }
                                Log.Debug("Dispense Port 3");
                                SyringePump.Dispense(cleanRecipe.CleanVolume, 3);
                                await Task.Delay(100, ctsPrepare3M.Token);
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port3_Wait:
                                if (SyringePump.IsReady() == false)
                                {
                                    await Task.Delay(100, ctsPrepare3M.Token);
                                    break;
                                }

                                RemainVolume -= cleanRecipe.CleanVolume;

                                Log.Debug("Dispense Port 3 Done");
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port4:
                                if (cleanRecipe.UsePort4 == false)
                                {
                                    prepare3MStep = (int)ECleanProcessPrepare3MStep.Dispense_Port5;
                                    break;
                                }
                                Log.Debug("Dispense Port 4");
                                SyringePump.Dispense(cleanRecipe.CleanVolume, 4);
                                await Task.Delay(100, ctsPrepare3M.Token);
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port4_Wait:
                                if (SyringePump.IsReady() == false)
                                {
                                    await Task.Delay(100, ctsPrepare3M.Token);
                                    break;
                                }

                                RemainVolume -= cleanRecipe.CleanVolume;

                                Log.Debug("Dispense Port 4 Done");
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port5:
                                if (cleanRecipe.UsePort5 == false)
                                {
                                    prepare3MStep = (int)ECleanProcessPrepare3MStep.Dispense_Port6;
                                    break;
                                }
                                Log.Debug("Dispense Port 5");
                                SyringePump.Dispense(cleanRecipe.CleanVolume, 5);
                                await Task.Delay(100, ctsPrepare3M.Token);
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port5_Wait:
                                if (SyringePump.IsReady() == false)
                                {
                                    await Task.Delay(100, ctsPrepare3M.Token);
                                    break;
                                }

                                RemainVolume -= cleanRecipe.CleanVolume;

                                Log.Debug("Dispense Port 5 Done");
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port6:
                                if (cleanRecipe.UsePort6 == false)
                                {
                                    prepare3MStep = (int)ECleanProcessPrepare3MStep.Feeding_Backward;
                                    break;
                                }
                                Log.Debug("Dispense Port 6");
                                SyringePump.Dispense(cleanRecipe.CleanVolume, 6);
                                await Task.Delay(100, ctsPrepare3M.Token);
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Dispense_Port6_Wait:
                                if (SyringePump.IsReady() == false)
                                {
                                    await Task.Delay(100, ctsPrepare3M.Token);
                                    break;
                                }

                                RemainVolume -= cleanRecipe.CleanVolume;

                                Log.Debug("Dispense Port 6 Done");
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Feeding_Backward:
                                Log.Debug("R Feeding Axis Move Backward");
                                FeedingAxis.ClearPosition();
                                FeedingAxis.MoveAbs(cleanRecipe.RFeedingAxisBackwardDistance);
                                StepWaitTime = Environment.TickCount;
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Feeding_Backward_Wait:
                                if (Environment.TickCount - StepWaitTime > _commonRecipe.MotionMoveTimeOut * 1000)
                                {
                                    EAlarm? alarm = cleanType switch
                                    {
                                        EClean.WETCleanLeft => EAlarm.WETCleanLeft_RFeedingAxis_MoveBackward_Fail,
                                        EClean.WETCleanRight => EAlarm.WETCleanRight_RFeedingAxis_MoveBackward_Fail,
                                        EClean.AFCleanLeft => EAlarm.AFCleanLeft_RFeedingAxis_MoveBackward_Fail,
                                        EClean.AFCleanRight => EAlarm.AFCleanRight_RFeedingAxis_MoveBackward_Fail,
                                        _ => null
                                    };
                                    RaiseAlarm((int)alarm!);
                                    break;
                                }
                                if (FeedingAxis.IsOnPosition(cleanRecipe.RFeedingAxisBackwardDistance) == false)
                                {
                                    await Task.Delay(10, ctsPrepare3M.Token);
                                    break;
                                }

                                Is3MPrepareDone = true;

                                Log.Debug("R Feeding Axis Move Backward Done");
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.RemainVolume_Check:
                                prepare3MStep++;
                                break;
                                if (RemainVolume >= 0.3)
                                {
                                    prepare3MStep = (int)ECleanProcessPrepare3MStep.End;
                                    break;
                                }

                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Fill:
                                SyringePump.Fill(1.0);
                                StepWaitTime = Environment.TickCount;
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.FlowSensor_Check:
                                prepare3MStep++;
                                break;
                                if (Environment.TickCount - StepWaitTime > 1000)
                                {
                                    EWarning? warning = cleanType switch
                                    {
                                        EClean.WETCleanLeft => EWarning.WETCleanLeft_Alcohol_Not_Detect,
                                        EClean.WETCleanRight => EWarning.WETCleanRight_Alcohol_Not_Detect,
                                        EClean.AFCleanLeft => EWarning.AFCleanLeft_Alcohol_Not_Detect,
                                        EClean.AFCleanRight => EWarning.AFCleanRight_Alcohol_Not_Detect,
                                        _ => null
                                    };
                                    RaiseWarning((int)warning!);
                                    break;
                                }
                                if (IsAlcoholDetect == false && _machineStatus.IsDryRunMode == false && _machineStatus.MachineTestMode == false)
                                {
                                    await Task.Delay(1, ctsPrepare3M.Token);
                                    break;
                                }

                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.Fill_Wait:
                                if (SyringePump.IsReady() == false)
                                {
                                    await Task.Delay(100, ctsPrepare3M.Token);
                                    break;
                                }

                                RemainVolume = 1.0;
                                prepare3MStep++;
                                break;
                            case ECleanProcessPrepare3MStep.End:
                                Log.Debug("Prepare 3M End");
                                prepare3MRun = false;
                                break;
                        }
                    }
                }
                catch
                {
                }
            });
            prepare3MThread.Start();
        }

        private void Sequence_CleanShuttle()
        {
            switch ((ECleanProcessCleanShuttleStep)Step.RunStep)
            {
                case ECleanProcessCleanShuttleStep.Start:
                    Log.Debug("Clean Shuttle Start");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.Cyl_Up:
                    Log.Debug("Cylinder Up");
                    BrushCyl.Backward();
                    PushCyl.Backward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => BrushCyl.IsBackward && PushCyl.IsBackward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.Cyl_Up_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (BrushCyl.IsBackward == false)
                        {
                            EWarning? warning = cleanType switch
                            {
                                EClean.WETCleanLeft => EWarning.WETCleanLeft_BrushCylinder_Up_Fail,
                                EClean.WETCleanRight => EWarning.WETCleanRight_BrushCylinder_Up_Fail,
                                EClean.AFCleanLeft => EWarning.AFCleanLeft_BrushCylinder_Up_Fail,
                                EClean.AFCleanRight => EWarning.AFCleanRight_BrushCylinder_Up_Fail,
                                _ => null
                            };
                            RaiseWarning((int)warning!);
                            break;
                        }
                        if (PushCyl.IsBackward == false)
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
                    }
                    Log.Debug("Cylinder Up Done");
                    if (cleanType == EClean.WETCleanLeft || cleanType == EClean.WETCleanRight)
                    {
                        Step.RunStep++;
                        break;
                    }
                    Step.RunStep = (int)ECleanProcessCleanShuttleStep.YAxis_MoveReadyPosition;
                    break;
                case ECleanProcessCleanShuttleStep.Clamp_Cyl_Unclamp:
                    Log.Debug("Clamp Cylinder Unclamp");
                    ClampCyl1.Backward();
                    ClampCyl2.Backward();
                    Wait((int)(_commonRecipe.CylinderMoveTimeout * 1000), () => ClampCyl1.IsBackward && ClampCyl2.IsBackward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.Clamp_Cyl_Unclamp_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        RaiseWarning((int)(port == EPort.Left ? EWarning.WETCleanLeft_ClampCylinder_Unclamp_Fail :
                                                          EWarning.WETCleanRight_ClampCylinder_Unclamp_Fail));
                        break;
                    }
                    Log.Debug("Clamp Cylinder Unclamp Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.YAxis_MoveReadyPosition:
                    Log.Debug("Y Axis Move Ready Position");
                    YAxis.MoveAbs(YAxisReadyPosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisReadyPosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.YAxis_MoveReadyPosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EAlarm? alarm = cleanType switch
                        {
                            EClean.WETCleanLeft => EAlarm.WETCleanLeft_YAxis_MoveReadyPosition_Fail,
                            EClean.WETCleanRight => EAlarm.WETCleanRight_YAxis_MoveReadyPosition_Fail,
                            EClean.AFCleanLeft => EAlarm.AFCleanLeft_YAxis_MoveReadyPosition_Fail,
                            EClean.AFCleanRight => EAlarm.AFCleanRight_YAxis_MoveReadyPosition_Fail,
                            _ => null
                        };
                        RaiseAlarm((int)alarm!);
                        break;
                    }
                    Log.Debug("Y Axis Move Ready Position Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.XTAxis_MoveCleanShuttlePosition:
                    Log.Debug("X T Axis Move Clean Shuttle Position");
                    XAxis.MoveAbs(XAxisCleanShuttlePosition);
                    TAxis.MoveAbs(TAxisCleanShuttlePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => XAxis.IsOnPosition(XAxisCleanShuttlePosition) &&
                                                               TAxis.IsOnPosition(TAxisCleanShuttlePosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.XTAxis_MoveCleanShuttlePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        if (XAxis.IsOnPosition(XAxisCleanShuttlePosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_XAxis_MoveCleanShuttlePosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_XAxis_MoveCleanShuttlePosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_XAxis_MoveCleanShuttlePosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_XAxis_MoveCleanShuttlePosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                        if (TAxis.IsOnPosition(TAxisCleanShuttlePosition) == false)
                        {
                            EAlarm? alarm = cleanType switch
                            {
                                EClean.WETCleanLeft => EAlarm.WETCleanLeft_TAxis_MoveCleanShuttlePosition_Fail,
                                EClean.WETCleanRight => EAlarm.WETCleanRight_TAxis_MoveCleanShuttlePosition_Fail,
                                EClean.AFCleanLeft => EAlarm.AFCleanLeft_TAxis_MoveCleanShuttlePosition_Fail,
                                EClean.AFCleanRight => EAlarm.AFCleanRight_TAxis_MoveCleanShuttlePosition_Fail,
                                _ => null
                            };
                            RaiseAlarm((int)alarm!);
                            break;
                        }
                    }
                    Log.Debug("X T Axis Move Clean Shuttle Position Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.YAxis_MoveCleanShuttlePosition:
                    Log.Debug("Y Axis Move Clean Shuttle Position");
                    YAxis.MoveAbs(YAxisCleanShuttlePosition);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => YAxis.IsOnPosition(YAxisCleanShuttlePosition));
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.YAxis_MoveCleanShuttlePosition_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EAlarm? alarm = cleanType switch
                        {
                            EClean.WETCleanLeft => EAlarm.WETCleanLeft_YAxis_MoveCleanShuttlePosition_Fail,
                            EClean.WETCleanRight => EAlarm.WETCleanRight_YAxis_MoveCleanShuttlePosition_Fail,
                            EClean.AFCleanLeft => EAlarm.AFCleanLeft_YAxis_MoveCleanShuttlePosition_Fail,
                            EClean.AFCleanRight => EAlarm.AFCleanRight_YAxis_MoveCleanShuttlePosition_Fail,
                            _ => null
                        };
                        RaiseAlarm((int)alarm!);
                        break;
                    }
                    Log.Debug("Y Axis Move Clean Shuttle Position Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.Brush_Cyl_Down:
                    Log.Debug("Brush Cylinder Down");
                    BrushCyl.Forward();
                    Wait((int)_commonRecipe.CylinderMoveTimeout * 1000, () => BrushCyl.IsForward);
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.Brush_Cyl_Down_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EWarning? warning = cleanType switch
                        {
                            EClean.WETCleanLeft => EWarning.WETCleanLeft_BrushCylinder_Down_Fail,
                            EClean.WETCleanRight => EWarning.WETCleanRight_BrushCylinder_Down_Fail,
                            EClean.AFCleanLeft => EWarning.AFCleanLeft_BrushCylinder_Down_Fail,
                            EClean.AFCleanRight => EWarning.AFCleanRight_BrushCylinder_Down_Fail,
                            _ => null
                        };
                        RaiseWarning((int)warning!);
                        break;
                    }
                    Log.Debug("Brush Cylinder Down Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.CleanShuttle:
                    Log.Debug("Clean Shuttle");
#if !SIMULATION
                    Clean(isHorizontal : true);
                    Wait((int)(_commonRecipe.MotionMoveTimeOut * 1000), () => _devices.Motions.IsContiMotioning(cleanType) == false);
#else
                    Thread.Sleep(100);
#endif
                    break;
                case ECleanProcessCleanShuttleStep.CleanShuttle_Wait:
                    if (WaitTimeOutOccurred)
                    {
                        EAlarm? alarm = cleanType switch
                        {
                            EClean.WETCleanLeft => EAlarm.WETCleanLeft_CleanShuttle_Fail,
                            EClean.WETCleanRight => EAlarm.WETCleanRight_CleanShuttle_Fail,
                            EClean.AFCleanLeft => EAlarm.AFCleanLeft_CleanShuttle_Fail,
                            EClean.AFCleanRight => EAlarm.AFCleanRight_CleanShuttle_Fail,
                            _ => null
                        };
                        RaiseAlarm((int)alarm!);
                        break;
                    }
                    Log.Debug("Clean Shuttle Done");
                    Step.RunStep++;
                    break;
                case ECleanProcessCleanShuttleStep.End:
                    if (Parent?.Sequence != ESequence.AutoRun)
                    {
                        Sequence = ESequence.Stop;
                        break;
                    }

                    GlassCleanCount = 0;

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
        #endregion

        public void CleanVertical(EClean eClean)
        {
            List<PointAndCount> pointData = cleanRecipe.VerticalPointData;

            if (pointData.Count <= 0) return;
        }

        public void CleanHorizontal(EClean eClean)
        {
            List<PointAndCount> pointData = cleanRecipe.HorizontalPointData;

            if (pointData.Count <= 0) return;

        }

        object ajinContiLock = new object();

        public void Clean(bool isHorizontal)
        {
            List<PointAndCount> pointDatas = isHorizontal ? cleanRecipe.HorizontalPointData : cleanRecipe.VerticalPointData;
            double length = isHorizontal ? cleanRecipe.HorizontalCleanLength : cleanRecipe.VerticalCleanLength;
            double speed = isHorizontal ? cleanRecipe.CleanHorizontalSpeed : cleanRecipe.CleanVerticalSpeed;

            if (pointDatas.Count <= 0) return;

            double[] pos;

            lock (ajinContiLock)
            {
                AXM.AxmContiSetAxisMap((int)cleanType, 2, new int[] { XAxis.Id, YAxis.Id });
                AXM.AxmContiBeginNode((int)cleanType);

                foreach (var pointData in pointDatas)
                {
                    double yPosition = cleanRecipe.YAxisCleanCenterPosition + pointData.Point;

                    for (int i = 0; i < pointData.Repeat; i++)
                    {
                        double xPosition = cleanRecipe.XAxisCleanCenterPosition + length / 2.0;

                        pos = new double[] { xPosition, yPosition };
                        AXM.AxmLineMove((int)cleanType, pos, speed, 0.2, 0.2);

                        xPosition = cleanRecipe.XAxisCleanCenterPosition - length / 2.0;

                        pos = new double[] { xPosition, yPosition };
                        AXM.AxmLineMove((int)cleanType, pos, speed, 0.2, 0.2);
                    }

                    if (isHorizontal == false)
                    {
                        pos = new double[] { cleanRecipe.XAxisCleanCenterPosition, yPosition };
                        AXM.AxmLineMove((int)cleanType, pos, speed, 0.2, 0.2);
                    }
                }

                AXM.AxmContiEndNode((int)cleanType);
                AXM.AxmContiStart((int)cleanType, 0, 0);
            }
        }
    }
}
