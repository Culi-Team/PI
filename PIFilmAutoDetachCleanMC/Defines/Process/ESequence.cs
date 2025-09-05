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

        InWorkCSTLoad,
        InWorkCSTUnLoad,

        CSTTilt,
        CSTUnTilt,

        OutWorkCSTLoad,
        OutWorkCSTUnLoad,

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
