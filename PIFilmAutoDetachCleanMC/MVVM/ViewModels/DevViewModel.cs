using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.InOut;
using EQX.Core.Interlock;
using EQX.Core.Robot;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Robot;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Windows.Input;
using System.Xml.Linq;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class DevViewModel : ViewModelBase
    {
        private readonly IRobot _robotLoad;
        private readonly IRobot _robotUnload;
        private readonly Devices _devices;
        private readonly DieHardK180Plasma _plasma;
        private bool isRobotLoadSelected = true;
        private bool isRobotUnloadSelected;
        private bool isCylinderInterlockBypassed;
        private ICylinder AlignCylL1 => _devices.Cylinders.AlignStageL_AlignCyl1;
        private ICylinder AlignCylL2 => _devices.Cylinders.AlignStageL_AlignCyl2;
        private ICylinder AlignCylL3 => _devices.Cylinders.AlignStageL_AlignCyl3;

        private ICylinder AlignCylR1 => _devices.Cylinders.AlignStageR_AlignCyl1;
        private ICylinder AlignCylR2 => _devices.Cylinders.AlignStageR_AlignCyl2;
        private ICylinder AlignCylR3 => _devices.Cylinders.AlignStageR_AlignCyl3;

        private ICylinder GlassTransferCylinderUpDown1 => _devices.Cylinders.GlassTransfer_UpDownCyl1;
        private ICylinder GlassTransferCylinderUpDown2 => _devices.Cylinders.GlassTransfer_UpDownCyl2;
        private ICylinder GlassTransferCylinderUpDown3 => _devices.Cylinders.GlassTransfer_UpDownCyl3;

        private ICylinder ULAlignCyl1 => _devices.Cylinders.UnloadAlign_UpDownCyl1;
        private ICylinder ULAlignCyl2 => _devices.Cylinders.UnloadAlign_UpDownCyl2;
        private ICylinder ULAlignCyl3 => _devices.Cylinders.UnloadAlign_UpDownCyl3;
        private ICylinder ULAlignCyl4 => _devices.Cylinders.UnloadAlign_UpDownCyl4;

        private ICylinder UnLoadRobCyl1 => _devices.Cylinders.UnloadRobot_UpDownCyl1;
        private ICylinder UnLoadRobCyl2 => _devices.Cylinders.UnloadRobot_UpDownCyl2;
        private ICylinder UnLoadRobCyl3 => _devices.Cylinders.UnloadRobot_UpDownCyl3;
        private ICylinder UnLoadRobCyl4 => _devices.Cylinders.UnloadRobot_UpDownCyl4;


        private IRobot _currentRobot => isRobotLoadSelected ? _robotLoad : _robotUnload;
        private string strRobotResponse;
        private int _robotResponse;

        #region Properties
        public int RobotResponse
        {
            get => _robotResponse;
            set
            {
                _robotResponse = value;
                OnPropertyChanged();
            }
        }

        public bool IsRobotLoadSelected
        {
            get => isRobotLoadSelected;
            set
            {
                if (SetProperty(ref isRobotLoadSelected, value))
                {
                    OnPropertyChanged(nameof(IsRobotLoadSelected));
                }
            }
        }

        public bool IsRobotUnloadSelected
        {
            get => isRobotUnloadSelected;
            set
            {
                if (SetProperty(ref isRobotUnloadSelected, value))
                {
                    OnPropertyChanged(nameof(IsRobotUnloadSelected));
                }
            }
        }
        public int RobotCommandNumber { get; set; }
        #endregion

        public ICommand RobotConnect
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _currentRobot.Connect();
                });
            }
        }

        public ICommand RobotDisconnect
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _currentRobot.Disconnect();
                });
            }
        }

        public ICommand RobotModelSelect
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _currentRobot.SendCommand($"model,0,0\r\n");
                });
            }
        }

        public ICommand RobotMotionCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _currentRobot.SendCommand(RobotHelpers.MotionCommands((ERobotCommand)RobotCommandNumber, RobotLowSpeed, RobotHighSpeed));
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

                    _currentRobot.SendCommand(RobotHelpers.MotionCommands((ERobotCommand)RobotCommandNumber, RobotLowSpeed, RobotHighSpeed, paras));
                });
            }
        }

        public ICommand PCPGMStartCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _currentRobot.SendCommand(RobotHelpers.PCPGMStart);

                });
            }
        }

        public ICommand RobotStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _currentRobot.SendCommand(RobotHelpers.RobotStop);
                });
            }
        }

        public ICommand RobotResponseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    strRobotResponse = _currentRobot.ReadResponse();

                });
            }
        }

        public ICommand LastPosition
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _currentRobot.SendCommand(RobotHelpers.CheckLastPosition);
                    Thread.Sleep(100);
                    strRobotResponse = _currentRobot.ReadResponse();

                    string[] parts = strRobotResponse.Split(',');

                    if (parts.Length > 0 && int.TryParse(parts[1], out int value))
                    {
                        _robotResponse = value;
                    }

                });
            }
        }

        public ICommand GlassTransferUp
        {
            get
            {
                return new RelayCommand(() =>
                {
                    GlassTransferCylinderUpDown1.Backward();
                    GlassTransferCylinderUpDown2.Backward();
                    GlassTransferCylinderUpDown3.Backward();
                });
            }
        }

        public ICommand GlassTransferDown
        {
            get
            {
                return new RelayCommand(() =>
                {
                    GlassTransferCylinderUpDown1.Forward();
                    GlassTransferCylinderUpDown2.Forward();
                    GlassTransferCylinderUpDown3.Forward();
                });
            }
        }

        public ICommand AlignRightUp
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AlignCylR1.Forward();
                    AlignCylR2.Forward();
                    AlignCylR3.Forward();
                });
            }
        }

        public ICommand AlignRightDown
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AlignCylR1.Backward();
                    AlignCylR2.Backward();
                    AlignCylR3.Backward();
                });
            }
        }

        public ICommand AlignLeftUp
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AlignCylL1.Forward();
                    AlignCylL2.Forward();
                    AlignCylL3.Forward();
                });
            }
        }

        public ICommand AlignLeftDown
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AlignCylL1.Backward();
                    AlignCylL2.Backward();
                    AlignCylL3.Backward();
                });
            }
        }

        public ICommand UnloadAignUp
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ULAlignCyl1.Forward();
                    ULAlignCyl2.Forward();
                    ULAlignCyl3.Forward();
                    ULAlignCyl4.Forward();
                });
            }
        }

        public ICommand UnloadAignDown
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ULAlignCyl1.Backward();
                    ULAlignCyl2.Backward();
                    ULAlignCyl3.Backward();
                    ULAlignCyl4.Backward();
                });
            }
        }

        public ICommand UnloadRobotUp
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UnLoadRobCyl1.Backward();
                    UnLoadRobCyl2.Backward();
                    UnLoadRobCyl3.Backward();
                    UnLoadRobCyl4.Backward();
                });
            }
        }

        public ICommand UnloadRobotDown
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UnLoadRobCyl1.Forward();
                    UnLoadRobCyl2.Forward();
                    UnLoadRobCyl3.Forward();
                    UnLoadRobCyl4.Forward();
                });
            }
        }

        public ICommand PlasmaRunTest
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Task plasmaPrepareTask = Task.Run(async () =>
                    {
                        bool running = true;
                        int plasmaPrepareStep = 0;
                        while (running)
                        {
                            switch ((EPlasmaPrepareStep)plasmaPrepareStep)
                            {
                                case EPlasmaPrepareStep.Start:
                                    _plasma.EnableRemote();
                                    await Task.Delay(500);
                                    plasmaPrepareStep++;
                                    break;

                                case EPlasmaPrepareStep.Air_Valve_Open:
                                    _plasma.AirOpenClose(true);
                                    await Task.Delay(500);
                                    plasmaPrepareStep++;
                                    break;

                                case EPlasmaPrepareStep.Plasma_On:
                                    _plasma.PlasmaOnOff(true);
                                    await Task.Delay(500);
                                    plasmaPrepareStep++;
                                    break;

                                case EPlasmaPrepareStep.End:
                                    running = false;
                                    break;
                            }
                        }
                    });
                });
            }
        }

        public ICommand PlasmaStop
        {
            get
            {
                return new RelayCommand(() =>
                {
                    _plasma.IdleMode();
                });
            }
        }
        public bool IsCylinderInterlockEnabled
        {
            get => !IsCylinderInterlockBypassed;
            set
            {
                if (value == IsCylinderInterlockEnabled)
                {
                    return;
                }

                IsCylinderInterlockBypassed = !value;
            }
        }

        public bool IsCylinderInterlockBypassed
        {
            get => isCylinderInterlockBypassed;
            set
            {
                if (SetProperty(ref isCylinderInterlockBypassed, value))
                {
                    InterlockService.Default.IsBypassAllEnabled = value;
                    OnPropertyChanged(nameof(IsCylinderInterlockEnabled));
                }
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
        public string Name { get; private set; }

        public DevViewModel(
            [FromKeyedServices("RobotLoad")] IRobot robotLoad,
            [FromKeyedServices("RobotUnload")] IRobot robotUnload,
            Devices devices,
            MachineStatus machineStatus,
            DieHardK180Plasma plasma)
        {
            _robotLoad = robotLoad;
            _robotUnload = robotUnload;
            _devices = devices;
            MachineStatus = machineStatus;
            this._plasma = plasma;
            isCylinderInterlockBypassed = InterlockService.Default.IsBypassAllEnabled;
        }

        #region Privates
        #endregion
    }
}
