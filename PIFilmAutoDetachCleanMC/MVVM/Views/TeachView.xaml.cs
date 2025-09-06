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
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using EQX.Core.Process;
using PIFilmAutoDetachCleanMC.Process;
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
                    case "In Cassette Position":
                        return viewModel.Processes?.InWorkConveyorProcess;
                    case "Out Cassette Position":
                        return viewModel.Processes?.OutWorkConveyorProcess;
                    case "Transfer Fixture Position":
                        return viewModel.Processes?.TransferFixtureProcess;
                    case "Detach Position":
                        return viewModel.Processes?.DetachProcess;
                    default:
                        return null;
                }
            }
            
            return null;
        }
    }
}
