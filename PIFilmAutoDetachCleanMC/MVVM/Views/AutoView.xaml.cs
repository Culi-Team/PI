using EQX.UI.Interlock;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
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
        private readonly Inputs _inputs;
        private static bool _ruleRegistered;
        public AutoView()
        {
            InitializeComponent();
            _inputs = App.AppHost!.Services.GetRequiredService<Inputs>();

            if (!_ruleRegistered)
            {
                InterlockService.Default.RegisterRule(
                    new LambdaInterlockRule("OriginDoorLock", ctx => ctx.IsSafetyDoorClosed));
                _ruleRegistered = true;
            }

            _inputs.DoorLock7R.ValueChanged += DoorLock7R_ValueChanged;
            InterlockService.Default.UpdateContext(c => c.IsSafetyDoorClosed = _inputs.DoorLock7R.Value);
            Unloaded += AutoView_Unloaded;
        }

        private void DoorLock7R_ValueChanged(object? sender, EventArgs e)
        {
            InterlockService.Default.UpdateContext(c => c.IsSafetyDoorClosed = _inputs.DoorLock7R.Value);
        }

        private void AutoView_Unloaded(object sender, RoutedEventArgs e)
        {
            _inputs.DoorLock7R.ValueChanged -= DoorLock7R_ValueChanged;
        }

    }
}
