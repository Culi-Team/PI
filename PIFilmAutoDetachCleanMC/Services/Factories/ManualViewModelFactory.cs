using EQX.Core.Motion;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Manual;
using System.Collections.ObjectModel;
using System.Windows;

namespace PIFilmAutoDetachCleanMC.Services.Factories
{
    public class ManualViewModelFactory
    {
        private readonly Devices _devices;

        public ManualViewModelFactory(Devices devices)
        {
            _devices = devices;

            _manualVMs = new Dictionary<string, ManualUnitViewModel>();
        }

        ~ManualViewModelFactory()
        {
            _manualVMs.Clear();
        }

        public ManualUnitViewModel Create(string name)
        {
            if (_manualVMs.ContainsKey(name) == false)
            {
                _manualVMs[name] = CreateVM(name);
            }

            return _manualVMs[name];
        }

        private ManualUnitViewModel CreateVM(string name)
        {
            switch (name)
            {
                case "In Conveyor":
                    return new ConveyorManualUnitViewModel("In Conveyor")
                    {
                        Cylinders = _devices.GetInConveyorCylinders(),
                        Inputs = _devices.GetInConveyorInputs(),
                        Outputs = _devices.GetInConveyorOutputs(),
                        Rollers = _devices.GetInConveyorRollers(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("InCassetteCVImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.InConveyorLoad,
                            ESemiSequence.InWorkCSTLoad
                        }
                    };
                case "In Work Conveyor":
                    return new ConveyorManualUnitViewModel("In Work Conveyor")
                    {
                        Cylinders = _devices.GetInWorkConveyorCylinders(),
                        Inputs = _devices.GetInWorkConveyorInputs(),
                        Outputs = _devices.GetInWorkConveyorOutputs(),
                        Rollers = _devices.GetInWorkConveyorRollers(),
                        Motions = _devices.GetCSTLoadMotions(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("LoadWorkCassetteStageImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.InWorkCSTLoad,
                            ESemiSequence.InWorkCSTTilt,
                            ESemiSequence.InWorkCSTUnLoad
                        }
                    };
                case "Buffer Conveyor":
                    return new ConveyorManualUnitViewModel("Buffer Conveyor")
                    {
                        Cylinders = _devices.GetBufferConveyorCylinders(),
                        Inputs = _devices.GetBufferConveyorInputs(),
                        Outputs = _devices.GetBufferConveyorOutputs(),
                        Rollers = _devices.GetBufferConveyorRollers(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("BufferCVImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.InWorkCSTUnLoad,
                            ESemiSequence.OutWorkCSTLoad
                        }
                    };
                case "Out Work Conveyor":
                    return new ConveyorManualUnitViewModel("Out Work Conveyor")
                    {
                        Cylinders = _devices.GetOutWorkConveyorCylinders(),
                        Inputs = _devices.GetOutWorkConveyorInputs(),
                        Outputs = _devices.GetOutWorkConveyorOutputs(),
                        Rollers = _devices.GetOutWorkConveyorRollers(),
                        Motions = _devices.GetCSTUnloadMotions(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("LoadWorkCassetteStageImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.OutWorkCSTLoad,
                            ESemiSequence.OutWorkCSTTilt,
                            ESemiSequence.OutWorkCSTUnLoad
                        }
                    };
                case "Out Conveyor":
                    return new ConveyorManualUnitViewModel("Out Conveyor")
                    {
                        Cylinders = _devices.GetOutConveyorCylinders(),
                        Inputs = _devices.GetOutConveyorInputs(),
                        Outputs = _devices.GetOutConveyorOutputs(),
                        Rollers = _devices.GetOutConveyorRollers(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("OutCassetteCVImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.OutWorkCSTUnLoad,
                            ESemiSequence.OutConveyorUnload
                        }
                    };
                case "Robot Load":
                    return new ManualUnitViewModel("Robot Load")
                    {
                        Cylinders = _devices.GetRobotLoadCylinders(),
                        Inputs = _devices.GetRobotLoadInputs(),
                        Outputs = _devices.GetRobotLoadOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("LoadRobotImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.RobotPickFixtureFromCST,
                            ESemiSequence.RobotPlaceFixtureToVinylClean,
                            ESemiSequence.RobotPickFixtureFromVinylClean,
                            ESemiSequence.RobotPlaceFixtureToAlign,
                            ESemiSequence.RobotPickFixtureFromRemoveZone,
                            ESemiSequence.RobotPlaceFixtureToOutWorkCST
                        }
                    };
                case "Vinyl Clean":
                    return new ManualUnitViewModel("Vinyl Clean")
                    {
                        Cylinders = _devices.GetVinylCleanCylinders(),
                        Motions = new ObservableCollection<IMotion> { _devices.VinylCleanEncoder },
                        Inputs = _devices.GetVinylCleanInputs(),
                        Outputs = _devices.GetVinylCleanOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("VinylCleanImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.RobotPlaceFixtureToVinylClean,
                            ESemiSequence.VinylClean,
                            ESemiSequence.RobotPickFixtureFromVinylClean
                        }
                    };
                case "Transfer Fixture":
                    return new ManualUnitViewModel("Transfer Fixture")
                    {
                        Cylinders = _devices.GetTransferFixtureCylinders(),
                        Outputs = _devices.GetTransferFixtureOutputs(),
                        Motions = _devices.GetTransferFixtureMotions(),
                        Inputs = _devices.GetTransferFixtureInputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferFixtureImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.TransferFixture,
                        }

                    };
                case "Fixture Align":
                    return new ManualUnitViewModel("Fixture Align")
                    {

                        Cylinders = _devices.GetFixtureAlignCylinders(),
                        Inputs = _devices.GetFixtureAlignInputs(),
                        Outputs = _devices.GetFixtureAlignOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AlignFixtureImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.RobotPlaceFixtureToAlign,
                            ESemiSequence.FixtureAlign
                        }
                    };
                case "Detach":
                    return new ManualUnitViewModel("Detach")
                    {
                        Cylinders = _devices.GetDetachCylinders(),
                        Motions = _devices.GetDetachMotions(),
                        Inputs = _devices.GetDetachInputs(),
                        Outputs = _devices.GetDetachOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("DetachImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.Detach,
                            ESemiSequence.DetachUnload
                        }
                    };
                case "Remove Film":
                    return new ManualUnitViewModel("Remove Film")
                    {
                        Cylinders = _devices.GetRemoveFilmCylinders(),
                        Inputs = _devices.GetRemoveFilmInputs(),
                        Outputs = _devices.GetRemoveFilmOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("RemoveZoneImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.RemoveFilm,
                            ESemiSequence.RobotPickFixtureFromRemoveZone,
                        }
                    };
                case "Glass Transfer":
                    return new ManualUnitViewModel("Glass Transfer")
                    {
                        Cylinders = _devices.GetGlassTransferCylinders(),
                        Motions = _devices.GetGlassTransferMotions(),
                        Inputs = _devices.GetGlassTransferInputs(),
                        Outputs = _devices.GetGlassTransferOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("GlassTransferImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.GlassTransferPick,
                            ESemiSequence.GlassTransferLeft,
                            ESemiSequence.GlassTransferRight,
                        }
                    };
                case "Transfer In Shuttle Left":
                    return new ManualUnitViewModel("Transfer In Shuttle Left")
                    {
                        Cylinders = _devices.GetTransferInShuttleLeftCylinders(),
                        Motions = _devices.GetTransferInShuttleLeftMotions(),
                        Inputs = _devices.GetTransferInShuttleLeftInputs(),
                        Outputs = _devices.GetTransferInShuttleLeftOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.GlassTransferLeft,
                            ESemiSequence.AlignGlassLeft,
                            ESemiSequence.WETCleanLeftLoad,
                        }
                    };
                case "Transfer In Shuttle Right":
                    return new ManualUnitViewModel("Transfer In Shuttle Right")
                    {
                        Cylinders = _devices.GetTransferInShuttleRightCylinders(),
                        Motions = _devices.GetTransferInShuttleRightMotions(),
                        Inputs = _devices.GetTransferInShuttleRightInputs(),
                        Outputs = _devices.GetTransferInShuttleRightOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.GlassTransferRight,
                            ESemiSequence.AlignGlassRight,
                            ESemiSequence.WETCleanRightLoad,
                        }
                    };
                case "WET Clean Left":
                    return new ManualUnitViewModel("WET Clean Left")
                    {
                        Cylinders = _devices.GetWETCleanLeftCylinders(),
                        Motions = _devices.GetWETCleanLeftMotions(),
                        Inputs = _devices.GetWETCleanLeftInputs(),
                        Outputs = _devices.GetWETCleanLeftOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.WETCleanLeftLoad,
                            ESemiSequence.WETCleanLeft,
                            ESemiSequence.InShuttleCleanLeft,
                        }
                    };
                case "WET Clean Right":
                    return new ManualUnitViewModel("WET Clean Right")
                    {
                        Cylinders = _devices.GetWETCleanRightCylinders(),
                        Motions = _devices.GetWETCleanRightMotions(),
                        Inputs = _devices.GetWETCleanRightInputs(),
                        Outputs = _devices.GetWETCleanRightOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.WETCleanRightLoad,
                            ESemiSequence.WETCleanRight,
                            ESemiSequence.InShuttleCleanRight,
                        }
                    };
                case "Transfer Rotation Left":
                    return new ManualUnitViewModel("Transfer Rotation Left")
                    {
                        Cylinders = _devices.GetTransferRotationLeftCylinders(),
                        Motions = _devices.GetTransferRotationLeftMotions(),
                        Inputs = _devices.GetTransferRotationLeftInputs(),
                        Outputs = _devices.GetTransferRotationLeftOutputs(),
                        Vacuums = _devices.GetTransferRotationLeftVacuums(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.WETCleanLeftUnload,
                            ESemiSequence.TransferRotationLeft,
                            ESemiSequence.AFCleanLeftLoad,
                        }
                    };
                case "Transfer Rotation Right":
                    return new ManualUnitViewModel("Transfer Rotation Right")
                    {
                        Cylinders = _devices.GetTransferRotationRightCylinders(),
                        Motions = _devices.GetTransferRotationRightMotions(),
                        Inputs = _devices.GetTransferRotationRightInputs(),
                        Outputs = _devices.GetTransferRotationRightOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.WETCleanRightUnload,
                            ESemiSequence.TransferRotationRight,
                            ESemiSequence.AFCleanRightLoad,
                        }
                    };
                case "AFClean Left":
                    return new ManualUnitViewModel("AF Clean Left")
                    {
                        Cylinders = _devices.GetAFCleanLeftCylinders(),
                        Motions = _devices.GetAFCleanLeftMotions(),
                        Inputs = _devices.GetAFCleanLeftInputs(),
                        Outputs = _devices.GetAFCleanLeftOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.AFCleanLeftLoad,
                            ESemiSequence.AFCleanLeft,
                            ESemiSequence.OutShuttleCleanLeft,
                        }
                    };
                case "AFClean Right":
                    return new ManualUnitViewModel("AF Clean Right")
                    {
                        Cylinders = _devices.GetAFCleanRightCylinders(),
                        Motions = _devices.GetAFCleanRightMotions(),
                        Inputs = _devices.GetAFCleanRightInputs(),
                        Outputs = _devices.GetAFCleanRightOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.AFCleanRightLoad,
                            ESemiSequence.AFCleanRight,
                            ESemiSequence.OutShuttleCleanRight,
                        }
                    };
                case "Unload Transfer Left":
                    return new ManualUnitViewModel("Unload Transfer Left")
                    {
                        Cylinders = _devices.GetUnloadTransferLeftCylinders(),
                        Motions = _devices.GetUnloadTransferLeftMotions(),
                        Inputs = _devices.GetUnloadTransferLeftInputs(),
                        Outputs = _devices.GetUnloadTransferLeftOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadTransferImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.AFCleanLeftUnload,
                            ESemiSequence.UnloadTransferLeftPlace,
                        }
                    };
                case "Unload Transfer Right":
                    return new ManualUnitViewModel("Unload Transfer Right")
                    {
                        Cylinders = _devices.GetUnloadTransferRightCylinders(),
                        Motions = _devices.GetUnloadTransferRightMotions(),
                        Inputs = _devices.GetUnloadTransferRightInputs(),
                        Outputs = _devices.GetUnloadTransferRightOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadTransferImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.AFCleanRightUnload,
                            ESemiSequence.UnloadTransferRightPlace,
                        }
                    };
                case "Unload Align":
                    return new ManualUnitViewModel("Unload Align")
                    {
                        Cylinders = _devices.GetUnloadAlignCylinders(),
                        Inputs = _devices.GetUnloadAlignInputs(),
                        Outputs = _devices.GetUnloadAlignOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadStageImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.UnloadAlignGlass,
                        }
                    };
                case "Unload Robot":
                    return new ManualUnitViewModel("Unload Robot")
                    {
                        Cylinders = _devices.GetUnloadRobotCylinders(),
                        Inputs = _devices.GetUnloadRobotInputs(),
                        Outputs = _devices.GetUnloadRobotOutputs(),
                        Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadRobotImage"),
                        SemiAutoSequences = new ObservableCollection<ESemiSequence>()
                        {
                            ESemiSequence.UnloadRobotPick,
                            ESemiSequence.UnloadRobotPlasma,
                            ESemiSequence.UnloadRobotPlace,
                        }
                    };
                default:
                    return new ManualUnitViewModel("INVALID");
            }

        }

        #region Private Fields
        private Dictionary<string, ManualUnitViewModel> _manualVMs;
        #endregion
    }
}
