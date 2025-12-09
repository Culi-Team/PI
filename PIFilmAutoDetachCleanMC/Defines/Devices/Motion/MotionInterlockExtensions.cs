using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQX.Core.Interlock;
using EQX.Core.Motion;

namespace PIFilmAutoDetachCleanMC.Defines.Devices.Motion
{
    public static class MotionInterlockExtensions
    {
        private static readonly PropertyChangedEventHandler PropertyChangedHandler = (sender, args) =>
        {
            InterlockService.Default.Reevaluate();
        };

        public static void ConfigureInterlock(this IMotion motion, string key, Func<bool> condition, params object[] dependencies)
        {
            if (motion == null)
            {
                return;
            }

            InterlockService.Default.RegisterRule(new LambdaInterlockRule(key, condition));
            AttachDependencyHandlers(motion, dependencies);
            InterlockService.Default.Reevaluate();
        }

        private static void AttachDependencyHandlers(IMotion motion, IEnumerable<object> dependencies)
        {
            if (motion.Status is INotifyPropertyChanged statusNotifier)
            {
                statusNotifier.PropertyChanged += PropertyChangedHandler;
            }

            foreach (var dependency in dependencies)
            {
                if (dependency is INotifyPropertyChanged notifyPropertyChanged)
                {
                    notifyPropertyChanged.PropertyChanged += PropertyChangedHandler;
                }
            }
        }
    }
}
