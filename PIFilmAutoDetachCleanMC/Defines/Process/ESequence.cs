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
        
        InConveyorLoad,
        InWorkCSTLoad,
        InWorkCSTUnLoad,
        CSTTilt,
        OutWorkCSTLoad,
        OutWorkCSTUnLoad,
        OutConveyorUnload,

        RobotPickFixtureFromCST,
        RobotPlaceFixtureToVinylClean,
        VinylClean,
        RobotPickFixtureFromVinylClean,
        RobotPlaceFixtureToAlign,
        FixtureAlign,
        RobotPickFixtureFromRemoveZone,
        RobotPlaceFixtureToOutWorkCST,

        TransferFixtureLoad,
        Detach,
        TransferFixtureUnload,
        DetachUnload,
        RemoveFilm,
        RemoveFilmThrow,

        GlassTransferPick,
        GlassTransferPlace,

        AlignGlass,

        TransferInShuttlePick,

        WETCleanLoad,
        WETClean,
        WETCleanUnload,

        TransferRotation,

        AFCleanLoad,
        AFClean,
        AFCleanUnload,

        UnloadTransferPlace,

        UnloadAlignGlass,

        UnloadRobotPick,
        UnloadRobotPlasma,
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
