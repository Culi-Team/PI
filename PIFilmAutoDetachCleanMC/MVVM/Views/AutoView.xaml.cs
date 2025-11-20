using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Controls;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels;
using PIFilmAutoDetachCleanMC.Process;
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

namespace PIFilmAutoDetachCleanMC.MVVM.Views
{
    /// <summary>
    /// Interaction logic for AutoView.xaml
    /// </summary>
    public partial class AutoView : UserControl
    {
        public AutoView()
        {
            InitializeComponent();
        }
  
        private void GlassDetachImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var image = sender as IInputElement;
            if (image == null)
            {
                return;
            }

            Point clickPosInElement = e.GetPosition((IInputElement)sender);
            Point screenPos = (sender as Visual)!.PointToScreen(clickPosInElement);

            var dialog = new DetachStatusSelectView
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = screenPos.X,
                Top = screenPos.Y
            };

            dialog.ShowInTaskbar = false;
            dialog.Topmost = true;

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                if(this.DataContext is AutoViewModel autoViewModel)
                {
                    autoViewModel.MachineStatus.IsFixtureDetached = dialog.IsDetached;
                }    
            }
        }

        private void UnloadAlignImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UnloadGlassSelectView unloadGlassSelectView = new UnloadGlassSelectView();
            unloadGlassSelectView.ShowDialog();
        }
    }
}
