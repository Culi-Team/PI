using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Recipe;
using EQX.Core.Units;
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
        private double cylinderPushPressure;
        private bool usePort1;
        private bool usePort2;
        private bool usePort3;
        private bool usePort4;
        private bool usePort5;
        private bool usePort6;
        private bool isCleanVertical;

        private double xAxisLoadPosition;
        private double yAxisLoadPosition;
        private double tAxisLoadPosition;

        private double xAxisUnloadPosition;
        private double yAxisUnloadPosition;
        private double tAxisUnloadPosition;


        [SingleRecipeDescription(Description = "UnWinder Torque", Unit = Unit.Percentage)]
        [SingleRecipeMinMax(Max = 100.0, Min = 0.0)]
        public int UnwinderTorque
		{
			get { return unwinderTorque; }
			set { unwinderTorque = value; }
		}

        [SingleRecipeDescription(Description = "Winder Torque", Unit = Unit.Percentage)]
        [SingleRecipeMinMax(Max = 100.0, Min = 0.0)]
        public int WinderTorque
		{
			get { return winderTorque; }
			set { winderTorque = value; }
		}

        [SingleRecipeDescription(Description = "Clean Volume Per Nozzle", Unit = Unit.Milliliter)]
        [SingleRecipeMinMax(Max = 100.0, Min = 0.0)]
        public double CleanVolume
		{
			get { return cleanVolume; }
			set { cleanVolume = value; }
		}

        [SingleRecipeDescription(Description = "Cylinder Push Pressure", Unit = Unit.MilliPascal)]
        [SingleRecipeMinMax(Max = 0.9, Min = 0)]
        public double CylinderPushPressure
        {
            get { return cylinderPushPressure; }
            set { cylinderPushPressure = value; }
        }

        [SingleRecipeDescription(Description = "X Axis Load Position", Unit = Unit.mm)]
        public double XAxisLoadPosition
        {
            get { return xAxisLoadPosition; }
            set { xAxisLoadPosition = value; }
        }

        [SingleRecipeDescription(Description = "Y Axis Load Position", Unit = Unit.mm)]
        public double YAxisLoadPosition
        {
            get { return yAxisLoadPosition; }
            set { yAxisLoadPosition = value; }
        }

        [SingleRecipeDescription(Description = "T Axis Load Position", Unit = Unit.mm)]
        public double TAxisLoadPosition
        {
            get { return tAxisLoadPosition; }
            set { tAxisLoadPosition = value; }
        }

        [SingleRecipeDescription(Description = "X Axis Unload Position", Unit = Unit.mm)]
        public double XAxisUnloadPosition
        {
            get { return xAxisUnloadPosition; }
            set { xAxisUnloadPosition = value; }
        }

        [SingleRecipeDescription(Description = "Y Axis Unload Position", Unit = Unit.mm)]
        public double YAxisUnloadPosition
        {
            get { return yAxisUnloadPosition; }
            set { yAxisUnloadPosition = value; }
        }

        [SingleRecipeDescription(Description = "T Axis Unload Position", Unit = Unit.mm)]
        public double TAxisUnloadPosition
        {
            get { return tAxisUnloadPosition; }
            set { tAxisUnloadPosition = value; }
        }


        [SingleRecipeDescription(Description = "Use Syringe Pump Port 1", Unit = Unit.ETC)]
        public bool UsePort1
		{
			get { return usePort1; }
			set { usePort1 = value; }
		}

        [SingleRecipeDescription(Description = "Use Syringe Pump Port 2", Unit = Unit.ETC)]
        public bool UsePort2
		{
			get { return usePort2; }
			set { usePort2 = value; }
		}

        [SingleRecipeDescription(Description = "Use Syringe Pump Port 3", Unit = Unit.ETC)]
        public bool UsePort3
		{
			get { return usePort3; }
			set { usePort3 = value; }
		}

        [SingleRecipeDescription(Description = "Use Syringe Pump Port 4", Unit = Unit.ETC)]
        public bool UsePort4
		{
			get { return usePort4; }
			set { usePort4 = value; }
		}

        [SingleRecipeDescription(Description = "Use Syringe Pump Port 5", Unit = Unit.ETC)]
        public bool UsePort5
        {
            get { return usePort5; }
            set { usePort5 = value; }
        }

        [SingleRecipeDescription(Description = "Use Syringe Pump Port 6", Unit = Unit.ETC)]
        public bool UsePort6
        {
            get { return usePort6; }
            set { usePort6 = value; }
        }

        [SingleRecipeDescription(Description = "Clean Vertical", Unit = Unit.ETC)]
        public bool IsCleanVertical
        {
            get { return isCleanVertical; }
            set { isCleanVertical = value; }
        }
    }
}
