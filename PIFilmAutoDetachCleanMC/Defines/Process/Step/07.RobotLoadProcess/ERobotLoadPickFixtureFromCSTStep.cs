namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadPickFixtureFromCSTStep
    {
        Start,
        Cyl_UnClamp,
        Cyl_UnClamp_Wait,
        Wait_InCST_Ready,
        Index_Initiation,
        Move_InCST_PickPositon,
        Move_InCST_PickPosition_Wait,

        Cyl_Clamp,
        Cyl_Clamp_Wait,
        Cyl_Align,
        Cyl_Align_Wait,

        Move_InCST_ReadyPositon,
        Move_InCST_ReadyPositon_Wait,

        Update_Cassette_Status,

        Set_Flag_RobotPickInCSTDone,
        Wait_InCST_PickDone,

        End
    }
}
