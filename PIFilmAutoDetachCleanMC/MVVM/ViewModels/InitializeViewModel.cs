using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using OpenCvSharp.Dnn;
using PIFilmAutoDetachCleanMC.Process;
using System.Reflection;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class InitializeViewModel : ViewModelBase
    {
        public InitializeViewModel(ProcessInitSelect processInitSelect)
        {
            ProcessInitSelect = processInitSelect;
        }

        public ProcessInitSelect ProcessInitSelect { get; }

        public ICommand SelectAllCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var props = ProcessInitSelect.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(bool) && prop.CanWrite)
                        {
                            prop.SetValue(ProcessInitSelect, true);
                        }
                    }
                });
            }
        }

        public ICommand UnSelectAllCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var props = ProcessInitSelect.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(bool) && prop.CanWrite)
                        {
                            prop.SetValue(ProcessInitSelect, false);
                        }
                    }
                });
            }
        }

        public ICommand InitializeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }
        }
    }
}
