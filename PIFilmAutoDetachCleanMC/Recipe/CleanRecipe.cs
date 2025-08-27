using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class CleanRecipe : RecipeBase
    {
		private int unwinderTorque;
        private int winderTorque;
        private double cleanVolume;
        private bool usePort1;
        private bool usePort2;
        private bool usePort3;
        private bool usePort4;
        private bool usePort5;
        private bool usePort6;

        public int UnwinderTorque
		{
			get { return unwinderTorque; }
			set { unwinderTorque = value; }
		}

		public int WinderTorque
		{
			get { return winderTorque; }
			set { winderTorque = value; }
		}

		public double CleanVolume
		{
			get { return cleanVolume; }
			set { cleanVolume = value; }
		}

		public bool UsePort1
		{
			get { return usePort1; }
			set { usePort1 = value; }
		}

		public bool UsePort2
		{
			get { return usePort2; }
			set { usePort2 = value; }
		}

		public bool UsePort3
		{
			get { return usePort3; }
			set { usePort3 = value; }
		}

		public bool UsePort4
		{
			get { return usePort4; }
			set { usePort4 = value; }
		}

        public bool UsePort5
        {
            get { return usePort5; }
            set { usePort5 = value; }
        }

        public bool UsePort6
        {
            get { return usePort6; }
            set { usePort6 = value; }
        }
    }
}
