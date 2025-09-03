using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.LogHistory
{
    public class LogEntry
    {
        public string Time { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
    }
}
