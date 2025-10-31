using PIFilmAutoDetachCleanMC.Defines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.Process
{
    internal class ProcessesWorkSequence
    {
        public static readonly List<ETransferFixtureProcessLoadStep> TransferFixtureLoadSequence = new List<ETransferFixtureProcessLoadStep>
        {
            ETransferFixtureProcessLoadStep.Cyl_Up, //Cyl Up 1st
            ETransferFixtureProcessLoadStep.Cyl_Up_Wait,
            ETransferFixtureProcessLoadStep.YAxis_Move_LoadPosition, // Move to Load Position 1st
            ETransferFixtureProcessLoadStep.YAxis_Move_LoadPosition_Wait,
            ETransferFixtureProcessLoadStep.Cyl_Down, // Cyl Down 1st
            ETransferFixtureProcessLoadStep.Cyl_Down_Wait,
            ETransferFixtureProcessLoadStep.Cyl_Clamp,
            ETransferFixtureProcessLoadStep.Cyl_Clamp_Wait,
            ETransferFixtureProcessLoadStep.Set_FlagClampDone,
            ETransferFixtureProcessLoadStep.Wait_FixtureUnClampDone,
            ETransferFixtureProcessLoadStep.Cyl_Up, // Cyl Up 2nd
            ETransferFixtureProcessLoadStep.Cyl_Up_Wait,
            ETransferFixtureProcessLoadStep.Wait_RemoveFilm_Done,
            ETransferFixtureProcessLoadStep.YAxis_Move_UnloadPosition,
            ETransferFixtureProcessLoadStep.YAxis_Move_UnloadPosition_Wait,
            ETransferFixtureProcessLoadStep.Cyl_Down, // Cyl Down 2nd
            ETransferFixtureProcessLoadStep.Cyl_Down_Wait,
            ETransferFixtureProcessLoadStep.Cyl_UnClamp,
            ETransferFixtureProcessLoadStep.Cyl_UnClamp_Wait,
            ETransferFixtureProcessLoadStep.Cyl_Up, // Cyl Up 3rd
            ETransferFixtureProcessLoadStep.Cyl_Up_Wait,
            ETransferFixtureProcessLoadStep.YAxis_Move_LoadPosition, // Move to Load Position 2nd
            ETransferFixtureProcessLoadStep.YAxis_Move_LoadPosition_Wait,
            ETransferFixtureProcessLoadStep.Cyl_Down, // Cyl Down 3rd
            ETransferFixtureProcessLoadStep.Cyl_Down_Wait,
        };

        public static readonly List<EDetachStep> DetachSequence = new List<EDetachStep>
        {
            EDetachStep.Cyl_Clamp_Forward,
            EDetachStep.Cyl_Clamp_Forward_Wait,
            EDetachStep.ShuttleZAxis_Move_ReadyPosition,
            EDetachStep.ShuttleZAxis_Move_ReadyPosition_Wait,
            EDetachStep.XAxis_Move_DetachPosition,
            EDetachStep.XAxis_Move_DetachPosition_Wait,

            EDetachStep.ZAxis_Move_ReadyDetach1Position, // Z Axis Move Ready Detach Position 1st
            EDetachStep.ZAxis_Move_ReadyDetach1Position_Wait,

            EDetachStep.Cyl_Detach1_Down,
            EDetachStep.Cyl_Detach1_Down_Wait,

            EDetachStep.ZAxis_Move_Detach1Position,
            EDetachStep.ZAxis_Move_Detach1Position_Wait,

            EDetachStep.Vacuum_On,
            EDetachStep.ZAxis_Move_ReadyDetach2Position, // Z Axis Move Ready Detach Position 2nd
            EDetachStep.ZAxis_Move_ReadyDetach2Position_Wait,

            EDetachStep.Cyl_Detach2_Down,
            EDetachStep.Cyl_Detach2_Down_Wait,

            EDetachStep.ZAxis_Move_Detach2Position,
            EDetachStep.ZAxis_Move_Detach2Position_Wait,

            EDetachStep.ZAxis_Move_ReadyPosition,
            EDetachStep.ZAxis_Move_ReadyPosition_Wait,

            EDetachStep.Cyl_Detach_Up,
            EDetachStep.Cyl_Detach_Up_Wait,

            EDetachStep.XAxis_Move_DetachCheck_Position,
            EDetachStep.XAxis_Move_DetachCheck_Position_Wait,
            EDetachStep.Vacuum_Check,

            EDetachStep.Cyl_Clamp_Backward,
            EDetachStep.Cyl_Clamp_Backward_Wait,

            EDetachStep.Set_FlagDetachDone,
        };

        public static readonly List<ERemoveFilmRobotPickFromRemoveZoneStep> RemoveFilmRobotPickFromRemoveZoneSequence = new List<ERemoveFilmRobotPickFromRemoveZoneStep>
        {
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UnClamp,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UnClamp_Wait,

            ERemoveFilmRobotPickFromRemoveZoneStep.Set_Flag_RemoveFilmRequestUnload,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown1_Down,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown1_Down_Wait,

            ERemoveFilmRobotPickFromRemoveZoneStep.FilmCyl_UnClamp,
            ERemoveFilmRobotPickFromRemoveZoneStep.FilmCyl_UnClamp_Wait,

            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Down,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Down_Wait,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Up,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Up_Wait,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Down,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Down_Wait,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Up,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown2_Up_Wait,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown1_Up,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_UpDown1_Up_Wait,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_Pusher_Down,
            ERemoveFilmRobotPickFromRemoveZoneStep.Cyl_Pusher_Down_Wait,
            ERemoveFilmRobotPickFromRemoveZoneStep.Wait_RemoveFilmUnloadDone,
        };
    }
}
