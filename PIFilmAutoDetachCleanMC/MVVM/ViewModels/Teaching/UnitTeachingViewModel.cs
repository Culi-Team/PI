using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.InOut;
using EQX.Core.Motion;
using EQX.Core.Recipe;
using EQX.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching
{
    public class UnitTeachingViewModel : ViewModelBase, INameable
    {
        public UnitTeachingViewModel(string name, RecipeSelector recipeSelector)
        {
            Name = name;
            RecipeSelector = recipeSelector;
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
                        // Check Interlocks before move
                        if (!CheckAxisCylinderAndPositionBeforMove(moveToDescription))
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

                    spt.Value = Motions.FirstOrDefault(m => m.Name == motionName)!.Status.ActualPosition;
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
        public bool CheckAxisCylinderAndPositionBeforMove(string moveToDescription)
        {
            RecipeSelector recipeSelector = App.AppHost!.Services.GetRequiredService<RecipeSelector>();
            Devices devices = App.AppHost!.Services.GetRequiredService<Devices>();
            if (Name == "CST Load")
            {
                if (moveToDescription == "In Cassette T Axis Load Position"
                    || moveToDescription == "In Cassette T Axis Work Position")
                {
                    if (devices.Cylinders.InCvSupportUpDown.IsBackward == false
                        || devices.Cylinders.InCvSupportBufferUpDown.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [InCvSupportUpDown] ," +
                            $"\n [InCvSupportBufferUpDown] , " +
                            $"\n need [Backward] befor move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
                if (moveToDescription == "Out Cassette T Axis Load Position"
                    || moveToDescription == "Out Cassette T Axis Work Position")
                {
                    if (devices.Cylinders.OutCvSupportUpDown.IsBackward == false
                        || devices.Cylinders.OutCvSupportBufferUpDown.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [OutCvSupportUpDown] ," +
                            $"\n [OutCvSupportBufferUpDown] ," +
                            $"\n need [Backward] befor move to " +
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
                    if (devices.Cylinders.InCvSupportUpDown.IsBackward == false
                        || devices.Cylinders.InCvSupportBufferUpDown.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [InCvSupportUpDown] ," +
                            $"\n [InCvSupportBufferUpDown] ," +
                            $"\n need [Backward] befor move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
                if (moveToDescription == "Out Cassette T Axis Load Position"
                    || moveToDescription == "Out Cassette T Axis Work Position")
                {
                    if (devices.Cylinders.OutCvSupportUpDown.IsBackward == false
                        || devices.Cylinders.OutCvSupportBufferUpDown.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [OutCvSupportUpDown] ," +
                            $"\n [OutCvSupportBufferUpDown] ," +
                            $"\n need [Backward] befor move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "Transfer Fixture")
            {
                if (moveToDescription == "Transfer Fixture Y Axis Load Position"
                    || moveToDescription == "Transfer Fixture Y Axis Unload Position")
                {
                    if (devices.Cylinders.TransferFixtureUpDown.IsBackward == true)
                    {
                        MessageBoxEx.ShowDialog($"Cylinder [TransferFixtureUpDown] ," +
                            $"\n need [move Up] befor move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
                if (moveToDescription == "Transfer Fixture Y Axis Load Position"
                    || moveToDescription == "Transfer Fixture Y Axis Unload Position")
                {
                    var detachGlassZAxis = devices.MotionsInovance.DetachGlassZAxis;
                    if (detachGlassZAxis != null && detachGlassZAxis.Status.ActualPosition
                        != recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetachReadyPosition)
                    {
                        MessageBoxEx.ShowDialog($"[Detach Z Axis] ," +
                            $"\n need Move [Ready Position] befor move to ," +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "Detach")
            {
                if (moveToDescription == "Sht Tr X Axis Detach Position"
                    || moveToDescription == "Sht Tr X Axis Detach Check Position"
                    || moveToDescription == "Sht Tr X Axis Unload Position")
                {
                    var shuttleTransferZAxis = devices.MotionsAjin.ShuttleTransferZAxis;
                    if (shuttleTransferZAxis != null && shuttleTransferZAxis.Status.ActualPosition
                        != recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisDetachReadyPosition)
                    {
                        MessageBoxEx.ShowDialog($"[Shuttle Transfer Z Axis] ," +
                            $"\n Need Move [Ready Position] befor move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "Glass Transfer")
            {
                if (moveToDescription == "Glass Transfer Y Axis Ready Position"
                    || moveToDescription == "Glass Transfer Y Axis Pick Position"
                    || moveToDescription == "Glass Transfer Y Axis Left Place Position"
                    || moveToDescription == "Glass Transfer Y Axis Right Place Position")
                {
                    var transferInShuttleLZAxis = devices.MotionsInovance.TransferInShuttleLZAxis;
                    var transferInShuttleRZAxis = devices.MotionsInovance.TransferInShuttleRZAxis;
                    if ((transferInShuttleLZAxis != null && transferInShuttleLZAxis.Status.ActualPosition
                        != recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisReadyPosition)
                        || (transferInShuttleRZAxis != null && transferInShuttleRZAxis.Status.ActualPosition
                        != recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[Transfer InShuttle Left Z Axis] ," +
                            $"\n [Transfer InShuttle Right Z Axis]," +
                            $"\n need move [Ready Position] befor move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }

                }
            }
            if (Name == "Transfer In Shuttle Left")
            {
                if (moveToDescription == "Y Axis Ready Position"
                    || moveToDescription == "Y Axis Pick Position 1"
                    || moveToDescription == "Y Axis Pick Position 2"
                    || moveToDescription == "Y Axis Pick Position 3"
                    || moveToDescription == "Y Axis Place Position")
                {
                    var transferInShuttleLZAxis = devices.MotionsInovance.TransferInShuttleLZAxis;
                    if (transferInShuttleLZAxis != null && transferInShuttleLZAxis.Status.ActualPosition
                        != recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.ZAxisReadyPosition)
                    {
                        MessageBoxEx.ShowDialog($"[Transfer InShuttle Left Z Axis] ," +
                            $"\n  need move [Ready Position] befor move to" +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "Transfer In Shuttle Right")
            {
                if (moveToDescription == "Y Axis Ready Position"
                    || moveToDescription == "Y Axis Pick Position 1"
                    || moveToDescription == "Y Axis Pick Position 2"
                    || moveToDescription == "Y Axis Pick Position 3"
                    || moveToDescription == "Y Axis Place Position")
                {
                    var transferInShuttleRZAxis = devices.MotionsInovance.TransferInShuttleRZAxis;
                    if (transferInShuttleRZAxis != null && transferInShuttleRZAxis.Status.ActualPosition
                        != recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.ZAxisReadyPosition)
                    {
                        MessageBoxEx.ShowDialog($"[Transfer InShuttle Right Z Axis] ," +
                            $"\n need move [Ready Position] befor move to" +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "WET Clean Left")
            {
                var outShuttleLXAxis = devices.MotionsAjin.OutShuttleLXAxis;
                var transferRotationLZAxis = devices.MotionsInovance.TransferRotationLZAxis;
                if (moveToDescription == "X Axis Ready Position"
                    ||moveToDescription == "X Axis Load Position"
                    || moveToDescription == "Y Axis Load Position"
                    || moveToDescription == "X Axis Clean Horizontal Position"
                    || moveToDescription == "Y Axis Clean Horizontal Position"
                    || moveToDescription == "X Axis Clean Vertical Position"
                    || moveToDescription == "Y Axis Clean Vertical Position"
                    || moveToDescription == "X Axis Unload Position")
                {
                    if ((devices.Cylinders.WetCleanBrushLeftUpDown.IsBackward == false
                        || devices.Cylinders.WetCleanPusherLeftUpDown.IsBackward == false)
                        && (outShuttleLXAxis != null && outShuttleLXAxis.Status.ActualVelocity
                        != recipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisLoadPosition)
                        && (devices.Cylinders.TransferInShuttleLRotate.IsBackward == false
                        || transferRotationLZAxis != null && transferRotationLZAxis.Status.ActualPosition
                        != recipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[WetCleanBrushLeftUpDown] ," +
                            $"\n [WetCleanBrushLeftUpDown] ," +
                            $"\n [WetCleanPusherLeftUpDown]," +
                            $"\n need [Backward] and [AF X Axis Position] ," +
                            $"\n need move to [Unload Position] befor move to " +
                            $"\n[{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "AF Clean Left")
            {
                var inShuttleLXAxis = devices.MotionsAjin.InShuttleLXAxis;
                var transferRotationLZAxis = devices.MotionsInovance.TransferRotationLZAxis;
                if (moveToDescription == "X Axis UnLoad Position"
                    || moveToDescription == "Y Axis Load Position"
                    || moveToDescription == "X Axis Clean Horizontal Position"
                    || moveToDescription == "Y Axis Clean Horizontal Position"
                    || moveToDescription == "X Axis Clean Vertical Position"
                    || moveToDescription == "Y Axis Clean Vertical Position"
                    || moveToDescription == "X Axis load Position")
                {
                    if ((devices.Cylinders.AFCleanBrushLeftUpDown.IsBackward == false
                        || devices.Cylinders.AFCleanPusherLeftUpDown.IsBackward == false)
                        && (inShuttleLXAxis != null && inShuttleLXAxis.Status.ActualVelocity
                        != recipeSelector.CurrentRecipe.WetCleanLeftRecipe.XAxisLoadPosition)
                        && (devices.Cylinders.TransferInShuttleLRotate.IsBackward == false
                        || transferRotationLZAxis != null && transferRotationLZAxis.Status.ActualPosition
                        != recipeSelector.CurrentRecipe.TransferRotationLeftRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[WetCleanBrushLeftUpDown] ," +
                            $"\n need [Backward] befor move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "WET Clean Right")
            {
                var outShuttleRXAxis = devices.MotionsAjin.OutShuttleRXAxis;
                var transferRotationRZAxis = devices.MotionsInovance.TransferRotationRZAxis;
                if (moveToDescription == "X Axis Load Position"
                    || moveToDescription == "Y Axis Load Position"
                    || moveToDescription == "X Axis Clean Horizontal Position"
                    || moveToDescription == "Y Axis Clean Horizontal Position"
                    || moveToDescription == "X Axis Clean Vertical Position"
                    || moveToDescription == "Y Axis Clean Vertical Position"
                    || moveToDescription == "X Axis Unload Position")
                {
                    if ((devices.Cylinders.WetCleanBrushRightUpDown.IsBackward == false
                        || devices.Cylinders.WetCleanPusherRightUpDown.IsBackward == false)
                        && (outShuttleRXAxis != null && outShuttleRXAxis.Status.ActualVelocity
                        != recipeSelector.CurrentRecipe.AfCleanRightRecipe.XAxisLoadPosition)
                        && (devices.Cylinders.TransferInShuttleRRotate.IsBackward == false
                        || transferRotationRZAxis != null && transferRotationRZAxis.Status.ActualPosition
                        != recipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[WetCleanBrushLeftUpDown] ," +
                            $"\n [WetCleanBrushLeftUpDown] ," +
                            $"\n [WetCleanPusherLeftUpDown] ," +
                            $"\n [WetCleanPusherLeftUpDown] ," +
                            $"\n [Backward] and [AF X Axis Left Position]" +
                            $"\n move to [Unload Position] befor move to " +
                            $"\n[{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "AF Clean Right")
            {
                var inShuttleRXAxis = devices.MotionsAjin.InShuttleRXAxis;
                var transferRotationRZAxis = devices.MotionsInovance.TransferRotationRZAxis;
                if (moveToDescription == "X Axis UnLoad Position"
                    || moveToDescription == "Y Axis Load Position"
                    || moveToDescription == "X Axis Clean Horizontal Position"
                    || moveToDescription == "Y Axis Clean Horizontal Position"
                    || moveToDescription == "X Axis Clean Vertical Position"
                    || moveToDescription == "Y Axis Clean Vertical Position"
                    || moveToDescription == "X Axis load Position")
                {
                    if ((devices.Cylinders.AFCleanBrushRightUpDown.IsBackward == false
                        || devices.Cylinders.AFCleanPusherRightUpDown.IsBackward == false)
                        && (inShuttleRXAxis != null && inShuttleRXAxis.Status.ActualVelocity
                        != recipeSelector.CurrentRecipe.WetCleanRightRecipe.XAxisLoadPosition)
                        && (devices.Cylinders.TransferInShuttleRRotate.IsBackward == false
                        || transferRotationRZAxis != null && transferRotationRZAxis.Status.ActualPosition
                        != recipeSelector.CurrentRecipe.TransferRotationRightRecipe.ZAxisReadyPosition))
                    {
                        MessageBoxEx.ShowDialog($"[AFCleanBrushRightUpDown] ," +
                            $"\n [AFCleanPusherRightUpDown] ," +
                            $"\n need [Backward] befor move to " +
                            $"\n[{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "Unload Transfer Left")
            {
                if (moveToDescription == "Y Axis Ready Position"
                    || moveToDescription == "Y Axis Pick Position"
                    || moveToDescription == "Y Axis Place Position 1"
                    || moveToDescription == "Y Axis Place Position 2"
                    || moveToDescription == "Y Axis Place Position 3"
                    || moveToDescription == "Y Axis Place Position 4")
                {
                    var glassUnloadLAxis = devices.MotionsInovance.GlassUnloadLZAxis;
                    if (glassUnloadLAxis != null && glassUnloadLAxis.Status.ActualPosition 
                        != recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.ZAxisReadyPosition)
                    {
                        MessageBoxEx.ShowDialog($"[Glass Unload Left Z Axis] ," +
                            $"\n need move to [Ready Position] befor move to " +
                            $"\n [{moveToDescription}]");
                        return false;
                    }
                }
            }
            if (Name == "Unload Transfer Right")
            {
                if (moveToDescription == "Y Axis Ready Position"
                    || moveToDescription == "Y Axis Pick Position"
                    || moveToDescription == "Y Axis Place Position 1"
                    || moveToDescription == "Y Axis Place Position 2"
                    || moveToDescription == "Y Axis Place Position 3"
                    || moveToDescription == "Y Axis Place Position 4")
                {
                    var glassUnloadRAxis = devices.MotionsInovance.GlassUnloadRZAxis;
                    if (glassUnloadRAxis != null && glassUnloadRAxis.Status.ActualPosition 
                        != recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.ZAxisReadyPosition)
                    {
                        MessageBoxEx.ShowDialog($"[Glass Unload Right Z Axis] ," +
                            $"\n move to [Ready Position] befor move to " +
                            $"\n[{moveToDescription}]");
                        return false;
                    }
                }
            }
            return true; 
        }
    }
}
