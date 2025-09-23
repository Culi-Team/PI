namespace PIFilmAutoDetachCleanMC.Defines.Devices.Robot
{
    public enum ERobotCommand : uint
    {
        NONE = 0,

        HOME = 1,
        READY = 2,

        S1_RDY = 10,
        S1_PP_UP = 11,
        S1_PP = 12,
        S1_PICKUP = 100,

        S2_RDY = 110,
        S2_PP_UP = 111,
        S2_PP = 112,
        S2_PLACE = 199,
        S2_PICKUP = 200,

        S3_RDY = 210,
        S3_PP_UP = 211,
        S3_PP = 212,
        S3_PLACE = 299,

        S4_RDY = 310,
        S4_PP_UP = 311,
        S4_PP = 312,
        S4_PICKUP = 400,

        S5_RDY = 410,
        S5_PP_UP = 411,
        S5_PP = 412,
        S5_PLACE = 499,
    }
}
