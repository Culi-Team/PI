using System;
using System.Windows.Controls;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using EQX.Core.Process;
using PIFilmAutoDetachCleanMC.Defines;

namespace PIFilmAutoDetachCleanMC.MVVM.Views
{
    /// <summary>
    /// Interaction logic for TeachView.xaml
    /// </summary>
    public partial class TeachView : UserControl
    {
        public TeachView()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is TeachViewModel viewModel)
            {
                // Lấy TabItem được chọn
                if (sender is TabControl tabControl && tabControl.SelectedItem is TabItem selectedTabItem)
                {
                    // Xác định process tương ứng với tab được chọn
                    IProcess<ESequence> selectedProcess = GetProcessFromTabItem(selectedTabItem);
                    if (selectedProcess != null)
                    {
                        viewModel.SelectedProcess = selectedProcess;
                    }
                }
            }
        }

        private IProcess<ESequence> GetProcessFromTabItem(TabItem tabItem)
        {
            string header = tabItem.Header?.ToString();
            
            if (DataContext is TeachViewModel viewModel)
            {
                switch (header)
                {
                    case "In Cassette":
                        return viewModel.Processes?.InWorkConveyorProcess;
                    case "Out Cassette":
                        return viewModel.Processes?.OutWorkConveyorProcess;
                    case "Transfer Fixture":
                        return viewModel.Processes?.TransferFixtureProcess;
                    case "Detach":
                        return viewModel.Processes?.DetachProcess;
                    case "Glass Transfer":
                        return viewModel.Processes?.GlassTransferProcess;
                    default:
                        return null;
                }
            }
            
            return null;
        }
    }
}
