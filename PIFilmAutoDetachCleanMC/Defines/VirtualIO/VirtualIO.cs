using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class VirtualIO<TEnum> where TEnum : Enum
    {
        private ConcurrentDictionary<TEnum, bool> Flags = new ConcurrentDictionary<TEnum, bool>();

        public void Initialize()
        {
            foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
            {
                Flags[value] = false; // Initialize all flags to false
            }
        }

        public void SetFlag(TEnum flag, bool value) => Flags[flag] = value;
        public bool GetFlag(TEnum flag) => Flags.TryGetValue(flag, out var val) && val;

    }
}
