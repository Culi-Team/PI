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
            if (moveToDescription == "In Cassette T Axis Load Position"
                || moveToDescription == "In Cassette T Axis Work Position")
            {
                if (devices.Cylinders.InCvSupportUpDown.IsBackward == false
                    || devices.Cylinders.InCvSupportBufferUpDown.IsBackward == false)
                {
                    MessageBoxEx.ShowDialog($"Cylinder [InCvSupportUpDown] and [InCvSupportBufferUpDown] need [Backward] befor move to [{moveToDescription}]");
                    return false;
                }
            }
            if (moveToDescription == "Out Cassette T Axis Load Position"
                || moveToDescription == "Out Cassette T Axis Work Position")
            {
                if (devices.Cylinders.OutCvSupportUpDown.IsBackward == false
                    || devices.Cylinders.OutCvSupportBufferUpDown.IsBackward == false)
                {
                    MessageBoxEx.ShowDialog($"Cylinder [OutCvSupportUpDown] and [OutCvSupportBufferUpDown] need [Backward] befor move to [{moveToDescription}]");
                    return false;
                }
            }
            if (moveToDescription == "Transfer Fixture Y Axis Load Position"
                || moveToDescription == "Transfer Fixture Y Axis Unload Position")
            {
                if (devices.Cylinders.TransferFixtureUpDown.IsBackward == true)
                {
                    MessageBoxEx.ShowDialog($"Cylinder [TransferFixtureUpDown] need [move Up] befor move to [{moveToDescription}]");
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
                    MessageBoxEx.ShowDialog($"[Detach Z Axis] need Move [Ready Position] befor move to [{moveToDescription}]");
                    return false;
                }
            }
            if (moveToDescription == "Sht Tr X Axis Detach Position"
                || moveToDescription == "Sht Tr X Axis Detach Check Position"
                || moveToDescription == "Sht Tr X Axis Unload Position")
            {
                var shuttleTransferZAxis = devices.MotionsAjin.ShuttleTransferZAxis;
                if (shuttleTransferZAxis != null && shuttleTransferZAxis.Status.ActualPosition 
                    != recipeSelector.CurrentRecipe.DetachRecipe.ShuttleTransferZAxisDetachReadyPosition)
                {
                    MessageBoxEx.ShowDialog($"[Shuttle Transfer Z Axis] need Move [Ready Position] befor move to [{moveToDescription}]");
                    return false;
                }
            }
            if (moveToDescription == "Glass Transfer Y Axis Ready Position"
                || moveToDescription == "Glass Transfer Y Axis Left Pick Position"
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
                    MessageBoxEx.ShowDialog($"[Transfer InShuttle Left Z Axis] , [Transfer InShuttle Right Z Axis] need move [Ready Position]");
                    return false;
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
                        MessageBoxEx.ShowDialog($"[Transfer InShuttle Left Z Axis] ,  need move [Ready Position]");
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
                        MessageBoxEx.ShowDialog($"[Transfer InShuttle Right Z Axis] need move [Ready Position]");
                        return false;
                    }
                }
            }
            if (Name == "WET Clean Left")
            {
                if (moveToDescription == "X Axis Load Position"
                    || moveToDescription == "Y Axis Load Position"
                    || moveToDescription == "X Axis Clean Horizontal Position"
                    || moveToDescription == "Y Axis Clean Horizontal Position"
                    || moveToDescription == "X Axis Clean Vertical Position"
                    || moveToDescription == "Y Axis Clean Vertical Position")
                {
                    if (devices.Cylinders.WetCleanBrushLeftUpDown.IsBackward == false 
                        || devices.Cylinders.WetCleanPusherLeftUpDown.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"[WetCleanBrushLeftUpDown] need [Backward]");
                        return false;
                    }
                }
                if ( moveToDescription == "X Axis Unload Position")
                {
                    var outShuttleLXAxis = devices.MotionsAjin.OutShuttleLXAxis;
                    if ((devices.Cylinders.WetCleanBrushLeftUpDown.IsBackward == false
                        || devices.Cylinders.WetCleanPusherLeftUpDown.IsBackward == false)
                        && (outShuttleLXAxis != null && outShuttleLXAxis.Status.ActualVelocity 
                        != recipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisLoadPosition))
                    // TODO: Thiếu điều kiện của cụm rotation
                    {
                        MessageBoxEx.ShowDialog($"[WetCleanBrushLeftUpDown] [WetCleanPusherLeftUpDown],  need [Backward] and [AF X Axis Position] move to [Unload Position]");
                        return false;
                    }
                }
            }
            if (Name == "AF Clean Left")
            {
                if (moveToDescription == "X Axis UnLoad Position"
                    || moveToDescription == "Y Axis Load Position"
                    || moveToDescription == "X Axis Clean Horizontal Position"
                    || moveToDescription == "Y Axis Clean Horizontal Position"
                    || moveToDescription == "X Axis Clean Vertical Position"
                    || moveToDescription == "Y Axis Clean Vertical Position")
                {
                    if (devices.Cylinders.AFCleanBrushLeftUpDown.IsBackward == false
                        || devices.Cylinders.AFCleanPusherLeftUpDown.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"[WetCleanBrushLeftUpDown] need [Backward]");
                        return false;
                    }
                }
                if (moveToDescription == "X Axis load Position")
                {
                    var inShuttleLXAxis = devices.MotionsAjin.InShuttleLXAxis;
                    if ((devices.Cylinders.AFCleanBrushLeftUpDown.IsBackward == false
                        || devices.Cylinders.AFCleanPusherLeftUpDown.IsBackward == false)
                        && (inShuttleLXAxis != null && inShuttleLXAxis.Status.ActualVelocity
                        != recipeSelector.CurrentRecipe.WetCleanLeftRecipe.XAxisLoadPosition))
                    // TODO: Thiếu điều kiện của cụm rotation
                    {
                        MessageBoxEx.ShowDialog($"[AFCleanBrushLeftUpDown] [AFCleanPusherLeftUpDown],  need [Backward] and [WET X Axis Position] move to [Load Position]");
                        return false;
                    }
                }
            }
            if (Name == "WET Clean Right")
            {
                if (moveToDescription == "X Axis Load Position"
                    || moveToDescription == "Y Axis Load Position"
                    || moveToDescription == "X Axis Clean Horizontal Position"
                    || moveToDescription == "Y Axis Clean Horizontal Position"
                    || moveToDescription == "X Axis Clean Vertical Position"
                    || moveToDescription == "Y Axis Clean Vertical Position")
                {
                    if (devices.Cylinders.WetCleanBrushLeftUpDown.IsBackward == false
                        || devices.Cylinders.WetCleanPusherLeftUpDown.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"[WetCleanBrushLeftUpDown] and [WetCleanPusherLeftUpDown] need [Backward]");
                        return false;
                    }
                }
                if (moveToDescription == "X Axis Unload Position")
                {
                    var outShuttleLXAxis = devices.MotionsAjin.OutShuttleLXAxis;
                    if ((devices.Cylinders.WetCleanBrushLeftUpDown.IsBackward == false
                        || devices.Cylinders.WetCleanPusherLeftUpDown.IsBackward == false)
                        && (outShuttleLXAxis != null && outShuttleLXAxis.Status.ActualVelocity
                        != recipeSelector.CurrentRecipe.AfCleanLeftRecipe.XAxisLoadPosition))
                    // TODO: Thiếu điều kiện của cụm rotation
                    {
                        MessageBoxEx.ShowDialog($"[WetCleanBrushLeftUpDown] [WetCleanPusherLeftUpDown],  need [Backward] and [AF X Axis Left Position] move to [Unload Position]");
                        return false;
                    }
                }
            }
            if (Name == "AF Clean Right")
            {
                if (moveToDescription == "X Axis UnLoad Position"
                    || moveToDescription == "Y Axis Load Position"
                    || moveToDescription == "X Axis Clean Horizontal Position"
                    || moveToDescription == "Y Axis Clean Horizontal Position"
                    || moveToDescription == "X Axis Clean Vertical Position"
                    || moveToDescription == "Y Axis Clean Vertical Position")
                {
                    if (devices.Cylinders.AFCleanBrushRightUpDown.IsBackward == false
                        || devices.Cylinders.AFCleanPusherRightUpDown.IsBackward == false)
                    {
                        MessageBoxEx.ShowDialog($"[AFCleanBrushRightUpDown] and [AFCleanPusherRightUpDown] need [Backward]");
                        return false;
                    }
                }
                if (moveToDescription == "X Axis load Position")
                {
                    var inShuttleRXAxis = devices.MotionsAjin.InShuttleRXAxis;
                    if ((devices.Cylinders.AFCleanBrushRightUpDown.IsBackward == false
                        || devices.Cylinders.AFCleanPusherRightUpDown.IsBackward == false)
                        && (inShuttleRXAxis != null && inShuttleRXAxis.Status.ActualVelocity
                    // TODO: Thiếu điều kiện của cụm rotation
                        != recipeSelector.CurrentRecipe.WetCleanRightRecipe.XAxisLoadPosition))

                    {
                        MessageBoxEx.ShowDialog($"[AFCleanBrushRightUpDown] [AFCleanPusherRightUpDown],  need [Backward] and [WET X Axis Right Position] move to [Load Position]");
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
                    if (glassUnloadLAxis != null && glassUnloadLAxis.Status.ActualPosition != recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.ZAxisReadyPosition)
                    {
                        MessageBoxEx.ShowDialog($"[Glass Unload Left Z Axis] move to [Ready Position]");
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
                    if (glassUnloadRAxis != null && glassUnloadRAxis.Status.ActualPosition != recipeSelector.CurrentRecipe.UnloadTransferRightRecipe.ZAxisReadyPosition)
                    {
                        MessageBoxEx.ShowDialog($"[Glass Unload Right Z Axis] move to [Ready Position]");
                        return false;
                    }
                }
            }
            return true; 
        }
    }
}
