using System;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class AppSettings
    {
        public Folders Folders { get; set; } = new Folders();
        public Files Files { get; set; } = new Files();
    }
    public class Folders
    {
        public string LogFolder { get; set; } = string.Empty;
        public string BackupFolder { get; set; } = string.Empty;
        public string RecipeFolder { get; set; } = string.Empty;
        public string VisionDataFolder { get; set; } = string.Empty;
        public string CountDataFolder { get; set; } = string.Empty;
    }
    public class Files
    {
        public string LogConfigFile { get; set; } = string.Empty;
        public string ProcessConfigFile { get; set; } = string.Empty;
        public string MotionInovanceParaConfigFile { get; set; } = string.Empty;
        public string MotionAjinParaConfigFile { get; set; } = string.Empty;
        public string VinylCleanEncoderParaConfigFile { get; set; } = string.Empty;
        public string VisionFlowListConfigFile { get; set; } = string.Empty;
        public string CameraConfigFile { get; set; } = string.Empty;
    }
}
