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
                return Left + Right;
            }
        }

        public uint Left
        {
            get { return _left; }
            set
            {
                if (_left == value) return;

                _left = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total));
            }
        }
        public uint Right
        {
            get { return _right; }
            set
            {
                if (_right == value) return;

                _right = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total));
            }
        }

        #endregion

        public void Reset()
        {
            Left = 0;
            Right = 0;
        }

        #region Privates
        private uint _left = 0;
        private uint _right = 0;
        #endregion
    }
}
