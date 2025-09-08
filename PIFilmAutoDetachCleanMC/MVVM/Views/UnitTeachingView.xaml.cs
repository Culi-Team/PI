using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Process;
using EQX.Core.Recipe;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using static PIFilmAutoDetachCleanMC.MVVM.ViewModels.TeachViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System;
using EQX.UI.Controls;
using EQX.Core.Recipe;

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

        public ObservableCollection<PositionTeaching> PositionTeachings
        {
            get { return (ObservableCollection<PositionTeaching>)GetValue(PositionTeachingsProperty); }
            set { SetValue(PositionTeachingsProperty, value); }
        }
        public static readonly DependencyProperty PositionTeachingsProperty =
            DependencyProperty.Register("PositionTeachings", typeof(ObservableCollection<PositionTeaching>), typeof(UnitTeachingView), new PropertyMetadata(new ObservableCollection<PositionTeaching> { }));

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

        private void EditPosition_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var positionTeaching = button?.DataContext as PositionTeaching;
            
            if (positionTeaching == null) return;

            // Tạo SingleRecipeMinMaxAttribute với giới hạn phù hợp
            var minMaxAttribute = new SingleRecipeMinMaxAttribute
            {
                Min = -999.999,
                Max = 999.999
            };
            
            // Sử dụng giá trị hiện tại hoặc motion position
            double currentValue = positionTeaching.Position;
            if (currentValue == 0 && positionTeaching.Motion?.Status?.ActualPosition != null)
            {
                currentValue = positionTeaching.Motion.Status.ActualPosition;
            }
            if (currentValue == 0)
            {
                currentValue = 0; // Giá trị mặc định
            }
            
            // Sử dụng DataEditor giống như SingleRecipe
            var dataEditor = new DataEditor(currentValue, minMaxAttribute);
            dataEditor.Title = $"Edit Position - {positionTeaching.Name}";

            // Show dialog và kiểm tra DialogResult
            if (dataEditor.ShowDialog() == true)
            {
                // Cập nhật giá trị giống như SingleRecipe
                positionTeaching.Position = dataEditor.NewValue;
                
                // Force refresh DataGrid để hiển thị giá trị mới
                PositionTeachingDataGrid.Items.Refresh();
            }
        }

        private void GetMotionPosition_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var positionTeaching = button?.DataContext as PositionTeaching;
            
            if (positionTeaching == null) return;

            // Lấy giá trị từ motion và cập nhật Position
            positionTeaching.UpdatePositionFromMotion();
            
            // Force refresh DataGrid để hiển thị giá trị mới
            PositionTeachingDataGrid.Items.Refresh();
            
        }

        private void CylinderForward_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var cylinder = button?.DataContext as ICylinder;
            
            if (cylinder == null) return;

            try
            {
                cylinder.Forward();
            }
            catch (Exception ex)
            {
            }
        }

        private void CylinderBackward_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var cylinder = button?.DataContext as ICylinder;
            
            if (cylinder == null) return;

            try
            {
                cylinder.Backward();
            }
            catch (Exception ex)
            {
            }
        }
    }
}