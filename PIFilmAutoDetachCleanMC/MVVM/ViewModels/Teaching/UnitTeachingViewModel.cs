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
                        if (!CheckCylinderPositionBeforMove(moveToDescription) || !CheckAxisPositionBeforMove(moveToDescription))
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
        public bool CheckCylinderPositionBeforMove(string moveToDescription)
        {
            Devices devices = App.AppHost!.Services.GetRequiredService<Devices>();
            if(moveToDescription == "In Cassette T Axis Load Position" 
                || moveToDescription == "In Cassette T Axis Work Position")
            {
                if(devices.Cylinders.InCvSupportUpDown.IsBackward == false 
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
            return true; 
        }
        public bool CheckAxisPositionBeforMove(string moveToDescription)
        {
            RecipeSelector recipeSelector = App.AppHost!.Services.GetRequiredService<RecipeSelector>();
            Devices devices = App.AppHost!.Services.GetRequiredService<Devices>();
            if (moveToDescription == "Transfer Fixture Y Axis Load Position" || moveToDescription == "Transfer Fixture Y Axis Unload Position")
            {
                var detachZAxis = devices.MotionsInovance.DetachGlassZAxis;
                if (detachZAxis != null && detachZAxis.Status.ActualPosition != recipeSelector.CurrentRecipe.DetachRecipe.DetachZAxisDetachReadyPosition)
                {
                    MessageBoxEx.ShowDialog($"[Detach Z Axis] need Move [Ready Position] befor move to [{moveToDescription}]");
                    return false;
                }
            }
            return true; 
        }
    }
}
