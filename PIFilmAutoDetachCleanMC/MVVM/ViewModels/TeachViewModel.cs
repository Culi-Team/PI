using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Process;
using EQX.Core.Sequence;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cylinder;
using PIFilmAutoDetachCleanMC.Process;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class TeachViewModel : ViewModelBase
    {        
        #region Properties
        public Devices Devices { get; }
        public MachineStatus MachineStatus { get; }
        public RecipeList RecipeList;
        public RecipeSelector RecipeSelector;
        public Processes Processes;
        public DataViewModel DataViewModel { get; }
        public ObservableCollection<IProcess<ESequence>> ProcessListTeaching => GetProcessList();
        private IProcess<ESequence> _selectedProcess;
        public IProcess<ESequence> SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
                SelectedPropertyProcess();
            }
        }
        private ObservableCollection<ICylinder> _cylinders;
        public ObservableCollection<ICylinder> Cylinders
        {
            get { return _cylinders; }
            set { _cylinders = value; OnPropertyChanged(); }
        }
        private ObservableCollection<IMotion> _motions;
        public ObservableCollection<IMotion> Motions
        {
            get { return _motions; }
            set { _motions = value; OnPropertyChanged(); }
        }
        private ObservableCollection<IDOutput> _outputs;
        public ObservableCollection<IDOutput> Outputs
        {
            get { return _outputs; }
            set { _outputs = value; OnPropertyChanged(); }
        }
        private ObservableCollection<IDInput> _inputs;
        public ObservableCollection<IDInput> Inputs
        {
            get { return _inputs; }
            set { _inputs = value; OnPropertyChanged(); }
        }
        private ObservableCollection<PositionTeaching> _positionTeachings;
        public ObservableCollection<PositionTeaching> PositionTeachings
        {
            get { return _positionTeachings; }
            set { _positionTeachings = value; OnPropertyChanged(); }
        }
        private ObservableCollection<SemiAutoTeaching> _semiAutoTeachings;

        public ObservableCollection<SemiAutoTeaching> SemiAutoTeachings
        {
            get { return _semiAutoTeachings; }
            set { _semiAutoTeachings = value; OnPropertyChanged(); }
        }
        #endregion

        #region GetCylinder
        private ObservableCollection<ICylinder> TransferFixtureCylinder()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            //cylinders.Add(Devices.Cylinders.RightIn_DoorLock);
            //cylinders.Add(Devices.Cylinders.RightIn_SliderLock);
            //cylinders.Add(Devices.Cylinders.RightIn_CSTUpDown);
            //cylinders.Add(Devices.Cylinders.RightIn_SPTUpDown);
            return cylinders;
        }
        private ObservableCollection<ICylinder> GetTrayNGCylinder()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            //cylinders.Add(Devices.Cylinders.NGTray_DoorLock);
            //cylinders.Add(Devices.Cylinders.NGTray_SliderLock);
            //cylinders.Add(Devices.Cylinders.NGTray_TrayAlign);
            return cylinders;
        }
        private ObservableCollection<ICylinder> GetTraySuplierCylinder()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            //cylinders.Add(Devices.Cylinders.Supplier_DoorLock);
            //cylinders.Add(Devices.Cylinders.Supplier_SliderLock);
            return cylinders;
        }
        //private ObservableCollection<ICylinder> GetLeftInTransCylinder()
        //{
        //    ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
        //    //if (isTrayToCst)
        //    //{
        //    //    //cylinders.Add(Devices.Cylinders.LeftIn_TransBWFW);
        //    //    //cylinders.Add(Devices.Cylinders.LeftIn_TransUpDown);
        //    //    //cylinders.Add(Devices.Cylinders.LeftIn_TransGrip);
        //    //}
        //    //return cylinders;
        //}
        private ObservableCollection<ICylinder> GetTrayNGTransCylinder()
        {
            ObservableCollection<ICylinder> cylinders = new ObservableCollection<ICylinder>();
            //cylinders.Add(Devices.Cylinders.NGTray_TransBWFW);
            //cylinders.Add(Devices.Cylinders.NGTray_TransUpDown);
            return cylinders;
        }

        #endregion

        #region GetProcess
        private ObservableCollection<IProcess<ESequence>> GetProcessList()
        {
            ObservableCollection<IProcess<ESequence>> processes = new ObservableCollection<IProcess<ESequence>>
            {
                Processes.TransferFixtureProcess,
                Processes.DetachProcess,
                Processes.WETCleanLeftProcess,
                Processes.WETCleanRightProcess,
                Processes.AFCleanLeftProcess,
                Processes.AFCleanRightProcess,
            };
            return processes;
        }
        #endregion

        #region GetDetailProcess
        public void Dispose()
        {
            Cylinders = null;
            Inputs = null;
            Outputs = null;
            Motions = null;
            PositionTeachings = null;
            SemiAutoTeachings = null;
        }
        private void SelectedPropertyProcess()
        {
            Dispose();
            if (SelectedProcess == Processes.TransferFixtureProcess)
            {
                Cylinders = TransferFixtureCylinder();
                //Motions = GetLeftInMotion();
                //PositionTeachings = GetLeftInPositionTeaching();
                //SemiAutoTeachings = GetLeftInSemiAutoTeaching();
                //Inputs = GetLeftInInput();
                //Outputs = GetLeftInOutput();
            }
            if (SelectedProcess == Processes.DetachProcess)
            {
                //Cylinders = GetRightInCylinder();
                //Motions = GetRightInMotion();
                //PositionTeachings = GetRightInPositionTeaching();
                //SemiAutoTeachings = GetRightInSemiAutoTeaching();
                //Inputs = GetRightInInput();
                //Outputs = GetRightInOutput();
            }
            if (SelectedProcess == Processes.WETCleanLeftProcess)
            {
                //Cylinders = GetTraySuplierCylinder();
                //Motions = GetTraySuplierMotion();
                //PositionTeachings = GetTraySuplierPositionTeaching();
                //SemiAutoTeachings = GetTraySuplierSemiAutoTeaching();
                //Inputs = GetTraySupplierInput();
                //Outputs = GetTraySupplierOutput();
            }
            if (SelectedProcess == Processes.WETCleanRightProcess)
            {
                //Cylinders = GetTrayNGCylinder();
                //Motions = GetTrayNGMotion();
                //PositionTeachings = GetTrayNGPositionTeaching();
                //SemiAutoTeachings = GetTrayNGSemiAutoTeaching();
                //Inputs = GetNGTrayInput();
                //Outputs = GetNGTrayOutput();
            }
            if (SelectedProcess == Processes.AFCleanLeftProcess)
            {
                //Cylinders = GetTrayNGTransCylinder();
                //Inputs = GetNGTranferInput();
                //Outputs = GetNGTranferOutput();

            }
            if (SelectedProcess == Processes.AFCleanRightProcess)
            {
                //Cylinders = GetLeftInTransCylinder();
                //Inputs = GetLeftInTranferInput();
                //Outputs = GetLeftInTranferOutput();
            }

        }
        #endregion

        public TeachViewModel(Devices devices, MachineStatus machineStatus, RecipeList recipeList, RecipeSelector recipeSelector, Processes processes, DataViewModel dataViewModel)
        {
            Devices = devices;
            MachineStatus = machineStatus;
            RecipeList = recipeList;
            Processes = processes;
            RecipeSelector = recipeSelector;
            DataViewModel = dataViewModel;
            SelectedProcess = ProcessListTeaching.FirstOrDefault();
        }

        public class PositionTeaching : ObservableObject
        {
            public PositionTeaching(RecipeSelector recipeSelector)
            {
                PositionChecker();
                _recipeSelector = recipeSelector;
            }
            private bool isActive;

            public string Name { get; set; }
            public bool IsActive
            {
                get => isActive;
                set
                {
                    isActive = value;
                    OnPropertyChanged("IsActive");
                }
            }
            public double Position { get; set; }
            public IMotion Motion { get; set; }
            public ICommand PositionTeachingCommand
            {
                get => new RelayCommand<object>((p) =>
                {
                    string MoveTo = (string)Application.Current.Resources["str_MoveTo"];
                    if (MessageBoxEx.ShowDialog($"{MoveTo} {Name} ?") == true)
                    {
                        Motion.MoveAbs(Position);
                    }
                });
            }
            private System.Timers.Timer _timer;
            private readonly RecipeSelector _recipeSelector;

            public void PositionChecker()
            {
                _timer = new System.Timers.Timer(1000);
                _timer.Elapsed += CheckPosition;
                _timer.AutoReset = true;
                _timer.Enabled = true;
            }

            private void CheckPosition(object sender, ElapsedEventArgs e)
            {
                IsActive = Motion.IsOnPosition(Position);
            }
            //public RecipeSelector RecipeSelector { get; set; }
            public double PositionRecipe { get; set; }
            public ICommand SaveRecipeCommand => new RelayCommand(SaveRecipe);

            private void SaveRecipe()
            {
                if (MessageBoxEx.ShowDialog($"{(string)Application.Current.Resources["str_Save"]}?") == true)
                {
                    Position = Math.Round(Motion.Status.ActualPosition, 3);
                    SavePosition(Motion.Id, Name);
                }
            }
            public void SavePosition(int id, string name)
            {
                switch (id)
                {
                    case 2:
                        if (name.Equals(Application.Current.Resources["str_ZAxisChangePosition"]))
                        {
                            //_recipeSelector.CurrentRecipe.LeftInRecipe.ChangePosition = Position;
                        }
                        if (name.Equals(Application.Current.Resources["str_ZTiltPosition"]))
                        {
                            //((CassetteRecipe)_recipeSelector.CurrentRecipe.LeftInRecipe).ZTiltPosition = Position;
                        }
                        break;
                    case 3:
                        if (name.Equals(Application.Current.Resources["str_ZAxisChangePosition"]))
                        {
                            //_recipeSelector.CurrentRecipe.RightInRecipe.ChangePosition = Position;
                        }
                        if (name.Equals(Application.Current.Resources["str_ZTiltPosition"]))
                        {
                            //_recipeSelector.CurrentRecipe.RightInRecipe.ZTiltPosition = Position;
                        }
                        break;
                    case 4:
                        if (name.Equals(Application.Current.Resources["str_ZAxisChangePosition"]))
                        {
                            //_recipeSelector.CurrentRecipe.TraySuplierRecipe.ZAxisChangePosition = Position;
                        }
                        if (name.Equals(Application.Current.Resources["str_TraySupplierZAxisStartWorkingPosition"]))
                        {
                            //_recipeSelector.CurrentRecipe.TraySuplierRecipe.ZAxisStartWorkingPosition = Position;
                        }
                        break;
                    case 5:
                        if (name.Equals(Application.Current.Resources["str_ZAxisChangePosition"]))
                        {
                            //_recipeSelector.CurrentRecipe.NgTrayRecipe.ChangePosition = Position;
                        }
                        break;
                    default:
                        break;
                }
                _recipeSelector.Save();
            }

        }
        public class SemiAutoTeaching : ObservableObject
        {
            private string _name;
            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }

            public ICommand SemiAutoTeachingCommand { get; }

            public IProcess<ESequence> Process { get; set; }
            public IProcess<ESequence> ProcessRoot { get; set; }
            public ESequence ESequence { get; set; }
            public ESequence ESemiAutoSequence { get; set; }
            public MachineStatus MachineStatus { get; set; }

            public SemiAutoTeaching()
            {
                SemiAutoTeachingCommand = new RelayCommand(ExecuteCommand);
            }

            private void ExecuteCommand()
            {
                if (MachineStatus != null)
                {
                    MachineStatus.OPCommand = EOperationCommand.SemiAuto;
                    //MachineStatus.SemiAutoSequence = ESemiAutoSequence;
                }
            }
        }
    }
}
