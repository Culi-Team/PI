using EQX.Core.InOut;
using EQX.Core.Sequence;
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
    public class UnloadAlignProcess : ProcessBase<ESequence>
    {
        #region Privates
        private readonly Devices _devices;
        private readonly CommonRecipe _commonRecipe;

        private ICylinder AlignCyl1 => _devices.Cylinders.UnloadAlignCyl1UpDown;
        private ICylinder AlignCyl2 => _devices.Cylinders.UnloadAlignCyl2UpDown;
        private ICylinder AlignCyl3 => _devices.Cylinders.UnloadAlignCyl3UpDown;
        private ICylinder AlignCyl4 => _devices.Cylinders.UnloadAlignCyl4UpDown;
        private bool IsAlign => AlignCyl1.IsForward && AlignCyl2.IsForward && AlignCyl3.IsForward && AlignCyl4.IsForward;
        private bool IsUnalign => AlignCyl1.IsBackward && AlignCyl2.IsBackward && AlignCyl3.IsBackward && AlignCyl4.IsBackward;

        private IDOutput AlignVac1 => _devices.Outputs.UnloadGlassAlignVac1OnOff;
        private IDOutput AlignVac2 => _devices.Outputs.UnloadGlassAlignVac2OnOff;
        private IDOutput AlignVac3 => _devices.Outputs.UnloadGlassAlignVac3OnOff;
        private IDOutput AlignVac4 => _devices.Outputs.UnloadGlassAlignVac4OnOff;

        private bool IsGlassVac1 => _devices.Inputs.UnloadGlassAlignVac1.Value;
        private bool IsGlassVac2 => _devices.Inputs.UnloadGlassAlignVac2.Value;
        private bool IsGlassVac3 => _devices.Inputs.UnloadGlassAlignVac3.Value;
        private bool IsGlassVac4 => _devices.Inputs.UnloadGlassAlignVac4.Value;
        private bool IsGlassVac => IsGlassVac1 && IsGlassVac2 && IsGlassVac3 && IsGlassVac4;

        private bool IsGlassDetect1 => _devices.Inputs.UnloadGlassDetect1.Value;
        private bool IsGlassDetect2 => _devices.Inputs.UnloadGlassDetect2.Value;
        private bool IsGlassDetect3 => _devices.Inputs.UnloadGlassDetect3.Value;
        private bool IsGlassDetect4 => _devices.Inputs.UnloadGlassDetect4.Value;
        private bool IsGlassDetect => IsGlassDetect1 && IsGlassDetect2 && IsGlassDetect3 && IsGlassDetect4;
        #endregion

        #region Private Methods
        private void AlignUnalign(bool bAlignUnAlign)
        {
            if (bAlignUnAlign)
            {
                AlignCyl1.Forward();
                AlignCyl2.Forward();
                AlignCyl3.Forward();
                AlignCyl4.Forward();
            }
            else
            {
                AlignCyl1.Backward();
                AlignCyl2.Backward();
                AlignCyl3.Backward();
                AlignCyl4.Backward();
            }
        }

        private void VacOnOff(bool bOnOff)
        {
                AlignVac1.Value = bOnOff;
                AlignVac2.Value = bOnOff;
                AlignVac3.Value = bOnOff;
                AlignVac4.Value = bOnOff;
        }
        #endregion

        #region Constructor
        public UnloadAlignProcess(Devices devices,CommonRecipe commonRecipe)
        {
            _devices = devices;
            _commonRecipe = commonRecipe;
        }
        #endregion

        #region Override Methods
        public override bool ProcessOrigin()
        {
            switch((EUnloadAlignOriginStep)Step.OriginStep)
            {
                case EUnloadAlignOriginStep.Start:
                    Log.Debug("Origin Start");
                    Step.OriginStep++;
                    break;
                case EUnloadAlignOriginStep.Cyl_Unalign:
                    Log.Debug("Cylinder Unalign");
                    AlignUnalign(false);
                    Wait(_commonRecipe.CylinderMoveTimeout, () => { return IsUnalign; });
                    Step.OriginStep++;
                    break;
                case EUnloadAlignOriginStep.Cyl_Unalign_Wait:
                    Log.Debug("Cylinder Unalign Wait");
                    if (!IsUnalign)
                    {
                        //Timeout ALARM
                        break;
                    }
                    Step.OriginStep++;
                    break;
                case EUnloadAlignOriginStep.End:
                    Log.Debug("Origin End");
                    ProcessStatus = EProcessStatus.OriginDone;
                    Step.OriginStep++;
                    return true;
            }
            return true;
        }
        #endregion
    }
}
