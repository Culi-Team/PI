using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Communication.Modbus;
using EQX.Core.Display;
using EQX.Core.InOut;
using EQX.UI.Display;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Manual;
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
        private ManualUnitViewModel selectedManualUnit;
        private readonly DisplayManager _displayManager = new();
        private readonly ViewOnlyOverlay _viewOnlyOverlay = new();
        private InteractiveScreen _activeScreen;
        private bool _isViewOnly;
        #endregion

        #region Properties
        public Devices Devices { get; }
        public MachineStatus MachineStatus { get; }
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

        public ConveyorManualUnitViewModel InConveyor { get; }
        public ConveyorManualUnitViewModel InWorkConveyor { get; }
        public ConveyorManualUnitViewModel BufferConveyor { get; }
        public ConveyorManualUnitViewModel OutWorkConveyor { get; }
        public ConveyorManualUnitViewModel OutConveyor { get; }

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
            [FromKeyedServices("TorqueControllerModbusCommunication")] IModbusCommunication torqueModbusCommunication)
        {
            Devices = devices;
            MachineStatus = machineStatus;
            
            _rollerModbusCommunication = rollerModbusCommunication;
            _torqueModbusCommunication = torqueModbusCommunication;
            _displayManager.EnableExtend();
            _activeScreen = InteractiveScreen.Primary;
            MoveMainWindowTo(_activeScreen);
            _isViewOnly = false;

            InConveyor =new ConveyorManualUnitViewModel("In Conveyor");
            InConveyor.Cylinders = Devices.GetInConveyorCylinders();
            InConveyor.Inputs = Devices.GetInConveyorInputs();
            InConveyor.Outputs = Devices.GetInConveyorOutputs();
            InConveyor.Rollers = Devices.GetInConveyorRollers();
            InConveyor.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("InCassetteCVImage");

            ManualUnits = new ObservableCollection<ManualUnitViewModel>()
            {
                InConveyor,
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
