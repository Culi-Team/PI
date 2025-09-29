using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using PIFilmAutoDetachCleanMC.Helpers;

namespace PIFilmAutoDetachCleanMC.Defines.ProductDatas
{
    public class CWorkData : ObservableObject
    {
        private readonly AppSettings _appSettings;

        public CWorkData(CCountData countData,
            CTaktTime taktTime,
            AppSettings appSettings)
        {
            CountData = countData;
            TaktTime = taktTime;
            _appSettings = appSettings;
        }
        public CWorkData()
        {
            CountData = new CCountData();
            TaktTime = new CTaktTime();
        }
        public CCountData CountData { get; }
        public CTaktTime TaktTime { get; }

        public void Reset()
        {
            Save();
            CountData.Reset();
            TaktTime.Reset();
        }

        public void Save()
        {
            string file = DefaulWorkDataFile.Replace(".json", $"_{WorkDataFileIndex + 1}.json");

            File.WriteAllText(file, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public void Load()
        {
            CWorkData loadedWorkData = null;

            if (File.Exists(WorkDataFilePath))
            {
                try
                {
                    string jsonContent = File.ReadAllText(WorkDataFilePath);
                    loadedWorkData = JsonConvert.DeserializeObject<CWorkData>(jsonContent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading file: {ex.Message}");
                }
            }

            if (loadedWorkData == null)
            {
                loadedWorkData = new CWorkData();
            }

            CountData.Copy(loadedWorkData.CountData);
            TaktTime.Copy(loadedWorkData.TaktTime);
        }
        private string WorkDataFilePath
        {
            get
            {
                string directoryPath = Path.Combine(_appSettings.Folders.CountDataFolder, $"{DateTime.Now:yyyy-MM}");
                Directory.CreateDirectory(directoryPath);

                return DefaulWorkDataFile.Replace(".json", $"_{WorkDataFileIndex}.json");
            }
        }

        private string DefaulWorkDataFile
        {
            get
            {
                string folderPath = Path.Combine(_appSettings.Folders.CountDataFolder, $"{DateTime.Now:yyyy-MM}", $"{DateTime.Now:yyyy-MM-dd}");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                return Path.Combine(folderPath, $"Count_{DateTime.Now:yyyy-MM-dd}.json");
            }
        }
               
        private int WorkDataFileIndex
        {
            get
            {
                string directoryPath = Path.Combine(_appSettings.Folders.CountDataFolder, $"{DateTime.Now:yyyy-MM}", $"{DateTime.Now:yyyy-MM-dd}");
                if (!Directory.Exists(directoryPath)) return 0;

                string[] filePaths = Directory.GetFiles(directoryPath, $"Count_{DateTime.Now:yyyy-MM-dd}*.json");
                if (filePaths.Length == 0) return 0;

                string fileLast = filePaths.Last();
                string fileName = Path.GetFileNameWithoutExtension(fileLast);

                if (int.TryParse(fileName.Split('_').Last(), out int index))
                {
                    return index;
                }

                return 0; 
            }
        }
    }
}
