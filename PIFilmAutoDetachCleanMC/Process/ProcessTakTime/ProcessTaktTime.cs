using EQX.Core.Sequence;
using PIFilmAutoDetachCleanMC.Defines;
using System.Collections.Concurrent;
using EQX.Core.Process;

namespace PIFilmAutoDetachCleanMC.Process
{
    public class ProcessTaktTime
    {
        #region Private Fields
        private readonly ConcurrentDictionary<EProcess, ProcessTaktTimeInfo> _processes = new();
        private readonly ConcurrentDictionary<EProcess, DateTime> _startTimes = new();
        #endregion

        #region Public Methods
        public void StartProcess(EProcess processType)
        {
            _startTimes[processType] = DateTime.Now;
        }

        public void StopProcess(EProcess processType)
        {
            if (!_startTimes.TryRemove(processType, out var startTime)) return;

            var executionTime = (DateTime.Now - startTime).TotalMilliseconds;
            
            _processes.AddOrUpdate(processType, 
                new ProcessTaktTimeInfo { TotalTime = executionTime, Count = 1, LastTime = executionTime },
                (key, existing) => new ProcessTaktTimeInfo
                { 
                    TotalTime = existing.TotalTime + executionTime, 
                    Count = existing.Count + 1,
                    LastTime = executionTime
                });
        }

        public double GetAverageTime(EProcess processType)
        {
            return _processes.TryGetValue(processType, out var info) && info.Count > 0 
                ? info.TotalTime / info.Count 
                : 0;
        }

        public double GetTotalTime(EProcess processType)
        {
            return _processes.TryGetValue(processType, out var info) ? info.TotalTime : 0;
        }

        public int GetRunCount(EProcess processType)
        {
            return _processes.TryGetValue(processType, out var info) ? info.Count : 0;
        }

        public double GetCurrentTime(EProcess processType)
        {
            return _startTimes.TryGetValue(processType, out var startTime) 
                ? (DateTime.Now - startTime).TotalMilliseconds 
                : 0;
        }

        public double GetLastTime(EProcess processType)
        {
            return _processes.TryGetValue(processType, out var info) ? info.LastTime : 0;
        }

        public bool IsProcessRunning(EProcess processType)
        {
            return _startTimes.ContainsKey(processType);
        }

        public Dictionary<EProcess, ProcessTaktTimeInfo> GetAllProcesses()
        {
            return _processes.ToDictionary(kvp => kvp.Key, kvp => new ProcessTaktTimeInfo 
            { 
                TotalTime = kvp.Value.TotalTime, 
                Count = kvp.Value.Count, 
                LastTime = kvp.Value.LastTime 
            });
        }

        public void Initialize(IEnumerable<IProcess<ESequence>> processes)
        {
            foreach (var process in processes)
            {
                if (Enum.TryParse<EProcess>(process.Name, out var processType))
                {
                    process.ProcessModeUpdated += (sender, e) => OnProcessModeChanged(processType, process);
                }
            }
        }

        private void OnProcessModeChanged(EProcess processType, IProcess<ESequence> process)
        {
            switch (process.ProcessMode)
            {
                case EProcessMode.Run:
                    StartProcess(processType);
                    break;
                case EProcessMode.Stop:
                case EProcessMode.ToStop:
                    StopProcess(processType);
                    break;
            }
        }

        public void Reset()
        {
            _processes.Clear();
            _startTimes.Clear();
        }

        public double GetMaxTaktTime()
        {
            return _processes.IsEmpty ? 0 : _processes.Values.Max(p => p.TotalTime);
        }

        public double GetMinTaktTime()
        {
            return _processes.IsEmpty ? 0 : _processes.Values.Min(p => p.TotalTime);
        }

        public double GetAverageTaktTimeAll()
        {
            if (_processes.IsEmpty) return 0;
            
            var totalTime = _processes.Values.Sum(p => p.TotalTime);
            var totalCount = _processes.Values.Sum(p => p.Count);
            
            return totalCount > 0 ? totalTime / totalCount : 0;
        }

        public ProcessTaktTimeInfo GetMachineTaktTimeInfo()
        {
            return new ProcessTaktTimeInfo
            {
                MaxTaktTime = GetMaxTaktTime(),
                MinTaktTime = GetMinTaktTime(),
                AverageTaktTime = GetAverageTaktTimeAll(),
                TotalProcesses = _processes.Count,
                TotalRuns = _processes.Values.Sum(p => p.Count)
            };
        }
        #endregion

        #region Public Class
        public class ProcessTaktTimeInfo
        {
            public double TotalTime { get; set; }
            public int Count { get; set; }
            public double LastTime { get; set; }
            public double MaxTaktTime { get; set; }
            public double MinTaktTime { get; set; }
            public double AverageTaktTime { get; set; }
            public int TotalProcesses { get; set; }
            public int TotalRuns { get; set; }
        }
        #endregion
    }
}
