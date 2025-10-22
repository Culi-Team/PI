using System.ComponentModel;

namespace PIFilmAutoDetachCleanMC.Process
{
    public enum EProcess
    {
        Root,
        InConveyor,
        InWorkConveyor,
        BufferConveyor,
        OutWorkConveyor,
        OutConveyor,
        RobotLoad,
        VinylClean,
        FixtureAlign,
        TransferFixture,
        Detach,
        RemoveFilm,
        GlassTransfer,
        TransferInShuttleLeft,
        TransferInShuttleRight,
        WETCleanLeft,
        WETCleanRight,
        TransferRotationLeft,
        TransferRotationRight,
        AFCleanLeft,
        AFCleanRight,
        UnloadTransferLeft,
        UnloadTransferRight,
        UnloadAlign,
        RobotUnload,
    }
}
