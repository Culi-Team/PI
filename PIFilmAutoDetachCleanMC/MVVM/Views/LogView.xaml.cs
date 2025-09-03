using PIFilmAutoDetachCleanMC.Defines.LogHistory;
using PIFilmAutoDetachCleanMC.Defines.Logs;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
                }
                else
                {
                    LogDataGrid.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading log file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterCheckBox_Changed(object sender, RoutedEventArgs e)
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
                return (DebugCheckBox.IsChecked == true && entry.Type == "DEBUG") ||
                       (InfoCheckBox.IsChecked == true && entry.Type == "INFO") ||
                       (WarnCheckBox.IsChecked == true && entry.Type == "WARN") ||
                       (ErrorCheckBox.IsChecked == true && entry.Type == "ERROR") ||
                       (FatalCheckBox.IsChecked == true && entry.Type == "FATAL");
            };
            LogDataGrid.ItemsSource = collectionView;
        }

    }
}
