using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
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
            Cylinders cylinders)
        {
            Inputs = inputs;
            Outputs = outputs;
            MotionsInovance = motionsInovance;
            MotionsAjin = motionsAjin;
            Cylinders = cylinders;
        }

        public Inputs Inputs { get; }
        public Outputs Outputs { get; }
        public MotionsInovance MotionsInovance { get; }
        public MotionsAjin MotionsAjin { get; }
        public Cylinders Cylinders { get; }
    }
}
