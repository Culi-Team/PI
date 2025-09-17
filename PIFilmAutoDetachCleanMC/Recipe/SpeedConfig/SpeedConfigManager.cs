using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class SpeedConfigManager
    {
        private readonly IConfiguration _configuration;
        private readonly string speedConfigFolder;
        private readonly string speedConfigFile;

        public SpeedConfigManager(IConfiguration configuration)
        {
            _configuration = configuration;
            string baseRecipeFolder = _configuration.GetValue<string>("Folders:RecipeFolder") ?? "";
            
            // Use the base recipe folder directly, not a subfolder
            speedConfigFolder = baseRecipeFolder;
            speedConfigFile = Path.Combine(speedConfigFolder, "MotionInovancePara.config");
        }

        public MontionSpeedConfig CurrentSpeedConfig { get; private set; }

        public bool Load()
        {
            try
            {
                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(speedConfigFolder))
                {
                    Directory.CreateDirectory(speedConfigFolder);
                }

                if (File.Exists(speedConfigFile))
                {
                    string jsonContent = File.ReadAllText(speedConfigFile);
                    CurrentSpeedConfig = JsonConvert.DeserializeObject<MontionSpeedConfig>(jsonContent);
                }
                else
                {
                    // Tạo config mặc định nếu chưa tồn tại
                    CurrentSpeedConfig = new MontionSpeedConfig();
                    // Không gọi Save() ở đây để tránh vòng lặp
                }
                return true;
            }
            catch (Exception ex)
            {
                // Log error nhưng không hiển thị MessageBox để tránh treo
                System.Diagnostics.Debug.WriteLine($"Error loading speed config: {ex.Message}");
                CurrentSpeedConfig = new MontionSpeedConfig();
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                if (CurrentSpeedConfig == null)
                {
                    CurrentSpeedConfig = new MontionSpeedConfig();
                }

                string jsonContent = JsonConvert.SerializeObject(CurrentSpeedConfig, Formatting.Indented);
                File.WriteAllText(speedConfigFile, jsonContent);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving speed config: {ex.Message}");
                return false;
            }
        }

        public void UpdateSpeedConfig(MontionSpeedConfig newConfig)
        {
            CurrentSpeedConfig = newConfig;
        }

        public void UpdateSpeedConfigProperty(string propertyName, object value)
        {
            if (CurrentSpeedConfig == null)
            {
                CurrentSpeedConfig = new MontionSpeedConfig();
            }

            var property = CurrentSpeedConfig.GetType().GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(CurrentSpeedConfig, value);
            }
        }
    }
}
