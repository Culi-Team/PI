using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class Devices
    {
        public Devices(Inputs inputs, Outputs outputs, Motions motions)
        {
            Inputs = inputs;
            Outputs = outputs;
            Motions = motions;
        }

        public Inputs Inputs { get; set; }
        public Outputs Outputs { get; set; }
        public Motions Motions { get; set; }
    }
}
