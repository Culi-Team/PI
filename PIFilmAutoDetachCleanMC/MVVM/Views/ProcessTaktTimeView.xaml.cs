using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using PIFilmAutoDetachCleanMC.Process;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace PIFilmAutoDetachCleanMC.MVVM.Views
{
    public partial class ProcessTaktTimeView : UserControl
    {
        public ProcessTaktTimeView()
        {
            InitializeComponent();
            if (DataContext == null)
            {
                var taktTime = App.AppHost?.Services?.GetService<ProcessTaktTime>();
                if (taktTime != null)
                {
                    DataContext = new ProcessTaktTimeViewModel(taktTime);
                }
            }
        }

        public ProcessTaktTimeView(ProcessTaktTime taktTime) : this()
        {
            DataContext = new ProcessTaktTimeViewModel(taktTime);
        }

        public ProcessTaktTimeView(ProcessTaktTimeViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window?.Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var window = Window.GetWindow(this);
                window?.DragMove();
            }
        }

    }
}
