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
        [Description("In Work CST Tilt")]
        InWorkCSTTilt,
        [Description("Out Work CST Load")]
        OutWorkCSTLoad,
        [Description("Out Work CST Unload")]
        OutWorkCSTUnLoad,
        [Description("Out Conveyor Unload")]
        OutConveyorUnload,
        [Description("Out Work CST Tilt")]
        OutWorkCSTTilt,

        [Description("Pick From CST")]
        RobotPickFixtureFromCST,
        [Description("Place To Vinyl Clean")]
        RobotPlaceFixtureToVinylClean,
        [Description("Vinyl Clean")]
        VinylClean,
        [Description("Pick From Vinyl Clean")]
        RobotPickFixtureFromVinylClean,
        [Description("Place To Align")]
        RobotPlaceFixtureToAlign,
        [Description("Fixture Align")]
        FixtureAlign,
        [Description("Pick From Remove Zone")]
        RobotPickFixtureFromRemoveZone,
        [Description("Place To Out CST")]
        RobotPlaceFixtureToOutWorkCST,

        [Description("Transfer Fixture")]
        TransferFixture,
        [Description("Detach")]
        Detach,
        [Description("Detach Unload")]
        DetachUnload,
        [Description("Remove Film")]
        RemoveFilm,

        [Description("Glass Transfer Pick")]
        GlassTransferPick,
        [Description("Glass Transfer Place")]
        GlassTransferPlace,

        [Description("Glass Align")]
        AlignGlassLeft,
        [Description("Glass Align")]
        AlignGlassRight,

        [Description("Transfer In Shuttle Pick")]
        TransferInShuttleLeftPick,
        [Description("Transfer In Shuttle Pick")]
        TransferInShuttleRightPick,

        [Description("WET Clean Load")]
        WETCleanLeftLoad,
        [Description("WET Clean Load")]
        WETCleanRightLoad,
        [Description("WET Clean")]
        WETCleanLeft,
        [Description("WET Clean")]
        WETCleanRight,
        [Description("WET Clean Unload")]
        WETCleanLeftUnload,
        [Description("WET Clean Unload")]
        WETCleanRightUnload,

        [Description("Transfer Rotation")]
        TransferRotationLeft,
        [Description("Transfer Rotation")]
        TransferRotationRight,

        [Description("AF Clean Load")]
        AFCleanLeftLoad,
        [Description("AF Clean Load")]
        AFCleanRightLoad,
        [Description("AF Clean")]
        AFCleanLeft,
        [Description("AF Clean")]
        AFCleanRight,
        [Description("AF Clean Unload")]
        AFCleanLeftUnload,
        [Description("AF Clean Unload")]
        AFCleanRightUnload,

        [Description("Unload Transfer Place")]
        UnloadTransferLeftPlace,
        [Description("Unload Transfer Place")]
        UnloadTransferRightPlace,

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

        [Description("In Conveyor Load")]
        InConveyorLoad,
        [Description("In Work CST Load")]
        InWorkCSTLoad,
        [Description("In Work CST Unload")]
        InWorkCSTUnLoad,
        [Description("In Work CST Tilt")]
        InWorkCSTTilt,
        [Description("Out Work CST Load")]
        OutWorkCSTLoad,
        [Description("Out Work CST Unload")]
        OutWorkCSTUnLoad,
        [Description("Out Conveyor Unload")]
        OutConveyorUnload,
        [Description("Out Work CST Tilt")]
        OutWorkCSTTilt,

        [Description("Pick From CST")]
        RobotPickFixtureFromCST,
        [Description("Place To Vinyl Clean")]
        RobotPlaceFixtureToVinylClean,
        [Description("Vinyl Clean")]
        VinylClean,
        [Description("Pick From Vinyl Clean")]
        RobotPickFixtureFromVinylClean,
        [Description("Place To Align")]
        RobotPlaceFixtureToAlign,
        [Description("Fixture Align")]
        FixtureAlign,
        [Description("Pick From Remove Zone")]
        RobotPickFixtureFromRemoveZone,
        [Description("Place To Out CST")]
        RobotPlaceFixtureToOutWorkCST,

        [Description("Transfer Fixture")]
        TransferFixture,
        [Description("Detach")]
        Detach,
        [Description("Detach Unload")]
        DetachUnload,
        [Description("Remove Film")]
        RemoveFilm,

        [Description("Glass Transfer Pick")]
        GlassTransferPick,
        [Description("Glass Transfer Place")]
        GlassTransferPlace,

        [Description("Glass Align")]
        AlignGlassLeft,
        [Description("Glass Align")]
        AlignGlassRight,

        [Description("Transfer In Shuttle Pick")]
        TransferInShuttleLeftPick,
        [Description("Transfer In Shuttle Pick")]
        TransferInShuttleRightPick,

        [Description("WET Clean Load")]
        WETCleanLeftLoad,
        [Description("WET Clean Load")]
        WETCleanRightLoad,
        [Description("WET Clean")]
        WETCleanLeft,
        [Description("WET Clean")]
        WETCleanRight,
        [Description("WET Clean Unload")]
        WETCleanLeftUnload,
        [Description("WET Clean Unload")]
        WETCleanRightUnload,

        [Description("Transfer Rotation")]
        TransferRotationLeft,
        [Description("Transfer Rotation")]
        TransferRotationRight,

        [Description("AF Clean Load")]
        AFCleanLeftLoad,
        [Description("AF Clean Load")]
        AFCleanRightLoad,
        [Description("AF Clean")]
        AFCleanLeft,
        [Description("AF Clean")]
        AFCleanRight,
        [Description("AF Clean Unload")]
        AFCleanLeftUnload,
        [Description("AF Clean Unload")]
        AFCleanRightUnload,

        [Description("Unload Transfer Place")]
        UnloadTransferLeftPlace,
        [Description("Unload Transfer Place")]
        UnloadTransferRightPlace,

        [Description("Unload Align")]
        UnloadAlignGlass,

        [Description("Robot Pick")]
        UnloadRobotPick,
        [Description("Plasma")]
        UnloadRobotPlasma,
        [Description("Robot Place")]
        UnloadRobotPlace,
    }
}
