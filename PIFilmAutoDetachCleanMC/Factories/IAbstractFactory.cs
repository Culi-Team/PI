using EQX.Core.Motion;
using EQX.Motion;

namespace PIFilmAutoDetachCleanMC.Factories
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }
}
