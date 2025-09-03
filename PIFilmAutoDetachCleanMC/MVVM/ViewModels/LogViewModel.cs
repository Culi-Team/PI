using EQX.Core.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using PIFilmAutoDetachCleanMC.Defines.LogHistory;
using PIFilmAutoDetachCleanMC.Defines.Logs;
using System.Collections.ObjectModel;
using System.IO;
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
            // Load file XML
            XDocument doc = XDocument.Load(filePath);

            // Lấy tất cả node <entry> trong <activity>
            var entries = doc.Element("activity")?.Elements("entry") ?? Enumerable.Empty<XElement>();

            // Chuyển các node <entry> thành danh sách LogEntry
            return entries.Select(entry => new LogEntry
            {
                Time = entry.Element("time")?.Value,
                Type = entry.Element("type")?.Value,
                Source = entry.Element("source")?.Value,
                Description = entry.Element("description")?.Value,
            }).ToList();
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
                foreach (var dir in Directory.GetDirectories(path))
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

                foreach (var file in Directory.GetFiles(path, "*.xml"))
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
