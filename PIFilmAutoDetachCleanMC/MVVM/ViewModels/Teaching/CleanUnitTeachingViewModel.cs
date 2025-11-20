using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Device.Regulator;
using EQX.Core.Device.SyringePump;
using EQX.Core.TorqueController;
using EQX.Device.SpeedController;
using EQX.Device.SyringePump;
using EQX.Device.Torque;
using log4net;
using PIFilmAutoDetachCleanMC.Defines.Devices.Regulator;
using PIFilmAutoDetachCleanMC.Recipe;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching
{
    public enum ESyringePumpTestStep
    {
        Fill,
        Fill_Wait,
        Dispense,
        Dispense_Wait,
        End
    }

    public enum ESyringePumpFillStep
    {
        Initialize,
        Initialize_Wait,

        DispenseCount_Check,

        Dispense_Port8,
        Dispense_Port8_Wait,

        Fill_Port7,
        Fill_Port7_Wait,

        End
    }
    public class CleanUnitTeachingViewModel : UnitTeachingViewModel
    {
        public CleanUnitTeachingViewModel(string name, RecipeSelector recipeSelector) : base(name, recipeSelector)
        {
            Name = name;
            _pressureUpdateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _pressureUpdateTimer.Tick += PressureUpdateTimer_Tick;
            _pressureUpdateTimer.Start();
        }

        private readonly DispatcherTimer _pressureUpdateTimer;

        private IRegulator _regulator;
        private double _currentPressure;

        public IRegulator Regulator
        {
            get => _regulator;
            set
            {
                if (SetProperty(ref _regulator, value))
                {
                    UpdateCurrentPressure();
                }
            }
        }

        public double CurrentPressure
        {
            get => _currentPressure;
            set
            {
                _currentPressure = value;
                OnPropertyChanged();
            }
        }
        public DX3000TorqueController Winder { get; set; }
        public DX3000TorqueController UnWinder { get; set; }
        public ISyringePump SyringePump { get; set; }

        public ICommand SetPressureCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Regulator.SetPressure(((CleanRecipe)Recipe).CylinderPressure);
                    UpdateCurrentPressure();
                });
            }
        }

        public ICommand WinderSetTorqueCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Winder.SetTorque(((CleanRecipe)Recipe).WinderTorque);
                });
            }
        }

        public ICommand UnWinderSetTorqueCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (this is VinylCleanTeachingViewModel)
                    {
                        UnWinder.SetTorque(((VinylCleanRecipe)Recipe).UnWinderTorque);
                        return;
                    }
                    UnWinder.SetTorque(((CleanRecipe)Recipe).UnWinderTorque);
                });
            }
        }

        public ICommand WinderRunCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Winder.Run(true);
                });
            }
        }

        public ICommand WinderStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Winder.Stop();
                });
            }
        }

        public ICommand UnWinderRunCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UnWinder.Run(false);
                });
            }
        }

        public ICommand UnWinderStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UnWinder.Stop();
                });
            }
        }

        public ICommand SyringePumpRunTestCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    int delay = 50;
                    isSyringePumpRunTest = true;
                    int step = 0;
                    Thread thread = new Thread(() =>
                    {
                        while (isSyringePumpRunTest)
                        {
                            switch ((ESyringePumpTestStep)step)
                            {
                                case ESyringePumpTestStep.Fill:
                                    SyringePump.SetSpeed(5);
                                    Thread.Sleep(300);
                                    SyringePump.SetAcceleration(20);
                                    Thread.Sleep(300);
                                    SyringePump.SetDeccelation(20);
                                    Thread.Sleep(300);

                                    SyringePump.Fill(1.0);
                                    Thread.Sleep(delay);
                                    step++;
                                    break;
                                case ESyringePumpTestStep.Fill_Wait:
                                    if (SyringePump.IsReady() == false)
                                    {
                                        Thread.Sleep(delay);
                                        break;
                                    }
                                    step++;
                                    break;
                                case ESyringePumpTestStep.Dispense:
                                    List<int> ports = new List<int>();
                                    if ((Recipe as CleanRecipe).UsePort1)
                                    {
                                        ports.Add(1);
                                    }
                                    if ((Recipe as CleanRecipe).UsePort2)
                                    {
                                        ports.Add(2);
                                    }
                                    if ((Recipe as CleanRecipe).UsePort3)
                                    {
                                        ports.Add(3);
                                    }
                                    if ((Recipe as CleanRecipe).UsePort4)
                                    {
                                        ports.Add(4);
                                    }
                                    if ((Recipe as CleanRecipe).UsePort5)
                                    {
                                        ports.Add(5);
                                    }
                                    if ((Recipe as CleanRecipe).UsePort6)
                                    {
                                        ports.Add(6);
                                    }

                                    SyringePump.Dispense((Recipe as CleanRecipe).CleanVolume, ports.ToArray());
                                    Thread.Sleep(delay);
                                    step++;
                                    break;
                                case ESyringePumpTestStep.Dispense_Wait:
                                    if(SyringePump.IsReady() == false)
                                    {
                                        Thread.Sleep(delay);
                                        break;
                                    }
                                    step++;
                                    break;
                                case ESyringePumpTestStep.End:
                                    isSyringePumpRunTest = false;
                                    break;
                            }
                        }

                    });
                    thread.Start();
                });
            }
        }

        public ICommand SyringePumpStopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    isSyringePumpRunTest = false;
                    isSyringePumpFill = false;
                    SyringePump.Stop();
                });
            }
        }


        public ICommand FillCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    isSyringePumpFill = true;
                    int step = 0;
                    int dispenseCount = 0;
                    Thread thread = new Thread(() =>
                    {
                        while (isSyringePumpFill)
                        {
                            switch ((ESyringePumpFillStep)step)
                            {
                                case ESyringePumpFillStep.Initialize:
                                    SyringePump.SetSpeed(5);
                                    Thread.Sleep(200);
                                    SyringePump.SetAcceleration(20);
                                    Thread.Sleep(200);
                                    SyringePump.SetDeccelation(20);
                                    Thread.Sleep(200);
                                    step++;
                                    break;
                                case ESyringePumpFillStep.Initialize_Wait:
                                    if (SyringePump.IsReady() == false)
                                    {
                                        Thread.Sleep(100);
                                        break;
                                    }
                                    step++;
                                    break;
                                case ESyringePumpFillStep.DispenseCount_Check:
                                    if (dispenseCount >= 5)
                                    {
                                        step = (int)ESyringePumpFillStep.End;
                                        break;
                                    }
                                    step++;
                                    break;
                                case ESyringePumpFillStep.Dispense_Port8:
                                    SyringePump.Dispense(1.0, 8);
                                    Thread.Sleep(100);
                                    step++;
                                    break;
                                case ESyringePumpFillStep.Dispense_Port8_Wait:
                                    if (SyringePump.IsReady() == false)
                                    {
                                        Thread.Sleep(100);
                                        break;
                                    }
                                    step++;
                                    break;
                                case ESyringePumpFillStep.Fill_Port7:
                                    SyringePump.Fill(1.0);
                                    Thread.Sleep(100);
                                    step++;
                                    break;
                                case ESyringePumpFillStep.Fill_Port7_Wait:
                                    if (SyringePump.IsReady() == false)
                                    {
                                        Thread.Sleep(100);
                                        break;
                                    }

                                    dispenseCount++;
                                    step = (int)ESyringePumpFillStep.DispenseCount_Check;
                                    break;
                                case ESyringePumpFillStep.End:
                                    isSyringePumpFill = false;
                                    break;
                            }
                        }
                    });
                    thread.Start();
                });
            }
        }
        public ICommand SyringePumpInitializeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SyringePump.Initialize();
                });
            }
        }

        private bool isSyringePumpRunTest = false;
        private bool isSyringePumpFill = false;

        private void PressureUpdateTimer_Tick(object? sender, EventArgs e)
        {
            UpdateCurrentPressure();
        }

        private void UpdateCurrentPressure()
        {
            if (Regulator == null)
            {
                CurrentPressure = 0;
                return;
            }

            CurrentPressure = Regulator.GetPressure();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _pressureUpdateTimer.Stop();
                _pressureUpdateTimer.Tick -= PressureUpdateTimer_Tick;
            }
        }
    }
}
