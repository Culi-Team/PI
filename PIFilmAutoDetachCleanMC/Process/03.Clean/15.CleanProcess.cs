using EQX.Core.Device.Regulator;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.TorqueController;
using EQX.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class CleanProcess : ProcessBase<ESequence>
    {
        private readonly Devices _devices;

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

        private ITorqueController winder
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

        private IMotion feedingAxis
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

        private IMotion xAxis
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

        private IMotion yAxis
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

        private IMotion tAxis
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

        private ICylinder pushCyl
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

        private IRegulator regulator
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

        private IDOutput glassVac
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

        private bool isVacDetect
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

        public CleanProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
