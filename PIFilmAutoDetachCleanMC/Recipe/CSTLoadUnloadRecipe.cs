using EQX.Core.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class CSTLoadUnloadRecipe : RecipeBase
    {
		private uint conveyorSpeed;
        private uint conveyorAcc;
        private uint conveyorDec;
        private uint casetteRow;
        private uint pitch;
        private double inCstTAxisLoadPosition;
        private double inCstTAxisWorkPosition;
        private double outCstTAxisLoadPositon;
        private double outCstTAxisWorkPosition;

        public uint ConveyorSpeed
		{
			get { return conveyorSpeed; }
			set { conveyorSpeed = value; }
		}

		public uint ConveyorAcc
		{
			get { return conveyorAcc; }
			set { conveyorAcc = value; }
		}

		public uint ConveyorDec
		{
			get { return conveyorDec; }
			set { conveyorDec = value; }
		}

		public uint CasetteRow
		{
			get { return casetteRow; }
			set { casetteRow = value; }
		}

		public uint Pitch
		{
			get { return pitch; }
			set { pitch = value; }
		}

		public double InCstTAxisLoadPosition
		{
			get { return inCstTAxisLoadPosition; }
			set { inCstTAxisLoadPosition = value; }
		}

		public double InCstTAxisWorkPosition
		{
			get { return inCstTAxisWorkPosition; }
			set { inCstTAxisWorkPosition = value; }
		}

		public double OutCstTAxisLoadPosition
		{
			get { return outCstTAxisLoadPositon; }
			set { outCstTAxisLoadPositon = value; }
		}

		public double OutCstTAxisWorkPosition
		{
			get { return outCstTAxisWorkPosition; }
			set { outCstTAxisWorkPosition = value; }
		}
	}
}
