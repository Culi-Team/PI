using EQX.Core.Common;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Process;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    /// <summary>
    /// Process Item for DataTemplate
    /// </summary>
    public class ProcessTakTime
    {
        public string ProcessName { get; set; }
        public double CurrentTime { get; set; }
        public double AverageTime { get; set; }
        public double LastTime { get; set; }
        public double TotalTime { get; set; }
    }

    /// <summary>
    /// </summary>
    public class ProcessTaktTimeViewModel : ViewModelBase
    {
        private readonly ProcessTaktTime _taktTime;
        private readonly ObservableCollection<ProcessTakTime> _processTakTime;

        public ProcessTaktTimeViewModel(ProcessTaktTime taktTime)
        {
            _taktTime = taktTime;
            _processTakTime = new ObservableCollection<ProcessTakTime>();
            ResetTaktTimeAllCommand = new RelayCommand(() => ResetTaktTimeAll());
            InitializeProcessItems();
        }

        public ObservableCollection<ProcessTakTime> ProcessTakTimeItems => _processTakTime;

        private void InitializeProcessItems()
        {
            _processTakTime.Clear();
            
            // Add processes in order according to EProcess.cs (excluding Root)
            var processes = new[]
            {
                (EProcess.InConveyor, "01. InConveyor"),
                (EProcess.InWorkConveyor, "02. InWorkConveyor"),
                (EProcess.BufferConveyor, "03. BufferConveyor"),
                (EProcess.OutWorkConveyor, "04. OutWorkConveyor"),
                (EProcess.OutConveyor, "05. OutConveyor"),
                (EProcess.RobotLoad, "06. RobotLoad"),
                (EProcess.VinylClean, "07. VinylClean"),
                (EProcess.FixtureAlign, "08. FixtureAlign"),
                (EProcess.TransferFixture, "09. TransferFixture"),
                (EProcess.Detach, "10. Detach"),
                (EProcess.RemoveFilm, "11. RemoveFilm"),
                (EProcess.GlassTransfer, "12. GlassTransfer"),
                (EProcess.GlassAlignLeft, "13. GlassAlignLeft"),
                (EProcess.GlassAlignRight, "14. GlassAlignRight"),
                (EProcess.TransferInShuttleLeft, "15. TransferInShuttleLeft"),
                (EProcess.TransferInShuttleRight, "16. TransferInShuttleRight"),
                (EProcess.WETCleanLeft, "17. WETCleanLeft"),
                (EProcess.WETCleanRight, "18. WETCleanRight"),
                (EProcess.TransferRotationLeft, "19. TransferRotationLeft"),
                (EProcess.TransferRotationRight, "20. TransferRotationRight"),
                (EProcess.AFCleanLeft, "21. AFCleanLeft"),
                (EProcess.AFCleanRight, "22. AFCleanRight"),
                (EProcess.UnloadTransferLeft, "23. UnloadTransferLeft"),
                (EProcess.UnloadTransferRight, "24. UnloadTransferRight"),
                (EProcess.UnloadAlign, "25. UnloadAlign"),
                (EProcess.RobotUnload, "26. RobotUnload")
            };

            foreach (var (processType, processName) in processes)
            {
                _processTakTime.Add(new ProcessTakTime
                { 
                    ProcessName = processName,
                    CurrentTime = _taktTime.GetCurrentTime(processType) / 1000.0,
                    AverageTime = _taktTime.GetAverageTime(processType) / 1000.0,
                    LastTime = _taktTime.GetLastTime(processType) / 1000.0,
                    TotalTime = _taktTime.GetTotalTime(processType) / 1000.0
                });
            }
        }

        public void ResetTaktTimeAll()
        {
            InitializeProcessItems();
        }

        #region Commands
        public ICommand ResetTaktTimeAllCommand { get; }
        #endregion



    }
}
