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
                //In CST
                EInput.IN_CST_DETECT_1,
                EInput.IN_CST_DETECT_2,
                //Out CST
                EInput.OUT_CST_DETECT_1,
                EInput.OUT_CST_DETECT_2,
                //In Work CST
                EInput.IN_CST_WORK_DETECT_1,
                EInput.IN_CST_WORK_DETECT_2,
                EInput.IN_CST_WORK_DETECT_3,
                EInput.IN_CST_WORK_DETECT_4,
                //Out Work CST
                EInput.OUT_CST_WORK_DETECT_1,
                EInput.OUT_CST_WORK_DETECT_2,
                EInput.OUT_CST_WORK_DETECT_3,
                //Buffer CST
                EInput.BUFFER_CST_DETECT_1,
                EInput.BUFFER_CST_DETECT_2,
                //Vinyl Clean
                EInput.VINYL_CLEAN_FIXTURE_DETECT,
                EInput.VINYL_CLEAN_FULL_DETECT,
                EInput.VINYL_CLEAN_RUNOFF_DETECT,
                //Detach
                EInput.DETACH_FIXTURE_DETECT,
                //Align
                EInput.ALIGN_FIXTURE_DETECT,
                EInput.ALIGN_FIXTURE_TILT_DETECT,
                EInput.ALIGN_FIXTURE_REVERSE_DETECT,
                //Remove
                EInput.REMOVE_ZONE_FULL_TAPE_DETECT,
                EInput.REMOVE_ZONE_FIXTURE_DETECT,
                //Align stage
                EInput.ALIGN_STAGE_L_GLASS_DETECT_1,
                EInput.ALIGN_STAGE_L_GLASS_DETECT_2,
                EInput.ALIGN_STAGE_L_GLASS_DETECT_3,
                EInput.ALIGN_STAGE_R_GLASS_DETECT_1,
                EInput.ALIGN_STAGE_R_GLASS_DETECT_2,
                EInput.ALIGN_STAGE_R_GLASS_DETECT_3,
                //Wet Clean
                EInput.WET_CLEAN_RIGHT_ALCOHOL_PUMP_DETECT,
                EInput.WET_CLEAN_LEFT_ALCOHOL_PUMP_DETECT,
                EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_1,
                EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_2,
                EInput.WET_CLEAN_RIGHT_WIPER_CLEAN_DETECT_3,
                EInput.WET_CLEAN_RIGHT_ALCOHOL_LEAK_DETECT,
                EInput.WET_CLEAN_LEFT_PUMP_LEAK_DETECT,
                EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_1,
                EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_2,
                EInput.WET_CLEAN_LEFT_WIPER_CLEAN_DETECT_3,
                EInput.WET_CLEAN_LEFT_ALCOHOL_LEAK_DETECT,
                EInput.WET_CLEAN_RIGHT_FEEDING_ROLLER_DETECT,
                EInput.WET_CLEAN_LEFT_FEEDING_ROLLER_DETECT,
                EInput.WET_CLEAN_RIGHT_PUMP_LEAK_DETECT,
                //AF Clean
                EInput.AF_CLEAN_RIGHT_ALCOHOL_PUMP_DETECT,
                EInput.AF_CLEAN_LEFT_ALCOHOL_PUMP_DETECT,
                EInput.AF_CLEAN_RIGHT_FEEDING_ROLLER_DETECT,
                EInput.AF_CLEAN_LEFT_FEEDING_ROLLER_DETECT,
                EInput.AF_CLEAN_RIGHT_PUMP_LEAK_DETECT,
                EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_1,
                EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_2,
                EInput.AF_CLEAN_RIGHT_WIPER_CLEAN_DETECT_3,
                EInput.AF_CLEAN_RIGHT_ALCOHOL_LEAK_DETECT,
                EInput.AF_CLEAN_LEFT_PUMP_LEAK_DETECT,
                EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_1,
                EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_2,
                EInput.AF_CLEAN_LEFT_WIPER_CLEAN_DETECT_3,
                EInput.AF_CLEAN_LEFT_ALCOHOL_LEAK_DETECT,
                EInput.OUT_SHUTTLE_GLASS_COATING_DETECT_1,
                EInput.OUT_SHUTTLE_GLASS_COATING_DETECT_2,
                //Unload Stage
                EInput.UNLOAD_GLASS_DETECT_1,
                EInput.UNLOAD_GLASS_DETECT_2,
                EInput.UNLOAD_GLASS_DETECT_3,
                EInput.UNLOAD_GLASS_DETECT_4,
                //Robot Unload Stage
                EInput.UNLOAD_ROBOT_DETECT_1,
                EInput.UNLOAD_ROBOT_DETECT_2,
                EInput.UNLOAD_ROBOT_DETECT_3,
                EInput.UNLOAD_ROBOT_DETECT_4);

            builder.AddGroup(DryRunBypassGroup.SensorVacuum,
                //Transfer In Shuttle
                EInput.TRANSFER_IN_SHUTTLE_L_VAC,
                EInput.TRANSFER_IN_SHUTTLE_R_VAC,
                //Detach
                EInput.DETACH_GLASS_SHT_VAC_1,
                EInput.DETACH_GLASS_SHT_VAC_2,
                EInput.DETACH_GLASS_SHT_VAC_3,
                //Align Stage
                EInput.ALIGN_STAGE_L_VAC_1,
                EInput.ALIGN_STAGE_L_VAC_2,
                EInput.ALIGN_STAGE_L_VAC_3,
                EInput.ALIGN_STAGE_R_VAC_1,
                EInput.ALIGN_STAGE_R_VAC_2,
                EInput.ALIGN_STAGE_R_VAC_3,
                //Glass Transfer
                EInput.GLASS_TRANSFER_VAC_1,
                EInput.GLASS_TRANSFER_VAC_2,
                EInput.GLASS_TRANSFER_VAC_3,
                //Transfer Rotate
                EInput.TR_ROTATE_RIGHT_ROTATE_VAC,
                EInput.TR_ROTATE_LEFT_ROTATE_VAC,
                EInput.TR_ROTATE_RIGHT_VAC_1,
                EInput.TR_ROTATE_RIGHT_VAC_2,
                EInput.TR_ROTATE_LEFT_VAC_1,
                EInput.TR_ROTATE_LEFT_VAC_2,
                //Shuttle
                EInput.OUT_SHUTTLE_R_VAC,
                EInput.OUT_SHUTTLE_L_VAC,
                EInput.IN_SHUTTLE_R_VAC,
                EInput.IN_SHUTTLE_L_VAC,
                //Unload Transfer
                EInput.UNLOAD_TRANSFER_L_VAC,
                EInput.UNLOAD_TRANSFER_R_VAC,
                //Unload Stage
                EInput.UNLOAD_GLASS_ALIGN_VAC_1,
                EInput.UNLOAD_GLASS_ALIGN_VAC_2,
                EInput.UNLOAD_GLASS_ALIGN_VAC_3,
                EInput.UNLOAD_GLASS_ALIGN_VAC_4,
                //Unload Robot
                EInput.UNLOAD_ROBOT_VAC_1,
                EInput.UNLOAD_ROBOT_VAC_2,
                EInput.UNLOAD_ROBOT_VAC_3,
                EInput.UNLOAD_ROBOT_VAC_4);

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

        public bool IsInputInGroup(EInput input, DryRunBypassGroup group)
        {
            if (_groupInputs.TryGetValue(group, out var inputs))
            {
                return inputs.Contains(input);
            }

            return false;
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
