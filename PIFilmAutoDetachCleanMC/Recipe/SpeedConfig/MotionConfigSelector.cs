using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Recipe;
using EQX.UI.Controls;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class MotionConfigSelector : ObservableObject
    {
        #region Privates
        private readonly IConfiguration _configuration;
        #endregion

        #region Properties
        public MotionConfigList CurrentMotionConfig { get; private set; }
        #endregion

        #region Constructor
        public MotionConfigSelector(IConfiguration configuration, MotionConfigList currentMotionConfig)
        {
            _configuration = configuration;
            CurrentMotionConfig = currentMotionConfig;
        }
        #endregion

        #region Methods
        public bool Load()
        {
            try
            {
                // Load Motion Ajin Config
                string motionAjinConfigFile = _configuration.GetValue<string>("Files:MotionAjinParaConfigFile");
                if (File.Exists(motionAjinConfigFile))
                {
                    string motionAjinConfigContent = File.ReadAllText(motionAjinConfigFile);
                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };
                    
                    // Try to deserialize as array first, then as single object
                    MotionAjinConfig backupMotionAjinConfig = null;
                    try
                    {
                        // Try as array first
                        var configArray = JsonConvert.DeserializeObject<MotionAjinConfig[]>(motionAjinConfigContent, settings);
                        if (configArray != null && configArray.Length > 0)
                        {
                            backupMotionAjinConfig = configArray[0]; // Take first element
                        }
                    }
                    catch
                    {
                        // If array deserialization fails, try as single object
                        backupMotionAjinConfig = JsonConvert.DeserializeObject<MotionAjinConfig>(motionAjinConfigContent, settings);
                    }
                    
                    if (backupMotionAjinConfig != null)
                    {
                        ((IRecipe)CurrentMotionConfig.MotionAjinConfig).Clone((IRecipe)backupMotionAjinConfig);
                    }
                }

                // Load Motion Inovance Config
                string motionInovanceConfigFile = _configuration.GetValue<string>("Files:MotionInovanceParaConfigFile");
                if (File.Exists(motionInovanceConfigFile))
                {
                    string motionInovanceConfigContent = File.ReadAllText(motionInovanceConfigFile);
                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };
                    
                    // Try to deserialize as array first, then as single object
                    MotionInovanceConfig backupMotionInovanceConfig = null;
                    try
                    {
                        // Try as array first
                        var configArray = JsonConvert.DeserializeObject<MotionInovanceConfig[]>(motionInovanceConfigContent, settings);
                        if (configArray != null && configArray.Length > 0)
                        {
                            backupMotionInovanceConfig = configArray[0]; // Take first element
                        }
                    }
                    catch
                    {
                        // If array deserialization fails, try as single object
                        backupMotionInovanceConfig = JsonConvert.DeserializeObject<MotionInovanceConfig>(motionInovanceConfigContent, settings);
                    }
                    
                    if (backupMotionInovanceConfig != null)
                    {
                        ((IRecipe)CurrentMotionConfig.MotionInovanceConfig).Clone((IRecipe)backupMotionInovanceConfig);
                    }
                }

                // Load Vinyl Clean Encoder Config
                string vinylCleanEncoderConfigFile = _configuration.GetValue<string>("Files:VinylCleanEncoderParaConfigFile");
                if (File.Exists(vinylCleanEncoderConfigFile))
                {
                    string vinylCleanEncoderConfigContent = File.ReadAllText(vinylCleanEncoderConfigFile);
                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };
                    
                    // Try to deserialize as array first, then as single object
                    VinylCleanEncoderConfig backupVinylCleanEncoderConfig = null;
                    try
                    {
                        // Try as array first
                        var configArray = JsonConvert.DeserializeObject<VinylCleanEncoderConfig[]>(vinylCleanEncoderConfigContent, settings);
                        if (configArray != null && configArray.Length > 0)
                        {
                            backupVinylCleanEncoderConfig = configArray[0]; // Take first element
                        }
                    }
                    catch
                    {
                        // If array deserialization fails, try as single object
                        backupVinylCleanEncoderConfig = JsonConvert.DeserializeObject<VinylCleanEncoderConfig>(vinylCleanEncoderConfigContent, settings);
                    }
                    
                    if (backupVinylCleanEncoderConfig != null)
                    {
                        ((IRecipe)CurrentMotionConfig.VinylCleanEncoderConfig).Clone((IRecipe)backupVinylCleanEncoderConfig);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading motion config: {ex.Message}");
                return false;
            }
        }

        public void Save()
        {
            try
            {
                // Save Motion Ajin Config
                UpdateConfigFile(CurrentMotionConfig.MotionAjinConfig, "Files:MotionAjinParaConfigFile");

                // Save Motion Inovance Config
                UpdateConfigFile(CurrentMotionConfig.MotionInovanceConfig, "Files:MotionInovanceParaConfigFile");

                // Save Vinyl Clean Encoder Config
                UpdateConfigFile(CurrentMotionConfig.VinylCleanEncoderConfig, "Files:VinylCleanEncoderParaConfigFile");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving motion config: {ex.Message}");
            }
        }

        private void UpdateConfigFile(IRecipe config, string configFileKey)
        {
            string configFile = _configuration.GetValue<string>(configFileKey);
            if (!File.Exists(configFile))
            {
                MessageBox.Show($"Config file not found: {configFile}");
                return;
            }

            try
            {
                // Read existing file content
                string existingContent = File.ReadAllText(configFile);
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                // Try to deserialize as single object first
                try
                {
                    var singleObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(existingContent, settings);
                    if (singleObject != null)
                    {
                        // Update only the values we care about
                        singleObject["Velocity"] = GetPropertyValue(config, "Velocity");
                        singleObject["Acceleration"] = GetPropertyValue(config, "Acceleration");
                        singleObject["Deceleration"] = GetPropertyValue(config, "Deceleration");
                        
                        // Write back to file
                        string updatedJson = JsonConvert.SerializeObject(singleObject, Formatting.Indented);
                        File.WriteAllText(configFile, updatedJson);
                        return;
                    }
                }
                catch
                {
                    // If single object fails, try as array
                }

                // Try to deserialize as array
                try
                {
                    var configArray = JsonConvert.DeserializeObject<object[]>(existingContent, settings);
                    if (configArray != null && configArray.Length > 0)
                    {
                        // Update the first element with new values
                        var firstElement = JsonConvert.DeserializeObject<Dictionary<string, object>>(configArray[0].ToString());
                        if (firstElement != null)
                        {
                            // Update only the values we care about
                            firstElement["Velocity"] = GetPropertyValue(config, "Velocity");
                            firstElement["Acceleration"] = GetPropertyValue(config, "Acceleration");
                            firstElement["Deceleration"] = GetPropertyValue(config, "Deceleration");
                            
                            // Update the first element in array
                            configArray[0] = firstElement;
                            
                            // Write back to file
                            string updatedJson = JsonConvert.SerializeObject(configArray, Formatting.Indented);
                            File.WriteAllText(configFile, updatedJson);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating config file {configFile}: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating config file {configFile}: {ex.Message}");
            }
        }

        private object GetPropertyValue(IRecipe config, string propertyName)
        {
            var property = config.GetType().GetProperty(propertyName);
            return property?.GetValue(config);
        }

        public void SaveMotionAjinConfig()
        {
            try
            {
                UpdateConfigFile(CurrentMotionConfig.MotionAjinConfig, "Files:MotionAjinParaConfigFile");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving Motion Ajin config: {ex.Message}");
            }
        }

        public void SaveMotionInovanceConfig()
        {
            try
            {
                UpdateConfigFile(CurrentMotionConfig.MotionInovanceConfig, "Files:MotionInovanceParaConfigFile");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving Motion Inovance config: {ex.Message}");
            }
        }

        public void SaveVinylCleanEncoderConfig()
        {
            try
            {
                UpdateConfigFile(CurrentMotionConfig.VinylCleanEncoderConfig, "Files:VinylCleanEncoderParaConfigFile");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving Vinyl Clean Encoder config: {ex.Message}");
            }
        }
        #endregion
    }
}

