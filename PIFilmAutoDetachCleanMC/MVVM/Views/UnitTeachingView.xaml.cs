using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PIFilmAutoDetachCleanMC.Defines.Devices;
using static PIFilmAutoDetachCleanMC.MVVM.ViewModels.TeachViewModel;

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

        public UnitTeachingView()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
