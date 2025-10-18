using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.InOut;
using EQX.Core.Interlock;
using EQX.InOut;

namespace PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder
{
    public static class CylinderInterlockExtensions
    {
        private static readonly PropertyChangedEventHandler PropertyChangedHandler = (sender, args) =>
        {
            InterlockService.Default.Reevaluate();
        };

        private static readonly EventHandler ValueChangedHandler = (sender, args) =>
        {
            InterlockService.Default.Reevaluate();
        };

        private static readonly EventHandler CylinderStateChangedHandler = (sender, args) =>
        {
            InterlockService.Default.Reevaluate();
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cylinder">Which cylinder</param>
        /// <param name="key">Should write "Cylinder.{Cylinder.Name}</param>
        /// <param name="condition">Condition to interlock</param>
        /// <param name="dependencies">Cylinder dependencies for what?</param>
        public static void ConfigureInterlock(this ICylinder cylinder, string key, Func<bool> condition, params object[] dependencies)
        {
            if (cylinder is not CylinderBase cylinderBase)
            {
                return;
            }

            cylinderBase.InterlockKey = key;
            cylinderBase.InterlockCondition = condition;

            InterlockService.Default.RegisterRule(new LambdaInterlockRule(key, condition));

            cylinderBase.StateChanged += CylinderStateChangedHandler;
            AttachDependencyHandlers(dependencies);

            InterlockService.Default.Reevaluate();
        }

        private static void AttachDependencyHandlers(IEnumerable<object> dependencies)
        {
            foreach (var dependency in dependencies)
            {
                if (dependency == null)
                {
                    continue;
                }

                switch (dependency)
                {
                    case CylinderBase cylinderBase:
                        cylinderBase.StateChanged += CylinderStateChangedHandler;
                        break;
                    case INotifyPropertyChanged notifyPropertyChanged:
                        notifyPropertyChanged.PropertyChanged += PropertyChangedHandler;
                        break;
                    case IDInput input:
                        input.ValueUpdated += ValueChangedHandler;
                        input.ValueChanged += ValueChangedHandler;
                        break;
                }
            }
        }
    }
}
