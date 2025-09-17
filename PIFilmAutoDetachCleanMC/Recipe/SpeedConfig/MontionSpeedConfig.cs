using EQX.Core.Recipe;
using EQX.Core.Units;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class MontionSpeedConfig : RecipeBase
    {
        private double inCstTAxisVelocity;
        private double inCstTAxisAcceleration;
        private double inCstTAxisDeceleration;
        private double outCstTAxisVelocity;
        private double outCstTAxisAcceleration;
        private double outCstTAxisDeceleration;


        public MontionSpeedConfig()
        {
            Name = "MotionInovancePara";
        }
        #region CST Load / Unload Speed Config

        [SingleRecipeDescription(Description = "In cassette T Axis", Unit = Unit.ETC, IsSpacer = true)]
        public string InCstTAxis
        {
            get { return "InCassetteTAxis"; }
        }

        [SingleRecipeDescription(Description = "InCassetteTAxis Velocity", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 1000.0, Min = 0.1)]
        public double InCstTAxisVelocity
        {
            get { return inCstTAxisVelocity; }
            set { inCstTAxisVelocity = value; }
        }

        [SingleRecipeDescription(Description = "InCassetteTAxis Acceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.1)]
        public double InCstTAxisAcceleration
        {
            get { return inCstTAxisAcceleration; }
            set { inCstTAxisAcceleration = value; }
        }

        [SingleRecipeDescription(Description = "InCassetteTAxis Deceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.1)]
        public double InCstTAxisDeceleration
        {
            get { return inCstTAxisDeceleration; }
            set { inCstTAxisDeceleration = value; }
        }

        [SingleRecipeDescription(Description = "Out cassette T Axis", Unit = Unit.ETC, IsSpacer = true)]
        public string OutCstTAxis
        {
            get { return "OutCassetteTAxis"; }
        }

        [SingleRecipeDescription(Description = "OutCassetteTAxis Velocity", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 1000.0, Min = 0.1)]
        public double OutCstTAxisVelocity
        {
            get { return outCstTAxisVelocity; }
            set { outCstTAxisVelocity = value; }
        }

        [SingleRecipeDescription(Description = "OutCassetteTAxis Acceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.1)]
        public double OutCstTAxisAcceleration
        {
            get { return outCstTAxisAcceleration; }
            set { outCstTAxisAcceleration = value; }
        }

        [SingleRecipeDescription(Description = "OutCassetteTAxis Deceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.1)]
        public double OutCstTAxisDeceleration
        {
            get { return outCstTAxisDeceleration; }
            set { outCstTAxisDeceleration = value; }
        }

        #endregion
    }
}


