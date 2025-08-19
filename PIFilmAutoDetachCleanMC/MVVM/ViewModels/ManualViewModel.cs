using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using System.Diagnostics;
using PIFilmAutoDetachCleanMC.Defines;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class ManualViewModel : ViewModelBase
    {
        public ManualViewModel(Devices devices)
        {
            Inputs = devices.Inputs;
            Outputs = devices.Outputs;
            Motions = devices.Motions;
        }

        public Inputs Inputs { get; }
        public Outputs Outputs { get; }
        public Motions Motions { get; }

        public ICommand ConnectMotionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Motions.All.ForEach(m => m.Connect());
                });
            }
        }
    }
}
