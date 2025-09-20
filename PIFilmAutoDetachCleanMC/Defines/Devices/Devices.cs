using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Defines.Devices.Regulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.Devices
{
    public class Devices
    {
        public Devices(Inputs inputs,
            Outputs outputs,
            MotionsInovance motionsInovance,
            MotionsAjin motionsAjin,
            Cylinders cylinders,
            TorqueControllerList torqueControllers,
            SpeedControllerList speedControllerList,
            Regulators regulators,
            AnalogInputs analogInputs)
        {
            Inputs = inputs;
            Outputs = outputs;
            MotionsInovance = motionsInovance;
            MotionsAjin = motionsAjin;
            Cylinders = cylinders;
            TorqueControllers = torqueControllers;
            SpeedControllerList = speedControllerList;
            Regulators = regulators;
            AnalogInputs = analogInputs;
        }

        public Inputs Inputs { get; }
        public Outputs Outputs { get; }
        public MotionsInovance MotionsInovance { get; }
        public MotionsAjin MotionsAjin { get; }
        public Cylinders Cylinders { get; }
        public TorqueControllerList TorqueControllers { get; }
        public SpeedControllerList SpeedControllerList { get; }
        public Regulators Regulators { get; }
        public AnalogInputs AnalogInputs { get; }
    }
}
