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

        //public static bool ShouldBypassVacuum(this MachineStatus machineStatus, IDInput input)
        //{
        //    if (machineStatus is null)
        //    {
        //        return false;
        //    }

        //    return machineStatus.ShouldBypassVacuum(input);
        //}

        //public static bool ShouldBypassVacuum(this MachineStatus machineStatus, IEnumerable<IDInput> inputs)
        //{
        //    if (machineStatus is null || inputs is null)
        //    {
        //        return false;
        //    }

        //    bool hasInput = false;

        //    foreach (var input in inputs)
        //    {
        //        if (input is null)
        //        {
        //            continue;
        //        }

        //        hasInput = true;

        //        if (!machineStatus.ShouldBypassVacuum(input))
        //        {
        //            return false;
        //        }
        //    }

        //    return hasInput;
        //}

        //public static int GetVacuumDelay(this MachineStatus machineStatus, double recipeDelaySeconds, IEnumerable<IDInput> inputs)
        //{
        //    if (machineStatus is null)
        //    {
        //        return (int)(recipeDelaySeconds * 1000);
        //    }

        //    return machineStatus.ShouldBypassVacuum(inputs)
        //        ? machineStatus.DryRunVacuumDuration
        //        : (int)(recipeDelaySeconds * 1000);
        //}

        //public static void ReleaseVacuumOutputsIfBypassed(this MachineStatus machineStatus, IEnumerable<IDInput> inputs, params IDOutput[] outputs)
        //{
        //    if (machineStatus is null || outputs is null)
        //    {
        //        return;
        //    }

        //    if (!machineStatus.ShouldBypassVacuum(inputs))
        //    {
        //        return;
        //    }

        //    foreach (var output in outputs)
        //    {
        //        if (output is null)
        //        {
        //            continue;
        //        }

        //        output.Value = false;
        //    }
        //}
    }
}
