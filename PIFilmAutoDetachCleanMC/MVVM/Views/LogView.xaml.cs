using PIFilmAutoDetachCleanMC.Defines.LogHistory;
using PIFilmAutoDetachCleanMC.Defines.Logs;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using PIFilmAutoDetachCleanMC.Process;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

public class LogLevelItem
{
    public string Name { get; set; }
    public string Value { get; set; }
    public Brush Color { get; set; }
    
    public override string ToString()
    {
        return Name;
    }
}

public class SourceItem
{
    public string Name { get; set; }
    public string Value { get; set; }
    public Brush Color { get; set; }
    
    public override string ToString()
    {
        return Name;
    }
}

namespace PIFilmAutoDetachCleanMC.MVVM.Views
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        public LogView()
        {
            InitializeComponent();
        }

        private List<LogEntry> currentLogEntries;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.DataContext is not LogViewModel viewModel)
                return;

            viewModel.LoadLogFiles();
            InitializeFilterTypeComboBox();
            InitializeFilterSourceComboBox();
        }

        private void InitializeFilterTypeComboBox()
        {
            var FilterType = new List<LogLevelItem>
            {
                new LogLevelItem { Name = "All", Value = "ALL", Color = Brushes.Black },
                new LogLevelItem { Name = "DEBUG", Value = "DEBUG", Color = Brushes.Gray },
                new LogLevelItem { Name = "INFO", Value = "INFO", Color = new SolidColorBrush(Color.FromRgb(33, 150, 243)) },
                new LogLevelItem { Name = "WARN", Value = "WARN", Color = new SolidColorBrush(Color.FromRgb(255, 152, 0)) },
                new LogLevelItem { Name = "ERROR", Value = "ERROR", Color = new SolidColorBrush(Color.FromRgb(244, 67, 54)) },
                new LogLevelItem { Name = "FATAL", Value = "FATAL", Color = new SolidColorBrush(Color.FromRgb(156, 39, 176)) }
            };

            FilterTypeComboBox.ItemsSource = FilterType;
            FilterTypeComboBox.SelectedIndex = 0; // Chọn "All" mặc định
        }

        private void LogTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.DataContext is not LogViewModel viewModel) return;
            try
            {
                if (LogTreeView.SelectedItem is FileSystemNode selectedNode && !selectedNode.IsDirectory)
                {
                    currentLogEntries = viewModel.LoadLogEntries(selectedNode.Path);
                    LogDataGrid.ItemsSource = currentLogEntries;
                    InitializeFilterSourceComboBox();
                }
                else
                {
                    LogDataGrid.ItemsSource = null;
                    FilterSourceComboBox.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading log file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeFilterSourceComboBox()
        {
            var sourceItems = new List<SourceItem>
            {
                new SourceItem { Name = "All", Value = "All" }
            };

            // Lấy tất cả các giá trị từ enum EProcess
            var processValues = Enum.GetValues<EProcess>()
                .Where(p => p != EProcess.Root) // Loại bỏ Root nếu không cần
                .OrderBy(p => p.ToString())
                .ToList();

            foreach (var process in processValues)
            {
                sourceItems.Add(new SourceItem 
                { 
                    Name = process.ToString(), 
                    Value = process.ToString()
                });
            }

            FilterSourceComboBox.ItemsSource = sourceItems;
            FilterSourceComboBox.SelectedIndex = 0; // Chọn "All" mặc định
        }

        private void FilterTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void FilterSourceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (currentLogEntries == null) return;

            if (LogDataGrid == null) return;

            var collectionView = CollectionViewSource.GetDefaultView(currentLogEntries);
            collectionView.Filter = item =>
            {
                var entry = item as LogEntry;
                
                // Filter theo Type
                var selectedFilterType = FilterTypeComboBox.SelectedItem as LogLevelItem;
                bool filterTypeMatch = selectedFilterType?.Value == "ALL" || entry.Type == selectedFilterType?.Value;
                
                // Filter theo Source
                var selectedFilterSource = FilterSourceComboBox.SelectedItem as SourceItem;
                bool filterSourceMatch = selectedFilterSource?.Value == "All" || entry.Source == selectedFilterSource?.Value;
                
                // Kết hợp cả hai filter 
                return filterTypeMatch && filterSourceMatch;
            };
            LogDataGrid.ItemsSource = collectionView;
        }

    }
}
