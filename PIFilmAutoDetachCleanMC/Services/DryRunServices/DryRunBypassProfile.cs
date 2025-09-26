using PIFilmAutoDetachCleanMC.Defines;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Services.DryRunServices
{
    public class DryRunBypassProfile
    {
        private readonly Dictionary<DryRunBypassGroup, ReadOnlyCollection<EInput>> _groupInputs;
        private readonly HashSet<DryRunBypassGroup> _enabledGroups = new();
        private readonly HashSet<EInput> _activeInputs = new();

        private DryRunBypassProfile(Dictionary<DryRunBypassGroup, ReadOnlyCollection<EInput>> groupInputs)
        {
            _groupInputs = groupInputs;
        }

        public IReadOnlyCollection<DryRunBypassGroup> EnabledGroups => _enabledGroups;

        public IReadOnlyCollection<EInput> ActiveInputs => _activeInputs;

        public IEnumerable<DryRunBypassGroup> AllGroups => _groupInputs.Keys;

        public static DryRunBypassProfile CreateDefault()
        {
            var builder = new Builder();

            builder.AddGroup(DryRunBypassGroup.SensorDetect,
                EInput.BUFFER_CST_DETECT_1,
                EInput.BUFFER_CST_DETECT_2,
                EInput.IN_CST_DETECT_1,
                EInput.IN_CST_DETECT_2,
                EInput.OUT_CST_DETECT_1,
                EInput.OUT_CST_DETECT_2,
                EInput.IN_CST_WORK_DETECT_1,
                EInput.IN_CST_WORK_DETECT_2,
                EInput.IN_CST_WORK_DETECT_3,
                EInput.IN_CST_WORK_DETECT_4,
                EInput.OUT_CST_WORK_DETECT_1,
                EInput.OUT_CST_WORK_DETECT_2,
                EInput.OUT_CST_WORK_DETECT_3,
                EInput.VINYL_CLEAN_FIXTURE_DETECT,
                EInput.DETACH_FIXTURE_DETECT,
                EInput.ALIGN_FIXTURE_DETECT,
                EInput.ALIGN_FIXTURE_TILT_DETECT,
                EInput.ALIGN_FIXTURE_REVERSE_DETECT,
                EInput.REMOVE_ZONE_FIXTURE_DETECT,
                EInput.ALIGN_STAGE_L_GLASS_DETECT_1,
                EInput.ALIGN_STAGE_L_GLASS_DETECT_2,
                EInput.ALIGN_STAGE_L_GLASS_DETECT_3,
                EInput.WET_CLEAN_RIGHT_ALCOHOL_PUMP_DETECT,
                EInput.WET_CLEAN_LEFT_ALCOHOL_PUMP_DETECT,
                EInput.ALIGN_STAGE_R_GLASS_DETECT_1,
                EInput.ALIGN_STAGE_R_GLASS_DETECT_2,
                EInput.ALIGN_STAGE_R_GLASS_DETECT_3,
                EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_1,
                EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_2,
                EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_3,
                EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_1,
                EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_2,
                EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_3,
                EInput.WET_CLEAN_RIGHT_FEEDING_ROLLER_DETECT,
                EInput.WET_CLEAN_LEFT_FEEDING_ROLLER_DETECT,
                EInput.AF_CLEAN_RIGHT_FEEDING_ROLLER_DETECT,
                EInput.AF_CLEAN_LEFT_FEEDING_ROLLER_DETECT,
                EInput.AF_CLEAN_RIGHT_ALCOHOL_PUMP_DETECT,
                EInput.AF_CLEAN_LEFT_ALCOHOL_PUMP_DETECT,
                EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_1,
                EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_2,
                EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_3,
                EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_1,
                EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_2,
                EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_3,
                EInput.OUT_SHUTTLE_GLASS_COATING_DETECT_1,
                EInput.OUT_SHUTTLE_GLASS_COATING_DETECT_2,
                EInput.UNLOAD_GLASS_DETECT_1,
                EInput.UNLOAD_GLASS_DETECT_2,
                EInput.UNLOAD_GLASS_DETECT_3,
                EInput.UNLOAD_GLASS_DETECT_4,
                EInput.UNLOAD_ROBOT_DETECT_1,
                EInput.UNLOAD_ROBOT_DETECT_2,
                EInput.UNLOAD_ROBOT_DETECT_3,
                EInput.UNLOAD_ROBOT_DETECT_4,
                EInput.VINYL_CLEAN_FULL_DETECT,
                EInput.VINYL_CLEAN_RUNOFF_DETECT,
                EInput.REMOVE_ZONE_FULL_TAPE_DETECT,
                EInput.WET_CLEAN_RIGHT_PUMP_LEAK_DETECT,
                EInput.WET_CLEAN_RIGHT_ALCOHOL_LEAK_DETECT,
                EInput.WET_CLEAN_LEFT_PUMP_LEAK_DETECT,
                EInput.WET_CLEAN_LEFT_ALCOHOL_LEAK_DETECT,
                EInput.AF_CLEAN_RIGHT_PUMP_LEAK_DETECT,
                EInput.AF_CLEAN_RIGHT_ALCOHOL_LEAK_DETECT,
                EInput.AF_CLEAN_LEFT_PUMP_LEAK_DETECT,
                EInput.AF_CLEAN_LEFT_ALCOHOL_LEAK_DETECT);

            var profile = builder.Build();
            profile.SetEnabledGroups(profile.AllGroups);
            return profile;
        }

        public void SetEnabledGroups(params DryRunBypassGroup[] groups)
        {
            SetEnabledGroups((IEnumerable<DryRunBypassGroup>)groups);
        }

        public void SetEnabledGroups(IEnumerable<DryRunBypassGroup> groups)
        {
            _enabledGroups.Clear();
            _activeInputs.Clear();

            foreach (var group in groups.Distinct())
            {
                if (_groupInputs.TryGetValue(group, out var inputs))
                {
                    _enabledGroups.Add(group);

                    foreach (var input in inputs)
                    {
                        _activeInputs.Add(input);
                    }
                }
            }
        }

        public bool ShouldBypass(EInput input)
        {
            return _activeInputs.Contains(input);
        }

        public sealed class Builder
        {
            private readonly Dictionary<DryRunBypassGroup, List<EInput>> _groups = new();

            public void AddGroup(DryRunBypassGroup group, params EInput[] inputs)
            {
                if (!_groups.TryGetValue(group, out var list))
                {
                    list = new List<EInput>();
                    _groups.Add(group, list);
                }

                list.AddRange(inputs);
            }

            public DryRunBypassProfile Build()
            {
                var snapshot = _groups.ToDictionary(
                    pair => pair.Key,
                    pair => new ReadOnlyCollection<EInput>(pair.Value.Distinct().ToList()));

                return new DryRunBypassProfile(snapshot);
            }
        }
    }
}
