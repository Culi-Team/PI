using EQX.Core.Recipe;
using EQX.Core.Units;
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
        private int casetteRows;
        private uint pitch;
        private double inCstTAxisLoadPosition;
        private double inCstTAxisWorkPosition;
        private double outCstTAxisLoadPosition;
        private double outCstTAxisWorkPosition;

        [SingleRecipeDescription(Description = "Conveyor Speed", Unit = Unit.RevolutionsPerMinute)]
        [SingleRecipeMinMax(Max = 5000, Min = 200)]
        public uint ConveyorSpeed
		{
			get { return conveyorSpeed; }
			set { conveyorSpeed = value; }
		}

        [SingleRecipeDescription(Description = "Conveyor Acceleration", Unit = Unit.RevolutionsPerMinutePerSecond)]
        [SingleRecipeMinMax(Max = 30000, Min = 500)]
        public uint ConveyorAcc
		{
			get { return conveyorAcc; }
			set { conveyorAcc = value; }
		}

        [SingleRecipeDescription(Description = "Conveyor Deceleration", Unit = Unit.RevolutionsPerMinutePerSecond)]
        [SingleRecipeMinMax(Max = 30000, Min = 500)]
        public uint ConveyorDec
		{
			get { return conveyorDec; }
			set { conveyorDec = value; }
		}

        [SingleRecipeDescription(Description = "Cassette Rows", Unit = Unit.ETC)]
        [SingleRecipeMinMax(Max = 20, Min = 1)]
        public int CasetteRows
        {
			get { return casetteRows; }
			set { casetteRows = value; }
		}

        [SingleRecipeDescription(Description = "Cassette Pitch", Unit = Unit.mm)]
        [SingleRecipeMinMax(Max = 50.0, Min = 0.0)]
        public uint Pitch
		{
			get { return pitch; }
			set { pitch = value; }
		}

        [SingleRecipeDescription(Description = "In Cassette T Axis Load Position", Unit = Unit.mm)]
		[SinglePositionTeaching(Motion = "T Axis")]
        public double InCstTAxisLoadPosition
		{
			get { return inCstTAxisLoadPosition; }
			set { inCstTAxisLoadPosition = value; }
		}

        [SingleRecipeDescription(Description = "In Cassette T Axis Work Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "T Axis")]
        public double InCstTAxisWorkPosition
		{
			get { return inCstTAxisWorkPosition; }
			set { inCstTAxisWorkPosition = value; }
		}

        [SingleRecipeDescription(Description = "Out Cassette T Axis Load Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "T Axis")]
        public double OutCstTAxisLoadPosition
		{
			get { return outCstTAxisLoadPosition; }
			set { outCstTAxisLoadPosition = value; }
		}

        [SingleRecipeDescription(Description = "Out Cassette T Axis Work Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "T Axis")]
        public double OutCstTAxisWorkPosition
		{
			get { return outCstTAxisWorkPosition; }
			set { outCstTAxisWorkPosition = value; }
		}
	}
}
