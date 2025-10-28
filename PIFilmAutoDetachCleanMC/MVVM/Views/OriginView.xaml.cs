using EQX.Core.Process;
using EQX.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using PIFilmAutoDetachCleanMC.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for OriginView.xaml
    /// </summary>
    public partial class OriginView : UserControl
    {
        public OriginView()
        {
            InitializeComponent();
        }
        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is OriginViewModel originVM == false) return;
            if (sender is Border border == false) return;
            if (border.DataContext is IProcess<ESequence> process == false) return;

            bool currentValue = process.IsOriginOrInitSelected;
            process.IsOriginOrInitSelected = !currentValue;

            if (originVM.Processes.TransferFixtureProcess.IsOriginOrInitSelected)
            {
                originVM.Processes.DetachProcess.IsOriginOrInitSelected = true;
                originVM.Processes.RobotLoadProcess.IsOriginOrInitSelected = true;
                originVM.Processes.RemoveFilmProcess.IsOriginOrInitSelected = true;
            }

            if (originVM.Processes.OutWorkConveyorProcess.IsOriginOrInitSelected ||
                originVM.Processes.InWorkConveyorProcess.IsOriginOrInitSelected ||
                originVM.Processes.VinylCleanProcess.IsOriginOrInitSelected ||
                originVM.Processes.FixtureAlignProcess.IsOriginOrInitSelected ||
                originVM.Processes.DetachProcess.IsOriginOrInitSelected ||
                originVM.Processes.RemoveFilmProcess.IsOriginOrInitSelected)
            {
                originVM.Processes.RobotLoadProcess.IsOriginOrInitSelected = true;
            }

            if(originVM.Processes.GlassTransferProcess.IsOriginOrInitSelected)
            {
                originVM.Processes.TransferInShuttleLeftProcess.IsOriginOrInitSelected = true;
                originVM.Processes.TransferInShuttleRightProcess.IsOriginOrInitSelected = true;
            }
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is OriginViewModel originVM == false) return;
            originVM.Processes.RootProcess.Childs!.ToList().ForEach(p => p.IsOriginOrInitSelected = false);
        }
    }
}
