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

        private int _selectedInputDeviceIndex;
        public int SelectedInputDeviceIndex
        {
            get => _selectedInputDeviceIndex;
            set
            {
                if (_selectedInputDeviceIndex != value)
                {
                    _selectedInputDeviceIndex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedInputBoardNumber));
                }
            }
        }

        public int SelectedInputBoardNumber => SelectedInputDeviceIndex + 1;

        private int _selectedOutputDeviceIndex;
        public int SelectedOutputDeviceIndex
        {
            get => _selectedOutputDeviceIndex;
            set
            {
                if (_selectedOutputDeviceIndex != value)
                {
                    _selectedOutputDeviceIndex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedOutputBoardNumber));
                }
            }
        }

        public int SelectedOutputBoardNumber => SelectedOutputDeviceIndex + 1;

        public ICommand InputDeviceIndexDecrease
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (SelectedInputDeviceIndex > 0)
                    {
                        SelectedInputDeviceIndex--;
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
                    if (SelectedInputDeviceIndex < MaxInputDeviceIndex)
                    {
                        SelectedInputDeviceIndex++;
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
                    if (SelectedOutputDeviceIndex < MaxOutputDeviceIndex)
                    {
                        SelectedOutputDeviceIndex++;
                    }
                });
            }
        }

        public Inputs InputList { get; }
        public Outputs OutputList { get; }
        private int MaxInputDeviceIndex => InputList.All
            .GroupBy(i => i.Id / 32)
            .Count() - 1;

        private int MaxOutputDeviceIndex => OutputList.All
            .GroupBy(o => o.Id / 32)
            .Count() - 1;
    }
}
