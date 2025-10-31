using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Recipe;
using EQX.InOut;
using EQX.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Recipe;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching
{
    public class UnitTeachingViewModel : ViewModelBase, INameable
    {
        protected System.Timers.Timer timer;

        public UnitTeachingViewModel(string name, RecipeSelector recipeSelector)
        {
            Name = name;
            RecipeSelector = recipeSelector;
            timer = new System.Timers.Timer(100);
            timer.Elapsed += Timer_Elapsed;
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }

        public void EnableTimer()
        {
            timer.Start();
        }

        public void DisableTimer()
        {
            timer.Stop();
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (Inputs == null) return;

            foreach (var input in Inputs)
            {
                input.RaiseValueUpdated();
            }
        }

        public ObservableCollection<ICylinder> Cylinders { get; set; }
        public ObservableCollection<IMotion> Motions { get; set; }
        public ObservableCollection<IDInput> Inputs { get; set; }
        public ObservableCollection<IDOutput> Outputs { get; set; }
        public IRecipe Recipe { get; set; }
        public string Name { get; init; }
        public RecipeSelector RecipeSelector { get; }
        public ImageSource Image { get; set; }

        public ICommand MovePositionTeachingCommand
        {
            get
            {
                return new RelayCommand<object>((o) =>
                {
                    if (o is SinglePositionTeaching spt == false) return;
                    string MoveTo = (string)Application.Current.Resources["str_MoveTo"];

                    string motionName = spt.SinglePosition.Motion;
                    string moveToDescription = spt.SingleRecipeDescription.Description;

                    if (MessageBoxEx.ShowDialog($"{MoveTo} {moveToDescription} ?") == true)
                    {
                        // Check Interlocks beforee move
                        if (!CheckAxisCylinderAndPositionbeforeMove(moveToDescription))
                        {
                            return;
                        }
                        Motions.FirstOrDefault(m => m.Name.Contains(motionName))!.MoveAbs(spt.Value);
                    }
                });
            }
        }

        public ICommand GetCurrentPositionCommand
        {
            get
            {
                return new RelayCommand<object>((o) =>
                {
                    if (o is SinglePositionTeaching spt == false) return;

                    string motionName = spt.SinglePosition.Motion;

                    spt.Value = Motions.FirstOrDefault(m => m.Name.Contains(motionName))!.Status.ActualPosition;
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand<object>((o) =>
                {
                    if (MessageBoxEx.ShowDialog($"{(string)Application.Current.Resources["str_Save"]}?") == true)
                    {
                        RecipeSelector.Save();
                    }
                });
            }
        }

        public ICommand CylinderForwardCommand
        {
            get
            {
                return new RelayCommand<object>((o) =>
                {
                    if (o is ICylinder cylinder == false) return;
                    if (CheckInterlockCylinderbeforeeMove(cylinder) == false) return;
                    if (cylinder.CylinderType == ECylinderType.ForwardBackwardReverse ||
                        cylinder.CylinderType == ECylinderType.UpDownReverse ||
                        cylinder.CylinderType == ECylinderType.RightLeftReverse ||
                        cylinder.CylinderType == ECylinderType.GripUngripReverse ||
                        cylinder.CylinderType == ECylinderType.AlignUnalignReverse ||
                        cylinder.CylinderType == ECylinderType.LockUnlockReverse ||
                        cylinder.CylinderType == ECylinderType.FlipUnflipReverse ||
                        cylinder.CylinderType == ECylinderType.ClampUnclampReverse
                        )
                    {
                        cylinder.Backward();
                        return;
                    }
                    cylinder.Forward();
                });
            }
        }

        public ICommand CylinderBackwardCommand
        {
            get
            {
                return new RelayCommand<object>((o) =>
                {
                    if (o is ICylinder cylinder == false) return;

                    if (cylinder.CylinderType == ECylinderType.ForwardBackwardReverse ||
                        cylinder.CylinderType == ECylinderType.UpDownReverse ||
                        cylinder.CylinderType == ECylinderType.RightLeftReverse ||
                        cylinder.CylinderType == ECylinderType.GripUngripReverse ||
                        cylinder.CylinderType == ECylinderType.AlignUnalignReverse ||
                        cylinder.CylinderType == ECylinderType.LockUnlockReverse ||
                        cylinder.CylinderType == ECylinderType.FlipUnflipReverse ||
                        cylinder.CylinderType == ECylinderType.ClampUnclampReverse)
                    {
                        cylinder.Forward();
                        return;
                    }
                    cylinder.Backward();
                });
            }
        }

        private bool CheckInterlockCylinderbeforeeMove(ICylinder cylinder)
        {
            Devices devices = App.AppHost!.Services.GetRequiredService<Devices>();
            if (cylinder.Name == "TransferFixture_UpDownCyl")
            {
                if (devices.Cylinders.Detach_ClampCyl1.IsBackward == false ||
                    devices.Cylinders.Detach_ClampCyl2.IsBackward == false ||
                    devices.Cylinders.Detach_ClampCyl3.IsBackward == false ||
                    devices.Cylinders.Detach_ClampCyl4.IsBackward == false ||
                    devices.Cylinders.FixtureAlign_AlignCyl1.IsBackward == false ||
                    devices.Cylinders.FixtureAlign_AlignCyl2.IsBackward == false ||
                    devices.Cylinders.RemoveZone_ClampCyl1.IsBackward == false ||
                    devices.Cylinders.RemoveZone_ClampCyl2.IsBackward == false ||
                    devices.Cylinders.RemoveZone_ClampCyl3.IsBackward == false ||
                    devices.Cylinders.RemoveZone_ClampCyl4.IsBackward == false)
                {
                    MessageBoxEx.ShowDialog($"Cylinder Clamp Detach / Fixture Align / Remove Zone Unit need move Backward");
                    return false;
                }
            }

            return true;
        }

        public bool CheckAxisCylinderAndPositionbeforeMove(string moveToDescription)
        {
            RecipeSelector recipeSelector = App.AppHost!.Services.GetRequiredService<RecipeSelector>();
            Devices devices = App.AppHost!.Services.GetRequiredService<Devices>();
            if (Name == "CST Load")
            {
                if (moveToDescription == "In Cassette T Axis Load Position"
                    || moveToDescription == "In Cassette T Axis Work Position")
                {
                    if (devices.Cylinders.InWorkCV_SupportCyl1.IsBackward == false
                        || devices.Cylinders.InWorkCV_SupportCyl2.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [InCvSupportUpDown] ," +
                            $"\n [InCvSupportBufferUpDown] , " +
                            $"\n need [Backward] before move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
                if (moveToDescription == "Out Cassette T Axis Load Position"
                    || moveToDescription == "Out Cassette T Axis Work Position")
                {
                    if (devices.Cylinders.OutWorkCV_SupportCyl2.IsBackward == false
                        || devices.Cylinders.OutWorkCV_SupportCyl1.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [OutCvSupportUpDown] ," +
                            $"\n [OutCvSupportBufferUpDown] ," +
                            $"\n need [Backward] before move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "CST Unload")
            {
                if (moveToDescription == "In Cassette T Axis Load Position"
                    || moveToDescription == "In Cassette T Axis Work Position")
                {
                    if (devices.Cylinders.InWorkCV_SupportCyl1.IsBackward == false
                        || devices.Cylinders.InWorkCV_SupportCyl2.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [InCvSupportUpDown] ," +
                            $"\n [InCvSupportBufferUpDown] ," +
                            $"\n need [Backward] before move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
                if (moveToDescription == "Out Cassette T Axis Load Position"
                    || moveToDescription == "Out Cassette T Axis Work Position")
                {
                    if (devices.Cylinders.OutWorkCV_SupportCyl2.IsBackward == false
                        || devices.Cylinders.OutWorkCV_SupportCyl1.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [OutCvSupportUpDown] ," +
                            $"\n [OutCvSupportBufferUpDown] ," +
                            $"\n need [Backward] before move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "Transfer Fixture")
            {
                var detachGlassZAxis = devices.Motions.DetachGlassZAxis;
                if (moveToDescription == "Transfer Fixture Y Axis Load Position"
                    || moveToDescription == "Transfer Fixture Y Axis Unload Position")
                {
                    if (devices.Cylinders.TransferFixture_UpDownCyl.IsBackward == true)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [TransferFixtureUpDown] ," +
                            $"\n need [move Up] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                    if (detachGlassZAxis != null && detachGlassZAxis.Status.ActualPosition > recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisReadyPosition)
                    {
                        MessageBoxEx.ShowDialog($"[Detach Z Axis] ," +
                            $"\n need Move [Ready Position] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "Detach")
            {
                var shuttleTransferZAxis = devices.Motions.ShuttleTransferZAxis;
                if (moveToDescription == "Sht Tr X Axis Detach Position"
                    || moveToDescription == "Sht Tr X Axis Detach Check Position"
                    || moveToDescription == "Sht Tr X Axis Unload Position")
                {
                    if (shuttleTransferZAxis != null && !(shuttleTransferZAxis.IsOnPosition(recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisReadyPosition)))
                    {
                        MessageBoxEx.ShowDialog($"[Shuttle Transfer Z Axis] ," +
                            $"\n Need Move [Ready Position] before move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "Glass Transfer")
            {
                var transferInShuttleLZAxis = devices.Motions.TransferInShuttleLZAxis;
                var transferInShuttleRZAxis = devices.Motions.TransferInShuttleRZAxis;
                if (moveToDescription == "Glass Transfer Y Axis Ready Position"
                    || moveToDescription == "Glass Transfer Y Axis Pick Position"
                    || moveToDescription == "Glass Transfer Y Axis Left Place Position"
                    || moveToDescription == "Glass Transfer Y Axis Right Place Position")
                {
                    if (transferInShuttleLZAxis != null && !transferInShuttleLZAxis.IsOnPosition(recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[Transfer InShuttle Left Z Axis] ," +
                            $"\n need move [Ready Position] before move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                    if (transferInShuttleRZAxis != null && !transferInShuttleRZAxis.IsOnPosition(recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[Transfer InShuttle Right Z Axis] ," +
                            $"\n need move [Ready Position] before move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }

                }
            }
            if (Name == "Transfer In Shuttle Left")
            {
                var transferInShuttleLZAxis = devices.Motions.TransferInShuttleLZAxis;
                if (moveToDescription == "Y Axis Ready Position"
                    || moveToDescription == "Y Axis Pick Position 1"
                    || moveToDescription == "Y Axis Pick Position 2"
                    || moveToDescription == "Y Axis Pick Position 3"
                    || moveToDescription == "Y Axis Place Position")
                {
                    if (transferInShuttleLZAxis != null && !transferInShuttleLZAxis.IsOnPosition(recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[Transfer InShuttle Left Z Axis] ," +
                            $"\n  need move [Ready Position] before move to" +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "Transfer In Shuttle Right")
            {
                var transferInShuttleRZAxis = devices.Motions.TransferInShuttleRZAxis;
                if (moveToDescription == "Y Axis Ready Position"
                    || moveToDescription == "Y Axis Pick Position 1"
                    || moveToDescription == "Y Axis Pick Position 2"
                    || moveToDescription == "Y Axis Pick Position 3"
                    || moveToDescription == "Y Axis Place Position")
                {
                    if (transferInShuttleRZAxis != null && !transferInShuttleRZAxis.IsOnPosition(recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[Transfer InShuttle Right Z Axis] ," +
                            $"\n need move [Ready Position] before move to" +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "WET Clean Left")
            {
                var transferInShuttlerLeft = devices.Motions.TransferInShuttleLZAxis;
                var transferRotationLZAxis = devices.Motions.TransferRotationLZAxis;
                var inShuttleLYAxis = devices.Motions.InShuttleLYAxis;
                var outtShuttleLXAxis = devices.Motions.OutShuttleLXAxis;

                if (moveToDescription == "X Axis Ready Position"
                    || moveToDescription == "X Axis Load Position"
                    || moveToDescription == "X Axis Unload Position")
                {
                    if (transferInShuttlerLeft != null && !transferInShuttlerLeft.IsOnPosition(recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[Transfer InShuttle Left Z Axis]" +
                            $"\n befor move to [{moveToDescription}]");
                        return false;
                    }
                    if (transferRotationLZAxis != null && !transferRotationLZAxis.IsOnPosition(recipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[Transfer Rotation Left Z Axis]" +
                            $"\n befor move to [{moveToDescription}]");
                        return false;
                    }
                    if (inShuttleLYAxis != null && !inShuttleLYAxis.IsOnPosition(recipeSelector.CurrentRecipe.WetCleanLeftRecipe.YAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[InShuttle Left Y Axis] need move to " +
                            $"\n [Ready Position]" +
                            $"\n befor move to [{moveToDescription}]");
                        return false;
                    }
                    if (devices.Cylinders.WetCleanL_PusherCyl.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"[AFClean Left Pusher Cyl] move Up" +
                            $"\n befor move to [{moveToDescription}]");
                        return false;
                    }
                    if ((outtShuttleLXAxis != null && !outtShuttleLXAxis.IsOnPosition(recipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisCleanHorizontalPosition)
                        && (moveToDescription == "X Axis Unload Position")))
                    {
                        MessageBoxEx.ShowDialog($"[Out Shuttle Left X Axis],need [Move Ready] before move to " +
                            $" \n [{moveToDescription}]");
                        return false;
                    }
                }
            }

            if (Name == "Transfer Rotation Left")
            {
                if (moveToDescription == "Z Axis Pick Position" 
                    || moveToDescription == "Z Axis Place Position")
                {
                    if (devices.Cylinders.TransferRotationL_BwFwCyl.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [Transfer Rotation Left Backward Forward] ," +
                            $"\n need [move Backward] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                    if(devices.Cylinders.TransferRotationL_UpDownCyl.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [Transfer Rotation Left Up Down] ," +
                            $"\n need [move Backward] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }

            }

            if (Name == "AF Clean Left")
            {
                var outShuttleLXAxis = devices.Motions.OutShuttleLXAxis;
                var transferRotationLZAxis = devices.Motions.TransferRotationLZAxis;
                var glassUnloadYAxis = devices.Motions.GlassUnloadLYAxis;
                var glassUnloadZAxis = devices.Motions.GlassTransferZAxis;
                var inShuttleLXAxis = devices.Motions.InShuttleLXAxis;

                if (moveToDescription == "X Axis Ready Position"
                    || moveToDescription == "X Axis UnLoad Position"
                    || moveToDescription == "X Axis Load Position")
                {
                    if (transferRotationLZAxis != null && !transferRotationLZAxis.IsOnPosition(recipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($" [Transfer Rotation Left Z Axis] ," +
                            $"\n need [move Ready Position] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                    if (devices.Cylinders.AFCleanL_PusherCyl.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [AF Clean Pusher Left Up Down] ," +
                            $"\n need [move Up] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                    if ((glassUnloadYAxis != null && glassUnloadYAxis.Status.ActualPosition >= recipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisLeftPlacePosition)
                        && (glassUnloadZAxis != null && glassUnloadZAxis.Status.ActualPosition >= recipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisLeftPlacePosition))
                    {
                        MessageBoxEx.ShowDialog($"[Glass transfer Z Axis] ," +
                            $"\n need [move Ready Position] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                    if ((inShuttleLXAxis != null && !inShuttleLXAxis.IsOnPosition(recipeSelector.CurrentRecipe.WetCleanLeftRecipe.XAxisCleanHorizontalPosition)
                        && (moveToDescription == "X Axis Load Position")))
                    {
                        MessageBoxEx.ShowDialog($"[In Shuttle Left X Axis],need [Move Ready] before move to " +
                            $" \n [{moveToDescription}]");
                        return false;
                    }
                }

            }

            if (Name == "WET Clean Right")
            {
                var transferInShuttlerRight = devices.Motions.TransferInShuttleRZAxis;
                var transferRotationRZAxis = devices.Motions.TransferRotationRZAxis;
                var inShuttleRYAxis = devices.Motions.InShuttleRYAxis;
                var outtShuttleRXAxis = devices.Motions.OutShuttleRXAxis;

                if (moveToDescription == "X Axis Ready Position"
                    || moveToDescription == "X Axis Load Position"
                    || moveToDescription == "X Axis Unload Position")
                {
                    if (transferInShuttlerRight != null && !transferInShuttlerRight.IsOnPosition(recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[Transfer InShuttle Right Z Axis]" +
                            $"\n befor move to [{moveToDescription}]");
                        return false;
                    }
                    if (transferRotationRZAxis != null && !transferRotationRZAxis.IsOnPosition(recipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[Transfer Rotation Right Z Axis]" +
                            $"\n befor move to [{moveToDescription}]");
                        return false;
                    }
                    if (inShuttleRYAxis != null && !inShuttleRYAxis.IsOnPosition(recipeSelector.CurrentRecipe.WetCleanRightRecipe.YAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[InShuttle Right Y Axis] need move to [Ready Position]" +
                            $"\n befor move to [{moveToDescription}]");
                        return false;
                    }
                    if (devices.Cylinders.WetCleanR_PusherCyl.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"[AFClean Right Pusher Cyl] move Up" +
                            $"\n befor move to [{moveToDescription}]");
                        return false;
                    }
                    if ((outtShuttleRXAxis != null && outtShuttleRXAxis.Status.ActualPosition <= (recipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisCleanHorizontalPosition - 100)
                        && (moveToDescription == "X Axis Unload Position")))
                    {
                        MessageBoxEx.ShowDialog($"[Out Shuttle Right X Axis],need [Move Ready] before move to " +
                            $" \n [{moveToDescription}]");
                        return false;
                    }
                }

            }

            if (Name == "Transfer Rotation Right")
            {
                if (moveToDescription == "Z Axis Pick Position" 
                    || moveToDescription == "Z Axis Place Position")
                {
                    if (devices.Cylinders.TransferRotationR_BwFwCyl.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [Transfer Rotation Right Backward Forward] ," +
                            $"\n need [move Backward] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }

                    if(devices.Cylinders.TransferRotationR_UpDownCyl.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [Transfer Rotation Right Up Down] ," +
                            $"\n need [move Backward] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "AF Clean Right")
            {
                var outShuttleRXAxis = devices.Motions.OutShuttleRXAxis;
                var transferRotationRZAxis = devices.Motions.TransferRotationRZAxis;
                var glassUnloadYAxis = devices.Motions.GlassUnloadRYAxis;
                var glassUnloadZAxis = devices.Motions.GlassTransferZAxis;
                var inShuttleRXAxis = devices.Motions.InShuttleRXAxis;

                if (moveToDescription == "X Axis Ready Position"
                    || moveToDescription == "X Axis UnLoad Position"
                    || moveToDescription == "X Axis Load Position")
                {
                    if (transferRotationRZAxis != null && !transferRotationRZAxis.IsOnPosition(recipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($" [Transfer Rotation Right Z Axis] ," +
                            $"\n need [move Ready Position] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                    if (devices.Cylinders.AFCleanR_PusherCyl.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [AF Clean Pusher Right Up Down] ," +
                            $"\n need [move Up] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                    if ((glassUnloadYAxis != null && glassUnloadYAxis.Status.ActualPosition >= recipeSelector.CurrentRecipe.GlassTransferRecipe.YAxisRightPlacePosition)
                        && (glassUnloadZAxis != null && glassUnloadZAxis.Status.ActualPosition >= recipeSelector.CurrentRecipe.GlassTransferRecipe.ZAxisRightPlacePosition))
                    {
                        MessageBoxEx.ShowDialog($"[Glass transfer Z Axis] ," +
                            $"\n need [move Ready Position] before move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                    if ((inShuttleRXAxis != null && inShuttleRXAxis.Status.ActualPosition >= (recipeSelector.CurrentRecipe.WetCleanRightRecipe.XAxisCleanHorizontalPosition - 100))
                        && (moveToDescription == "X Axis Load Position"))
                    {
                        MessageBoxEx.ShowDialog($"[In Shuttle Left X Axis],need [Move Ready] before move to " +
                            $" \n [{moveToDescription}]");
                        return false;
                    }
                }

            }
            if (Name == "Unload Transfer Left")
            {
                var glassUnloadLAxis = devices.Motions.GlassUnloadLZAxis;
                if (moveToDescription == "Y Axis Ready Position"
                    || moveToDescription == "Y Axis Pick Position"
                    || moveToDescription == "Y Axis Place Position 1"
                    || moveToDescription == "Y Axis Place Position 2"
                    || moveToDescription == "Y Axis Place Position 3"
                    || moveToDescription == "Y Axis Place Position 4")
                {
                    if (glassUnloadLAxis != null && !glassUnloadLAxis.IsOnPosition(recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[Glass Unload Left Z Axis] ," +
                            $"\n need move to [Ready Position] before move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "Unload Transfer Right")
            {
                var glassUnloadRAxis = devices.Motions.GlassUnloadRZAxis;
                if (moveToDescription == "Y Axis Ready Position"
                    || moveToDescription == "Y Axis Pick Position"
                    || moveToDescription == "Y Axis Place Position 1"
                    || moveToDescription == "Y Axis Place Position 2"
                    || moveToDescription == "Y Axis Place Position 3"
                    || moveToDescription == "Y Axis Place Position 4")
                {
                    if (glassUnloadRAxis != null && !glassUnloadRAxis.IsOnPosition(recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[Glass Unload Right Z Axis] ," +
                            $"\n move to [Ready Position] before move to " +
                            $"\n[{moveToDescription}]");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
