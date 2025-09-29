using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.ProductDatas
{
    public class CCountData : ObservableObject
    {
        #region Properties
        public uint Total
        {
            get
            {
                return OK;
            }
        }

        public uint OK
        {
            get { return _OK; }
            set
            {
                if (_OK == value) return;

                _OK = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total));
            }
        }

        #endregion

        public void Reset()
        {
            OK = 0;
        }

        #region Privates
        private uint _OK = 0;
        #endregion
    }
}
