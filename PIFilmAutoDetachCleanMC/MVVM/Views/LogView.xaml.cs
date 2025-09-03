using PIFilmAutoDetachCleanMC.Defines.LogHistory;
using PIFilmAutoDetachCleanMC.Defines.Logs;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
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
                    SourceComboBox.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading log file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeFilterSourceComboBox()
        {
            if (currentLogEntries == null) return;

            var sources = currentLogEntries
                .Select(entry => entry.Source)
                .Where(source => !string.IsNullOrEmpty(source))
                .Distinct()
                .OrderBy(source => source)
                .ToList();

            var sourceItems = new List<SourceItem>
            {
                new SourceItem { Name = "All", Value = "All", Color = Brushes.Black }
            };

            // Màu sắc cho các source khác nhau
            var colors = new List<Brush>
            {
                new SolidColorBrush(Color.FromRgb(76, 175, 80)),   // Xanh lá
                new SolidColorBrush(Color.FromRgb(33, 150, 243)),  // Xanh dương
                new SolidColorBrush(Color.FromRgb(255, 152, 0)),   // Cam
                new SolidColorBrush(Color.FromRgb(156, 39, 176)),  // Tím
                new SolidColorBrush(Color.FromRgb(244, 67, 54)),   // Đỏ
                new SolidColorBrush(Color.FromRgb(0, 150, 136)),   // Teal
                new SolidColorBrush(Color.FromRgb(255, 193, 7)),   // Vàng
                new SolidColorBrush(Color.FromRgb(96, 125, 139)),  // Xám xanh
                new SolidColorBrush(Color.FromRgb(121, 85, 72)),   // Nâu
                new SolidColorBrush(Color.FromRgb(233, 30, 99))    // Hồng
            };

            for (int i = 0; i < sources.Count; i++)
            {
                var colorIndex = i % colors.Count;
                sourceItems.Add(new SourceItem 
                { 
                    Name = sources[i], 
                    Value = sources[i], 
                    Color = colors[colorIndex]
                });
            }

            SourceComboBox.ItemsSource = sourceItems;
            SourceComboBox.SelectedIndex = 0; // Chọn "All" mặc định
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
                var selectedFilterSource = SourceComboBox.SelectedItem as SourceItem;
                bool filterSourceMatch = selectedFilterSource?.Value == "All" || entry.Source == selectedFilterSource?.Value;
                
                // Kết hợp cả hai filter 
                return filterTypeMatch && filterSourceMatch;
            };
            LogDataGrid.ItemsSource = collectionView;
        }

    }
}
