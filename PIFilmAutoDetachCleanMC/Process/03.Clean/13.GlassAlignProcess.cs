using EQX.Core.InOut;
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
    public class GlassAlignProcess : ProcessBase<ESequence>
    {
        private EPort port => Name == EProcess.GlassAlignLeft.ToString() ? EPort.Left : EPort.Right;
        private readonly Devices _devices;

        private IDOutput AlignStageVac1 => port == EPort.Left ? _devices.Outputs.AlignStageLVac1OnOff : _devices.Outputs.AlignStageRVac1OnOff;
        private IDOutput AlignStageVac2 => port == EPort.Left ? _devices.Outputs.AlignStageLVac2OnOff : _devices.Outputs.AlignStageRVac2OnOff;
        private IDOutput AlignStageVac3 => port == EPort.Left ? _devices.Outputs.AlignStageLVac3OnOff : _devices.Outputs.AlignStageRVac3OnOff;

        private ICylinder AlignCyl1 => port == EPort.Left ? _devices.Cylinders.AlignStageL1AlignUnalign : _devices.Cylinders.AlignStageR1AlignUnalign;
        private ICylinder AlignCyl2 => port == EPort.Left ? _devices.Cylinders.AlignStageL2AlignUnalign : _devices.Cylinders.AlignStageR2AlignUnalign;
        private ICylinder AlignCyl3 => port == EPort.Left ? _devices.Cylinders.AlignStageL3AlignUnalign : _devices.Cylinders.AlignStageR3AlignUnalign;

        private bool IsGlass1Detect => port == EPort.Left ? _devices.Inputs.AlignStageRGlassDetect1.Value : _devices.Inputs.AlignStageRGlassDetect1.Value;
        private bool IsGlass2Detect => port == EPort.Left ? _devices.Inputs.AlignStageRGlassDetect1.Value : _devices.Inputs.AlignStageRGlassDetect1.Value;
        private bool IsGlass3Detect => port == EPort.Left ? _devices.Inputs.AlignStageRGlassDetect1.Value : _devices.Inputs.AlignStageRGlassDetect1.Value;

        private bool IsAlign => AlignCyl1.IsForward && AlignCyl2.IsForward && AlignCyl3.IsForward;
        private bool IsUnalign => AlignCyl1.IsBackward && AlignCyl2.IsBackward && AlignCyl3.IsBackward;

        private void AlignUnAlign(bool bAlignUnalign)
        {
            if(bAlignUnalign)
            {
                AlignCyl1.Forward();
                AlignCyl2.Forward();
                AlignCyl3.Forward();
            }
            else
            {
                AlignCyl1.Backward();
                AlignCyl2.Backward();
                AlignCyl3.Backward();
            }
        }

        public GlassAlignProcess(Devices devices)
        {
            _devices = devices;
        }
    }
}
