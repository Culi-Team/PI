using System.ComponentModel;

namespace PIFilmAutoDetachCleanMC.Defines
{
    /// <summary>
    /// Sequences of the machine
    /// </summary>
    public enum ESequence
    {
        Stop,

        AutoRun,
        Ready,

        [Description("In Conveyor Load")]
        InConveyorLoad,
        [Description("In Work CST Load")]
        InWorkCSTLoad,
        [Description("In Work CST Unload")]
        InWorkCSTUnLoad,
        [Description("CST Tilt")]
        CSTTilt,
        [Description("Out Work CST Load")]
        OutWorkCSTLoad,
        [Description("Out Work CST Unload")]
        OutWorkCSTUnLoad,
        [Description("Out Conveyor Unload")]
        OutConveyorUnload,

        [Description("Robot Pick")]
        RobotPickFixtureFromCST,
        [Description("Robot Place")]
        RobotPlaceFixtureToVinylClean,
        [Description("Vinyl Clean")]
        VinylClean,
        [Description("Robot Pick")]
        RobotPickFixtureFromVinylClean,
        [Description("Robot Place")]
        RobotPlaceFixtureToAlign,
        [Description("Fixture Align")]
        FixtureAlign,
        [Description("Robot Pick")]
        RobotPickFixtureFromRemoveZone,
        [Description("Robot Place")]
        RobotPlaceFixtureToOutWorkCST,

        [Description("Transfer Fixture Load")]
        TransferFixtureLoad,
        [Description("Detach")]
        Detach,
        [Description("Transfer Fixture Unload")]
        TransferFixtureUnload,
        [Description("Detach Unload")]
        DetachUnload,
        [Description("Remove Film")]
        RemoveFilm,
        [Description("Remove Film Throw")]
        RemoveFilmThrow,

        [Description("Glass Transfer Pick")]
        GlassTransferPick,
        [Description("Glass Transfer Place")]
        GlassTransferPlace,

        [Description("Glass Align")]
        AlignGlass,

        [Description("Transfer In Shuttle Pick")]
        TransferInShuttlePick,

        [Description("WET Clean Load")]
        WETCleanLoad,
        [Description("WET Clean")]
        WETClean,
        [Description("WET Clean Unload")]
        WETCleanUnload,

        [Description("Transfer Rotation")]
        TransferRotation,

        [Description("AF Clean Load")]
        AFCleanLoad,
        [Description("AF Clean")]
        AFClean,
        [Description("AF Clean Unload")]
        AFCleanUnload,

        [Description("Unload Transfer Place")]
        UnloadTransferPlace,

        [Description("Unload Align")]
        UnloadAlignGlass,

        [Description("Robot Pick")]
        UnloadRobotPick,
        [Description("Plasma")]
        UnloadRobotPlasma,
        [Description("Robot Place")]
        UnloadRobotPlace,
    }

    public enum ESemiSequence
    {
        None,

        InWorkCSTLoad,
        InWorkCSTUnLoad,

        OutWorkCSTLoad,
        OutWorkCSTUnLoad,

        RobotPickFixtureFromCST,
        RobotPlaceFixtureToVinylClean,
        RobotPickFixtureFromVinylClean,
        RobotPlaceFixtureToAlign,
        RobotPickFixtureFromRemoveZone,
        RobotPlaceFixtureToOutWorkCST,

        FixtureTransfer,
        Detach,
        DetachUnload,
        RemoveFilm,

        GlassTransferPick,
        GlassTransferPlace,

        AlignGlass,

        TransferInShuttlePick,
        TransferInShuttlePlace,

        WETCleanLoad,
        WETClean,
        WETCleanUnload,

        TransferRotationPick,
        TransferRotationPlace,

        AFCleanLoad,
        AFClean,
        AFCleanUnload,

        UnloadTransferPick,
        UnloadTransferPlace,

        UnloadAlignGlass,

        UnloadRobotPick,
        UnloadRobotPlasma,
        UnloadRobotPlace,
    }
}
