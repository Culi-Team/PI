using CommunityToolkit.Mvvm.Input;
using EQX.Core.Device.Regulator;
using EQX.Core.Device.SyringePump;
using EQX.Core.TorqueController;
using EQX.Device.SyringePump;
using log4net;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching
{
    public enum ESyringePumpTestStep
    {
        Fill,
        Fill_Wait,
        DispensePort1,
        DispensePort1_Wait,
        DispensePort2,
        DispensePort2_Wait,
        DispensePort3,
        DispensePort3_Wait,
        DispensePort4,
        DispensePort4_Wait,
        DispensePort5,
        DispensePort5_Wait,
        DispensePort6,
        DispensePort6_Wait,
        End
    }
    public class CleanUnitTeachingViewModel : UnitTeachingViewModel
    {
        public CleanUnitTeachingViewModel(string name, RecipeSelector recipeSelector) : base(name, recipeSelector)
        {
            Name = name;
        }

        public ITorqueController Winder { get; set; }
        public ITorqueController UnWinder { get; set; }
        public ISyringePump SyringePump { get; set; }
        public IRegulator Regulator { get; set; }

        public ICommand SetPressureCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Regulator.SetPressure(((CleanRecipe)Recipe).CylinderPushPressure);
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
                    Winder.Run();
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
                    UnWinder.Run();
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
                                    SyringePump.SetSpeed(1);
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
                                    if (SyringePump.IsReady == false)
                                    {
                                        Thread.Sleep(100);
                                        break;
                                    }
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort1:
                                    if ((Recipe as CleanRecipe).UsePort1 == false)
                                    {
                                        step = (int)ESyringePumpTestStep.DispensePort2;
                                        break;
                                    }

                                    SyringePump.Dispense((Recipe as CleanRecipe).CleanVolume, 1);
                                    Thread.Sleep(delay);
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort1_Wait:
                                    if (SyringePump.IsReady == false)
                                    {
                                        Thread.Sleep(100);
                                        break;
                                    }
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort2:
                                    if ((Recipe as CleanRecipe).UsePort2 == false)
                                    {
                                        step = (int)ESyringePumpTestStep.DispensePort3;
                                        break;
                                    }

                                    SyringePump.Dispense((Recipe as CleanRecipe).CleanVolume, 2);
                                    Thread.Sleep(delay);
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort2_Wait:
                                    if (SyringePump.IsReady == false)
                                    {
                                        Thread.Sleep(100);
                                        break;
                                    }
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort3:
                                    if ((Recipe as CleanRecipe).UsePort3 == false)
                                    {
                                        step = (int)ESyringePumpTestStep.DispensePort4;
                                        break;
                                    }

                                    SyringePump.Dispense((Recipe as CleanRecipe).CleanVolume, 3);
                                    Thread.Sleep(delay);
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort3_Wait:
                                    if (SyringePump.IsReady == false)
                                    {
                                        Thread.Sleep(100);
                                        break;
                                    }
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort4:
                                    if ((Recipe as CleanRecipe).UsePort4 == false)
                                    {
                                        step = (int)ESyringePumpTestStep.DispensePort5;
                                        break;
                                    }

                                    SyringePump.Dispense((Recipe as CleanRecipe).CleanVolume, 4);
                                    Thread.Sleep(delay);
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort4_Wait:
                                    if (SyringePump.IsReady == false)
                                    {
                                        Thread.Sleep(100);
                                        break;
                                    }
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort5:
                                    if ((Recipe as CleanRecipe).UsePort5 == false)
                                    {
                                        step = (int)ESyringePumpTestStep.DispensePort6;
                                        break;
                                    }

                                    SyringePump.Dispense((Recipe as CleanRecipe).CleanVolume, 5);
                                    Thread.Sleep(delay);
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort5_Wait:
                                    if (SyringePump.IsReady == false)
                                    {
                                        Thread.Sleep(100);
                                        break;
                                    }
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort6:
                                    if ((Recipe as CleanRecipe).UsePort6 == false)
                                    {
                                        step++;
                                        break;
                                    }

                                    SyringePump.Dispense((Recipe as CleanRecipe).CleanVolume, 6);
                                    Thread.Sleep(delay);
                                    step++;
                                    break;
                                case ESyringePumpTestStep.DispensePort6_Wait:
                                    if (SyringePump.IsReady == false)
                                    {
                                        Thread.Sleep(100);
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
                    SyringePump.Stop();
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
    }
}
