namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferFixtureProcess_ReadyStep
    {
        Start,

        ErrorStatus_Check,

        Wait_RemoveFilm_and_Detach,

        PositionStatus_Check,

        Cylinder_Up,
        Cylinder_Up_Wait,

        YAxis_ReadyMove,
        YAxis_ReadyMove_Wait,

        Cylinder_Down,
        Cylinder_Down_Wait,

        Cylinder_Unclamp,
        Cylinder_Unclamp_Wait,

        End,
    }
}
