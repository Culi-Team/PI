using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ERobotLoadAutoRunStep
    {
        Start,
        Check_Flag_VinylCleanRequestFixture,
        Check_Flag_RemoveFilm,
        Check_Flag_VinylCleanRequestUnload,
        End
    }
}
