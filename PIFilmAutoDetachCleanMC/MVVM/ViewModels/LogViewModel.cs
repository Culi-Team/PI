using EQX.Core.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using PIFilmAutoDetachCleanMC.Defines.LogHistory;
using PIFilmAutoDetachCleanMC.Defines.Logs;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class LogViewModel : ViewModelBase
    {
        private readonly IConfiguration _configuration;
        private string LogFolder => _configuration["Folders:LogFolder"] ?? "";
        public LogViewModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ObservableCollection<FileSystemNode> LogFiles { get; set; }

        public List<LogEntry> LoadLogEntries(string filePath)
        {
            var logEntries = new List<LogEntry>();

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(new[] { ',' }, 4);

                if (parts.Length < 4)
                    continue; 

                var time = parts[0].Trim().Trim('[', ']');
                var type = parts[1].Trim();
                var source = parts[2].Trim();
                var description = parts[3].Trim(); 

                logEntries.Add(new LogEntry
                {
                    Time = time,
                    Type = type,
                    Source = source,
                    Description = description
                });
            }

            return logEntries;
        }

        public void LoadLogFiles()
        {
            try
            {
                var rootNode = new FileSystemNode
                {
                    Name = "Log",
                    Path = LogFolder,
                    IsDirectory = true
                };

                PopulateTreeView(rootNode, LogFolder);

                LogFiles = new ObservableCollection<FileSystemNode> { rootNode };

                OnPropertyChanged(nameof(LogFiles));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading directory: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PopulateTreeView(FileSystemNode parentNode, string path)
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(path).OrderByDescending(d => d))
                {
                    var dirInfo = new DirectoryInfo(dir);
                    var dirNode = new FileSystemNode
                    {
                        Name = dirInfo.Name,
                        Path = dir,
                        IsDirectory = true
                    };

                    PopulateTreeView(dirNode, dir);
                    parentNode.Children.Add(dirNode);
                }

                foreach (var file in Directory.GetFiles(path, "*.txt"))
                {
                    var fileInfo = new FileInfo(file);
                    var fileNode = new FileSystemNode
                    {
                        Name = fileInfo.Name,
                        Path = file,
                        IsDirectory = false
                    };
                    parentNode.Children.Add(fileNode);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error populating TreeView for {path}: {ex}");
            }
        }

    }
}
