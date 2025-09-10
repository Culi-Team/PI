using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Process;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Threading.Tasks;
using System.Threading;
using PIFilmAutoDetachCleanMC.Defines;
using EQX.UI.Controls;
using EQX.Core.Device.Regulator;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Windows.Threading;

namespace PIFilmAutoDetachCleanMC.MVVM.Views
{
    /// <summary>
    /// Interaction logic for ManualUnitView.xaml
    /// </summary>
    public partial class ManualUnitView : UserControl
    {
        public ObservableCollection<IMotion> Motions
        {
            get { return (ObservableCollection<IMotion>)GetValue(MotionsProperty); }
            set { SetValue(MotionsProperty, value); }
        }
        public static readonly DependencyProperty MotionsProperty =
            DependencyProperty.Register("Motions", typeof(ObservableCollection<IMotion>), typeof(ManualUnitView), new PropertyMetadata(new ObservableCollection<IMotion> { }));

        public ObservableCollection<ICylinder> Cylinders
        {
            get { return (ObservableCollection<ICylinder>)GetValue(CylindersProperty); }
            set { SetValue(CylindersProperty, value); }
        }
        public static readonly DependencyProperty CylindersProperty =
            DependencyProperty.Register("Cylinders", typeof(ObservableCollection<ICylinder>), typeof(ManualUnitView), new PropertyMetadata(new ObservableCollection<ICylinder> { }));

        public ObservableCollection<IDInput> Inputs
        {
            get { return (ObservableCollection<IDInput>)GetValue(InputsProperty); }
            set { SetValue(InputsProperty, value); }
        }
        public static readonly DependencyProperty InputsProperty =
            DependencyProperty.Register("Inputs", typeof(ObservableCollection<IDInput>), typeof(ManualUnitView), new PropertyMetadata(new ObservableCollection<IDInput> { }));

        public ObservableCollection<IDOutput> Outputs
        {
            get { return (ObservableCollection<IDOutput>)GetValue(OutputsProperty); }
            set { SetValue(OutputsProperty, value); }
        }
        public static readonly DependencyProperty OutputsProperty =
            DependencyProperty.Register("Outputs", typeof(ObservableCollection<IDOutput>), typeof(ManualUnitView), new PropertyMetadata(new ObservableCollection<IDOutput> { }));

        public ObservableCollection<KeyValuePair<IRegulator, CleanRecipe>> Regulators
        {
            get { return (ObservableCollection<KeyValuePair<IRegulator, CleanRecipe>>)GetValue(RegulatorsProperty); }
            set { SetValue(RegulatorsProperty, value); }
        }

        public static readonly DependencyProperty RegulatorsProperty =
            DependencyProperty.Register("Regulators", typeof(ObservableCollection<KeyValuePair<IRegulator, CleanRecipe>>), typeof(ManualUnitView), new PropertyMetadata(new ObservableCollection<KeyValuePair<IRegulator, CleanRecipe>> { }));

        public IProcess<ESequence> SelectedProcess
        {
            get { return (IProcess<ESequence>)GetValue(SelectedProcessProperty); }
            set { SetValue(SelectedProcessProperty, value); }
        }
        public static readonly DependencyProperty SelectedProcessProperty =
            DependencyProperty.Register("SelectedProcess", typeof(IProcess<ESequence>), typeof(ManualUnitView), new PropertyMetadata(null));

        public ManualUnitView()
        {
            InitializeComponent();
            var timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            timer.Tick += (_, _) => RegulatorItemsControl?.Items.Refresh();
            timer.Start();
        }

        private void SetPressureFromRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is KeyValuePair<IRegulator, CleanRecipe> pair)
            {
                pair.Key.SetPressure(pair.Value.CylinderPushPressure);
                RegulatorItemsControl.Items.Refresh();
            }
        }

        private void CylinderForward_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var cylinder = button?.DataContext as ICylinder;
            if (cylinder == null) return;
            button.IsEnabled = false;
            try
            {
               cylinder.Forward();
            }
            catch (Exception ex)
            {
                MessageBoxEx.ShowDialog($"Cylinder {cylinder.Name} Forward Error: {ex.Message}");
            }
            finally
            {
                button.IsEnabled = true;
            }
        }
        private void CylinderBackward_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var cylinder = button?.DataContext as ICylinder;
            if (cylinder == null) return;
            button.IsEnabled = false;
            try
            {

               cylinder.Backward();
            }
            catch (Exception ex)
            {
                MessageBoxEx.ShowDialog($"Cylinder {cylinder.Name} Backward Error: {ex.Message}");
            }
            finally
            {
                button.IsEnabled = true;
            }
        }
    }
}
