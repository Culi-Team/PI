namespace PIFilmAutoDetachCleanMC.Defines.Devices.Robot
{
    public enum ERobotCommand : uint
    {
        NONE = 0,

        HOME = 1,
        READY = 2,

        S1_PICK = 10,
        S1_PICK_UP = 11,

        S2_PICK = 110,
        S2_PICK_UP = 111,

        S3_PICK = 210,
        S3_PICK_UP = 211,

        S4_PICK = 310,
        S4_PICK_UP = 311,

        S5_PICK = 410,
        S5_PICK_UP = 411,
    }
}
