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

        [SingleRecipeDescription(Description = "Use Port 1", Unit = Unit.ETC)]
        public bool UsePort1
		{
			get { return usePort1; }
			set { usePort1 = value; }
		}

        [SingleRecipeDescription(Description = "Use Port 2", Unit = Unit.ETC)]
        public bool UsePort2
		{
			get { return usePort2; }
			set { usePort2 = value; }
		}

        [SingleRecipeDescription(Description = "Use Port 3", Unit = Unit.ETC)]
        public bool UsePort3
		{
			get { return usePort3; }
			set { usePort3 = value; }
		}

        [SingleRecipeDescription(Description = "Use Port 4", Unit = Unit.ETC)]
        public bool UsePort4
		{
			get { return usePort4; }
			set { usePort4 = value; }
		}

        [SingleRecipeDescription(Description = "Use Port 5", Unit = Unit.ETC)]
        public bool UsePort5
        {
            get { return usePort5; }
            set { usePort5 = value; }
        }

        [SingleRecipeDescription(Description = "Use Port 6", Unit = Unit.ETC)]
        public bool UsePort6
        {
            get { return usePort6; }
            set { usePort6 = value; }
        }
    }
}
