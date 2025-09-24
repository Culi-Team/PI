namespace PIFilmAutoDetachCleanMC.Defines.Devices.Robot
{
    public static class RobotHelpers
    {
        public static string PCPGMStart => string.Format("PCPGMSTART,0\r\n");
        public static string HomePositionCheck => string.Format("HomePositionCheck,0\r\n");
        public static string SeqHomeCheck => string.Format("SeqHomeCheck,0\r\n");
        public static string RobotStop => string.Format("stop,0\r\n");

        public static string SetModel(int model)
        {
            return $"model,{model},0\r\n";
        }

        public static string MotionCommands(ERobotCommand robotCommand, int lowSpeed, int highSpeed)
        {
            return $"motion,{(uint)robotCommand},{lowSpeed},{highSpeed},0\r\n";
        }

        public static string MotionRspComplete(ERobotCommand robotCommand)
        {
            return $"motion,{(uint)robotCommand},complete\r\n";
        }

        public static string MotionRspStart(ERobotCommand robotCommand)
        {
            return $"motion,{(uint)robotCommand},start\r\n";
        }

        public static string MotionCommands(ERobotCommand robotCommand, int lowSpeed, int highSpeed, params string[] paras)
        {
            if (paras == null || paras.Length != 8) throw new ArgumentException("Parameter format exception");

            return $"motion,{(uint)robotCommand},{lowSpeed},{highSpeed}," + string.Format(
                "{0}," +    // INDEX X
                "{1}," +    // INDEX Y
                "{2}," +    // OFFSET X
                "{3}," +    // OFFSET Y
                "{4}," +    // OFFSET Z
                "{5}," +    // OFFSET A
                "{6}," +    // OFFSET B
                "{7}," +    // OFFSET C
                "0\r\n", paras);
        }
    }
}
