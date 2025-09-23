using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Communication.Modbus;
using EQX.Core.Display;
using EQX.Core.InOut;
using EQX.Core.Robot;
using EQX.UI.Display;
using Microsoft.Extensions.DependencyInjection;
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
    public class ManualViewModel : ViewModelBase
    {
        #region Privates
        private readonly IModbusCommunication _rollerModbusCommunication;
        private readonly IModbusCommunication _torqueModbusCommunication;
        private readonly IRobot _robotLoad;
        private readonly IRobot _robotUnload;
        private ManualUnitViewModel selectedManualUnit;
        private readonly DisplayManager _displayManager = new();
        private readonly ViewOnlyOverlay _viewOnlyOverlay = new();
        private InteractiveScreen _activeScreen;
        private bool _isViewOnly;
        #endregion

        #region Properties
        public Devices Devices { get; }
        public MachineStatus MachineStatus { get; }
        public bool RobotLoadIsConnected => _robotLoad .IsConnected;
        public bool RobotUnloadIsConnected => _robotUnload.IsConnected;
        public bool MotionsInovanceIsConnected => Devices.MotionsInovance.MotionControllerInovance.IsConnected;
        public bool MotionAjinIsConnected => Devices.MotionsAjin.All.All(m => m.IsConnected);
        public bool SpeedControllersIsConnected => Devices.SpeedControllerList.All.All(sc => sc.IsConnected);
        public bool TorqueControllersIsConnected => Devices.TorqueControllers.All.All(tc => tc.IsConnected);
        public InteractiveScreen ActiveScreen
        {
            get => _activeScreen;
            set
            {
                if (_activeScreen != value)
                {
                    _activeScreen = value;
                    OnPropertyChanged(nameof(ActiveScreen));
                    MoveMainWindowTo(_activeScreen);
                    if (_isViewOnly)
                    {
                        _viewOnlyOverlay.ShowOn(_activeScreen == InteractiveScreen.Primary ? 1 : 0);
                    }
                }
            }
        }
        public bool IsViewOnly
        {
            get => _isViewOnly;
            set
            {
                if (_isViewOnly != value)
                {
                    _isViewOnly = value;
                    OnPropertyChanged(nameof(IsViewOnly));
                    if (_isViewOnly)
                    {
                        _viewOnlyOverlay.ShowOn(_activeScreen == InteractiveScreen.Primary ? 1 : 0);
                    }
                    else
                    {
                        _viewOnlyOverlay.Hide();
                    }
                    SetPrimaryInteractiveCommand.NotifyCanExecuteChanged();
                    SetSecondaryInteractiveCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<ManualUnitViewModel> ManualUnits { get; }

        public ManualUnitViewModel SelectedManualUnit
        {
            get 
            {
                return selectedManualUnit; 
            }
            set 
            {
                selectedManualUnit = value; 
                OnPropertyChanged(nameof(SelectedManualUnit));
            }
        }
        #endregion

        #region Commands
        public ICommand MotionInovanceConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Devices.MotionsInovance.MotionControllerInovance.Connect();
                    OnPropertyChanged(nameof(MotionsInovanceIsConnected));
                });
            }
        }

        public ICommand MotionAjinConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Devices.MotionsAjin.All.ForEach(m => m.Connect());
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
                });
            }
        }

        public ICommand RobotUnloadConnectCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
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

        public IRelayCommand SetPrimaryInteractiveCommand
        {
            get
            {
                return new RelayCommand(
                () => ActiveScreen = InteractiveScreen.Primary,
                () => !IsViewOnly);
            }
        }
        public IRelayCommand SetSecondaryInteractiveCommand
        {
            get
            {
                return new RelayCommand(
                () => ActiveScreen = InteractiveScreen.Secondary,
                () => !IsViewOnly);
            }
        }
        #endregion

        //public bool CylinderInterLock(ICylinder cylinder, bool isForward, out string CylinderInterlockMsg)
        //{
        //    CylinderInterlockMsg = string.Empty;

        //    // Interlock for TrRotateLeftRotate
        //    if (cylinder.Name.Contains("TrRotateLeftRotate") || cylinder.Name.Contains("TrRotateLeftFwBw"))
        //    {
        //        CylinderInterlockMsg = "Need Transfer Rotation ZAxis at Ready Position before Moving";
        //        return Devices?.MotionsInovance?.TransferRotationLZAxis?.IsOnPosition(RecipeSelector?.CurrentRecipe?.TransferRotationLeftRecipe?.ZAxisReadyPosition ?? 0) == true;
        //    }
        //    // Interlock for TrRotateRightRotate
        //    if (cylinder.Name.Contains("TrRotateRightRotate") || cylinder.Name.Contains("TrRotateRightFwBw"))
        //    {
        //        CylinderInterlockMsg = "Need Transfer Rotation ZAxis at Ready Position before Moving";
        //        return Devices?.MotionsInovance?.TransferRotationRZAxis?.IsOnPosition(RecipeSelector?.CurrentRecipe?.TransferRotationRightRecipe?.ZAxisReadyPosition ?? 0) == true;
        //    }

        //    return true;
        //}

        #region Constructor
        public ManualViewModel(Devices devices,
            MachineStatus machineStatus,
            [FromKeyedServices("WETCleanLeftRecipe")] CleanRecipe wetCleanLeftRecipe,
            [FromKeyedServices("WETCleanRightRecipe")] CleanRecipe wetCleanRightRecipe,
            [FromKeyedServices("AFCleanLeftRecipe")] CleanRecipe afCleanLeftRecipe,
            [FromKeyedServices("AFCleanRightRecipe")] CleanRecipe afCleanRightRecipe,
            [FromKeyedServices("RollerModbusCommunication")] IModbusCommunication rollerModbusCommunication,
            [FromKeyedServices("TorqueControllerModbusCommunication")] IModbusCommunication torqueModbusCommunication,
            [FromKeyedServices("RobotLoad")] IRobot robotLoad,
            [FromKeyedServices("RobotUnload")] IRobot robotUnload)
        {
            Devices = devices;
            MachineStatus = machineStatus;
            
            _rollerModbusCommunication = rollerModbusCommunication;
            _torqueModbusCommunication = torqueModbusCommunication;
            _robotLoad = robotLoad;
            _robotUnload = robotUnload;
            _displayManager.EnableExtend();
            _activeScreen = InteractiveScreen.Primary;
            MoveMainWindowTo(_activeScreen);
            _isViewOnly = false;

            ConveyorManualUnitViewModel inConveyor = new ConveyorManualUnitViewModel("In Conveyor");
            inConveyor.Cylinders = Devices.GetInConveyorCylinders();
            inConveyor.Inputs = Devices.GetInConveyorInputs();
            inConveyor.Outputs = Devices.GetInConveyorOutputs();
            inConveyor.Rollers = Devices.GetInConveyorRollers();
            inConveyor.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("InCassetteCVImage");

            ConveyorManualUnitViewModel inWorkConveyor = new ConveyorManualUnitViewModel("In Work Conveyor");
            inWorkConveyor.Cylinders = Devices.GetInWorkConveyorCylinders();
            inWorkConveyor.Inputs = Devices.GetInWorkConveyorInputs();
            inWorkConveyor.Outputs = Devices.GetInWorkConveyorOutputs();
            inWorkConveyor.Rollers = Devices.GetInWorkConveyorRollers();
            inWorkConveyor.Motions = Devices.GetCSTLoadMotions();
            inWorkConveyor.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("LoadWorkCassetteStageImage");

            ConveyorManualUnitViewModel bufferConveyor = new ConveyorManualUnitViewModel("Buffer Conveyor");
            bufferConveyor.Cylinders = Devices.GetBufferConveyorCylinders();
            bufferConveyor.Inputs = Devices.GetBufferConveyorInputs();
            bufferConveyor.Outputs = Devices.GetBufferConveyorOutputs();
            bufferConveyor.Rollers = Devices.GetBufferConveyorRollers();
            bufferConveyor.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("BufferCVImage");

            ConveyorManualUnitViewModel outWorkConveyor = new ConveyorManualUnitViewModel("Out Work Conveyor");
            outWorkConveyor.Cylinders = Devices.GetOutWorkConveyorCylinders();
            outWorkConveyor.Inputs = Devices.GetOutWorkConveyorInputs();
            outWorkConveyor.Outputs = Devices.GetOutWorkConveyorOutputs();
            outWorkConveyor.Rollers = Devices.GetOutWorkConveyorRollers();
            outWorkConveyor.Motions = Devices.GetCSTUnloadMotions();
            outWorkConveyor.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("LoadWorkCassetteStageImage");

            ConveyorManualUnitViewModel outConveyor = new ConveyorManualUnitViewModel("Out Conveyor");
            outConveyor.Cylinders = Devices.GetOutConveyorCylinders();
            outConveyor.Inputs = Devices.GetOutConveyorInputs();
            outConveyor.Outputs = Devices.GetOutConveyorOutputs();
            outConveyor.Rollers = Devices.GetOutConveyorRollers();
            outConveyor.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("OutCassetteCVImage");

            ManualUnitViewModel vinylClean = new ManualUnitViewModel("Vinyl Clean");
            vinylClean.Cylinders = Devices.GetVinylCleanCylinders();
            vinylClean.Inputs = Devices.GetVinylCleanInputs();
            vinylClean.Outputs = Devices.GetVinylCleanOutputs();
            vinylClean.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("VinylCleanImage");

            ManualUnitViewModel transferFixture = new ManualUnitViewModel("Transfer Fixture");
            transferFixture.Cylinders = Devices.GetGlassTransferCylinders();
            transferFixture.Inputs = Devices.GetTransferFixtureInputs();
            transferFixture.Outputs = Devices.GetTransferFixtureOutputs();
            transferFixture.Motions = Devices.GetTransferFixtureMotions();
            transferFixture.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferFixtureImage");

            ManualUnitViewModel detach = new ManualUnitViewModel("Detach");
            detach.Cylinders = Devices.GetDetachCylinders();
            detach.Motions = Devices.GetDetachMotions();
            detach.Inputs = Devices.GetDetachInputs();
            detach.Outputs = Devices.GetDetachOutputs();
            detach.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("DetachImage");

            ManualUnitViewModel glassTransfer = new ManualUnitViewModel("Glass Transfer");
            glassTransfer.Cylinders = Devices.GetGlassTransferCylinders();
            glassTransfer.Motions = Devices.GetGlassTransferMotions();
            glassTransfer.Inputs = Devices.GetGlassTransferInputs();
            glassTransfer.Outputs = Devices.GetGlassTransferOutputs();
            glassTransfer.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("GlassTransferImage");

            ManualUnitViewModel transferInShuttleLeft = new ManualUnitViewModel("Transfer In Shuttle Left");
            transferInShuttleLeft.Cylinders = Devices.GetTransferInShuttleLeftCylinders();
            transferInShuttleLeft.Motions = Devices.GetTransferShutterLeftMotions();
            transferInShuttleLeft.Inputs = Devices.GetTransferShutterLeftInputs();
            transferInShuttleLeft.Outputs = Devices.GetTransferShutterLeftOutputs();
            transferInShuttleLeft.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage");

            ManualUnitViewModel transferInShuttleRight = new ManualUnitViewModel("Transfer In Shuttle Right");
            transferInShuttleRight.Cylinders = Devices.GetTransferInShuttleRightCylinders();
            transferInShuttleRight.Motions = Devices.GetTransferShutterRightMotions();
            transferInShuttleRight.Inputs = Devices.GetTransferShutterRightInputs();
            transferInShuttleRight.Outputs = Devices.GetTransferShutterRightOutputs();
            transferInShuttleRight.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage");

            ManualUnitViewModel wetCleanLeft = new ManualUnitViewModel("WET Clean Left");
            wetCleanLeft.Cylinders = Devices.GetWETCleanLeftCylinders();
            wetCleanLeft.Motions = Devices.GetWETCleanLeftMotions();
            wetCleanLeft.Inputs = Devices.GetWETCleanLeftInputs();
            wetCleanLeft.Outputs = Devices.GetWETCleanLeftOutputs();
            wetCleanLeft.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");

            ManualUnitViewModel wetCleanRight = new ManualUnitViewModel("WET Clean Right");
            wetCleanRight.Cylinders = Devices.GetWETCleanRightCylinders();
            wetCleanRight.Motions = Devices.GetWETCleanRightMotions();
            wetCleanRight.Inputs = Devices.GetWETCleanRightInputs();
            wetCleanRight.Outputs = Devices.GetWETCleanRightOutputs();
            wetCleanRight.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");

            ManualUnitViewModel transferRotationLeft = new ManualUnitViewModel("Transfer Rotation Left");
            transferRotationLeft.Cylinders = Devices.GetTransferRotationLeftCylinders();
            transferRotationLeft.Motions = Devices.GetTransferRotationLeftMotions();
            transferRotationLeft.Inputs = Devices.GetTransferRotationLeftInputs();
            transferRotationLeft.Outputs = Devices.GetTransferRotationLeftOutputs();
            transferRotationLeft.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage");

            ManualUnitViewModel transferRotationRight = new ManualUnitViewModel("Transfer Rotation Right");
            transferRotationRight.Cylinders = Devices.GetTransferRotationRightCylinders();
            transferRotationRight.Motions = Devices.GetTransferRotationRightMotions();
            transferRotationRight.Inputs = Devices.GetTransferRotationRightInputs();
            transferRotationRight.Outputs = Devices.GetTransferRotationRightOutputs();
            transferRotationRight.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage");

            ManualUnitViewModel afCleanLeft = new ManualUnitViewModel("AF Clean Left");
            afCleanLeft.Cylinders = Devices.GetAFCleanLeftCylinders();
            afCleanLeft.Motions = Devices.GetAFCleanLeftMotions();
            afCleanLeft.Inputs = Devices.GetAFCleanLeftInputs();
            afCleanLeft.Outputs = Devices.GetAFCleanLeftOutputs();
            afCleanLeft.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");

            ManualUnitViewModel afCleanRight = new ManualUnitViewModel("AF Clean Right");
            afCleanRight.Cylinders = Devices.GetAFCleanRightCylinders();
            afCleanRight.Motions = Devices.GetAFCleanRightMotions();
            afCleanRight.Inputs = Devices.GetAFCleanRightInputs();
            afCleanRight.Outputs = Devices.GetAFCleanRightOutputs();
            afCleanRight.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");

            ManualUnitViewModel unloadTransferLeft = new ManualUnitViewModel("Unload Transfer Left");
            unloadTransferLeft.Cylinders = Devices.GetUnloadTransferLeftCylinders();
            unloadTransferLeft.Motions = Devices.GetUnloadTransferLeftMotions();
            unloadTransferLeft.Inputs = Devices.GetUnloadTransferLeftInputs();
            unloadTransferLeft.Outputs = Devices.GetUnloadTransferLeftOutputs();
            unloadTransferLeft.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadTransferImage");

            ManualUnitViewModel unloadTransferRight = new ManualUnitViewModel("Unload Transfer Right");
            unloadTransferRight.Cylinders = Devices.GetUnloadTransferRightCylinders();
            unloadTransferRight.Motions = Devices.GetUnloadTransferRightMotions();
            unloadTransferRight.Inputs = Devices.GetUnloadTransferRightInputs();
            unloadTransferRight.Outputs = Devices.GetUnloadTransferRightOutputs();
            unloadTransferRight.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadTransferImage");

            ManualUnits = new ObservableCollection<ManualUnitViewModel>()
            {
                inConveyor,
                inWorkConveyor,
                bufferConveyor,
                outWorkConveyor,
                outConveyor,
                vinylClean,
                transferFixture,
                detach,
                glassTransfer,
                transferInShuttleLeft,
                transferInShuttleRight,
                wetCleanLeft,
                wetCleanRight,
                transferRotationLeft,
                transferRotationRight,
                afCleanLeft,
                afCleanRight,
                unloadTransferLeft,
                unloadTransferRight,
            };

            SelectedManualUnit = ManualUnits.First();
        }
        #endregion

        #region Private Methods
        private void MoveMainWindowTo(InteractiveScreen screen)
        {
            var monitors = GetMonitors();
            if (screen == InteractiveScreen.Primary && monitors.Count > 0)
            {
                PositionWindow(monitors[0]);
            }
            else if (screen == InteractiveScreen.Secondary && monitors.Count > 1)
            {
                PositionWindow(monitors[1]);
            }
        }

        private static void PositionWindow(MonitorInfo info)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = Application.Current.MainWindow;
                if (window != null)
                {
                    var wasMaximized = window.WindowState == WindowState.Maximized;
                    if (wasMaximized)
                    {
                        window.WindowState = WindowState.Normal;
                    }
                    window.Left = info.Left;
                    window.Top = info.Top;
                    window.Width = info.Width;
                    window.Height = info.Height;
                    if (wasMaximized)
                    {
                        window.WindowState = WindowState.Maximized;
                    }
                }
            });
        }

        private static IList<MonitorInfo> GetMonitors()
        {
            var result = new List<MonitorInfo>();
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                (IntPtr hMonitor, IntPtr hdc, ref RECT rect, IntPtr data) =>
                {
                    result.Add(new MonitorInfo(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top));
                    return true;
                }, IntPtr.Zero);
            return result;
        }

        private record MonitorInfo(int Left, int Top, int Width, int Height);

        private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdc, ref RECT lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion
    }
}
