using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using PIFilmAutoDetachCleanMC.Defines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class IOMonitoringViewModel : ViewModelBase
    {
        public IOMonitoringViewModel(Inputs inputList,Outputs outputList) 
        {
            InputList = inputList;
            OutputList = outputList;
        }

        public uint SelectedInputDeviceIndex { get; set; }
        public uint SelectedOutputDeviceIndex { get; set; }

        public ICommand InputDeviceIndexDecrease
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedInputDeviceIndex > 0)
                    {
                        SelectedInputDeviceIndex--;
                        OnPropertyChanged(nameof(SelectedInputDeviceIndex));
                    }
                });
            }
        }
        public ICommand InputDeviceIndexIncrease
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedInputDeviceIndex < InputList.All.Count / 32 - 1)
                    {
                        SelectedInputDeviceIndex++;
                        OnPropertyChanged(nameof(SelectedInputDeviceIndex));
                    }
                });
            }
        }

        public ICommand OutputDeviceIndexDecrease
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedOutputDeviceIndex > 0)
                    {
                        SelectedOutputDeviceIndex--;
                        OnPropertyChanged(nameof(SelectedOutputDeviceIndex));
                    }
                });
            }
        }
        public ICommand OutputDeviceIndexIncrease
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedOutputDeviceIndex < OutputList.All.Count / 32 - 1)
                    {
                        SelectedOutputDeviceIndex++;
                        OnPropertyChanged(nameof(SelectedOutputDeviceIndex));
                    }
                });
            }
        }

        public Inputs InputList { get; }
        public Outputs OutputList { get; }
    }
}
