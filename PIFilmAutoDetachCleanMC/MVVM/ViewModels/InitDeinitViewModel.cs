using EQX.Core.Common;
using EQX.Core.InOut;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Process;
using EQX.Core.Device.SpeedController;
using EQX.Core.Communication.Modbus;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Recipe;
using log4net;
using System.Runtime.CompilerServices;
using PIFilmAutoDetachCleanMC.Defines.VirtualIO;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    /// <summary>
    /// Step list for initialization / deinitialization machine
    /// </summary>
    public enum EHandleStep
    {
        Start,

        FileSystemHandle,

        CommunicationHandle,

        MotionDeviceHandle,
        IODeviceHandle,

        RecipeHandle,

        ProcessHandle,


        End,
        Error,

        Navigate,
    }

    public enum EHandleMode
    {
        None,
        Init,
        Deinit,
    }

    public class InitDeinitViewModel : ViewModelBase
    {
        #region Properties
        public string MessageText
        {
            get { return _messageText; }
            set
            {
                _messageText = value;
                // The property may be updated from another thread.
                // So that, call OnPropertyChanged() inside and UI Invoke to make sure UI update properly
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    OnPropertyChanged();
                }), DispatcherPriority.Render);
            }
        }

        public List<string> ErrorMessages { get; private set; }
        #endregion

        public InitDeinitViewModel(Devices devices,
            Processes processes,
            INavigationService navigationService,
            [FromKeyedServices("RollerModbusCommunication")]IModbusCommunication rollerModbusCommunication,
            [FromKeyedServices("TorqueControllerModbusCommunication")]IModbusCommunication torqueModbusCommnication,
            RecipeSelector recipeSelector,
            VirtualIO<EFlags> virtualIO)
        {
            _devices = devices;
            _processes = processes;
            _navigationService = navigationService;
            _rollerModbusCommunication = rollerModbusCommunication;
            _torqueModbusCommnication = torqueModbusCommnication;
            _recipeSelector = recipeSelector;
            _virtualIO = virtualIO;
            _task = new Task(() => { });
            ErrorMessages = new List<string>();

            Log = LogManager.GetLogger("InitVM");
        }

        public void Initialization()
        {
            ErrorMessages = new List<string>();

            _task = new Task(() =>
            {
                if (HandlePreCheck(EHandleMode.Init) == false) return;

                OnInitialization();
                mode = EHandleMode.None;
            });

            _task.Start();
        }

        public void Deinitialization()
        {
            _task = new Task(() =>
            {
                if (HandlePreCheck(EHandleMode.Deinit) == false) return;

                OnDeinitialization();
                mode = EHandleMode.None;
            });

            _task.Start();
        }

        private bool HandlePreCheck(EHandleMode _mode)
        {
            if (mode == _mode)
            {
                // Already handle the same mode
                return false;
            }

            while (mode != EHandleMode.None)
            {
                Thread.Sleep(10);
            }

            mode = _mode;
            isHandling = true;
            _step = EHandleStep.Start;

            return true;
        }

        private void OnInitialization()
        {
            while (isHandling)
            {
                switch (_step)
                {
                    case EHandleStep.Start:
                        MessageText = "Init Start";
                        _step++;
                        break;
                    case EHandleStep.FileSystemHandle:
                        _step++;
                        break;
                    case EHandleStep.CommunicationHandle:
                        Log.Debug("Connect Modbus Communication");
                        _rollerModbusCommunication.Connect();
                        _torqueModbusCommnication.Connect();
                        _step++;
                        break;
                    case EHandleStep.MotionDeviceHandle:
                        Log.Debug("Connect Motion Devices");

                        MessageText = "Connect Motion Devices";

                        _devices.MotionsInovance.MotionControllerInovance.Connect();

                        _devices.MotionsAjin.All.ForEach(m => m.Connect());

                        _devices.MotionsInovance.All.ForEach(m => m.Connect());

                        if (_devices.MotionsInovance.All.Any(m => m.IsConnected == false))
                        {
                            ErrorMessages.Add($"Motion device is not connected: " +
                                $"{string.Join(", ", _devices.MotionsInovance.All.Where(m => m.IsConnected == false).Select(m => m.Name))}");
                        }

                        if (_devices.MotionsAjin.All.Any(m => m.IsConnected == false))
                        {
                            ErrorMessages.Add($"Motion device is not connected: " +
                                $"{string.Join(", ", _devices.MotionsAjin.All.Where(m => m.IsConnected == false).Select(m => m.Name))}");
                        }

                        _devices.MotionsInovance.All.ForEach(m => m.Initialization());
                        _devices.MotionsAjin.All.ForEach(m => m.Initialization());

                        _step++;
                        break;
                    case EHandleStep.IODeviceHandle:
                        Log.Debug("Connect IO Devices");

                        MessageText = "Connect IO Devices";
                        _isSuccess = true;

                        _isSuccess &= _devices.Inputs.Initialize();
                        _isSuccess &= _devices.Outputs.Initialize();

                        _isSuccess &= _devices.Inputs.Connect();
                        _isSuccess &= _devices.Outputs.Connect();

                        if (_isSuccess == false)
                        {
                            ErrorMessages.Add("IO Devices init failed.");
                        }

                        _step++;
                        break;
                    case EHandleStep.RecipeHandle:
                        Log.Debug("Load Recipe");

                        _recipeSelector.Load();
                        _step++;
                        break;
                    case EHandleStep.ProcessHandle:
                        Log.Debug("Init processes");

                        _processes.Initialize();

                        _virtualIO.Initialize();
                        _step++;
                        break;
                    case EHandleStep.End:
                        _step++;
                        break;
                    case EHandleStep.Error:
                        if (ErrorMessages.Count > 0)
                        {
                            Log.Error("Error: " + string.Join(", ", ErrorMessages));
                            MessageText = "Error: " + string.Join(", ", ErrorMessages);
                            Thread.Sleep(500);
                        }
                        else
                        {
                            MessageText = "Initialization completed successfully.";
                        }
                        _step++;
                        break;
                    case EHandleStep.Navigate:
                        MessageText = "Navigating...";
                        _navigationService.NavigateTo<AutoViewModel>();
                        _step++;
                        break;
                    default:
                        isHandling = false;
                        return;
                }
                Thread.Sleep(2);
            }
        }

        private void OnDeinitialization()
        {
            while (isHandling)
            {
                switch (_step)
                {
                    case EHandleStep.Start:
                        MessageText = "Deinit Start";
                        _step++;
                        break;
                    case EHandleStep.FileSystemHandle:
                        _step++;
                        break;
                    case EHandleStep.CommunicationHandle:
                        _rollerModbusCommunication.Disconnect();
                        _torqueModbusCommnication.Disconnect();
                        _step++;
                        break;
                    case EHandleStep.MotionDeviceHandle:
                        MessageText = "Connect Motion Devices";

                        _devices.MotionsInovance.MotionControllerInovance.Disconnect();

                        _devices.MotionsInovance.All.ForEach(m => m.Disconnect());

                        _rollerModbusCommunication.Disconnect();
                        _step++;
                        break;
                    case EHandleStep.IODeviceHandle:
                        MessageText = "Connect IO Devices";
                        _devices.Inputs.Disconnect();
                        _devices.Outputs.Disconnect();
                        _step++;
                        break;
                    case EHandleStep.RecipeHandle:
                        _recipeSelector.Save();
                        _step++;
                        break;
                    case EHandleStep.ProcessHandle:
                        _step++;
                        break;
                    case EHandleStep.End:
                        _step++;
                        break;
                    case EHandleStep.Error:
                        _step++;
                        break;
                    case EHandleStep.Navigate:
                        _step++;
                        break;
                    default:
                        // Set isHandling to false to stop the loop

                        Thread.Sleep(1000);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Application.Current.Shutdown(100);
                        });
                        break;
                }
                Thread.Sleep(2);
            }
        }

        #region Private fields
        private readonly INavigationService _navigationService;
        private readonly IModbusCommunication _rollerModbusCommunication;
        private readonly IModbusCommunication _torqueModbusCommnication;
        private readonly RecipeSelector _recipeSelector;
        private readonly VirtualIO<EFlags> _virtualIO;
        private readonly Devices _devices;
        private readonly Processes _processes;

        private string _messageText = "";
        private Task _task;

        EHandleMode mode = EHandleMode.None;
        bool isHandling = true;

        private EHandleStep _step;

        /// <summary>
        /// Common use for all case (in switch statement)
        /// </summary>
        private bool _isSuccess = true;
        #endregion
    }
}
