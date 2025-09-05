using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Process;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using static PIFilmAutoDetachCleanMC.MVVM.ViewModels.TeachViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace PIFilmAutoDetachCleanMC.MVVM.Views
{
    /// <summary>
    /// Interaction logic for UnitTeachingView.xaml
    /// </summary>
    public partial class UnitTeachingView : UserControl
    {
        public ObservableCollection<IMotion> Motions
        {
            get { return (ObservableCollection<IMotion>)GetValue(MotionsProperty); }
            set { SetValue(MotionsProperty, value); }
        }
        public static readonly DependencyProperty MotionsProperty =
            DependencyProperty.Register("Motions", typeof(ObservableCollection<IMotion>), typeof(UnitTeachingView), new PropertyMetadata(new ObservableCollection<IMotion> { }));

        public ObservableCollection<ICylinder> Cylinders
        {
            get { return (ObservableCollection<ICylinder>)GetValue(CylindersProperty); }
            set { SetValue(CylindersProperty, value); }
        }
        public static readonly DependencyProperty CylindersProperty =
            DependencyProperty.Register("Cylinders", typeof(ObservableCollection<ICylinder>), typeof(UnitTeachingView), new PropertyMetadata(new ObservableCollection<ICylinder> { }));
        public ObservableCollection<PositionTeaching> PositionTeachings
        {
            get { return (ObservableCollection<PositionTeaching>)GetValue(PositionTeachingsProperty); }
            set { SetValue(PositionTeachingsProperty, value); }
        }
        public static readonly DependencyProperty PositionTeachingsProperty =
            DependencyProperty.Register("PositionTeachings", typeof(ObservableCollection<PositionTeaching>), typeof(UnitTeachingView), new PropertyMetadata(new ObservableCollection<PositionTeaching> { }));
        public ObservableCollection<IDOutput> Outputs
        {
            get { return (ObservableCollection<IDOutput>)GetValue(OutputsProperty); }
            set { SetValue(OutputsProperty, value); }
        }
        public static readonly DependencyProperty OutputsProperty =
            DependencyProperty.Register("Outputs", typeof(ObservableCollection<IDOutput>), typeof(UnitTeachingView), new PropertyMetadata(new ObservableCollection<IDOutput> { }));
        public ObservableCollection<IDInput> Inputs
        {
            get { return (ObservableCollection<IDInput>)GetValue(InputsProperty); }
            set { SetValue(InputsProperty, value); }
        }
        public static readonly DependencyProperty InputsProperty =
            DependencyProperty.Register("Inputs", typeof(ObservableCollection<IDInput>), typeof(UnitTeachingView), new PropertyMetadata(new ObservableCollection<IDInput> { }));

        public IProcess<ESequence> SelectedProcess
        {
            get { return (IProcess<ESequence>)GetValue(SelectedProcessProperty); }
            set { SetValue(SelectedProcessProperty, value); }
        }
        public static readonly DependencyProperty SelectedProcessProperty =
            DependencyProperty.Register("SelectedProcess", typeof(IProcess<ESequence>), typeof(UnitTeachingView), new PropertyMetadata(null));

        public UnitTeachingView()
        {
            InitializeComponent();
        }


        private void UpdateMotionsBasedOnProcess()
        {
            if (DataContext is TeachViewModel viewModel && SelectedProcess != null)
            {
                ObservableCollection<IMotion> processMotions = GetMotionsForProcess(viewModel, SelectedProcess);
                if (processMotions != null)
                {
                    Motions = processMotions;
                }
            }
        }

        private ObservableCollection<IMotion> GetMotionsForProcess(TeachViewModel viewModel, IProcess<ESequence> process)
        {
            // Get process name to determine which motion property to use
            string processName = process?.GetType().Name ?? "";
            
            switch (processName)
            {
                // CSTLoadUnload Tab
                case "InConveyorProcess":
                    return viewModel.InConveyorMotions;
                case "InWorkConveyorProcess":
                    return viewModel.InWorkConveyorMotions;
                case "OutWorkConveyorProcess":
                    return viewModel.OutWorkConveyorMotions;
                case "OutConveyorProcess":
                    return viewModel.OutConveyorMotions;

                // Detach Tab
                case "TransferFixtureProcess":
                    return viewModel.TransferFixtureMotions;
                case "DetachProcess":
                    return viewModel.DetachMotions;

                // Clean Tab
                case "GlassTransferProcess":
                    return viewModel.GlassTransferMotions;
                case "GlassAlignLeftProcess":
                    return viewModel.GlassAlignLeftMotions;
                case "GlassAlignRightProcess":
                    return viewModel.GlassAlignRightMotions;
                case "TransferInShuttleLeftProcess":
                    return viewModel.TransferInShuttleLeftMotions;
                case "TransferInShuttleRightProcess":
                    return viewModel.TransferInShuttleRightMotions;
                case "WETCleanLeftProcess":
                    return viewModel.WETCleanLeftMotions;
                case "WETCleanRightProcess":
                    return viewModel.WETCleanRightMotions;
                case "AFCleanLeftProcess":
                    return viewModel.AFCleanLeftMotions;
                case "AFCleanRightProcess":
                    return viewModel.AFCleanRightMotions;
                case "TransferRotationLeftProcess":
                    return viewModel.TransferRotationLeftMotions;
                case "TransferRotationRightProcess":
                    return viewModel.TransferRotationRightMotions;

                // Unload Tab
                case "UnloadTransferLeftProcess":
                    return viewModel.UnloadTransferLeftMotions;
                case "UnloadTransferRightProcess":
                    return viewModel.UnloadTransferRightMotions;
                case "UnloadAlignProcess":
                    return viewModel.UnloadAlignMotions;

                default:
                    return viewModel.Motions; 
            }
        }

    }
}
