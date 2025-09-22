namespace PIFilmAutoDetachCleanMC.Defines.Devices.Robot
{
    public class RobotMotionParameter
    {
        public uint IndexX { get; set; }
        public uint IndexY { get; set; }
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public double OffsetZ { get; set; }
        public double OffsetA { get; set; }
        public double OffsetB { get; set; }
        public double OffsetC { get; set; }

        public RobotMotionParameter()
        {
            IndexX = IndexY = 0;
            OffsetX = OffsetY = OffsetZ = OffsetA = OffsetB = OffsetC = 0f;
        }
        public string[] GetStringArray()
        {
            return new string[]
            {
                IndexX.ToString(),
                IndexY.ToString(),
                OffsetX.ToString("0.###"),
                OffsetY.ToString("0.###"),
                OffsetZ.ToString("0.###"),
                OffsetA.ToString("0.###"),
                OffsetB.ToString("0.###"),
                OffsetC.ToString("0.###")
            };
        }
    }
}
