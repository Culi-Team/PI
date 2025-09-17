using EQX.Core.Recipe;
using EQX.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    /// <summary>
    /// Base class for motion parameters
    /// </summary>
    public abstract class MotionParameterBase : RecipeBase
    {
        private double velocity;
        private double acceleration;
        private double deceleration;

        [SingleRecipeDescription(Description = "Velocity", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 1000.0, Min = 0.0)]
        public double Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        [SingleRecipeDescription(Description = "Acceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        [SingleRecipeDescription(Description = "Deceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double Deceleration
        {
            get { return deceleration; }
            set { deceleration = value; }
        }
    }

    /// <summary>
    /// Motion parameter for Inovance controller
    /// </summary>
    public class MotionInovanceParameter : MotionParameterBase
    {
        private int id;
        private string name;

        [SingleRecipeDescription(Description = "Axis ID", Unit = Unit.ETC)]
        [SingleRecipeMinMax(Max = 100, Min = 1)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [SingleRecipeDescription(Description = "Axis Name", Unit = Unit.ETC)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    /// <summary>
    /// Motion parameter for Ajin controller
    /// </summary>
    public class MotionAjinParameter : MotionParameterBase
    {
        private int id;
        private string name;

        [SingleRecipeDescription(Description = "Axis ID", Unit = Unit.ETC)]
        [SingleRecipeMinMax(Max = 100, Min = 1)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [SingleRecipeDescription(Description = "Axis Name", Unit = Unit.ETC)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    /// <summary>
    /// Vinyl Clean Encoder configuration
    /// </summary>
    public class VinylCleanEncoderConfig : RecipeBase
    {
        private double velocity;
        private double acceleration;
        private double deceleration;

        [SingleRecipeDescription(Description = "Velocity", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 1000.0, Min = 0.0)]
        public double Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        [SingleRecipeDescription(Description = "Acceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        [SingleRecipeDescription(Description = "Deceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double Deceleration
        {
            get { return deceleration; }
            set { deceleration = value; }
        }
    }

    /// <summary>
    /// Unit Process Motion Configuration
    /// </summary>
    public class UnitProcessMotionConfig : RecipeBase
    {
        private double xAxisVelocity;
        private double xAxisAcceleration;
        private double xAxisDeceleration;
        private double yAxisVelocity;
        private double yAxisAcceleration;
        private double yAxisDeceleration;
        private double zAxisVelocity;
        private double zAxisAcceleration;
        private double zAxisDeceleration;
        private double tAxisVelocity;
        private double tAxisAcceleration;
        private double tAxisDeceleration;

        [SingleRecipeDescription(Description = "X Axis", Unit = Unit.ETC)]
        public string XAxisName => "X Axis";

        [SingleRecipeDescription(Description = "X Axis Speed", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 1000.0, Min = 0.0)]
        public double XAxisVelocity
        {
            get { return xAxisVelocity; }
            set { xAxisVelocity = value; }
        }

        [SingleRecipeDescription(Description = "X Axis Acceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double XAxisAcceleration
        {
            get { return xAxisAcceleration; }
            set { xAxisAcceleration = value; }
        }

        [SingleRecipeDescription(Description = "X Axis Deceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double XAxisDeceleration
        {
            get { return xAxisDeceleration; }
            set { xAxisDeceleration = value; }
        }

        [SingleRecipeDescription(Description = "Y Axis", Unit = Unit.ETC)]
        public string YAxisName => "Y Axis";

        [SingleRecipeDescription(Description = "Y Axis Speed", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 1000.0, Min = 0.0)]
        public double YAxisVelocity
        {
            get { return yAxisVelocity; }
            set { yAxisVelocity = value; }
        }

        [SingleRecipeDescription(Description = "Y Axis Acceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double YAxisAcceleration
        {
            get { return yAxisAcceleration; }
            set { yAxisAcceleration = value; }
        }

        [SingleRecipeDescription(Description = "Y Axis Deceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double YAxisDeceleration
        {
            get { return yAxisDeceleration; }
            set { yAxisDeceleration = value; }
        }

        [SingleRecipeDescription(Description = "Z Axis", Unit = Unit.ETC)]
        public string ZAxisName => "Z Axis";

        [SingleRecipeDescription(Description = "Z Axis Speed", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 1000.0, Min = 0.0)]
        public double ZAxisVelocity
        {
            get { return zAxisVelocity; }
            set { zAxisVelocity = value; }
        }

        [SingleRecipeDescription(Description = "Z Axis Acceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double ZAxisAcceleration
        {
            get { return zAxisAcceleration; }
            set { zAxisAcceleration = value; }
        }

        [SingleRecipeDescription(Description = "Z Axis Deceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double ZAxisDeceleration
        {
            get { return zAxisDeceleration; }
            set { zAxisDeceleration = value; }
        }

        [SingleRecipeDescription(Description = "T Axis", Unit = Unit.ETC)]
        public string TAxisName => "T Axis";

        [SingleRecipeDescription(Description = "T Axis Speed", Unit = Unit.mmPerSecond)]
        [SingleRecipeMinMax(Max = 1000.0, Min = 0.0)]
        public double TAxisVelocity
        {
            get { return tAxisVelocity; }
            set { tAxisVelocity = value; }
        }

        [SingleRecipeDescription(Description = "T Axis Acceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double TAxisAcceleration
        {
            get { return tAxisAcceleration; }
            set { tAxisAcceleration = value; }
        }

        [SingleRecipeDescription(Description = "T Axis Deceleration", Unit = Unit.mmPerSecondSquare)]
        [SingleRecipeMinMax(Max = 10000.0, Min = 0.0)]
        public double TAxisDeceleration
        {
            get { return tAxisDeceleration; }
            set { tAxisDeceleration = value; }
        }
    }

    /// <summary>
    /// Container class for all motion speed configurations organized by unit process
    /// </summary>
    public class MontionSpeedConfig : RecipeBase
    {
        public MontionSpeedConfig()
        {
            Name = "Motion Speed Config";
        }

        // Unit Process Configurations
        [SingleRecipeDescription(Description = "Detach Process", Unit = Unit.ETC, IsSpacer = true)]
        public UnitProcessMotionConfig DetachProcessConfig { get; set; } = new UnitProcessMotionConfig { Name = "Detach Process" };
        
        [SingleRecipeDescription(Description = "Clean Process", Unit = Unit.ETC, IsSpacer = true)]
        public UnitProcessMotionConfig CleanProcessConfig { get; set; } = new UnitProcessMotionConfig { Name = "Clean Process" };
        
        [SingleRecipeDescription(Description = "Transfer Process", Unit = Unit.ETC, IsSpacer = true)]
        public UnitProcessMotionConfig TransferProcessConfig { get; set; } = new UnitProcessMotionConfig { Name = "Transfer Process" };
        
        [SingleRecipeDescription(Description = "Unload Process", Unit = Unit.ETC, IsSpacer = true)]
        public UnitProcessMotionConfig UnloadProcessConfig { get; set; } = new UnitProcessMotionConfig { Name = "Unload Process" };
        
        [SingleRecipeDescription(Description = "CST Load/Unload Process", Unit = Unit.ETC, IsSpacer = true)]
        public UnitProcessMotionConfig CSTLoadUnloadProcessConfig { get; set; } = new UnitProcessMotionConfig { Name = "CST Load/Unload Process" };
        
        [SingleRecipeDescription(Description = "Vinyl Clean Process", Unit = Unit.ETC, IsSpacer = true)]
        public UnitProcessMotionConfig VinylCleanProcessConfig { get; set; } = new UnitProcessMotionConfig { Name = "Vinyl Clean Process" };


    }
}
