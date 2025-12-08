using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.InOut;
using static EQX.InOut.VacuumHelpers;

namespace PIFilmAutoDetachCleanMC.Defines.Devices.Vacuum
{
    public class Vacuums
    {
        public Vacuums(IVacuumFactory vacuumFactory, Inputs inputs, Outputs outputs)
        {
            DetachGlassShtVac1 = vacuumFactory
                .Create(outputs.DetachGlassShtVac1OnOff, null, inputs.DetachGlassShtVac1)
                .SetIdentity((int)EVacuum.DetachGlassShtVac1, EVacuum.DetachGlassShtVac1.ToString());

            DetachGlassShtVac2 = vacuumFactory
                .Create(outputs.DetachGlassShtVac2OnOff, null, inputs.DetachGlassShtVac2)
                .SetIdentity((int)EVacuum.DetachGlassShtVac2, EVacuum.DetachGlassShtVac2.ToString());

            DetachGlassShtVac3 = vacuumFactory
                .Create(outputs.DetachGlassShtVac3OnOff, null, inputs.DetachGlassShtVac3)
                .SetIdentity((int)EVacuum.DetachGlassShtVac3, EVacuum.DetachGlassShtVac3.ToString());

            TransferInShuttleLVac = vacuumFactory
                .Create(outputs.TransferInShuttleLVacOnOff, null, inputs.TransferInShuttleLVac)
                .SetIdentity((int)EVacuum.TransferInShuttleLVac, EVacuum.TransferInShuttleLVac.ToString());

            TransferInShuttleRVac = vacuumFactory
                .Create(outputs.TransferInShuttleRVacOnOff, null, inputs.TransferInShuttleRVac)
                .SetIdentity((int)EVacuum.TransferInShuttleRVac, EVacuum.TransferInShuttleRVac.ToString());

            AlignStageLVac1 = vacuumFactory
                .Create(outputs.AlignStageLVac1OnOff, outputs.AlignStageLBlow1OnOff, inputs.AlignStageLVac1)
                .SetIdentity((int)EVacuum.AlignStageLVac1, EVacuum.AlignStageLVac1.ToString());

            AlignStageLVac2 = vacuumFactory
                .Create(outputs.AlignStageLVac2OnOff, outputs.AlignStageLBlow2OnOff, inputs.AlignStageLVac2)
                .SetIdentity((int)EVacuum.AlignStageLVac2, EVacuum.AlignStageLVac2.ToString());

            AlignStageLVac3 = vacuumFactory
                .Create(outputs.AlignStageLVac3OnOff, outputs.AlignStageLBlow3OnOff, inputs.AlignStageLVac3)
                .SetIdentity((int)EVacuum.AlignStageLVac3, EVacuum.AlignStageLVac3.ToString());

            AlignStageRVac1 = vacuumFactory
                .Create(outputs.AlignStageRVac1OnOff, outputs.AlignStageRBlow1OnOff, inputs.AlignStageRVac1)
                .SetIdentity((int)EVacuum.AlignStageRVac1, EVacuum.AlignStageRVac1.ToString());

            AlignStageRVac2 = vacuumFactory
                .Create(outputs.AlignStageRVac2OnOff, outputs.AlignStageRBlow2OnOff, inputs.AlignStageRVac2)
                .SetIdentity((int)EVacuum.AlignStageRVac2, EVacuum.AlignStageRVac2.ToString());

            AlignStageRVac3 = vacuumFactory
                .Create(outputs.AlignStageRVac3OnOff, outputs.AlignStageRBlow3OnOff, inputs.AlignStageRVac3)
                .SetIdentity((int)EVacuum.AlignStageRVac3, EVacuum.AlignStageRVac3.ToString());

            GlassTransferVac1 = vacuumFactory
                .Create(outputs.GlassTransferVac1OnOff, null, inputs.GlassTransferVac1)
                .SetIdentity((int)EVacuum.GlassTransferVac1, EVacuum.GlassTransferVac1.ToString());

            GlassTransferVac2 = vacuumFactory
                .Create(outputs.GlassTransferVac2OnOff, null, inputs.GlassTransferVac2)
                .SetIdentity((int)EVacuum.GlassTransferVac2, EVacuum.GlassTransferVac2.ToString());

            GlassTransferVac3 = vacuumFactory
                .Create(outputs.GlassTransferVac3OnOff, null, inputs.GlassTransferVac3)
                .SetIdentity((int)EVacuum.GlassTransferVac3, EVacuum.GlassTransferVac3.ToString());

            InShuttleRVac = vacuumFactory
                .Create(outputs.InShuttleRVacOnOff, null, inputs.InShuttleRVac)
                .SetIdentity((int)EVacuum.InShuttleRVac, EVacuum.InShuttleRVac.ToString());

            InShuttleLVac = vacuumFactory
                .Create(outputs.InShuttleLVacOnOff, null, inputs.InShuttleLVac)
                .SetIdentity((int)EVacuum.InShuttleLVac, EVacuum.InShuttleLVac.ToString());

            TransferRotationRightVac1 = vacuumFactory
                .Create(outputs.TrRotateRightVac1OnOff, outputs.TransferRotationRightBlow1OnOff, inputs.TrRotateRightVac1)
                .SetIdentity((int)EVacuum.TransferRotationRightVac1, EVacuum.TransferRotationRightVac1.ToString());

            TransferRotationRightVac2 = vacuumFactory
                .Create(outputs.TrRotateRightVac2OnOff, outputs.TransferRotationRightBlow2OnOff, inputs.TrRotateRightVac2)
                .SetIdentity((int)EVacuum.TransferRotationRightVac2, EVacuum.TransferRotationRightVac2.ToString());

            TransferRotationLeftVac1 = vacuumFactory
                .Create(outputs.TrRotateLeftVac1OnOff, outputs.TransferRotationLeftBlow1OnOff, inputs.TrRotateLeftVac1)
                .SetIdentity((int)EVacuum.TransferRotationLeftVac1, EVacuum.TransferRotationLeftVac1.ToString());

            TransferRotationLeftVac2 = vacuumFactory
                .Create(outputs.TrRotateLeftVac2OnOff, outputs.TransferRotationLeftBlow2OnOff, inputs.TrRotateLeftVac2)
                .SetIdentity((int)EVacuum.TransferRotationLeftVac2, EVacuum.TransferRotationLeftVac2.ToString());

            TransferRotationRightRotVac = vacuumFactory
                .Create(outputs.TrRotateRightRotVacOnOff, outputs.TransferRotationRightRotBlowOnOff, inputs.TrRotateRightRotVac)
                .SetIdentity((int)EVacuum.TransferRotationRightRotVac, EVacuum.TransferRotationRightRotVac.ToString());

            TransferRotationLeftRotVac = vacuumFactory
                .Create(outputs.TrRotateLeftRotVacOnOff, outputs.TransferRotationLeftRotBlowOnOff, inputs.TrRotateLeftRotVac)
                .SetIdentity((int)EVacuum.TransferRotationLeftRotVac, EVacuum.TransferRotationLeftRotVac.ToString());

            OutShuttleRVac = vacuumFactory
                .Create(outputs.OutShuttleRVacOnOff, null, inputs.OutShuttleRVac)
                .SetIdentity((int)EVacuum.OutShuttleRVac, EVacuum.OutShuttleRVac.ToString());

            OutShuttleLVac = vacuumFactory
                .Create(outputs.OutShuttleLVacOnOff, null, inputs.OutShuttleLVac)
                .SetIdentity((int)EVacuum.OutShuttleLVac, EVacuum.OutShuttleLVac.ToString());

            UnloadTransferLVac = vacuumFactory
                .Create(outputs.UnloadTransferLVacOnOff, null, inputs.UnloadTransferLVac)
                .SetIdentity((int)EVacuum.UnloadTransferLVac, EVacuum.UnloadTransferLVac.ToString());

            UnloadTransferRVac = vacuumFactory
                .Create(outputs.UnloadTransferRVacOnOff, null, inputs.UnloadTransferRVac)
                .SetIdentity((int)EVacuum.UnloadTransferRVac, EVacuum.UnloadTransferRVac.ToString());

            UnloadGlassAlignVac1 = vacuumFactory
                .Create(outputs.UnloadGlassAlignVac1OnOff, null, inputs.UnloadGlassAlignVac1)
                .SetIdentity((int)EVacuum.UnloadGlassAlignVac1, EVacuum.UnloadGlassAlignVac1.ToString());

            UnloadGlassAlignVac2 = vacuumFactory
                .Create(outputs.UnloadGlassAlignVac2OnOff, null, inputs.UnloadGlassAlignVac2)
                .SetIdentity((int)EVacuum.UnloadGlassAlignVac2, EVacuum.UnloadGlassAlignVac2.ToString());

            UnloadGlassAlignVac3 = vacuumFactory
                .Create(outputs.UnloadGlassAlignVac3OnOff, null, inputs.UnloadGlassAlignVac3)
                .SetIdentity((int)EVacuum.UnloadGlassAlignVac3, EVacuum.UnloadGlassAlignVac3.ToString());

            UnloadGlassAlignVac4 = vacuumFactory
                .Create(outputs.UnloadGlassAlignVac4OnOff, null, inputs.UnloadGlassAlignVac4)
                .SetIdentity((int)EVacuum.UnloadGlassAlignVac4, EVacuum.UnloadGlassAlignVac4.ToString());

            UnloadRobotVac1 = vacuumFactory
                .Create(outputs.UnloadRobotVac1OnOff, null, inputs.UnloadRobotVac1)
                .SetIdentity((int)EVacuum.UnloadRobotVac1, EVacuum.UnloadRobotVac1.ToString());

            UnloadRobotVac2 = vacuumFactory
                .Create(outputs.UnloadRobotVac2OnOff, null, inputs.UnloadRobotVac2)
                .SetIdentity((int)EVacuum.UnloadRobotVac2, EVacuum.UnloadRobotVac2.ToString());

            UnloadRobotVac3 = vacuumFactory
                .Create(outputs.UnloadRobotVac3OnOff, null, inputs.UnloadRobotVac3)
                .SetIdentity((int)EVacuum.UnloadRobotVac3, EVacuum.UnloadRobotVac3.ToString());

            UnloadRobotVac4 = vacuumFactory
                .Create(outputs.UnloadRobotVac4OnOff, null, inputs.UnloadRobotVac4)
                .SetIdentity((int)EVacuum.UnloadRobotVac4, EVacuum.UnloadRobotVac4.ToString());

            All = new[]
            {
                DetachGlassShtVac1,
                DetachGlassShtVac2,
                DetachGlassShtVac3,
                TransferInShuttleLVac,
                TransferInShuttleRVac,
                AlignStageLVac1,
                AlignStageLVac2,
                AlignStageLVac3,
                AlignStageRVac1,
                AlignStageRVac2,
                AlignStageRVac3,
                GlassTransferVac1,
                GlassTransferVac2,
                GlassTransferVac3,
                InShuttleRVac,
                InShuttleLVac,
                TransferRotationRightVac1,
                TransferRotationRightVac2,
                TransferRotationLeftVac1,
                TransferRotationLeftVac2,
                TransferRotationRightRotVac,
                TransferRotationLeftRotVac,
                OutShuttleRVac,
                OutShuttleLVac,
                UnloadTransferLVac,
                UnloadTransferRVac,
                UnloadGlassAlignVac1,
                UnloadGlassAlignVac2,
                UnloadGlassAlignVac3,
                UnloadGlassAlignVac4,
                UnloadRobotVac1,
                UnloadRobotVac2,
                UnloadRobotVac3,
                UnloadRobotVac4
            };
        }

        public IVacuum DetachGlassShtVac1 { get; }
        public IVacuum DetachGlassShtVac2 { get; }
        public IVacuum DetachGlassShtVac3 { get; }
        public IVacuum TransferInShuttleLVac { get; }
        public IVacuum TransferInShuttleRVac { get; }
        public IVacuum AlignStageLVac1 { get; }
        public IVacuum AlignStageLVac2 { get; }
        public IVacuum AlignStageLVac3 { get; }
        public IVacuum AlignStageRVac1 { get; }
        public IVacuum AlignStageRVac2 { get; }
        public IVacuum AlignStageRVac3 { get; }
        public IVacuum GlassTransferVac1 { get; }
        public IVacuum GlassTransferVac2 { get; }
        public IVacuum GlassTransferVac3 { get; }
        public IVacuum InShuttleRVac { get; }
        public IVacuum InShuttleLVac { get; }
        public IVacuum TransferRotationRightVac1 { get; }
        public IVacuum TransferRotationRightVac2 { get; }
        public IVacuum TransferRotationLeftVac1 { get; }
        public IVacuum TransferRotationLeftVac2 { get; }
        public IVacuum TransferRotationRightRotVac { get; }
        public IVacuum TransferRotationLeftRotVac { get; }
        public IVacuum OutShuttleRVac { get; }
        public IVacuum OutShuttleLVac { get; }
        public IVacuum UnloadTransferLVac { get; }
        public IVacuum UnloadTransferRVac { get; }
        public IVacuum UnloadGlassAlignVac1 { get; }
        public IVacuum UnloadGlassAlignVac2 { get; }
        public IVacuum UnloadGlassAlignVac3 { get; }
        public IVacuum UnloadGlassAlignVac4 { get; }
        public IVacuum UnloadRobotVac1 { get; }
        public IVacuum UnloadRobotVac2 { get; }
        public IVacuum UnloadRobotVac3 { get; }
        public IVacuum UnloadRobotVac4 { get; }

        public IReadOnlyList<IVacuum> All { get; }
    }
}
