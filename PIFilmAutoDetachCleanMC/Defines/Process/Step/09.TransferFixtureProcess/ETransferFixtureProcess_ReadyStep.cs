namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETransferFixtureProcess_ReadyStep
    {
        Start,

        Status_Check,

        RobotReady_Wait,

        Cylinder_Up,
        Cylinder_Up_Wait,

        YAxis_ReadyMove,
        YAxis_ReadyMove_Wait,

        Cylinder_Down,
        Cylinder_Down_Wait,

        End,
    }
}
