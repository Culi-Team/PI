using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Robot;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Robot;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class DevViewModel : ViewModelBase
    {
        private readonly IRobot _robotLoad;
        private readonly IRobot _robotUnload;
        private readonly RobotLoadRecipe _robotLoadRecipe;
        private readonly RobotUnloadRecipe _robotUnloadRecipe;
       
        #region Properties
        public int RobotCommandNumber { get; set; }
       
        #endregion

        public ICommand RobotConnect
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _robotUnload.Connect();
                });
            }
        }

        public ICommand RobotDisconnect
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _robotUnload.Disconnect();
                });
            }
        }

        public ICommand RobotModelSelect
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _robotUnload.SendCommand($"model,0,0\r\n");
                });
            }
        }

        public ICommand RobotMotionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _robotUnload.SendCommand(RobotHelpers.MotionCommands((ERobotCommand)RobotCommandNumber, _robotLoadRecipe.RobotSpeedLow, _robotLoadRecipe.RobotSpeedHigh));
                });
            }
        }

        public ICommand RobotMotionParameterCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string[] paras = new string[] {
                        IndexX.ToString(),
                        IndexY.ToString(),
                        OffsetX.ToString("0.###"),
                        OffsetY.ToString("0.###"),
                        OffsetZ.ToString("0.###"),
                        OffsetA.ToString("0.###"),
                        OffsetB.ToString("0.###"),
                        OffsetC.ToString("0.###")
                    };

                    _robotUnload.SendCommand(RobotHelpers.MotionCommands((ERobotCommand)RobotCommandNumber, RobotLowSpeed, RobotHighSpeed, paras));
                });
            }
        }

        public ICommand PCPGMStartCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _robotUnload.SendCommand(RobotHelpers.PCPGMStart);

                });
            }
        }

        public ICommand RobotStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _robotUnload.SendCommand(RobotHelpers.RobotStop);
                });
            }
        }

        public int RobotLowSpeed { get; set; }
        public int RobotHighSpeed { get; set; }
        public int IndexX { get; set; }
        public int IndexY { get; set; }
        public double OffsetX = 0;
        public double OffsetY = 0;
        public double OffsetZ = 0;
        public double OffsetA = 0;
        public double OffsetB = 0;
        public double OffsetC = 0;
        public MachineStatus MachineStatus { get; }

        public DevViewModel([FromKeyedServices("RobotLoad")] IRobot robotLoad,
            RobotLoadRecipe robotLoadRecipe,
            [FromKeyedServices("RobotUnload")] IRobot robotUnload,
            RobotUnloadRecipe robotUnloadRecipe)
        {
            _robotLoad = robotLoad;
            _robotLoadRecipe = robotLoadRecipe;
            _robotUnload = robotUnload;
            _robotUnloadRecipe = robotUnloadRecipe;
        }

        #region Privates
        #endregion
    }
}
