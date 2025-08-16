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

        Load,

        Pick,
        Inspect,
        Place,
        PlaceNG,

        Unload,
    }

    public enum ESemiSequence
    {
        None,

        Load,

        Pick,
        Inspect,
        Place,
        PlaceNG,

        Unload,
    }
}
