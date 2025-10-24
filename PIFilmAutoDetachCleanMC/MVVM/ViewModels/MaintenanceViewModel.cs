using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.Process;
using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class MaintenanceViewModel : ViewModelBase
    {
        public ICommand SwitchActiveScreen
        {
            get => new RelayCommand<string>((screenName) =>
            {
                //var detectedScreen = DetectTouchPosition();
                if (_machineStatus.ActiveScreen == Defines.EScreen.RightScreen)
                    _machineStatus.ActiveScreen = Defines.EScreen.LeftScreen;
                else _machineStatus.ActiveScreen = Defines.EScreen.RightScreen;
            });
        }

        public MaintenanceViewModel(MachineStatus machineStatus)
        {
            _machineStatus = machineStatus;
        }

        #region Privates
        private readonly MachineStatus _machineStatus;
        
        [DllImport("user32.dll")]
        private static extern bool GetTouchPos(out POINT lpPoint);
        
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }
        
        private Defines.EScreen DetectTouchPosition()
        {
            if (GetTouchPos(out POINT cursorPos))
            {
                var validDisplays = EQX.Core.Helpers.DisplayHelpers.GetValidMonitors();

                if (validDisplays.Count >= 2)
                {
                    // validDisplays[0] = RightScreen, validDisplays[1] = LeftScreen
                    // ["RightScreen", "LeftScreen"]
                    var firstScreen = validDisplays[0];
                    var secondScreen = validDisplays[1];

                    if (cursorPos.X >= firstScreen.Left &&
                        cursorPos.X < firstScreen.Left + firstScreen.Width &&
                        cursorPos.Y >= firstScreen.Top &&
                        cursorPos.Y < firstScreen.Top + firstScreen.Height)
                    {
                        return Defines.EScreen.RightScreen;
                    }
                    else if (cursorPos.X >= secondScreen.Left &&
                             cursorPos.X < secondScreen.Left + secondScreen.Width &&
                             cursorPos.Y >= secondScreen.Top &&
                             cursorPos.Y < secondScreen.Top + secondScreen.Height)
                    {
                        return Defines.EScreen.LeftScreen;
                    }
                }
            }

            return Defines.EScreen.RightScreen;
        }
        #endregion
    }
}
