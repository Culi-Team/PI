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
        private int cleanHorizontalCount;
        private int cleanVerticalCount;
        private double cylinderPushPressure;

        private double rFeedingAxisForwardDistance;
        private double rFeedingAxisBackwardDistance;
        private double rFeedingAxisCleaningSpeed;

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

        private double xAxisCleanHorizontalPosition;
        private double yAxisCleanHorizontalPosition;
        private double tAxisCleanHorizontalPosition;

        private double xAxisCleanVerticalPosition;
        private double yAxisCleanVerticalPosition;
        private double tAxisCleanVerticalPosition;

        [SingleRecipeDescription(Description = "UnWinder Torque", Unit = Unit.Percentage)]
        [SingleRecipeMinMax(Max = 100.0, Min = 0.0)]
        public int UnWinderTorque
		{
			get { return unwinderTorque; }
			set 
            {
                OnRecipeChanged(unwinderTorque,value);
                unwinderTorque = value;
            }
		}

        [SingleRecipeDescription(Description = "Winder Torque", Unit = Unit.Percentage)]
        [SingleRecipeMinMax(Max = 100.0, Min = 0.0)]
        public int WinderTorque
		{
			get 
            {
                return winderTorque; 
            }
			set 
            {
                OnRecipeChanged(winderTorque, value);
                winderTorque = value; 
            }
		}

        [SingleRecipeDescription(Description = "Clean Volume Per Nozzle", Unit = Unit.Milliliter)]
        [SingleRecipeMinMax(Max = 100.0, Min = 0.0)]
        public double CleanVolume
		{
			get { return cleanVolume; }
			set 
            {
                OnRecipeChanged(cleanVolume, value);
                cleanVolume = value; 

            }
		}

        [SingleRecipeDescription(Description = "Clean Horizontal Count", Unit = Unit.ETC)]
        [SingleRecipeMinMax(Max = 50, Min = 1)]
        public int CleanHorizontalCount
        {
            get { return cleanHorizontalCount; }
            set 
            {
                OnRecipeChanged(cleanHorizontalCount, value);
                cleanHorizontalCount = value; 
            }
        }

        [SingleRecipeDescription(Description = "Clean Vertical Count", Unit = Unit.ETC)]
        [SingleRecipeMinMax(Max = 50, Min = 1)]
        public int CleanVerticalCount
        {
            get { return cleanVerticalCount; }
            set 
            {
                OnRecipeChanged(cleanVerticalCount, value);
                cleanVerticalCount = value; 
            }
        }

        [SingleRecipeDescription(Description = "Cylinder Push Pressure", Unit = Unit.MilliPascal)]
        [SingleRecipeMinMax(Max = 0.9, Min = 0)]
        public double CylinderPushPressure
        {
            get { return cylinderPushPressure; }
            set 
            {
                OnRecipeChanged(cylinderPushPressure, value);
                cylinderPushPressure = value; 
            }
        }


        [SingleRecipeDescription(Description = "R Feeding Axis Forward Distance", Unit = Unit.mm)]
        public double RFeedingAxisForwardDistance
        {
            get { return rFeedingAxisForwardDistance; }
            set 
            {
                OnRecipeChanged(rFeedingAxisForwardDistance, value);
                rFeedingAxisForwardDistance = value; 
            }
        }

        [SingleRecipeDescription(Description = "R Feeding Axis Backward Distance", Unit = Unit.mm)]
        public double RFeedingAxisBackwardDistance
        {
            get { return rFeedingAxisBackwardDistance; }
            set 
            {
                OnRecipeChanged(rFeedingAxisBackwardDistance, value);
                rFeedingAxisBackwardDistance = value; 
            }
        }


        [SingleRecipeDescription(Description = "R Feeding Axis Cleaning Speed", Unit = Unit.mmPerSecond)]
        public double RFeedingAxisCleaningSpeed
        {
            get { return rFeedingAxisCleaningSpeed; }
            set 
            {
                OnRecipeChanged(rFeedingAxisCleaningSpeed, value);
                rFeedingAxisCleaningSpeed = value; 
            }
        }

        [SingleRecipeDescription(Description = "X Axis Load Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "X Axis")]
        public double XAxisLoadPosition
        {
            get { return xAxisLoadPosition; }
            set 
            {
                OnRecipeChanged(xAxisLoadPosition, value);
                xAxisLoadPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Y Axis Load Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisLoadPosition
        {
            get { return yAxisLoadPosition; }
            set 
            {
                OnRecipeChanged(yAxisLoadPosition, value);
                yAxisLoadPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "T Axis Load Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "T Axis")]
        public double TAxisLoadPosition
        {
            get { return tAxisLoadPosition; }
            set 
            {
                OnRecipeChanged(tAxisLoadPosition, value);
                tAxisLoadPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "X Axis Clean Horizontal Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "X Axis")]
        public double XAxisCleanHorizontalPosition
        {
            get { return xAxisCleanHorizontalPosition; }
            set 
            {
                OnRecipeChanged(xAxisCleanHorizontalPosition, value);
                xAxisCleanHorizontalPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Y Axis Clean Horizontal Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisCleanHorizontalPosition
        {
            get { return yAxisCleanHorizontalPosition; }
            set 
            {
                OnRecipeChanged(yAxisCleanHorizontalPosition, value);
                yAxisCleanHorizontalPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "T Axis Clean Horizontal Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "T Axis")]
        public double TAxisCleanHorizontalPosition
        {
            get { return tAxisCleanHorizontalPosition; }
            set 
            {
                OnRecipeChanged(tAxisCleanHorizontalPosition, value);
                tAxisCleanHorizontalPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "X Axis Clean Vertical Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "X Axis")]
        public double XAxisCleanVerticalPosition
        {
            get { return xAxisCleanVerticalPosition; }
            set 
            {
                OnRecipeChanged(xAxisCleanVerticalPosition, value);
                xAxisCleanVerticalPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Y Axis Clean Vertical Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisCleanVerticalPosition
        {
            get { return yAxisCleanVerticalPosition; }
            set 
            {
                OnRecipeChanged(yAxisCleanVerticalPosition, value);
                yAxisCleanVerticalPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "T Axis Clean Vertical Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "T Axis")]
        public double TAxisCleanVerticalPosition
        {
            get { return tAxisCleanVerticalPosition; }
            set 
            {
                OnRecipeChanged(tAxisCleanVerticalPosition, value);
                tAxisCleanVerticalPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "X Axis Unload Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "X Axis")]
        public double XAxisUnloadPosition
        {
            get { return xAxisUnloadPosition; }
            set 
            {
                OnRecipeChanged(xAxisUnloadPosition, value);
                xAxisUnloadPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Y Axis Unload Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "Y Axis")]
        public double YAxisUnloadPosition
        {
            get { return yAxisUnloadPosition; }
            set 
            {
                OnRecipeChanged(yAxisUnloadPosition, value);
                yAxisUnloadPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "T Axis Unload Position", Unit = Unit.mm)]
        [SinglePositionTeaching(Motion = "T Axis")]
        public double TAxisUnloadPosition
        {
            get { return tAxisUnloadPosition; }
            set 
            {
                OnRecipeChanged(tAxisUnloadPosition, value);
                tAxisUnloadPosition = value; 
            }
        }

        [SingleRecipeDescription(Description = "Use Syringe Pump Port 1", Unit = Unit.ETC)]
        public bool UsePort1
		{
			get { return usePort1; }
			set 
            {
                OnRecipeChanged(usePort1, value);
                usePort1 = value; 
            }
		}

        [SingleRecipeDescription(Description = "Use Syringe Pump Port 2", Unit = Unit.ETC)]
        public bool UsePort2
		{
			get { return usePort2; }
			set 
            {
                OnRecipeChanged(usePort2, value);
                usePort2 = value; 
            }
		}

        [SingleRecipeDescription(Description = "Use Syringe Pump Port 3", Unit = Unit.ETC)]
        public bool UsePort3
		{
			get { return usePort3; }
			set 
            {
                OnRecipeChanged(usePort3, value);
                usePort3 = value; 
            }
		}

        [SingleRecipeDescription(Description = "Use Syringe Pump Port 4", Unit = Unit.ETC)]
        public bool UsePort4
		{
			get { return usePort4; }
			set 
            {
                OnRecipeChanged(usePort4, value);
                usePort4 = value; 
            }
		}

        [SingleRecipeDescription(Description = "Use Syringe Pump Port 5", Unit = Unit.ETC)]
        public bool UsePort5
        {
            get { return usePort5; }
            set 
            {
                OnRecipeChanged(usePort5, value);
                usePort5 = value; 
            }
        }

        [SingleRecipeDescription(Description = "Use Syringe Pump Port 6", Unit = Unit.ETC)]
        public bool UsePort6
        {
            get { return usePort6; }
            set 
            {
                OnRecipeChanged(usePort6, value);
                usePort6 = value; 
            }
        }

        [SingleRecipeDescription(Description = "Clean Vertical", Unit = Unit.ETC)]
        public bool IsCleanVertical
        {
            get { return isCleanVertical; }
            set 
            {
                OnRecipeChanged(isCleanVertical, value);
                isCleanVertical = value; 
            }
        }
    }
}
