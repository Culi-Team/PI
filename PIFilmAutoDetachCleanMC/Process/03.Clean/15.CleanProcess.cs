using EQX.Core.Device.Regulator;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Sequence;
using EQX.Core.TorqueController;
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
    public class CleanProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;

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
                    EClean.WETCleanLeft => _devices.Outputs.Shuttle1LVacOnOff,
                    EClean.WETCleanRight => _devices.Outputs.Shuttle1RVacOnOff,
                    EClean.AFCleanLeft => _devices.Outputs.Shuttle2LVacOnOff,
                    EClean.AFCleanRight => _devices.Outputs.Shuttle2RVacOnOff,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private bool IsVacDetect
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Inputs.Shuttle1LVac.Value,
                    EClean.WETCleanRight => _devices.Inputs.Shuttle1RVac.Value,
                    EClean.AFCleanLeft => _devices.Inputs.Shuttle2LVac.Value,
                    EClean.AFCleanRight => _devices.Inputs.Shuttle2RVac.Value,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private bool IsLeakDetect
        {
            get
            {
                return cleanType switch
                {
                    EClean.WETCleanLeft => _devices.Inputs.WetCleanLeftPumpLeakDetect.Value || _devices.Inputs.WetCleanleftAlcoholLeakDectect.Value,
                    EClean.WETCleanRight => _devices.Inputs.WetCleanRightPumpLeakDetect.Value || _devices.Inputs.WetCleanRightAlcoholLeakDectect.Value,
                    EClean.AFCleanLeft => _devices.Inputs.AfCleanLeftPumpLeakDetect.Value || _devices.Inputs.AFCleanLefttAlcoholLeakDectect.Value,
                    EClean.AFCleanRight => _devices.Inputs.AfCleanRightPumpLeakDetect.Value || _devices.Inputs.AFCleanRightAlcoholLeakDectect.Value,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        #endregion

        #region Constructor
        public CleanProcess(Devices devices, CommonRecipe commonRecipe)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
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
                    Log.Debug("Origin done");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    break;
            }

            return true;
        }
        #endregion
    }
}
