using System;
using System.Windows;
using EQX.Core.Helpers;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Process;

namespace PIFilmAutoDetachCleanMC.Services
{
    public class TouchDetectionService
    {
        private readonly MachineStatus _machineStatus;

        public TouchDetectionService(MachineStatus machineStatus)
        {
            _machineStatus = machineStatus;
        }

        public void HandleTouchEvent(Point touchPosition, Window window)
        {
            var detectedScreen = DetectTouchPosition(touchPosition, window);
            _machineStatus.ActiveScreen = detectedScreen;
        }

        public EScreen DetectTouchPosition(Point touchPosition, Window window)
        {
            var validDisplays = DisplayHelpers.GetValidMonitors();
            
            if (validDisplays.Count >= 2)
            {
                var screenPos = window.PointToScreen(touchPosition);
                var firstScreen = validDisplays[0];
                var secondScreen = validDisplays[1];
                
                if (screenPos.X >= firstScreen.Left && 
                    screenPos.X < firstScreen.Left + firstScreen.Width &&
                    screenPos.Y >= firstScreen.Top && 
                    screenPos.Y < firstScreen.Top + firstScreen.Height)
                {
                    return EScreen.RightScreen;
                }
                else if (screenPos.X >= secondScreen.Left && 
                         screenPos.X < secondScreen.Left + secondScreen.Width &&
                         screenPos.Y >= secondScreen.Top && 
                         screenPos.Y < secondScreen.Top + secondScreen.Height)
                {
                    return EScreen.LeftScreen;
                }
            }
            
            return EScreen.RightScreen;
        }
    }
}