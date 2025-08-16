namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum EOutput1
    {
        OUT_SW_START_MAIN = 00,
        OUT_SW_START_LEFT = 01,
        OUT_SW_START_RIGHT = 02,
        OUT_SW_STOP = 03,
        OUT_SW_RESET = 04,
        OUT_BYPASS = 05,
        OUT_BUZZER_1 = 06,
        OUT_BUZZER_2 = 07,
        OUT_BUZZER_3 = 08,
        OUT_BUZZER_4 = 09,
        OUT_DISPLAY_PICKER_VAC_ON = 15,
        OUT_DISPLAY_PICKER_VAC_OFF = 16,
        OUT_GRIPPER_ON_L = 20,       // Rotate Unit
        OUT_GRIPPER_OFF_L = 21,       // Rotate Unit
        OUT_ROTATE_UP_L = 22,       // Rotate Unit
        OUT_ROTATE_DOWN_L = 23,       // Rotate Unit
        OUT_ROTATE_DEGREE_0_L = 24,       // Rotate Unit
        OUT_ROTATE_DEGREE_180_L = 25,       // Rotate Unit
        OUT_GRIPPER_ON_R = 26,       // Rotate Unit
        OUT_GRIPPER_OFF_R = 27,       // Rotate Unit
        OUT_ROTATE_UP_R = 28,       // Rotate Unit
        OUT_ROTATE_DOWN_R = 29,       // Rotate Unit
        OUT_ROTATE_DEGREE_0_R = 30,       // Rotate Unit
        OUT_ROTATE_DEGREE_180_R = 31,       // Rotate Unit
    }

    public enum EOutput2
    {
        OUT_TRAY_UNIT_LEFT_FWBW,
        OUT_TRAY_UNIT_RIGHT_FWBW,
    }
}
