using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Motion;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class MotionWrapper : ObservableObject
    {
        public IMotion Motion { get; set; }
        public string Label { get; set; }
        
        public MotionWrapper(IMotion motion, string label)
        {
            Motion = motion;
            Label = label;
        }
    }
}
