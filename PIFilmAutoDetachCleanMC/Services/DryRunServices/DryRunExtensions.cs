using EQX.Core.InOut;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Services.DryRunServices
{
    public static class DryRunExtensions
    {
        public static bool ShouldBypass(this MachineStatus machineStatus, IDInput input)
        {
            if (machineStatus is null || input is null)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(EInput), input.Id))
            {
                return false;
            }

            return machineStatus.ShouldBypass((EInput)input.Id);
        }

        public static bool IsSatisfied(this MachineStatus machineStatus, IDInput input, bool expected = true)
        {
            if (input is null)
            {
                return false;
            }

            if (machineStatus.ShouldBypass(input))
            {
                return true;
            }

            return input.Value == expected;
        }
    }
}
