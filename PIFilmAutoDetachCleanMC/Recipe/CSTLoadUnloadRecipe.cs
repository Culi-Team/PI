using EQX.Core.Recipe;
using EQX.Core.Units;
using Newtonsoft.Json;
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
        private double pitch;
        private double inCstTAxisLoadPosition;
        private double inCstTAxisWorkPosition;
        private double outCstTAxisLoadPosition;
        private double outCstTAxisWorkPosition;

		[JsonIgnore]
		public EventHandler CassetteSizeChanged;

        [SingleRecipeDescription(Description = "Conveyor Speed", Unit = Unit.RevolutionsPerMinute)]
        [SingleRecipeMinMax(Max = 5000, Min = 200)]
        public uint ConveyorSpeed
		{
			get { return conveyorSpeed; }
			set 
			{
                OnRecipeChanged(conveyorSpeed, value);
                conveyorSpeed = value; 
			}
		}

        [SingleRecipeDescription(Description = "Conveyor Acceleration", Unit = Unit.RevolutionsPerMinutePerSecond)]
        [SingleRecipeMinMax(Max = 30000, Min = 500)]
        public uint ConveyorAcc
		{
			get { return conveyorAcc; }
			set 
			{
                OnRecipeChanged(conveyorAcc, value);
                conveyorAcc = value; 
			}
		}

        [SingleRecipeDescription(Description = "Conveyor Deceleration", Unit = Unit.RevolutionsPerMinutePerSecond)]
        [SingleRecipeMinMax(Max = 30000, Min = 500)]
        public uint ConveyorDec
		{
			get { return conveyorDec; }
			set 
			{
                OnRecipeChanged(conveyorDec, value);
                conveyorDec = value; 
			}
		}

        [SingleRecipeDescription(Description = "Cassette Rows", Unit = Unit.ETC)]
        [SingleRecipeMinMax(Max = 20, Min = 1)]
        public int CasetteRows
        {
			get { return casetteRows; }
			set 
			{
                OnRecipeChanged(casetteRows, value);
                casetteRows = value; 

				CassetteSizeChanged?.Invoke(this, EventArgs.Empty);
            }
		}

        [SingleRecipeDescription(Description = "Cassette Pitch", Unit = Unit.mm)]
        [SingleRecipeMinMax(Max = 50.0, Min = 0.0)]
        public double Pitch
		{
			get { return pitch; }
			set 
			{
                OnRecipeChanged(pitch, value);
                pitch = value; 
			}
		}

        [SingleRecipeDescription(Description = "In Cassette T Axis Load Position", Unit = Unit.mm)]
		[SinglePositionTeaching(Motion = "TAxis")]
        public double InCstTAxisLoadPosition
		{
			get { return inCstTAxisLoadPosition; }
			set 
			{
                OnRecipeChanged(inCstTAxisLoadPosition, value);
                inCstTAxisLoadPosition = value; 
			}
		}

        [SingleRecipeDescription(Description = "In Cassette T Axis Work Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "TAxis")]
        public double InCstTAxisWorkPosition
		{
			get { return inCstTAxisWorkPosition; }
			set 
			{
                OnRecipeChanged(inCstTAxisWorkPosition, value);
                inCstTAxisWorkPosition = value; 
			}
		}

        [SingleRecipeDescription(Description = "Out Cassette T Axis Load Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "TAxis")]
        public double OutCstTAxisLoadPosition
		{
			get { return outCstTAxisLoadPosition; }
			set 
			{
                OnRecipeChanged(outCstTAxisLoadPosition, value);
                outCstTAxisLoadPosition = value; 
			}
		}

        [SingleRecipeDescription(Description = "Out Cassette T Axis Work Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "TAxis")]
        public double OutCstTAxisWorkPosition
		{
			get { return outCstTAxisWorkPosition; }
			set 
			{
                OnRecipeChanged(outCstTAxisWorkPosition, value);
                outCstTAxisWorkPosition = value; 
			}
		}
	}
}
