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
using PIFilmAutoDetachCleanMC.Defines.Devices.Cassette;
using EQX.Core.Robot;
using EQX.Core.Communication;
using PIFilmAutoDetachCleanMC.Defines.ProductDatas;

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

        ProcessHandle_Wait,

        CassetteHandle,

        WorkDataHandle,

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
            [FromKeyedServices("RollerModbusCommunication")] IModbusCommunication rollerModbusCommunication,
            [FromKeyedServices("TorqueControllerModbusCommunication")] IModbusCommunication torqueModbusCommnication,
            RecipeSelector recipeSelector,
            VirtualIO virtualIO,
            CassetteList cassetteList,
            [FromKeyedServices("RobotLoad")] IRobot robotLoad,
            [FromKeyedServices("RobotUnload")] IRobot robotUnload,
            [FromKeyedServices("SyringePumpSerialCommunicator")] SerialCommunicator syringePumpSerialCommunicator,
            CWorkData workData,
            [FromKeyedServices("IndicatorModbusCommunication")] IModbusCommunication indicatorModbusCommunication)
        {
            _devices = devices;
            _processes = processes;
            _navigationService = navigationService;
            _rollerModbusCommunication = rollerModbusCommunication;
            _torqueModbusCommnication = torqueModbusCommnication;
            _recipeSelector = recipeSelector;
            _virtualIO = virtualIO;
            _cassetteList = cassetteList;
            _robotLoad = robotLoad;
            _robotUnload = robotUnload;
            _syringePumpSerialCommunicator = syringePumpSerialCommunicator;
            _workData = workData;
            _indicatorModbusCommunication = indicatorModbusCommunication;
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

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.FileSystemHandle:

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.CommunicationHandle:
                        Log.Debug("Connect Modbus Communication");
                        if (_rollerModbusCommunication.Connect() == false)
                        {
                            ErrorMessages.Add("Speed Controller Connect Fail");
                        }
                        if (_torqueModbusCommnication.Connect() == false)
                        {
                            ErrorMessages.Add("Torque Controller Connect Fail");
                        }
                        if(_syringePumpSerialCommunicator.Connect() == false)
                        {
                            ErrorMessages.Add("Syringe Pump Connect Fail");
                        }
                        if(_indicatorModbusCommunication.Connect() == false)
                        {
                            ErrorMessages.Add("Indicator Connect Fail");
                        }

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.MotionDeviceHandle:
                        Log.Debug("Connect Motion Devices");

                        MessageText = "Connect Motion Devices";

                        _devices.Motions.AjinMaster.Connect();
                        _devices.Motions.InovanceMaster.Connect();

                        _devices.Motions.All.ForEach(m => m.Connect());

                        _devices.RollerList.All.ForEach(s => s.Connect());

                        if(_rollerModbusCommunication.IsConnected)
                        {
                            _devices.RollerList.SetDirection();
                        }

                        _devices.TorqueControllers.All.ForEach(t => t.Connect());

                        _robotLoad.Connect();
                        _robotUnload.Connect();

                        if (_devices.RollerList.All.Any(m => m.IsConnected == false))
                        {
                            ErrorMessages.Add($"Speed Controller is not connected: " +
                                $"{string.Join(", ", _devices.RollerList.All.Where(m => m.IsConnected == false).Select(m => m.Name))}");
                        }

                        if (_devices.TorqueControllers.All.Any(m => m.IsConnected == false))
                        {
                            ErrorMessages.Add($"Torque controller is not connected: " +
                                $"{string.Join(", ", _devices.TorqueControllers.All.Where(m => m.IsConnected == false).Select(m => m.Name))}");
                        }

                        if (_devices.Motions.All.Any(m => m.IsConnected == false))
                        {
                            ErrorMessages.Add($"Motion device is not connected: " +
                                $"{string.Join(", ", _devices.Motions.All.Where(m => m.IsConnected == false).Select(m => m.Name))}");
                        }

                        if(_robotLoad.IsConnected == false)
                        {
                            ErrorMessages.Add("Robot Load is not connected");
                        }

                        if (_robotUnload.IsConnected == false)
                        {
                            ErrorMessages.Add("Robot Unload is not connected");
                        }

                        _devices.Motions.All.ForEach(m => m.Initialization());

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.IODeviceHandle:
                        Log.Debug("Connect IO Devices");

                        MessageText = "Connect IO Devices";
                        _isSuccess = true;

                        _isSuccess &= _devices.Inputs.Connect();
                        _isSuccess &= _devices.Outputs.Connect();
                        _isSuccess &= _devices.AnalogInputs.Connect();

                        _devices.Inputs.InputStatusSet();

                        _isSuccess &= _devices.Regulators.WetCleanLRegulator.Connect();
                        _isSuccess &= _devices.Regulators.WetCleanRRegulator.Connect();
                        _isSuccess &= _devices.Regulators.AfCleanLRegulator.Connect();
                        _isSuccess &= _devices.Regulators.AfCleanRRegulator.Connect();

                        if (_isSuccess == false)
                        {
                            ErrorMessages.Add("IO Devices init failed.");
                        }

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.RecipeHandle:
                        Log.Debug("Load Recipes");
                        MessageText = "Load Recipes";
                        if (_recipeSelector.Load() == false)
                        {
                            ErrorMessages.Add("Recipes Load Fail.");
                            Log.Debug("Recipe Load Fail");
                        }

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.ProcessHandle:
                        Log.Debug("Init processes");

                        _processes.Initialize();

                        _virtualIO.Initialize();
                        _virtualIO.Mappings();

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.ProcessHandle_Wait:
                        if (_processes.RootProcess.IsAlive && _processes.RootProcess.Childs.All(c => c.IsAlive))
                        {
                            _step++;
                            break;
                        }

                        Thread.Sleep(50);
                        break;
                    case EHandleStep.CassetteHandle:
                        Log.Debug("Init Cassette");
                        if(_cassetteList.Load() == false)
                        {
                            _cassetteList.RecipeUpdateHandle();
                        }
                        _cassetteList.SubscribeCellClickedEvent();

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.WorkDataHandle:
                        _workData.Load();

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.End:

                        Thread.Sleep(50);
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
                        MessageText = "Stop Processes";
                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.FileSystemHandle:
                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.CommunicationHandle:
                        Thread.Sleep(50);
                        _torqueModbusCommnication.Disconnect();
                        _syringePumpSerialCommunicator.Disconnect();
                        _indicatorModbusCommunication.Disconnect();

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.MotionDeviceHandle:
                        MessageText = "Disconnect Motion Devices";
                        _devices.Motions.InovanceMaster.Disconnect();
                        _devices.Motions.AjinMaster.Disconnect();

                        _devices.Motions.All.ForEach(m => m.Disconnect());

                        _devices.TorqueControllers.All.ForEach(t => t.Disconnect());

                        if(_rollerModbusCommunication.IsConnected)
                        {
                            _devices.RollerList.All.ForEach(r => r.Stop());
                        }    
                        _rollerModbusCommunication.Disconnect();

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.IODeviceHandle:
                        MessageText = "Disconnect IO Devices";
                        _devices.Inputs.Disconnect();
                        _devices.Outputs.Disconnect();
                        _devices.AnalogInputs.Disconnect();
                        _devices.Regulators.WetCleanLRegulator.Disconnect();
                        _devices.Regulators.WetCleanRRegulator.Disconnect();
                        _devices.Regulators.AfCleanLRegulator.Disconnect();
                        _devices.Regulators.AfCleanRRegulator.Disconnect();

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.RecipeHandle:
                        _recipeSelector.Save();

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.ProcessHandle:
                        _processes.ProcessesStop();

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.ProcessHandle_Wait:
                        if (_processes.RootProcess.IsAlive == false && _processes.RootProcess.Childs.All(c => c.IsAlive == false))
                        {
                            _step++;
                            break;
                        }

                        Thread.Sleep(50);
                        break;
                    case EHandleStep.CassetteHandle:
                        MessageText = "Cassette Save";
                        _cassetteList.Save();

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.WorkDataHandle:
                        _workData.Save();

                        Thread.Sleep(50);
                        _step++;
                        break;
                    case EHandleStep.End:

                        Thread.Sleep(50);
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
        private readonly VirtualIO _virtualIO;
        private readonly CassetteList _cassetteList;
        private readonly IRobot _robotLoad;
        private readonly IRobot _robotUnload;
        private readonly SerialCommunicator _syringePumpSerialCommunicator;
        private readonly CWorkData _workData;
        private readonly IModbusCommunication _indicatorModbusCommunication;
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
