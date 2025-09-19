using CommunityToolkit.Mvvm.Input;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Process;
using EQX.Core.Recipe;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        public ObservableCollection<IDInput> Inputs
        {
            get { return (ObservableCollection<IDInput>)GetValue(InputsProperty); }
            set { SetValue(InputsProperty, value); }
        }
        public static readonly DependencyProperty InputsProperty =
            DependencyProperty.Register("Inputs", typeof(ObservableCollection<IDInput>), typeof(UnitTeachingView), new PropertyMetadata(new ObservableCollection<IDInput> { }));

        public ObservableCollection<IDOutput> Outputs
        {
            get { return (ObservableCollection<IDOutput>)GetValue(OutputsProperty); }
            set { SetValue(OutputsProperty, value); }
        }
        public static readonly DependencyProperty OutputsProperty =
            DependencyProperty.Register("Outputs", typeof(ObservableCollection<IDOutput>), typeof(UnitTeachingView), new PropertyMetadata(new ObservableCollection<IDOutput> { }));

        public IProcess<ESequence> SelectedProcess
        {
            get { return (IProcess<ESequence>)GetValue(SelectedProcessProperty); }
            set { SetValue(SelectedProcessProperty, value); }
        }
        public static readonly DependencyProperty SelectedProcessProperty =
            DependencyProperty.Register("SelectedProcess", typeof(IProcess<ESequence>), typeof(UnitTeachingView), new PropertyMetadata(null));

        public ImageSource TeachingImage
        {
            get { return (ImageSource)GetValue(TeachingImageProperty); }
            set { SetValue(TeachingImageProperty, value); }
        }
        public static readonly DependencyProperty TeachingImageProperty =
            DependencyProperty.Register(nameof(TeachingImage), typeof(ImageSource), typeof(UnitTeachingView), new PropertyMetadata(null));

        public TeachViewModel TeachViewModel
        {
            get { return (TeachViewModel)GetValue(TeachViewModelProperty); }
            set { SetValue(TeachViewModelProperty, value); }
        }
        public static readonly DependencyProperty TeachViewModelProperty =
            DependencyProperty.Register("TeachViewModel", typeof(TeachViewModel), typeof(UnitTeachingView), new PropertyMetadata(null));
        public UnitTeachingView()
        {
            InitializeComponent();
        }

        private void EditPosition_Click(object sender, RoutedEventArgs e)
        {
        }
        private void GetMotionPosition_Click(object sender, RoutedEventArgs e)
        {
        }
    }

    public class PositionTeachingCommandParameter
    {
        public TeachViewModel TeachViewModel { get; set; }
    }
}