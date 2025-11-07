using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Communication;
using EQX.Core.Communication.Modbus;
using EQX.Core.Robot;
using EQX.Core.Sequence;
using EQX.Device.Indicator;
using EQX.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.MVVM.Models;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Manual;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Services.Factories;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public partial class ManualViewModel : ViewModelBase
    {
        #region Privates
        private readonly IModbusCommunication _rollerModbusCommunication;
        private readonly IModbusCommunication _torqueModbusCommunication;
        private readonly IRobot _robotLoad;
        private readonly IRobot _robotUnload;
        private readonly Processes _processes;
        private readonly SerialCommunicator _syringPumpSerialCommunicator;
        private readonly IModbusCommunication _indicatorModbusCommunication;
        private readonly ViewModelNavigationStore _navigationStore;
        private readonly ManualViewModelFactory _factory;
        private System.Timers.Timer _inoutUpdateTimer;
        #endregion

        #region Properties
        public Devices Devices { get; }
        public MachineStatus MachineStatus { get; }
        public NEOSHSDIndicator Indicator { get; }

        public bool RobotLoadIsConnected => _robotLoad.IsConnected;
        public bool RobotUnloadIsConnected => _robotUnload.IsConnected;
        public bool MotionsInovanceIsConnected => Devices.Motions.InovanceMaster.IsConnected;
        public bool MotionAjinIsConnected => Devices.Motions.AjinMaster.IsConnected;
        public bool SpeedControllersIsConnected => Devices.RollerList.All.All(sc => sc.IsConnected);
        public bool TorqueControllersIsConnected => Devices.TorqueControllers.All.All(tc => tc.IsConnected);
        public bool SyringePumpIsConnected
        {
            get
            {
                return Devices.SyringePumps.WetCleanLeftSyringePump.IsConnected &&
                       Devices.SyringePumps.WetCleanRightSyringePump.IsConnected &&
                       Devices.SyringePumps.AfCleanLeftSyringePump.IsConnected &&
                       Devices.SyringePumps.AfCleanRightSyringePump.IsConnected;
            }
        }

        public bool VinylCleanEncoderIsConnected => Devices.VinylCleanEncoder.IsConnected;

        public bool IndicatorIsConnected => Indicator.IsConnected;
        public bool WETCleanLeftRegulatorIsConnected => Devices.Regulators.WetCleanLRegulator.IsConnected;
        public bool WETCleanRightRegulatorIsConnected => Devices.Regulators.WetCleanRRegulator.IsConnected;
        public bool AFCleanLeftRegulatorIsConnected => Devices.Regulators.AfCleanLRegulator.IsConnected;
        public bool AFCleanRightRegulatorIsConnected => Devices.Regulators.AfCleanRRegulator.IsConnected;

        public ObservableCollection<ManualVMWithSelection> ManualUnits { get; }

        private string SelectedManualUnit => ManualUnits.First(u => u.IsSelected).UnitName;

        public ManualUnitViewModel CurrentManualUnitViewModel => _factory.Create(SelectedManualUnit);
        private ManualUnitViewModel _currentManualUnitVM;
        #endregion

        #region Commands
        public IRelayCommand<string> ManualUnitSelectCommand
        {
            get
            {
                return new RelayCommand<string>((unitName) =>
                {
                    if (string.IsNullOrEmpty(unitName)) return;
                    if (ManualUnits.Any(u => u.UnitName == unitName) == false) return;

                    foreach (var unit in ManualUnits)
                    {
                        unit.IsSelected = unit.UnitName == unitName;
                    }

                    OnPropertyChanged(nameof(CurrentManualUnitViewModel));
                    _currentManualUnitVM = CurrentManualUnitViewModel;
                });
            }
        }

        public ICommand MotionInovanceConnectCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    IsConnecting = true;
                    OnPropertyChanged(nameof(IsConnecting));

                    try
                    {
                        await Task.Run(() =>
                        {
                            Devices.Motions.InovanceMaster.Connect();
                            Devices.Inputs.InputStatusSet();
                        });

                        OnPropertyChanged(nameof(MotionsInovanceIsConnected));
                    }
                    catch (Exception ex)
                    {
                        MessageBoxEx.ShowDialog($"Inovance Motion Connect Fail\r\n{ex.Message}");
                    }
                    finally
                    {
                        IsConnecting = false;
                        OnPropertyChanged(nameof(IsConnecting));
                        Devices.Outputs.Lamp_Stop();
                    }
                });
            }
        }

        private bool isConnecting = false;

        public bool IsConnecting
        {
            get { return isConnecting; }
            set { isConnecting = value; }
        }

        public ICommand MotionAjinConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Devices.Motions.AjinMaster.Connect();
                    OnPropertyChanged(nameof(MotionAjinIsConnected));
                });
            }
        }

        public ICommand VinylCleanEncoderConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Devices.VinylCleanEncoder.Connect();
                    OnPropertyChanged(nameof(VinylCleanEncoderIsConnected));
                });
            }
        }
        public ICommand RobotLoadConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _robotLoad.Connect();
                    OnPropertyChanged(nameof(RobotLoadIsConnected));
                });
            }
        }

        public ICommand RobotUnloadConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _robotUnload.Connect();
                    OnPropertyChanged(nameof(RobotUnloadIsConnected));
                });
            }
        }

        public ICommand SpeedControllerConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _rollerModbusCommunication.Connect();
                    OnPropertyChanged(nameof(SpeedControllersIsConnected));
                });
            }
        }

        public ICommand TorqueControllerConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _torqueModbusCommunication.Connect();
                    OnPropertyChanged(nameof(TorqueControllersIsConnected));
                });
            }
        }

        public ICommand SemiAutoCommand
        {
            get
            {
                return new RelayCommand<object>((o) =>
                {
                    if (Devices.Motions.All.Count(motion => motion.Status.IsMotionOn != true) > 0)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_TurnOnAllMotionsFirst"], (string)Application.Current.Resources["str_Confirm"]);
                        return;
                    }

                    if (MachineStatus.OriginDone == false)
                    {
                        MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_MachineNeedToBeOriginBeforeRun"]);
                        return;
                    }

                    if (o is ESemiSequence sequence == false) return;

                    MachineStatus.OPCommand = EOperationCommand.SemiAuto;
                    MachineStatus.SemiAutoSequence = sequence;
                });
            }
        }

        public ICommand SyringePumpConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _syringPumpSerialCommunicator.Connect();
                    OnPropertyChanged(nameof(SyringePumpIsConnected));
                });
            }
        }

        public ICommand IndicatorConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _indicatorModbusCommunication.Connect();
                    OnPropertyChanged(nameof(IndicatorIsConnected));
                });
            }
        }

        public ICommand RegulatorConnectCommand
        {
            get
            {
                return new RelayCommand<object>((parameter) =>
                {
                    if (parameter == null)
                    {
                        MessageBoxEx.Show("Regulator Connect Fail");
                        return;
                    }
                    if (parameter.ToString() == "WETCleanLeft")
                    {
                        Devices.Regulators.WetCleanLRegulator.Connect();
                        OnPropertyChanged(nameof(WETCleanLeftRegulatorIsConnected));
                    }
                    if (parameter.ToString() == "WETCleanRight")
                    {
                        Devices.Regulators.WetCleanRRegulator.Connect();
                        OnPropertyChanged(nameof(WETCleanRightRegulatorIsConnected));
                    }
                    if (parameter.ToString() == "AFCleanLeft")
                    {
                        Devices.Regulators.AfCleanLRegulator.Connect();
                        OnPropertyChanged(nameof(AFCleanLeftRegulatorIsConnected));
                    }
                    if (parameter.ToString() == "AFCleanRight")
                    {
                        Devices.Regulators.AfCleanRRegulator.Connect();
                        OnPropertyChanged(nameof(AFCleanRightRegulatorIsConnected));
                    }
                });
            }
        }
        #endregion

        #region Constructor
        public ManualViewModel(Devices devices,
            MachineStatus machineStatus,
            [FromKeyedServices("RollerModbusCommunication")] IModbusCommunication rollerModbusCommunication,
            [FromKeyedServices("TorqueControllerModbusCommunication")] IModbusCommunication torqueModbusCommunication,
            [FromKeyedServices("RobotLoad")] IRobot robotLoad,
            [FromKeyedServices("RobotUnload")] IRobot robotUnload,
            Processes processes,
            [FromKeyedServices("SyringePumpSerialCommunicator")] SerialCommunicator SyringPumpSerialCommunicator,
            [FromKeyedServices("IndicatorModbusCommunication")] IModbusCommunication indicatorModbusCommunication,
            NEOSHSDIndicator indicator,
            ViewModelNavigationStore navigationStore,
            ManualViewModelFactory factory)
        {
            Devices = devices;
            MachineStatus = machineStatus;

            _rollerModbusCommunication = rollerModbusCommunication;
            _torqueModbusCommunication = torqueModbusCommunication;
            _robotLoad = robotLoad;
            _robotUnload = robotUnload;
            _processes = processes;
            _syringPumpSerialCommunicator = SyringPumpSerialCommunicator;
            _indicatorModbusCommunication = indicatorModbusCommunication;
            Indicator = indicator;
            _navigationStore = navigationStore;
            _factory = factory;

            ManualUnits = new ObservableCollection<ManualVMWithSelection>()
            {
                new ManualVMWithSelection("In Conveyor", true),
                new ManualVMWithSelection("In Work Conveyor", false),
                new ManualVMWithSelection("Buffer Conveyor", false),
                new ManualVMWithSelection("Out Work Conveyor", false),
                new ManualVMWithSelection("Out Conveyor", false),
                new ManualVMWithSelection("Robot Load", false),
                new ManualVMWithSelection("Vinyl Clean", false),
                new ManualVMWithSelection("Transfer Fixture", false),
                new ManualVMWithSelection("Fixture Align", false),
                new ManualVMWithSelection("Detach", false),
                new ManualVMWithSelection("Remove Film", false),
                new ManualVMWithSelection("Glass Transfer", false),
                new ManualVMWithSelection("Transfer In Shuttle Left", false),
                new ManualVMWithSelection("Transfer In Shuttle Right", false),
                new ManualVMWithSelection("WET Clean Left", false),
                new ManualVMWithSelection("WET Clean Right", false),
                new ManualVMWithSelection("Transfer Rotation Left", false),
                new ManualVMWithSelection("Transfer Rotation Right", false),
                new ManualVMWithSelection("AFClean Left", false),
                new ManualVMWithSelection("AFClean Right", false),
                new ManualVMWithSelection("Unload Transfer Left", false),
                new ManualVMWithSelection("Unload Transfer Right", false),
                new ManualVMWithSelection("Unload Align", false),
                new ManualVMWithSelection("Unload Robot", false),
            };

            _inoutUpdateTimer = new System.Timers.Timer(100);
            _inoutUpdateTimer.Elapsed += _inoutUpdateTimer_Elapsed;
            _inoutUpdateTimer.Start();

            _currentManualUnitVM = CurrentManualUnitViewModel;
        }

        private void _inoutUpdateTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (_navigationStore.CurrentViewModel.GetType() != typeof(ManualViewModel)) return;

            if (_currentManualUnitVM == null) return;
            if (_currentManualUnitVM.Inputs == null) return;
            if (_currentManualUnitVM.Inputs.Count <= 0) return;

            foreach (var input in _currentManualUnitVM.Inputs)
            {
                input.RaiseValueUpdated();
            }
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
