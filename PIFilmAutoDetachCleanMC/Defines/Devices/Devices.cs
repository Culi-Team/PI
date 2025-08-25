using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class Devices
    {
        public Devices(Inputs inputs, Outputs outputs, MotionsInovance motionsInovance , MotionsAjin motionsAjin)
        {
            Inputs = inputs;
            Outputs = outputs;
            MotionsInovance = motionsInovance;
            MotionsAjin = motionsAjin;
        }

        public Inputs Inputs { get; set; }
        public Outputs Outputs { get; set; }
        public MotionsInovance MotionsInovance { get; set; }
        public MotionsAjin MotionsAjin { get; }
    }
}
