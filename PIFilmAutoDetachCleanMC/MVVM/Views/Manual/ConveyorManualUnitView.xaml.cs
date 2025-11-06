using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Manual;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching;
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

namespace PIFilmAutoDetachCleanMC.MVVM.Views.Manual
{
    /// <summary>
    /// Interaction logic for ConveyorManualUnitView.xaml
    /// </summary>
    public partial class ConveyorManualUnitView : UserControl
    {
        public ConveyorManualUnitView()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label label)
            {
                DataEditor dataEditor = new DataEditor(Convert.ToDouble(label.Content), null);
                if (dataEditor.ShowDialog() == true)
                {
                    label.Content = dataEditor.NewValue;
                }
            }
        }
    }
}
