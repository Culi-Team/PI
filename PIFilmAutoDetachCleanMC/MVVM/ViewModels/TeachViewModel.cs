using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.InOut;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class TeachViewModel : ViewModelBase
    {
        #region Properties

        public UnitTeachingViewModel DetachUnitTeaching { get; }
        public UnitTeachingViewModel GlassTransferUnitTeaching { get; }
        public UnitTeachingViewModel TransferInShuttleLeftUnitTeaching { get; }
        public UnitTeachingViewModel TransferInShuttleRightUnitTeaching { get; }
        public CleanUnitTeachingViewModel WETCleanLeftUnitTeaching { get; }
        public CleanUnitTeachingViewModel WETCleanRightUnitTeaching { get; }
        public UnitTeachingViewModel TransferRotationLeftUnitTeaching { get; }
        public UnitTeachingViewModel TransferRotationRightUnitTeaching { get; }
        public CleanUnitTeachingViewModel AFCleanLeftUnitTeaching { get; }
        public CleanUnitTeachingViewModel AFCleanRightUnitTeaching { get; }

        public Devices Devices { get; }
        public RecipeList RecipeList;
        public RecipeSelector RecipeSelector;
        public Processes Processes;
      

        public ObservableCollection<UnitTeachingViewModel> UnitTeachings { get; }

        private UnitTeachingViewModel selectedUnitTeaching;

        public UnitTeachingViewModel SelectedUnitTeaching
        {
            get 
            {
                return selectedUnitTeaching; 
            }
            set 
            {
                selectedUnitTeaching = value;
                OnPropertyChanged(nameof(SelectedUnitTeaching));
            }
        }

        #endregion

        #region GetMotions
        // CSTLoadUnload Tab Motions
        private ObservableCollection<IMotion> GetInWorkConveyorMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.InCassetteTAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetOutWorkConveyorMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.OutCassetteTAxis);
            return motions;
        }


        // Detach Tab Motions
        private ObservableCollection<IMotion> GetTransferFixtureMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
                motions.Add(Devices.MotionsInovance.FixtureTransferYAxis);
            // TransferFixtureProcess chỉ sử dụng FixtureTransferYAxis, không sử dụng ShuttleTransferZAxis
            return motions;
        }

        private ObservableCollection<IMotion> GetDetachMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
                motions.Add(Devices.MotionsInovance.DetachGlassZAxis);
                motions.Add(Devices.MotionsAjin.ShuttleTransferZAxis);
                motions.Add(Devices.MotionsInovance.ShuttleTransferXAxis);
            return motions;
        }

        // Glass Transfer Tab Motions
        private ObservableCollection<IMotion> GetGlassTransferMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
                motions.Add(Devices.MotionsInovance.GlassTransferYAxis);
                motions.Add(Devices.MotionsInovance.GlassTransferZAxis);
            return motions;
        }

        // Transfer Shutter Tab Motions
        private ObservableCollection<IMotion> GetTransferShutterLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.TransferInShuttleLYAxis);
            motions.Add(Devices.MotionsInovance.TransferInShuttleLZAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetTransferShutterRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.TransferInShuttleRYAxis);
            motions.Add(Devices.MotionsInovance.TransferInShuttleRZAxis);
            return motions;
        }

        // Transfer Rotation Tab Motions
        private ObservableCollection<IMotion> GetTransferRotationLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.TransferRotationLZAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetTransferRotationRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.TransferRotationRZAxis);
            return motions;
        }

        // Unload Transfer Tab Motions
        private ObservableCollection<IMotion> GetUnloadTransferLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.GlassUnloadLYAxis);
            motions.Add(Devices.MotionsInovance.GlassUnloadLZAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetUnloadTransferRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            motions.Add(Devices.MotionsInovance.GlassUnloadRYAxis);
            motions.Add(Devices.MotionsInovance.GlassUnloadRZAxis);
            return motions;
        }

        // Clean Tab Motions
        private ObservableCollection<IMotion> GetWETCleanLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // WET Clean Left sử dụng InShuttle axes
            motions.Add(Devices.MotionsAjin.InShuttleLXAxis);
            motions.Add(Devices.MotionsAjin.InShuttleLYAxis);
            motions.Add(Devices.MotionsInovance.InShuttleLTAxis);
            motions.Add(Devices.MotionsInovance.WETCleanLFeedingAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetWETCleanRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // WET Clean Right sử dụng InShuttle axes
            motions.Add(Devices.MotionsAjin.InShuttleRXAxis);
            motions.Add(Devices.MotionsAjin.InShuttleRYAxis);
            motions.Add(Devices.MotionsInovance.InShuttleRTAxis);
            motions.Add(Devices.MotionsInovance.WETCleanRFeedingAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetAFCleanLeftMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // AF Clean Left sử dụng OutShuttle axes
            motions.Add(Devices.MotionsAjin.OutShuttleLXAxis);
            motions.Add(Devices.MotionsAjin.OutShuttleLYAxis);
            motions.Add(Devices.MotionsInovance.OutShuttleLTAxis);
            motions.Add(Devices.MotionsInovance.AFCleanLFeedingAxis);
            return motions;
        }

        private ObservableCollection<IMotion> GetAFCleanRightMotions()
        {
            ObservableCollection<IMotion> motions = new ObservableCollection<IMotion>();
            // AF Clean Right sử dụng OutShuttle axes
            motions.Add(Devices.MotionsAjin.OutShuttleRXAxis);
            motions.Add(Devices.MotionsAjin.OutShuttleRYAxis);
            motions.Add(Devices.MotionsInovance.OutShuttleRTAxis);
            motions.Add(Devices.MotionsInovance.AFCleanRFeedingAxis);
            return motions;
        }

        #endregion

        #region GetCylinders
        // CSTLoadUnload Tab Cylinders
        private ObservableCollection<ICylinder> GetInWorkConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // In CST Stopper
            cylinders.Add(Devices.Cylinders.InCstStopperUpDown);
            // In CST Work cylinders
            cylinders.Add(Devices.Cylinders.InCstFixCyl1FwBw);
            cylinders.Add(Devices.Cylinders.InCstFixCyl2FwBw);
            cylinders.Add(Devices.Cylinders.InCstTiltCylUpDown);
            // In CV Support cylinders
            cylinders.Add(Devices.Cylinders.InCvSupportUpDown);
            cylinders.Add(Devices.Cylinders.InCvSupportBufferUpDown);
            return cylinders;
        }
        
        private ObservableCollection<ICylinder> GetOutWorkConveyorCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Out CST Stopper
            cylinders.Add(Devices.Cylinders.OutCstStopperUpDown);
            // Out CST Work cylinders
            cylinders.Add(Devices.Cylinders.OutCstFixCyl1FwBw);
            cylinders.Add(Devices.Cylinders.OutCstFixCyl2FwBw);
            cylinders.Add(Devices.Cylinders.OutCstTiltCylUpDown);
            // Out CV Support cylinders
            cylinders.Add(Devices.Cylinders.OutCvSupportUpDown);
            cylinders.Add(Devices.Cylinders.OutCvSupportBufferUpDown);
            return cylinders;
        }

        // Detach Tab Cylinders
        private ObservableCollection<ICylinder> GetTransferFixtureCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            
            // Add Transfer Fixture cylinders
            cylinders.Add(Devices.Cylinders.TransferFixtureUpDown);
            cylinders.Add(Devices.Cylinders.TransferFixture1ClampUnclamp);
            cylinders.Add(Devices.Cylinders.TransferFixture2ClampUnclamp);

            return cylinders;
        }
        
        private ObservableCollection<ICylinder> GetDetachCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            
            // Add Detach fix fixture cylinders
            cylinders.Add(Devices.Cylinders.DetachFixFixtureCyl1FwBw);
            cylinders.Add(Devices.Cylinders.DetachFixFixtureCyl2FwBw);
            
            // Add Detach cylinders
            cylinders.Add(Devices.Cylinders.DetachCyl1UpDown);
            cylinders.Add(Devices.Cylinders.DetachCyl2UpDown);
            
            return cylinders;
        }

        // Glass Transfer Tab Cylinders
        private ObservableCollection<ICylinder> GetGlassTransferCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            
            // Add Glass Transfer cylinders
            cylinders.Add(Devices.Cylinders.GlassTransferCyl1UpDown);
            cylinders.Add(Devices.Cylinders.GlassTransferCyl2UpDown);
            cylinders.Add(Devices.Cylinders.GlassTransferCyl3UpDown);
            
            return cylinders;
        }

        // Transfer Shutter Tab Cylinders (No cylinders used in TransferInShuttle process)
        private ObservableCollection<ICylinder> GetTransferShutterLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Transfer In Shuttle Left cylinders
            cylinders.Add(Devices.Cylinders.TransferInShuttleLRotate);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetTransferShutterRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Transfer In Shuttle Right cylinders
            cylinders.Add(Devices.Cylinders.TransferInShuttleRRotate);
            return cylinders;
        }

        // Transfer Rotation Tab Cylinders
        private ObservableCollection<ICylinder> GetTransferRotationLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Transfer Rotation Left cylinders
            cylinders.Add(Devices.Cylinders.TrRotateLeftRotate);
            cylinders.Add(Devices.Cylinders.TrRotateLeftFwBw);
            cylinders.Add(Devices.Cylinders.TrRotateLeftUpDown);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetTransferRotationRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Transfer Rotation Right cylinders
            cylinders.Add(Devices.Cylinders.TrRotateRightRotate);
            cylinders.Add(Devices.Cylinders.TrRotateRightFwBw);
            cylinders.Add(Devices.Cylinders.TrRotateRightUpDown);
            return cylinders;
        }

        // Unload Transfer Tab Cylinders
        private ObservableCollection<ICylinder> GetUnloadTransferLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Unload Robot Left cylinders
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl1UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl2UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl3UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl4UpDown);
            // Add Unload Align Left cylinders
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl1UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl2UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl3UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl4UpDown);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetUnloadTransferRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add Unload Robot Right cylinders
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl1UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl2UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl3UpDown);
            cylinders.Add(Devices.Cylinders.UnloadRobotCyl4UpDown);
            // Add Unload Align Right cylinders
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl1UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl2UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl3UpDown);
            cylinders.Add(Devices.Cylinders.UnloadAlignCyl4UpDown);
            return cylinders;
        }

        // Clean Tab Cylinders
        private ObservableCollection<ICylinder> GetWETCleanLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add WET Clean Left cylinders
            cylinders.Add(Devices.Cylinders.WetCleanPusherLeftUpDown);
            cylinders.Add(Devices.Cylinders.WetCleanBrushLeftUpDown);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetWETCleanRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add WET Clean Right cylinders
            cylinders.Add(Devices.Cylinders.WetCleanPusherRightUpDown);
            cylinders.Add(Devices.Cylinders.WetCleanBrushRightUpDown);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetAFCleanLeftCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add AF Clean Left cylinders
            cylinders.Add(Devices.Cylinders.AFCleanPusherLeftUpDown);
            cylinders.Add(Devices.Cylinders.AFCleanBrushLeftUpDown);
            return cylinders;
        }

        private ObservableCollection<ICylinder> GetAFCleanRightCylinders()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            // Add AF Clean Right cylinders
            cylinders.Add(Devices.Cylinders.AFCleanPusherRightUpDown);
            cylinders.Add(Devices.Cylinders.AFCleanBrushRightUpDown);
            return cylinders;
        }

        #endregion

        #region GetInputs
        // CSTLoadUnload Tab Inputs
        private ObservableCollection<IDInput> GetInWorkConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add In Cassette detection inputs
            inputs.Add(Devices.Inputs.InCstDetect1);
            inputs.Add(Devices.Inputs.InCstDetect2);
            // Add In Cassette work detection inputs
            inputs.Add(Devices.Inputs.InCstWorkDetect1);
            inputs.Add(Devices.Inputs.InCstWorkDetect2);
            inputs.Add(Devices.Inputs.InCstWorkDetect3);
            inputs.Add(Devices.Inputs.InCstWorkDetect4);
            // Add In Cassette button inputs
            inputs.Add(Devices.Inputs.InCompleteButton);
            inputs.Add(Devices.Inputs.InMutingButton);
            // Add In Cassette light curtain safety input
            inputs.Add(Devices.Inputs.InCstLightCurtainAlarmDetect);
            // Add In CV Support detection inputs
            inputs.Add(Devices.Inputs.InCvSupportUp);
            inputs.Add(Devices.Inputs.InCvSupportDown);
            inputs.Add(Devices.Inputs.InCvSupportBufferUp);
            inputs.Add(Devices.Inputs.InCvSupportBufferDown);
            // Add In CST Fix cylinder detection inputs
            inputs.Add(Devices.Inputs.InCstFixCyl1Fw);
            inputs.Add(Devices.Inputs.InCstFixCyl1Bw);
            inputs.Add(Devices.Inputs.InCstFixCyl2Fw);
            inputs.Add(Devices.Inputs.InCstFixCyl2Bw);
            // Add In CST Tilt cylinder detection inputs
            inputs.Add(Devices.Inputs.InCstTiltCylUp);
            inputs.Add(Devices.Inputs.InCstTiltCylDown);
            // Add In CST Stopper detection inputs
            inputs.Add(Devices.Inputs.InCstStopperUp);
            inputs.Add(Devices.Inputs.InCstStopperDown);
            
            return inputs;
        }
        
        private ObservableCollection<IDInput> GetOutWorkConveyorInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Out Cassette work detection inputs
            inputs.Add(Devices.Inputs.OutCstWorkDetect1);
            inputs.Add(Devices.Inputs.OutCstWorkDetect2);
            inputs.Add(Devices.Inputs.OutCstWorkDetect3);
            // Add Out Cassette detection inputs
            inputs.Add(Devices.Inputs.OutCstDetect1);
            inputs.Add(Devices.Inputs.OutCstDetect2);
            // Add Out Cassette button inputs
            inputs.Add(Devices.Inputs.OutCompleteButton);
            inputs.Add(Devices.Inputs.OutMutingButton);
            // Add Out Cassette light curtain safety input
            inputs.Add(Devices.Inputs.OutCstLightCurtainAlarmDetect);
            // Add Out CV Support detection inputs
            inputs.Add(Devices.Inputs.OutCvSupportUp);
            inputs.Add(Devices.Inputs.OutCvSupportDown);
            inputs.Add(Devices.Inputs.OutCvSupportBufferUp);
            inputs.Add(Devices.Inputs.OutCvSupportBufferDown);
            // Add Out CST Fix cylinder detection inputs
            inputs.Add(Devices.Inputs.OutCstFixCyl1Fw);
            inputs.Add(Devices.Inputs.OutCstFixCyl1Bw);
            inputs.Add(Devices.Inputs.OutCstFixCyl2Fw);
            inputs.Add(Devices.Inputs.OutCstFixCyl2Bw);
            // Add Out CST Tilt cylinder detection inputs
            inputs.Add(Devices.Inputs.OutCstTiltCylUp);
            inputs.Add(Devices.Inputs.OutCstTiltCylDown);
            // Add Out CST Stopper detection inputs
            inputs.Add(Devices.Inputs.OutCstStopperUp);
            inputs.Add(Devices.Inputs.OutCstStopperDown);
            
            return inputs;
        }

        // Detach Tab Inputs
        private ObservableCollection<IDInput> GetTransferFixtureInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Transfer Fixture detection inputs
            inputs.Add(Devices.Inputs.TransferFixtureUp);
            inputs.Add(Devices.Inputs.TransferFixtureDown);
            // Add Transfer Fixture clamp detection inputs
            inputs.Add(Devices.Inputs.TransferFixture11Clamp);
            inputs.Add(Devices.Inputs.TransferFixture11Unclamp);
            inputs.Add(Devices.Inputs.TransferFixture12Clamp);
            inputs.Add(Devices.Inputs.TransferFixture12Unclamp);
            inputs.Add(Devices.Inputs.TransferFixture21Clamp);
            inputs.Add(Devices.Inputs.TransferFixture21Unclamp);
            inputs.Add(Devices.Inputs.TransferFixture22Clamp);
            inputs.Add(Devices.Inputs.TransferFixture22Unclamp);
            
            return inputs;
        }
        
        private ObservableCollection<IDInput> GetDetachInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Detach fixture detection input
            inputs.Add(Devices.Inputs.DetachFixtureDetect);
            // Add Detach glass shuttle vacuum inputs
            inputs.Add(Devices.Inputs.DetachGlassShtVac1);
            inputs.Add(Devices.Inputs.DetachGlassShtVac2);
            inputs.Add(Devices.Inputs.DetachGlassShtVac3);
            // Add Detach cylinder detection inputs
            inputs.Add(Devices.Inputs.DetachCyl1Up);
            inputs.Add(Devices.Inputs.DetachCyl1Down);
            inputs.Add(Devices.Inputs.DetachCyl2Up);
            inputs.Add(Devices.Inputs.DetachCyl2Down);
            // Add Detach fix fixture cylinder detection inputs
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl11Fw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl11Bw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl12Fw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl12Bw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl21Fw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl21Bw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl22Fw);
            inputs.Add(Devices.Inputs.DetachFixFixtureCyl22Bw);
            
            return inputs;
        }

        // Transfer Shutter Tab Inputs
        private ObservableCollection<IDInput> GetTransferShutterLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Glass detection inputs
            inputs.Add(Devices.Inputs.AlignStageLGlassDettect1);
            inputs.Add(Devices.Inputs.AlignStageLGlassDettect2);
            inputs.Add(Devices.Inputs.AlignStageLGlassDettect3);
            // Add Vacuum detection inputs
            inputs.Add(Devices.Inputs.TransferInShuttleLVac);
            // Add Transfer In Shuttle Left rotate detection inputs
            inputs.Add(Devices.Inputs.TransferInShuttleL0Degree);
            inputs.Add(Devices.Inputs.TransferInShuttleL180Degree);
            return inputs;
        }

        private ObservableCollection<IDInput> GetTransferShutterRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Glass detection inputs
            inputs.Add(Devices.Inputs.AlignStageRGlassDetect1);
            inputs.Add(Devices.Inputs.AlignStageRGlassDetect2);
            inputs.Add(Devices.Inputs.AlignStageRGlassDetect3);
            // Add Vacuum detection inputs
            inputs.Add(Devices.Inputs.TransferInShuttleRVac);
            // Add Transfer In Shuttle Right rotate detection inputs
            inputs.Add(Devices.Inputs.TransferInShuttleR0Degree);
            inputs.Add(Devices.Inputs.TransferInShuttleR180Degree);
            return inputs;
        }

        // Transfer Rotation Tab Inputs
        private ObservableCollection<IDInput> GetTransferRotationLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Vacuum detection inputs
            inputs.Add(Devices.Inputs.TrRotateLeftVac1);
            inputs.Add(Devices.Inputs.TrRotateLeftVac2);
            inputs.Add(Devices.Inputs.TrRotateLeftRotVac);
            // Add Transfer Rotation Left cylinder detection inputs
            inputs.Add(Devices.Inputs.TrRotateLeft0Degree);
            inputs.Add(Devices.Inputs.TrRotateLeft180Degree);
            inputs.Add(Devices.Inputs.TrRotateLeftFw);
            inputs.Add(Devices.Inputs.TrRotateLeftBw);
            inputs.Add(Devices.Inputs.TrRotateLeftUp);
            inputs.Add(Devices.Inputs.TrRotateLeftDown);
            return inputs;
        }

        private ObservableCollection<IDInput> GetTransferRotationRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Vacuum detection inputs
            inputs.Add(Devices.Inputs.TrRotateRightVac1);
            inputs.Add(Devices.Inputs.TrRotateRightVac2);
            inputs.Add(Devices.Inputs.TrRotateRightRotVac);
            // Add Transfer Rotation Right cylinder detection inputs
            inputs.Add(Devices.Inputs.TrRotateRight0Degree);
            inputs.Add(Devices.Inputs.TrRotateRight180Degree);
            inputs.Add(Devices.Inputs.TrRotateRightFw);
            inputs.Add(Devices.Inputs.TrRotateRightBw);
            inputs.Add(Devices.Inputs.TrRotateRightUp);
            inputs.Add(Devices.Inputs.TrRotateRightDown);
            return inputs;
        }

        // Unload Transfer Tab Inputs
        private ObservableCollection<IDInput> GetUnloadTransferLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Unload Transfer Left vacuum detection inputs
            inputs.Add(Devices.Inputs.UnloadTransferLVac);
            // Add Unload Align vacuum detection inputs
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac1);
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac2);
            // Add Unload Robot cylinder detection inputs
            inputs.Add(Devices.Inputs.UnloadRobotCyl1Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl1Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl2Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl2Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl3Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl3Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl4Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl4Down);
            // Add Unload Align cylinder detection inputs
            inputs.Add(Devices.Inputs.UnloadAlignCyl1Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl1Down);
            inputs.Add(Devices.Inputs.UnloadAlignCyl2Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl2Down);
            return inputs;
        }

        private ObservableCollection<IDInput> GetUnloadTransferRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Unload Transfer Right vacuum detection inputs
            inputs.Add(Devices.Inputs.UnloadTransferRVac);
            // Add Unload Align vacuum detection inputs
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac3);
            inputs.Add(Devices.Inputs.UnloadGlassAlignVac4);
            // Add Unload Robot cylinder detection inputs
            inputs.Add(Devices.Inputs.UnloadRobotCyl1Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl1Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl2Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl2Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl3Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl3Down);
            inputs.Add(Devices.Inputs.UnloadRobotCyl4Up);
            inputs.Add(Devices.Inputs.UnloadRobotCyl4Down);
            // Add Unload Align cylinder detection inputs
            inputs.Add(Devices.Inputs.UnloadAlignCyl3Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl3Down);
            inputs.Add(Devices.Inputs.UnloadAlignCyl4Up);
            inputs.Add(Devices.Inputs.UnloadAlignCyl4Down);
            return inputs;
        }

        // Clean Tab Inputs
        private ObservableCollection<IDInput> GetWETCleanLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add WET Clean Left detection inputs
            inputs.Add(Devices.Inputs.WetCleanLeftFeedingRollerDetect);
            // Add WET Clean Left pusher cylinder detection inputs
            inputs.Add(Devices.Inputs.WetCleanPusherLeftUp);
            inputs.Add(Devices.Inputs.WetCleanPusherLeftDown);
            // Add WET Clean Left brush cylinder detection inputs
            inputs.Add(Devices.Inputs.WetCleanBrushLeftUp);
            inputs.Add(Devices.Inputs.WetCleanBrushLeftDown);
            return inputs;
        }

        private ObservableCollection<IDInput> GetWETCleanRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add WET Clean Right detection inputs
            inputs.Add(Devices.Inputs.WetCleanRightFeedingRollerDetect);
            // Add WET Clean Right pusher cylinder detection inputs
            inputs.Add(Devices.Inputs.WetCleanPusherRightUp);
            inputs.Add(Devices.Inputs.WetCleanPusherRightDown);
            // Add WET Clean Right brush cylinder detection inputs
            inputs.Add(Devices.Inputs.WetCleanBrushRightUp);
            inputs.Add(Devices.Inputs.WetCleanBrushRightDown);
            return inputs;
        }

        private ObservableCollection<IDInput> GetAFCleanLeftInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add AF Clean Left detection inputs
            inputs.Add(Devices.Inputs.AfCleanLeftFeedingRollerDetect);
            // Add AF Clean Left pusher cylinder detection inputs
            inputs.Add(Devices.Inputs.AfCleanPusherLeftUp);
            inputs.Add(Devices.Inputs.AfCleanPusherLeftDown);
            // Add AF Clean Left brush cylinder detection inputs
            inputs.Add(Devices.Inputs.AfCleanBrushLeftUp);
            inputs.Add(Devices.Inputs.AfCleanBrushLeftDown);
            return inputs;
        }

        private ObservableCollection<IDInput> GetAFCleanRightInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add AF Clean Right detection inputs
            inputs.Add(Devices.Inputs.AfCleanRightFeedingRollerDetect);
            // Add AF Clean Right pusher cylinder detection inputs
            inputs.Add(Devices.Inputs.AfCleanPusherRightUp);
            inputs.Add(Devices.Inputs.AfCleanPusherRightDown);
            // Add AF Clean Right brush cylinder detection inputs
            inputs.Add(Devices.Inputs.AfCleanBrushRightUp);
            inputs.Add(Devices.Inputs.AfCleanBrushRightDown);
            return inputs;
        }

        // Glass Transfer Tab Inputs
        private ObservableCollection<IDInput> GetGlassTransferInputs()
        {
            ObservableCollection<IDInput> inputs = new ObservableCollection<IDInput>();
            // Add Glass Transfer vacuum detection inputs
            inputs.Add(Devices.Inputs.GlassTransferVac1);
            inputs.Add(Devices.Inputs.GlassTransferVac2);
            inputs.Add(Devices.Inputs.GlassTransferVac3);
            // Add Glass Transfer cylinder detection inputs
            inputs.Add(Devices.Inputs.GlassTransferCyl1Up);
            inputs.Add(Devices.Inputs.GlassTransferCyl1Down);
            inputs.Add(Devices.Inputs.GlassTransferCyl2Up);
            inputs.Add(Devices.Inputs.GlassTransferCyl2Down);
            inputs.Add(Devices.Inputs.GlassTransferCyl3Up);
            inputs.Add(Devices.Inputs.GlassTransferCyl3Down);
            return inputs;
        }

        // Unload Tab Inputs

        #endregion

        #region GetOutputs
        // CSTLoadUnload Tab Outputs
        private ObservableCollection<IDOutput> GetInWorkConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add In Cassette button lamp outputs
            outputs.Add(Devices.Outputs.InCompleteButtonLamp);
            outputs.Add(Devices.Outputs.InMutingButtonLamp);
            // Add In Cassette light curtain muting output
            outputs.Add(Devices.Outputs.InCstLightCurtainMuting1);
            outputs.Add(Devices.Outputs.InCstLightCurtainMuting2);
            // Add In CST Stopper outputs
            outputs.Add(Devices.Outputs.InCstStopperUp);
            outputs.Add(Devices.Outputs.InCstStopperDown);
            // Add In CST Fix cylinder outputs
            outputs.Add(Devices.Outputs.InCstFixCyl1Fw);
            outputs.Add(Devices.Outputs.InCstFixCyl1Bw);
            outputs.Add(Devices.Outputs.InCstFixCyl2Fw);
            outputs.Add(Devices.Outputs.InCstFixCyl2Bw);
            // Add In CST Tilt cylinder outputs
            outputs.Add(Devices.Outputs.InCstTiltCylUp);
            outputs.Add(Devices.Outputs.InCstTiltCylDown);
            // Add In CV Support outputs
            outputs.Add(Devices.Outputs.InCvSupportUp);
            outputs.Add(Devices.Outputs.InCvSupportDown);
            outputs.Add(Devices.Outputs.InCvSupportBufferUp);
            outputs.Add(Devices.Outputs.InCvSupportBufferDown);

            return outputs;
        }
        
        private ObservableCollection<IDOutput> GetOutWorkConveyorOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Out Cassette button lamp outputs
            outputs.Add(Devices.Outputs.OutCompleteButtonLamp);
            outputs.Add(Devices.Outputs.OutMutingButtonLamp);
            // Add Out Cassette light curtain muting output
            outputs.Add(Devices.Outputs.OutCstLightCurtainMuting1);
            outputs.Add(Devices.Outputs.OutCstLightCurtainMuting2);
            // Add Out CST Stopper outputs
            outputs.Add(Devices.Outputs.OutCstStopperUp);
            outputs.Add(Devices.Outputs.OutCstStopperDown);
            // Add Out CST Fix cylinder outputs
            outputs.Add(Devices.Outputs.OutCstFixCyl1Fw);
            outputs.Add(Devices.Outputs.OutCstFixCyl1Bw);
            outputs.Add(Devices.Outputs.OutCstFixCyl2Fw);
            outputs.Add(Devices.Outputs.OutCstFixCyl2Bw);
            // Add Out CST Tilt cylinder outputs
            outputs.Add(Devices.Outputs.OutCstTiltCylUp);
            outputs.Add(Devices.Outputs.OutCstTiltCylDown);
            // Add Out CV Support outputs
            outputs.Add(Devices.Outputs.OutCvSupportUp);
            outputs.Add(Devices.Outputs.OutCvSupportDown);
            outputs.Add(Devices.Outputs.OutCvSupportBufferUp);
            outputs.Add(Devices.Outputs.OutCvSupportBufferDown);
            return outputs;
        }

        // Detach Tab Outputs
        private ObservableCollection<IDOutput> GetTransferFixtureOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Transfer Fixture outputs
            outputs.Add(Devices.Outputs.TransferFixtureUp);
            outputs.Add(Devices.Outputs.TransferFixtureDown);
            // Add Transfer Fixture clamp outputs
            outputs.Add(Devices.Outputs.TransferFixture1Clamp);
            outputs.Add(Devices.Outputs.TransferFixture1Unclamp);
            outputs.Add(Devices.Outputs.TransferFixture2Clamp);
            outputs.Add(Devices.Outputs.TransferFixture2Unclamp);
            
            return outputs;
        }
        
        private ObservableCollection<IDOutput> GetDetachOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Detach glass shuttle vacuum outputs
            outputs.Add(Devices.Outputs.DetachGlassShtVac1OnOff);
            outputs.Add(Devices.Outputs.DetachGlassShtVac2OnOff);
            outputs.Add(Devices.Outputs.DetachGlassShtVac3OnOff);
            // Add Detach cylinder outputs
            outputs.Add(Devices.Outputs.DetachCyl1Up);
            outputs.Add(Devices.Outputs.DetachCyl1Down);
            outputs.Add(Devices.Outputs.DetachCyl2Up);
            outputs.Add(Devices.Outputs.DetachCyl2Down);
            // Add Detach fix fixture cylinder outputs
            outputs.Add(Devices.Outputs.DetachFixFixtureCyl1Fw);
            outputs.Add(Devices.Outputs.DetachFixFixtureCyl1Bw);
            outputs.Add(Devices.Outputs.DetachFixFixtureCyl2Fw);
            outputs.Add(Devices.Outputs.DetachFixFixtureCyl2Bw);
            return outputs;
        }

        // Glass Transfer Tab Outputs
        private ObservableCollection<IDOutput> GetGlassTransferOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Glass Transfer vacuum outputs
            outputs.Add(Devices.Outputs.GlassTransferVac1OnOff);
            outputs.Add(Devices.Outputs.GlassTransferVac2OnOff);
            outputs.Add(Devices.Outputs.GlassTransferVac3OnOff);
            // Add Glass Transfer cylinder outputs
            outputs.Add(Devices.Outputs.GlassTransferCyl1Up);
            outputs.Add(Devices.Outputs.GlassTransferCyl1Down);
            outputs.Add(Devices.Outputs.GlassTransferCyl2Up);
            outputs.Add(Devices.Outputs.GlassTransferCyl2Down);
            outputs.Add(Devices.Outputs.GlassTransferCyl3Up);
            outputs.Add(Devices.Outputs.GlassTransferCyl3Down);
            return outputs;
        }

        // Transfer Shutter Tab Outputs
        private ObservableCollection<IDOutput> GetTransferShutterLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Vacuum outputs
            outputs.Add(Devices.Outputs.TransferInShuttleLVacOnOff);
            // Add Transfer In Shuttle Left rotate outputs
            outputs.Add(Devices.Outputs.TransferInShuttleL0Degree);
            outputs.Add(Devices.Outputs.TransferInShuttleL180Degree);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetTransferShutterRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Vacuum outputs
            outputs.Add(Devices.Outputs.TransferInShuttleRVacOnOff);
            // Add Transfer In Shuttle Right rotate outputs
            outputs.Add(Devices.Outputs.TransferInShuttleR0Degree);
            outputs.Add(Devices.Outputs.TransferInShuttleR180Degree);
            return outputs;
        }

        // Transfer Rotation Tab Outputs
        private ObservableCollection<IDOutput> GetTransferRotationLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Vacuum outputs
            outputs.Add(Devices.Outputs.TrRotateLeftVac1OnOff);
            outputs.Add(Devices.Outputs.TrRotateLeftVac2OnOff);
            outputs.Add(Devices.Outputs.TrRotateLeftRotVacOnOff);
            // Add Transfer Rotation Left cylinder control outputs
            outputs.Add(Devices.Outputs.TrRotateLeft0Degree);
            outputs.Add(Devices.Outputs.TrRotateLeft180Degree);
            outputs.Add(Devices.Outputs.TrRotateLeftFw);
            outputs.Add(Devices.Outputs.TrRotateLeftBw);
            outputs.Add(Devices.Outputs.TrRotateLeftUp);
            outputs.Add(Devices.Outputs.TrRotateLeftDown);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetTransferRotationRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Vacuum outputs
            outputs.Add(Devices.Outputs.TrRotateRightVac1OnOff);
            outputs.Add(Devices.Outputs.TrRotateRightVac2OnOff);
            outputs.Add(Devices.Outputs.TrRotateRightRotVacOnOff);
            // Add Transfer Rotation Right cylinder control outputs
            outputs.Add(Devices.Outputs.TrRotateRight0Degree);
            outputs.Add(Devices.Outputs.TrRotateRight180Degree);
            outputs.Add(Devices.Outputs.TrRotateRightFw);
            outputs.Add(Devices.Outputs.TrRotateRightBw);
            outputs.Add(Devices.Outputs.TrRotateRightUp);
            outputs.Add(Devices.Outputs.TrRotateRightDown);
            return outputs;
        }

        // Unload Transfer Tab Outputs
        private ObservableCollection<IDOutput> GetUnloadTransferLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Unload Transfer Left vacuum outputs
            outputs.Add(Devices.Outputs.UnloadTransferLVacOnOff);
            // Add Unload Align vacuum outputs
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac1OnOff);
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac2OnOff);
            // Add Unload Robot cylinder control outputs
            outputs.Add(Devices.Outputs.UnloadRobotCyl1Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl2Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl3Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl4Down);
            // Add Unload Align cylinder control outputs
            outputs.Add(Devices.Outputs.UnloadAlignCyl1Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl2Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl3Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl4Up);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetUnloadTransferRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add Unload Transfer Right vacuum outputs
            outputs.Add(Devices.Outputs.UnloadTransferRVacOnOff);
            // Add Unload Align vacuum outputs
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac3OnOff);
            outputs.Add(Devices.Outputs.UnloadGlassAlignVac4OnOff);
            // Add Unload Robot cylinder control outputs
            outputs.Add(Devices.Outputs.UnloadRobotCyl1Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl2Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl3Down);
            outputs.Add(Devices.Outputs.UnloadRobotCyl4Down);
            // Add Unload Align cylinder control outputs
            outputs.Add(Devices.Outputs.UnloadAlignCyl1Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl2Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl3Up);
            outputs.Add(Devices.Outputs.UnloadAlignCyl4Up);
            return outputs;
        }

        // Clean Tab Outputs
        private ObservableCollection<IDOutput> GetWETCleanLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add WET Clean Left cylinder control outputs
            outputs.Add(Devices.Outputs.WetCleanPusherLeftUp);
            outputs.Add(Devices.Outputs.WetCleanPusherLeftDown);
            outputs.Add(Devices.Outputs.WetCleanBrushLeftDown);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetWETCleanRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add WET Clean Right cylinder control outputs
            outputs.Add(Devices.Outputs.WetCleanPusherRightUp);
            outputs.Add(Devices.Outputs.WetCleanPusherRightDown);
            outputs.Add(Devices.Outputs.WetCleanBrushRightDown);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetAFCleanLeftOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add AF Clean Left cylinder control outputs
            outputs.Add(Devices.Outputs.AfCleanPusherLeftUp);
            outputs.Add(Devices.Outputs.AfCleanPusherLeftDown);
            outputs.Add(Devices.Outputs.AfCleanBrushLeftDown);
            return outputs;
        }

        private ObservableCollection<IDOutput> GetAFCleanRightOutputs()
        {
            ObservableCollection<IDOutput> outputs = new ObservableCollection<IDOutput>();
            // Add AF Clean Right cylinder control outputs
            outputs.Add(Devices.Outputs.AfCleanPusherRightUp);
            outputs.Add(Devices.Outputs.AfCleanPusherRightDown);
            outputs.Add(Devices.Outputs.AfCleanBrushRightDown);
            return outputs;
        }

        #endregion

        #region GetDetailProcess
        #endregion

        public TeachViewModel(Devices devices,
            RecipeList recipeList,
            RecipeSelector recipeSelector,
            Processes processes)
        {
            Devices = devices;
            RecipeList = recipeList;
            Processes = processes;
            RecipeSelector = recipeSelector;
            
            // Initialize Commands
            CylinderForwardCommand = new RelayCommand<ICylinder>(CylinderForward);
            CylinderBackwardCommand = new RelayCommand<ICylinder>(CylinderBackward);

            DetachUnitTeaching = new UnitTeachingViewModel("Detach",recipeSelector);
            DetachUnitTeaching.Cylinders = GetDetachCylinders();
            DetachUnitTeaching.Motions = GetDetachMotions();
            DetachUnitTeaching.Inputs = GetDetachInputs();
            DetachUnitTeaching.Outputs = GetDetachOutputs();
            DetachUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.DetachRecipe;
            DetachUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("DetachImage");

            GlassTransferUnitTeaching = new UnitTeachingViewModel("Glass Transfer",recipeSelector);
            GlassTransferUnitTeaching.Cylinders = GetGlassTransferCylinders();
            GlassTransferUnitTeaching.Motions = GetGlassTransferMotions();
            GlassTransferUnitTeaching.Inputs = GetGlassTransferInputs();
            GlassTransferUnitTeaching.Outputs = GetGlassTransferOutputs();
            GlassTransferUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.GlassTransferRecipe;
            GlassTransferUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("GlassTransferImage");


            TransferInShuttleLeftUnitTeaching = new UnitTeachingViewModel("Transfer In Shuttle Left", recipeSelector);
            TransferInShuttleLeftUnitTeaching.Cylinders = GetTransferShutterLeftCylinders();
            TransferInShuttleLeftUnitTeaching.Motions = GetTransferShutterLeftMotions();
            TransferInShuttleLeftUnitTeaching.Inputs = GetTransferShutterLeftInputs();
            TransferInShuttleLeftUnitTeaching.Outputs = GetTransferShutterLeftOutputs();
            TransferInShuttleLeftUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe;
            TransferInShuttleLeftUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage");

            TransferInShuttleRightUnitTeaching = new UnitTeachingViewModel("Transfer In Shuttle Right", recipeSelector);
            TransferInShuttleRightUnitTeaching.Cylinders = GetTransferShutterRightCylinders();
            TransferInShuttleRightUnitTeaching.Motions = GetTransferShutterRightMotions();
            TransferInShuttleRightUnitTeaching.Inputs = GetTransferShutterRightInputs();
            TransferInShuttleRightUnitTeaching.Outputs = GetTransferShutterRightOutputs();
            TransferInShuttleRightUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.TransferInShuttleRightRecipe;
            TransferInShuttleRightUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage");

            WETCleanLeftUnitTeaching = new CleanUnitTeachingViewModel("WET Clean Left", recipeSelector);
            WETCleanLeftUnitTeaching.Cylinders = GetWETCleanLeftCylinders();
            WETCleanLeftUnitTeaching.Motions = GetWETCleanLeftMotions();
            WETCleanLeftUnitTeaching.Inputs = GetWETCleanLeftInputs();
            WETCleanLeftUnitTeaching.Outputs = GetWETCleanLeftOutputs();
            WETCleanLeftUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.WetCleanLeftRecipe;
            WETCleanLeftUnitTeaching.Winder = Devices.TorqueControllers.WETCleanLeftWinder;
            WETCleanLeftUnitTeaching.UnWinder = Devices.TorqueControllers.WETCleanLeftUnWinder;
            WETCleanLeftUnitTeaching.Regulator = Devices.Regulators.WetCleanLRegulator;
            WETCleanLeftUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");

            WETCleanRightUnitTeaching = new CleanUnitTeachingViewModel("WET Clean Right", recipeSelector);
            WETCleanRightUnitTeaching.Cylinders = GetWETCleanRightCylinders();
            WETCleanRightUnitTeaching.Motions = GetWETCleanRightMotions();
            WETCleanRightUnitTeaching.Inputs = GetWETCleanRightInputs();
            WETCleanRightUnitTeaching.Outputs = GetWETCleanRightOutputs();
            WETCleanRightUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.WetCleanRightRecipe;
            WETCleanRightUnitTeaching.Winder = Devices.TorqueControllers.WETCleanRightWinder;
            WETCleanRightUnitTeaching.UnWinder = Devices.TorqueControllers.WETCleanRightUnWinder;
            WETCleanRightUnitTeaching.Regulator = Devices.Regulators.WetCleanRRegulator;
            WETCleanRightUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");

            TransferRotationLeftUnitTeaching = new UnitTeachingViewModel("Transfer Rotation Left", recipeSelector);
            TransferRotationLeftUnitTeaching.Cylinders = GetTransferRotationLeftCylinders();
            TransferRotationLeftUnitTeaching.Motions = GetTransferRotationLeftMotions();
            TransferRotationLeftUnitTeaching.Inputs = GetTransferRotationLeftInputs();
            TransferRotationLeftUnitTeaching.Outputs = GetTransferRotationLeftOutputs();
            TransferRotationLeftUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.TransferRotationLeftRecipe;
            TransferRotationLeftUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage");

            TransferRotationRightUnitTeaching = new UnitTeachingViewModel("Transfer Rotation Right", recipeSelector);
            TransferRotationRightUnitTeaching.Cylinders = GetTransferRotationRightCylinders();
            TransferRotationRightUnitTeaching.Motions = GetTransferRotationRightMotions();
            TransferRotationRightUnitTeaching.Inputs = GetTransferRotationRightInputs();
            TransferRotationRightUnitTeaching.Outputs = GetTransferRotationRightOutputs();
            TransferRotationRightUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.TransferRotationRightRecipe;
            TransferRotationRightUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage");

            AFCleanLeftUnitTeaching = new CleanUnitTeachingViewModel("AF Clean Left", recipeSelector);
            AFCleanLeftUnitTeaching.Cylinders = GetAFCleanLeftCylinders();
            AFCleanLeftUnitTeaching.Motions = GetAFCleanLeftMotions();
            AFCleanLeftUnitTeaching.Inputs = GetAFCleanLeftInputs();
            AFCleanLeftUnitTeaching.Outputs = GetAFCleanLeftOutputs();
            AFCleanLeftUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.AfCleanLeftRecipe;
            AFCleanLeftUnitTeaching.Winder = Devices.TorqueControllers.AFCleanLeftWinder;
            AFCleanLeftUnitTeaching.UnWinder = Devices.TorqueControllers.AFCleanLeftUnWinder;
            AFCleanLeftUnitTeaching.Regulator = Devices.Regulators.AfCleanLRegulator;
            AFCleanLeftUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");

            AFCleanRightUnitTeaching = new CleanUnitTeachingViewModel("AF Clean Right", recipeSelector);
            AFCleanRightUnitTeaching.Cylinders = GetAFCleanRightCylinders();
            AFCleanRightUnitTeaching.Motions = GetAFCleanRightMotions();
            AFCleanRightUnitTeaching.Inputs = GetAFCleanRightInputs();
            AFCleanRightUnitTeaching.Outputs = GetAFCleanRightOutputs();
            AFCleanRightUnitTeaching.Recipe = RecipeSelector.CurrentRecipe.AfCleanRightRecipe;
            AFCleanRightUnitTeaching.Winder = Devices.TorqueControllers.AFCleanRightWinder;
            AFCleanRightUnitTeaching.UnWinder = Devices.TorqueControllers.AFCleanRightUnWinder;
            AFCleanRightUnitTeaching.Regulator = Devices.Regulators.AfCleanRRegulator;
            AFCleanRightUnitTeaching.Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage");


            UnitTeachings = new ObservableCollection<UnitTeachingViewModel>()
            {
                DetachUnitTeaching,
                GlassTransferUnitTeaching,

                TransferInShuttleLeftUnitTeaching,
                TransferInShuttleRightUnitTeaching,

                WETCleanLeftUnitTeaching,
                WETCleanRightUnitTeaching,

                TransferRotationLeftUnitTeaching,
                TransferRotationRightUnitTeaching,
                
                AFCleanLeftUnitTeaching,
                AFCleanRightUnitTeaching
            };

            SelectedUnitTeaching = UnitTeachings.First();
        }

        #region Commands
        public ICommand CylinderForwardCommand { get; }
        public ICommand CylinderBackwardCommand { get; }

        public void CylinderForward(ICylinder cylinder)
        {
            if (cylinder == null) return;

            // Check interlock before moving
            if (!CylinderInterLock(cylinder, true, out string CylinderInterlockMsg))
            {
                MessageBoxEx.ShowDialog($"{CylinderInterlockMsg}");
                return;
            }
            if (cylinder.CylinderType == ECylinderType.ForwardBackward ||
                    cylinder.CylinderType == ECylinderType.ForwardBackwardReverse ||
                    cylinder.CylinderType == ECylinderType.UpDown ||
                    cylinder.CylinderType == ECylinderType.UpDownReverse ||
                    cylinder.CylinderType == ECylinderType.RightLeft ||
                    cylinder.CylinderType == ECylinderType.RightLeftReverse ||
                    cylinder.CylinderType == ECylinderType.GripUngrip ||
                    cylinder.CylinderType == ECylinderType.GripUngripReverse ||
                    cylinder.CylinderType == ECylinderType.AlignUnalign ||
                    cylinder.CylinderType == ECylinderType.AlignUnalignReverse ||
                    cylinder.CylinderType == ECylinderType.LockUnlock ||
                    cylinder.CylinderType == ECylinderType.LockUnlockReverse ||
                    cylinder.CylinderType == ECylinderType.FlipUnflip ||
                    cylinder.CylinderType == ECylinderType.FlipUnflipReverse ||
                    cylinder.CylinderType == ECylinderType.ClampUnclamp ||
                    cylinder.CylinderType == ECylinderType.ClampUnclampReverse
                    )
            {
                cylinder.Backward();
                return;
            }
            cylinder.Forward();

            // For OnOff cylinders in simulation mode, auto-set input feedback
#if SIMULATION
            if (cylinder.CylinderType == ECylinderType.GripUngrip)
            {
                // Use reflection to access protected InForward property
                var inForwardProperty = cylinder.GetType().GetProperty("InForward", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var inForward = inForwardProperty?.GetValue(cylinder) as List<IDInput>;
                
                if (inForward?.Count > 0)
                {
                    // Set vacuum detect input to true when vacuum is turned on
                    SimulationInputSetter.SetSimModbusInput(inForward[0], true);
                }
            }
#endif
        }

        public void CylinderBackward(ICylinder cylinder)
        {
            if (cylinder == null) return;

            // Check interlock before moving
            if (!CylinderInterLock(cylinder, false, out string CylinderInterlockMsg))
            {
                MessageBoxEx.ShowDialog($"{CylinderInterlockMsg}");
                return;
            }
            if (cylinder.CylinderType == ECylinderType.ForwardBackward ||
                    cylinder.CylinderType == ECylinderType.ForwardBackwardReverse ||
                    cylinder.CylinderType == ECylinderType.UpDown ||
                    cylinder.CylinderType == ECylinderType.UpDownReverse ||
                    cylinder.CylinderType == ECylinderType.RightLeft ||
                    cylinder.CylinderType == ECylinderType.RightLeftReverse ||
                    cylinder.CylinderType == ECylinderType.GripUngrip ||
                    cylinder.CylinderType == ECylinderType.GripUngripReverse ||
                    cylinder.CylinderType == ECylinderType.AlignUnalign ||
                    cylinder.CylinderType == ECylinderType.AlignUnalignReverse ||
                    cylinder.CylinderType == ECylinderType.LockUnlock ||
                    cylinder.CylinderType == ECylinderType.LockUnlockReverse ||
                    cylinder.CylinderType == ECylinderType.FlipUnflip ||
                    cylinder.CylinderType == ECylinderType.FlipUnflipReverse ||
                    cylinder.CylinderType == ECylinderType.ClampUnclamp ||
                    cylinder.CylinderType == ECylinderType.ClampUnclampReverse
                    )
            {
                cylinder.Forward();
                return;
            }
            cylinder.Backward();

            // For OnOff cylinders in simulation mode, auto-set input feedback
#if SIMULATION
            if (cylinder.CylinderType == ECylinderType.GripUngrip)
            {
                // Use reflection to access protected InForward property
                var inForwardProperty = cylinder.GetType().GetProperty("InForward", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var inForward = inForwardProperty?.GetValue(cylinder) as List<IDInput>;
                
                if (inForward?.Count > 0)
                {
                    // Set vacuum detect input to false when vacuum is turned off
                    SimulationInputSetter.SetSimModbusInput(inForward[0], false);
                }
            }
#endif
        }

        // InterLock for Cylinder
        public bool CylinderInterLock(ICylinder cylinder, bool isForward, out string CylinderInterlockMsg)
        {
            CylinderInterlockMsg = string.Empty;

            // Interlock for TrRotateLeftRotate
            if (cylinder.Name.Contains("TrRotateLeftRotate") || cylinder.Name.Contains("TrRotateLeftFwBw"))
            {
                CylinderInterlockMsg = "Need Transfer Rotation ZAxis at Ready Position before Moving";
                return Devices?.MotionsInovance?.TransferRotationLZAxis?.IsOnPosition(RecipeSelector?.CurrentRecipe?.TransferRotationLeftRecipe?.ZAxisReadyPosition ?? 0) == true;
            }
            // Interlock for TrRotateRightRotate
            if (cylinder.Name.Contains("TrRotateRightRotate") || cylinder.Name.Contains("TrRotateRightFwBw"))
            {
                CylinderInterlockMsg = "Need Transfer Rotation ZAxis at Ready Position before Moving";
                return Devices?.MotionsInovance?.TransferRotationRZAxis?.IsOnPosition(RecipeSelector?.CurrentRecipe?.TransferRotationRightRecipe?.ZAxisReadyPosition ?? 0) == true;
            }

            return true;
        }
        #endregion
    }

}
