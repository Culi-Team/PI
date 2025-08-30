using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Sequence;
using EQX.Motion;
using EQX.UI.Controls;
using log4net;
using PIFilmAutoDetachCleanMC.Process;
using System.Windows;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class AutoViewModel : ViewModelBase
    {
        #region Properties
        public AutoViewModel(MachineStatus machineStatus,
            INavigationService navigationService)
        {
            MachineStatus = machineStatus;
            _navigationService = navigationService;
        }

        public MachineStatus MachineStatus { get; }

        public bool IsInputStop
        {
            get => _isInputStop;
            set
            {
                if (_isInputStop != value)
                {
                    _isInputStop = value;
                    OnPropertyChanged(nameof(IsInputStop));
                }
            }
        }

        public bool IsOutputStop
        {
            get => _isOutputStop;
            set
            {
                if (_isOutputStop != value)
                {
                    _isOutputStop = value;
                    OnPropertyChanged(nameof(IsOutputStop));
                }
            }
        }

        public ICommand IOMonitoringNavigate
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _navigationService.NavigateTo<IOMonitoringViewModel>();
                });
            }
        }

        public ICommand InputStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (IsInputStop == false)
                    {
                        IsInputStop = true;
                        LogManager.GetLogger("ENABLE STOP INTPUT!");
                    }
                    else
                    {
                        IsInputStop = false;
                        LogManager.GetLogger("DISABLE STOP INTPUT!");
                    }
                });
            }
        }

        public ICommand OutputStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (IsOutputStop == false)
                    {
                        IsOutputStop = true;
                        LogManager.GetLogger("ENABLE STOP OUTPUT!");
                    }
                    else
                    {
                        IsOutputStop = false;
                        LogManager.GetLogger("DISABLE STOP OUTPUT!");
                    }
                });
            }
        }
        #endregion

        #region Privates
        private readonly INavigationService _navigationService;
        private bool _isInputStop;
        private bool _isOutputStop;
        #endregion

        public ICommand OriginCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_AreYouSureYouWantToSetMachineOrigin"], (string)Application.Current.Resources["str_Confirm"]) == false)
                    {
                        return;
                    }

                    MachineStatus.OPCommand = EOperationCommand.Origin;
                });
            }
        }

    }
}
