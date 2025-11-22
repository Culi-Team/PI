using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public enum ETrayCellStatus
    {
        Ready = 0,

        /// <summary>
        /// There is no material in the cell
        /// </summary>
        Skip,
        /// <summary>
        /// To-Pick cell
        /// </summary>
        Working,
        /// <summary>
        /// Cell may placed or picked, need result update (auto or manual)
        /// </summary>
        NeedConfirm,
        /// <summary>
        /// Pick done, material have been removed successed
        /// </summary>
        Done,
        /// <summary>
        /// Pick done with fail
        /// </summary>
        PickPlaceFail,

        Exist,

        Empty,
    }
}
