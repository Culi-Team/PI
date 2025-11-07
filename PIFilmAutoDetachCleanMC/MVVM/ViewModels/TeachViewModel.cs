using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Motion;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using PIFilmAutoDetachCleanMC.Services.Factories;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class TeachViewModel : ViewModelBase
    {
        #region Properties
        public MachineStatus MachineStatus { get; }

        public ObservableCollection<UnitTeachingViewModel> TeachingUnits { get; }

        public UnitTeachingViewModel SelectedTeachingUnit
        {
            get 
            {
                return selectedTeachingUnit; 
            }
            set 
            {
                selectedTeachingUnit = value;
                OnPropertyChanged(nameof(SelectedTeachingUnit));
            }
        }
        #endregion

        public void SelectedUnitTeachingOnChanged()
        {
            OnPropertyChanged(nameof(SelectedTeachingUnit));
        }

        public ICommand GetTeachingUnitViewModel
        {
            get
            {
                return new RelayCommand<object>((teachingUnit) =>
                {
                    if(teachingUnit is UnitTeachingViewModel unitTeachingViewModel)
                    {
                        if (SelectedTeachingUnit.Name == unitTeachingViewModel.Name) return;

                        SelectedTeachingUnit.IsSelected = false;
                        SelectedTeachingUnit = unitTeachingViewModel;
                        SelectedTeachingUnit.IsSelected = true;
                        SelectedUnitTeachingOnChanged();
                    }
                });
            }
        }

        public TeachViewModel(MachineStatus machineStatus,
            ViewModelNavigationStore navigationStore,
            TeachingViewModelFactory factory)
        {
            MachineStatus = machineStatus;
            _navigationStore = navigationStore;

            TeachingUnits = new ObservableCollection<UnitTeachingViewModel>()
            {
                factory.Create("CST Load"),
                factory.Create("CST Unload"),
                factory.Create("Vinyl Clean"),
                factory.Create("Transfer Fixture"),
                factory.Create("Detach"),
                factory.Create("Glass Transfer"),
                factory.Create("Transfer In Shuttle Left"),
                factory.Create("Transfer In Shuttle Right"),
                factory.Create("WET Clean Left"),
                factory.Create("WET Clean Right"),
                factory.Create("Transfer Rotation Left"),
                factory.Create("Transfer Rotation Right"),
                factory.Create("AF Clean Left"),
                factory.Create("AF Clean Right"),
                factory.Create("Unload Transfer Left"),
                factory.Create("Unload Transfer Right")
            };

            SelectedTeachingUnit = TeachingUnits.First();

            _inoutUpdateTimer = new System.Timers.Timer(100);
            _inoutUpdateTimer.Elapsed += _inoutUpdateTimer_Elapsed;
            _inoutUpdateTimer.Start();
        }

        private void _inoutUpdateTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (_navigationStore.CurrentViewModel.GetType() != typeof(TeachViewModel)) return;

            if (SelectedTeachingUnit == null) return;
            if (SelectedTeachingUnit.Inputs == null) return;
            if (SelectedTeachingUnit.Inputs.Count <= 0) return;

            foreach (var input in SelectedTeachingUnit.Inputs)
            {
                input.RaiseValueUpdated();
            }
        }

        #region Privates
        private readonly ViewModelNavigationStore _navigationStore;
        private System.Timers.Timer _inoutUpdateTimer;
        private UnitTeachingViewModel selectedTeachingUnit;
        #endregion
    }
}
