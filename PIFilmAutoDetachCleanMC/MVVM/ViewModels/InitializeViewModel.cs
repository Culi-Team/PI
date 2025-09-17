using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Process;
using EQX.Core.Sequence;
using log4net;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Process;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class InitializeViewModel : ViewModelBase
    {
        public const int SkipToRunStepValue = int.MaxValue - 1;
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);

        private readonly Processes _processes;
        private readonly MachineStatus _machineStatus;
        private readonly INavigationService _navigationService;

        private bool _isBusy;
        private string _statusMessage = string.Empty;

        public ObservableCollection<InitializeModuleItemViewModel> Modules { get; }
        public IRelayCommand SelectAllCommand { get; }
        public IRelayCommand UnselectAllCommand { get; }
        public IAsyncRelayCommand InitializeCommand { get; }
        public IRelayCommand NavigateBackCommand { get; }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    RefreshCommands();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            private set => SetProperty(ref _statusMessage, value);
        }

        public bool CanInitialize => !IsBusy /*&& IsRootIdle()*/;

        //IsRootIdle will fix after

        public InitializeViewModel(Processes processes, MachineStatus machineStatus, INavigationService navigationService)
        {
            _processes = processes;
            _machineStatus = machineStatus;
            _navigationService = navigationService;

            Log = LogManager.GetLogger("InitializeVM");

            Modules = new ObservableCollection<InitializeModuleItemViewModel>(CreateModules());

            SelectAllCommand = new RelayCommand(SelectAll);
            UnselectAllCommand = new RelayCommand(UnselectAll);
            InitializeCommand = new AsyncRelayCommand(InitializeSelectedAsync, () => CanInitialize);
            NavigateBackCommand = new RelayCommand(() => _navigationService.NavigateTo<AutoViewModel>());

            StatusMessage = "Select PI Detach module(s) and press Initialize";

            _machineStatus.PropertyChanged += MachineStatusOnPropertyChanged;
            _processes.RootProcess.ProcessModeUpdated += RootProcess_ProcessModeUpdated;
        }

        public override void Dispose()
        {
            base.Dispose();

            _machineStatus.PropertyChanged -= MachineStatusOnPropertyChanged;
            _processes.RootProcess.ProcessModeUpdated -= RootProcess_ProcessModeUpdated;

            foreach (var module in Modules)
            {
                module.Dispose();
            }
        }

        private void MachineStatusOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            InvokeOnUiThread(RefreshCommands);
        }

        private static void InvokeOnUiThread(Action action)
        {
            if(action == null)
            {
                return;
            }

            var dispatcher = Application.Current?.Dispatcher;
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action);
            }
        }

        private void RootProcess_ProcessModeUpdated(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(RefreshCommands));
        }

        private void RefreshCommands()
        {
            InitializeCommand.NotifyCanExecuteChanged();
            foreach (var module in Modules)
            {
                module.RefreshCanExecute();
            }
        }

        private void SelectAll()
        {
            foreach (var module in Modules)
            {
                module.IsSelected = true;
            }
        }

        private void UnselectAll()
        {
            foreach (var module in Modules)
            {
                module.IsSelected = false;
            }
        }

        private IEnumerable<InitializeModuleItemViewModel> CreateModules()
        {
            return new List<InitializeModuleItemViewModel>
            {
                new InitializeModuleItemViewModel(this, "Conveyors", new[]
                {
                    _processes.InConveyorProcess,
                    _processes.InWorkConveyorProcess,
                    _processes.BufferConveyorProcess,
                    _processes.OutWorkConveyorProcess,
                    _processes.OutConveyorProcess
                }, 0, 2, 254, 356),
                new InitializeModuleItemViewModel(this, "Transfer Fixture", new[]
                {
                    _processes.TransferFixtureProcess
                }, 323, 67, 55, 190),
                new InitializeModuleItemViewModel(this, "Load Robot", new[]
                {
                    _processes.RobotLoadProcess
                }, 269, 151, 60, 60),
                new InitializeModuleItemViewModel(this, "Vinyl Clean", new[]
                {
                    _processes.VinylCleanProcess
                }, 257, 284, 90, 90),
                new InitializeModuleItemViewModel(this, "Align Fixture", new[]
                {
                    _processes.FixtureAlignProcess
                }, 367, 237, 65, 80),
                new InitializeModuleItemViewModel(this, "Detach Glass", new[]
                {
                    _processes.DetachProcess
                }, 425, 142, 140, 80),
                new InitializeModuleItemViewModel(this, "Remove Film", new[]
                {
                    _processes.RemoveFilmProcess
                }, 369, 40, 130, 90),
                new InitializeModuleItemViewModel(this, "Glass Transfer", new[]
                {
                    _processes.GlassTransferProcess
                }, 604, 76, 40, 190),
                new InitializeModuleItemViewModel(this, "Glass Align Left", new[]
                {
                    _processes.GlassAlignLeftProcess
                }, 558, 75, 60, 60),
                new InitializeModuleItemViewModel(this, "Glass Align Right", new[]
                {
                    _processes.GlassAlignRightProcess
                }, 558, 231, 60, 60),
                new InitializeModuleItemViewModel(this, "Transfer In Shuttle Left", new[]
                {
                    _processes.TransferInShuttleLeftProcess
                }, 510, 30, 60, 90),
                new InitializeModuleItemViewModel(this, "Transfer In Shuttle Right", new[]
                {
                    _processes.TransferInShuttleRightProcess
                }, 510, 224, 60, 90),
                new InitializeModuleItemViewModel(this, "Wet Clean Left", new[]
                {
                    _processes.WETCleanLeftProcess
                }, 653, 25, 90, 70),
                new InitializeModuleItemViewModel(this, "Wet Clean Right", new[]
                {
                    _processes.WETCleanRightProcess
                }, 653, 261, 90, 70),
                new InitializeModuleItemViewModel(this, "AF Clean Right", new[]
                {
                    _processes.AFCleanRightProcess
                }, 802, 261, 90, 70),
                new InitializeModuleItemViewModel(this, "AF Clean Left", new[]
                {
                    _processes.AFCleanLeftProcess
                }, 802, 25, 90, 70),
                new InitializeModuleItemViewModel(this, "Transfer Rotation Left", new[]
                {
                    _processes.TransferRotationLeftProcess
                }, 758, 88, 45, 90),
                new InitializeModuleItemViewModel(this, "Transfer Rotation Right", new[]
                {
                    _processes.TransferRotationRightProcess
                }, 758, 188, 45, 90),
                new InitializeModuleItemViewModel(this, "Unload Transfer Left", new[]
                {
                    _processes.UnloadTransferLeftProcess
                }, 946, 52, 90, 55),
                new InitializeModuleItemViewModel(this, "Unload Transfer Right", new[]
                {
                    _processes.UnloadTransferRightProcess
                }, 946, 248, 90, 55),
                new InitializeModuleItemViewModel(this, "Unload Align", new[]
                {
                    _processes.UnloadAlignProcess
                }, 980, 142, 75, 75),
                new InitializeModuleItemViewModel(this, "Unload Robot", new[]
                {
                    _processes.RobotUnloadProcess
                }, 1081, 156, 50, 60)
            };
        }

        private async Task InitializeSelectedAsync()
        {
            //if (!CanInitialize)
            //{
            //    StatusMessage = "Machine must be stopped before initializing";
            //    return;
            //}

            var selected = Modules.Where(m => m.IsSelected).ToList();
            if (selected.Count == 0)
            {
                StatusMessage = "Select at least one module.";
                return;
            }

            Log.Info($"Initialize comand triggered for modules: {string.Join(",", selected.Select(m => m.DisplayName))}");
            
            IsBusy = true;

            try
            {
                foreach (var module in selected)
                {
                    try
                    {
                        var success = await InitializeModuleAsync(module);
                        if (!success)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        internal async Task<bool> InitializeModuleAsync(InitializeModuleItemViewModel module)
        {
            //if (!CanInitialize)
            //{
            //    StatusMessage = "Machine must be stopped before initializing.";
            //    return;
            //}

            module.ResetCompletionState();
            module.IsInitializing = true;
            RefreshCommands();

            try
            {
                StatusMessage = $"Initializing {module.DisplayName}...";
                Log.Info($"Initializing module '{module.DisplayName}'.");
                var success = await Task.Run(() => RunModule(module));

                if (success)
                {
                    StatusMessage = $"{module.DisplayName} initialized successfully.";
                    Log.Info($"Module '{module.DisplayName}' initialized successfully.");
                }
                else
                {
                    StatusMessage = $"{module.DisplayName} initialize timeout. Warning raised.";
                }

                return success;
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to initialize {module.DisplayName}", ex);
                StatusMessage = $"Failed to initialize {module.DisplayName}: {ex.Message}";
                module.ResetCompletionState();
                throw;
            }
            finally
            {
                module.IsInitializing = false;
                RefreshCommands();
            }
        }

        private bool RunModule(InitializeModuleItemViewModel module)
        {
            foreach (var process in module.Processes)
            {
                if (!RunProcess(process, module.DisplayName))
                {
                    return false;
                }
            }
            return true;
        }

        private bool RunProcess(IProcess<ESequence> targetProcess, string moduleName)
        {
            var root = _processes.RootProcess;

            //if (!IsRootIdle())
            //{
            //    throw new InvalidOperationException("Machine must be stopped before initializing.");
            //}

            var rootSnapshot = new ProcessSnapshot(root);
            var childProcesses = root.Childs?.OfType<IProcess<ESequence>>().ToList() ?? new List<IProcess<ESequence>>();
            if (!childProcesses.Contains(targetProcess))
            {
                childProcesses.Add(targetProcess);
            }
            var childSnapshots = childProcesses.Select(p => new ProcessSnapshot(p)).ToList();

            var completed = false;

            try
            {
                foreach (var snapshot in childSnapshots)
                {
                    if (snapshot.Process == targetProcess)
                    {
                        continue;
                    }

                    snapshot.Process.ProcessStatus = EProcessStatus.ToRunDone;
                    snapshot.Process.Sequence = ESequence.Stop;
                    snapshot.Process.Step.ToRunStep = SkipToRunStepValue;
                }

                targetProcess.ProcessStatus = EProcessStatus.None;
                targetProcess.Sequence = ESequence.Ready;
                targetProcess.Step.ToRunStep = 0;

                root.ProcessStatus = EProcessStatus.None;
                root.Sequence = ESequence.Ready;
                root.Step.ToRunStep = 0;

                root.ProcessMode = EProcessMode.ToRun;

                completed = WaitForProcess(targetProcess);

                if (completed)
                {
                    WaitForRootToExitToRun();
                }
                else
                {
                    var warningMessage = $"{moduleName} initialize timeout.";
                    Log.Warn(warningMessage);
                    root.RaiseWarning((int)EWarning.InitializeTimeout, warningMessage);
                }
            }
            finally
            {
                root.ProcessMode = EProcessMode.Stop;

                foreach (var snapshot in childSnapshots)
                {
                    snapshot.Restore();
                }

                rootSnapshot.Restore();
            }

            return completed;
        }

        private bool WaitForProcess(IProcess<ESequence> process)
        {
            var sw = Stopwatch.StartNew();
            while (process.ProcessStatus != EProcessStatus.ToRunDone)
            {
                if (sw.Elapsed > DefaultTimeout)
                {
                    return false;
                }

                Thread.Sleep(50);
            }
            return true;
        }

        private void WaitForRootToExitToRun()
        {
            var root = _processes.RootProcess;
            var sw = Stopwatch.StartNew();
            while (root.ProcessMode == EProcessMode.ToRun)
            {
                if (sw.Elapsed > TimeSpan.FromSeconds(10))
                {
                    break;
                }

                Thread.Sleep(20);
            }
        }

        //private bool IsRootIdle()
        //{
        //    var mode = _processes.RootProcess.ProcessMode;
        //    return mode == EProcessMode.None || mode == EProcessMode.Stop || mode == EProcessMode.Warning;
        //}

        private class ProcessSnapshot
        {
            public ProcessSnapshot(IProcess<ESequence> process)
            {
                Process = process;
                Sequence = process.Sequence;
                Status = process.ProcessStatus;
                ToRunStep = process.Step.ToRunStep;
            }

            public IProcess<ESequence> Process { get; }
            public ESequence Sequence { get; }
            public EProcessStatus Status { get; }
            public int ToRunStep { get; }

            public void Restore()
            {
                Process.Sequence = Sequence;
                Process.ProcessStatus = Status;
                Process.Step.ToRunStep = ToRunStep;
            }
        }
    }
    public class InitializeModuleItemViewModel : ObservableObject, IDisposable
    {
        private readonly InitializeViewModel _owner;
        private readonly List<IProcess<ESequence>> _processes;
        private readonly List<EventHandler> _handlers = new();
        private readonly DispatcherTimer _statusTimer;

        private bool _isSelected;
        private bool _isInitializing;
        private bool _isCompleted;

        public InitializeModuleItemViewModel(
            InitializeViewModel owner,
            string displayName,
            IEnumerable<IProcess<ESequence>> processes,
            double positionX,
            double positionY,
            double width,
            double height)
        {
            _owner = owner;
            DisplayName = displayName;
            _processes = processes.ToList();
            PositionX = positionX;
            PositionY = positionY;
            Width = width;
            Height = height;
            ProcessSummary = string.Join(Environment.NewLine, _processes.Select(GetProcessDisplayName));

            InitializeCommand = new AsyncRelayCommand(async () => await _owner.InitializeModuleAsync(this), () => _owner.CanInitialize && !IsInitializing);

            ResetCompletionState();

            foreach (var process in _processes)
            {
                EventHandler handler = (s, e) =>
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        OnPropertyChanged(nameof(StepText));
                    }));
                };

                process.Step.StepChangedHandler += handler;
                _handlers.Add(handler);
            }
            _statusTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            _statusTimer.Tick += StatusTimerOnTick;
            _statusTimer.Start();
            UpdateCompletionState();
        }

        public string DisplayName { get; }
        public double PositionX { get; }
        public double PositionY { get; }
        public double Width { get; }
        public double Height { get; }
        public IReadOnlyList<IProcess<ESequence>> Processes => _processes;
        public string ProcessSummary { get; }
        public string ButtonContent => IsCompleted ? "Initialize done" : DisplayName;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public bool IsInitializing
        {
            get => _isInitializing;
            set
            {
                if (SetProperty(ref _isInitializing, value))
                {
                    InitializeCommand.NotifyCanExecuteChanged();
                    if (!value)
                    {
                        UpdateCompletionState();
                    }
                }
            }
        }

        public string StepText
        {
            get
            {
                var step = _processes
                    .Select(p => p.Step.ToRunStep)
                    .Where(v => v >= 0 && v < InitializeViewModel.SkipToRunStepValue)
                    .DefaultIfEmpty(0)
                    .Max();

                return $"Step: {step}";
            }
        }

        public IAsyncRelayCommand InitializeCommand { get; }

        public bool IsCompleted
        {
            get => _isCompleted;
            private set
            {
                if (SetProperty(ref _isCompleted, value))
                {
                    OnPropertyChanged(nameof(ButtonContent));
                }
            }
        }

        internal void RefreshCanExecute()
        {
            InitializeCommand.NotifyCanExecuteChanged();
        }

        internal void ResetCompletionState()
        {
            IsCompleted = false;
        }

        public void Dispose()
        {
            _statusTimer.Stop();
            _statusTimer.Tick -= StatusTimerOnTick;
            for (int i = 0; i < _processes.Count; i++)
            {
                _processes[i].Step.StepChangedHandler -= _handlers[i];
            }
        }
        private void UpdateCompletionState()
        {
            var completed = !IsInitializing && _processes.Count > 0 && _processes.All(p => p.ProcessStatus == EProcessStatus.ToRunDone);
            if (completed != IsCompleted)
            {
                IsCompleted = completed;
            }
        }

        private void StatusTimerOnTick(object? sender, EventArgs e)
        {
            UpdateCompletionState();
        }

        private static string GetProcessDisplayName(IProcess<ESequence> process)
        {
            var name = process.Name?.Replace('_', ' ') ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            var builder = new StringBuilder(name.Length * 2);

            for (int i = 0; i < name.Length; i++)
            {
                var current = name[i];
                var previous = i > 0 ? name[i - 1] : '\0';
                var next = i + 1 < name.Length ? name[i + 1] : '\0';
                var insertSpace = false;

                if (i > 0 && !char.IsWhiteSpace(previous))
                {
                    var currentIsUpper = char.IsUpper(current);
                    var previousIsUpper = char.IsUpper(previous);

                    if (currentIsUpper && !previousIsUpper)
                    {
                        insertSpace = true;
                    }
                    else if (currentIsUpper && previousIsUpper && char.IsLower(next))
                    {
                        insertSpace = true;
                    }
                }

                if (insertSpace)
                {
                    builder.Append(' ');
                }

                builder.Append(current);
            }

            return builder.ToString();

        }
    }
}

    



