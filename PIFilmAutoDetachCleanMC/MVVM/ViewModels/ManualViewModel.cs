using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Communication;
using EQX.Core.Communication.Modbus;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Robot;
using EQX.Core.Sequence;
using EQX.Device.Indicator;
using EQX.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using OpenCvSharp;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Manual;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class ManualVMWithSelection : ObservableObject
    {
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                _UnitName = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged();
            }
        }

        public ManualVMWithSelection(string name, bool selected)
        {
            UnitName = name;
            IsSelected = selected;
        }

        #region Privates
        private string _UnitName = "";
        private bool _IsSelected;
        #endregion
    }

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

        public bool IndicatorIsConnected => Indicator.IsConnected;
        public bool WETCleanLeftRegulatorIsConnected => Devices.Regulators.WetCleanLRegulator.IsConnected;
        public bool WETCleanRightRegulatorIsConnected => Devices.Regulators.WetCleanRRegulator.IsConnected;
        public bool AFCleanLeftRegulatorIsConnected => Devices.Regulators.AfCleanLRegulator.IsConnected;
        public bool AFCleanRightRegulatorIsConnected => Devices.Regulators.AfCleanRRegulator.IsConnected;

        public ObservableCollection<ManualVMWithSelection> ManualUnits { get; }

        private string SelectedManualUnit => ManualUnits.First(u => u.IsSelected).UnitName;

        public ManualUnitViewModel CurrentManualUnitViewModel => GetManualUnitViewModel(SelectedManualUnit);
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
            NEOSHSDIndicator indicator)
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
        }
        #endregion

        private ManualUnitViewModel GetManualUnitViewModel(string name)
        {
            switch (name)
            {
                case "In Conveyor":
                    return new ConveyorManualUnitViewModel("In Conveyor")
                    {
                        Cylinders = Devices.GetInConveyorCylinders(),
                        Inputs = Devices.GetInConveyorInputs(),
                        Outputs = Devices.GetInConveyorOutputs(),
                        Rollers = Devices.GetInConveyorRollers(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("InCassetteCVImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.InConveyorLoad,
                            ESemiSequence.InWorkCSTLoad
                        }
                    };
                case "In Work Conveyor":
                    return new ConveyorManualUnitViewModel("In Work Conveyor")
                    {
                        Cylinders = Devices.GetInWorkConveyorCylinders(),
                        Inputs = Devices.GetInWorkConveyorInputs(),
                        Outputs = Devices.GetInWorkConveyorOutputs(),
                        Rollers = Devices.GetInWorkConveyorRollers(),
                        Motions = Devices.GetCSTLoadMotions(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("LoadWorkCassetteStageImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.InWorkCSTLoad,
                            ESemiSequence.InWorkCSTTilt,
                            ESemiSequence.InWorkCSTUnLoad
                        }
                    };
                case "Buffer Conveyor":
                    return new ConveyorManualUnitViewModel("Buffer Conveyor")
                    {
                        Cylinders = Devices.GetBufferConveyorCylinders(),
                        Inputs = Devices.GetBufferConveyorInputs(),
                        Outputs = Devices.GetBufferConveyorOutputs(),
                        Rollers = Devices.GetBufferConveyorRollers(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("BufferCVImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.InWorkCSTUnLoad,
                            ESemiSequence.OutWorkCSTLoad
                        }
                    };
                case "Out Work Conveyor":
                    return new ConveyorManualUnitViewModel("Out Work Conveyor")
                    {
                        Cylinders = Devices.GetOutWorkConveyorCylinders(),
                        Inputs = Devices.GetOutWorkConveyorInputs(),
                        Outputs = Devices.GetOutWorkConveyorOutputs(),
                        Rollers = Devices.GetOutWorkConveyorRollers(),
                        Motions = Devices.GetCSTUnloadMotions(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("LoadWorkCassetteStageImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.OutWorkCSTLoad,
                            ESemiSequence.OutWorkCSTTilt,
                            ESemiSequence.OutWorkCSTUnLoad
                        }
                    };
                case "Out Conveyor":
                    return new ConveyorManualUnitViewModel("Out Conveyor")
                    {
                        Cylinders = Devices.GetOutConveyorCylinders(),
                        Inputs = Devices.GetOutConveyorInputs(),
                        Outputs = Devices.GetOutConveyorOutputs(),
                        Rollers = Devices.GetOutConveyorRollers(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("OutCassetteCVImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.OutWorkCSTUnLoad,
                            ESemiSequence.OutConveyorUnload
                        }
                    };
                case "Robot Load":
                    return new ManualUnitViewModel("Robot Load")
                    {
                        Cylinders = Devices.GetRobotLoadCylinders(),
                        Inputs = Devices.GetRobotLoadInputs(),
                        Outputs = Devices.GetRobotLoadOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("LoadRobotImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.RobotPickFixtureFromCST,
                            ESemiSequence.RobotPlaceFixtureToVinylClean,
                            ESemiSequence.RobotPickFixtureFromVinylClean,
                            ESemiSequence.RobotPlaceFixtureToAlign,
                            ESemiSequence.RobotPickFixtureFromRemoveZone,
                            ESemiSequence.RobotPlaceFixtureToOutWorkCST
                        }
                    };
                case "Vinyl Clean":
                    return new ManualUnitViewModel("Vinyl Clean")
                    {
                        Cylinders = Devices.GetVinylCleanCylinders(),
                        Motions = new ObservableCollection<IMotion> { Devices.VinylCleanEncoder },
                        Inputs = Devices.GetVinylCleanInputs(),
                        Outputs = Devices.GetVinylCleanOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("VinylCleanImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.RobotPlaceFixtureToVinylClean,
                            ESemiSequence.VinylClean,
                            ESemiSequence.RobotPickFixtureFromVinylClean
                        }
                    };
                case "Transfer Fixture":
                    return new ManualUnitViewModel("Transfer Fixture")
                    {
                        Cylinders = Devices.GetTransferFixtureCylinders(),
                        Outputs = Devices.GetTransferFixtureOutputs(),
                        Motions = Devices.GetTransferFixtureMotions(),
                        Inputs = Devices.GetTransferFixtureInputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferFixtureImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.TransferFixture,
                        }

                    };
                case "Fixture Align":
                    return new ManualUnitViewModel("Fixture Align")
                    {

                        Cylinders = Devices.GetFixtureAlignCylinders(),
                        Inputs = Devices.GetFixtureAlignInputs(),
                        Outputs = Devices.GetFixtureAlignOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AlignFixtureImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.RobotPlaceFixtureToAlign,
                            ESemiSequence.FixtureAlign
                        }
                    };
                case "Detach":
                    return new ManualUnitViewModel("Detach")
                    {
                        Cylinders = Devices.GetDetachCylinders(),
                        Motions = Devices.GetDetachMotions(),
                        Inputs = Devices.GetDetachInputs(),
                        Outputs = Devices.GetDetachOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("DetachImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.Detach,
                            ESemiSequence.DetachUnload
                        }
                    };
                case "Remove Film":
                    return new ManualUnitViewModel("Remove Film")
                    {
                        Cylinders = Devices.GetRemoveFilmCylinders(),
                        Inputs = Devices.GetRemoveFilmInputs(),
                        Outputs = Devices.GetRemoveFilmOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("RemoveZoneImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.RemoveFilm,
                            ESemiSequence.RobotPickFixtureFromRemoveZone,
                        }
                    };
                case "Glass Transfer":
                    return new ManualUnitViewModel("Glass Transfer")
                    {
                        Cylinders = Devices.GetGlassTransferCylinders(),
                        Motions = Devices.GetGlassTransferMotions(),
                        Inputs = Devices.GetGlassTransferInputs(),
                        Outputs = Devices.GetGlassTransferOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("GlassTransferImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.GlassTransferPick,
                            ESemiSequence.GlassTransferLeft,
                            ESemiSequence.GlassTransferRight,
                        }
                    };
                case "Transfer In Shuttle Left":
                    return new ManualUnitViewModel("Transfer In Shuttle Left")
                    {
                        Cylinders = Devices.GetTransferInShuttleLeftCylinders(),
                        Motions = Devices.GetTransferInShuttleLeftMotions(),
                        Inputs = Devices.GetTransferInShuttleLeftInputs(),
                        Outputs = Devices.GetTransferInShuttleLeftOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.GlassTransferLeft,
                            ESemiSequence.AlignGlassLeft,
                            ESemiSequence.WETCleanLeftLoad,
                        }
                    };
                case "Transfer In Shuttle Right":
                    return new ManualUnitViewModel("Transfer In Shuttle Right")
                    {
                        Cylinders = Devices.GetTransferInShuttleRightCylinders(),
                        Motions = Devices.GetTransferInShuttleRightMotions(),
                        Inputs = Devices.GetTransferInShuttleRightInputs(),
                        Outputs = Devices.GetTransferInShuttleRightOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.GlassTransferRight,
                            ESemiSequence.AlignGlassRight,
                            ESemiSequence.WETCleanRightLoad,
                        }
                    };
                case "WET Clean Left":
                    return new ManualUnitViewModel("WET Clean Left")
                    {
                        Cylinders = Devices.GetWETCleanLeftCylinders(),
                        Motions = Devices.GetWETCleanLeftMotions(),
                        Inputs = Devices.GetWETCleanLeftInputs(),
                        Outputs = Devices.GetWETCleanLeftOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.WETCleanLeftLoad,
                            ESemiSequence.WETCleanLeft,
                            ESemiSequence.InShuttleCleanLeft,
                        }
                    };
                case "WET Clean Right":
                    return new ManualUnitViewModel("WET Clean Right")
                    {
                        Cylinders = Devices.GetWETCleanRightCylinders(),
                        Motions = Devices.GetWETCleanRightMotions(),
                        Inputs = Devices.GetWETCleanRightInputs(),
                        Outputs = Devices.GetWETCleanRightOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.WETCleanRightLoad,
                            ESemiSequence.WETCleanRight,
                            ESemiSequence.InShuttleCleanRight,
                        }
                    };
                case "Transfer Rotation Left":
                    return new ManualUnitViewModel("Transfer Rotation Left")
                    {
                        Cylinders = Devices.GetTransferRotationLeftCylinders(),
                        Motions = Devices.GetTransferRotationLeftMotions(),
                        Inputs = Devices.GetTransferRotationLeftInputs(),
                        Outputs = Devices.GetTransferRotationLeftOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.WETCleanLeftUnload,
                            ESemiSequence.TransferRotationLeft,
                            ESemiSequence.AFCleanLeftLoad,
                        }
                    };
                case "Transfer Rotation Right":
                    return new ManualUnitViewModel("Transfer Rotation Right")
                    {
                        Cylinders = Devices.GetTransferRotationRightCylinders(),
                        Motions = Devices.GetTransferRotationRightMotions(),
                        Inputs = Devices.GetTransferRotationRightInputs(),
                        Outputs = Devices.GetTransferRotationRightOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.WETCleanRightUnload,
                            ESemiSequence.TransferRotationRight,
                            ESemiSequence.AFCleanRightLoad,
                        }
                    };
                case "AFClean Left":
                    return new ManualUnitViewModel("AF Clean Left")
                    {
                        Cylinders = Devices.GetAFCleanLeftCylinders(),
                        Motions = Devices.GetAFCleanLeftMotions(),
                        Inputs = Devices.GetAFCleanLeftInputs(),
                        Outputs = Devices.GetAFCleanLeftOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.AFCleanLeftLoad,
                            ESemiSequence.AFCleanLeft,
                            ESemiSequence.OutShuttleCleanLeft,
                        }
                    };
                case "AFClean Right":
                    return new ManualUnitViewModel("AF Clean Right")
                    {
                        Cylinders = Devices.GetAFCleanRightCylinders(),
                        Motions = Devices.GetAFCleanRightMotions(),
                        Inputs = Devices.GetAFCleanRightInputs(),
                        Outputs = Devices.GetAFCleanRightOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.AFCleanRightLoad,
                            ESemiSequence.AFCleanRight,
                            ESemiSequence.OutShuttleCleanRight,
                        }
                    };
                case "Unload Transfer Left":
                    return new ManualUnitViewModel("Unload Transfer Left")
                    {
                        Cylinders = Devices.GetUnloadTransferLeftCylinders(),
                        Motions = Devices.GetUnloadTransferLeftMotions(),
                        Inputs = Devices.GetUnloadTransferLeftInputs(),
                        Outputs = Devices.GetUnloadTransferLeftOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadTransferImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.AFCleanLeftUnload,
                            ESemiSequence.UnloadTransferLeftPlace,
                        }
                    };
                case "Unload Transfer Right":
                    return new ManualUnitViewModel("Unload Transfer Right")
                    {
                        Cylinders = Devices.GetUnloadTransferRightCylinders(),
                        Motions = Devices.GetUnloadTransferRightMotions(),
                        Inputs = Devices.GetUnloadTransferRightInputs(),
                        Outputs = Devices.GetUnloadTransferRightOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadTransferImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.AFCleanRightUnload,
                            ESemiSequence.UnloadTransferRightPlace,
                        }
                    };
                case "Unload Align":
                    return new ManualUnitViewModel("Unload Align")
                    {
                        Cylinders = Devices.GetUnloadAlignCylinders(),
                        Inputs = Devices.GetUnloadAlignInputs(),
                        Outputs = Devices.GetUnloadAlignOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadStageImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.UnloadAlignGlass,
                        }
                    };
                case "Unload Robot":
                    return new ManualUnitViewModel("Unload Robot")
                    {
                        Cylinders = Devices.GetUnloadRobotCylinders(),
                        Inputs = Devices.GetUnloadRobotInputs(),
                        Outputs = Devices.GetUnloadRobotOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadRobotImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.UnloadRobotPick,
                            ESemiSequence.UnloadRobotPlasma,
                            ESemiSequence.UnloadRobotPlace,
                        }
                    };
                default:
                    return null;
            }
        }

        #region Private Methods
        #endregion
    }
}
