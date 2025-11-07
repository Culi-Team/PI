using EQX.Core.Motion;
using PIFilmAutoDetachCleanMC.Defines.Devices;
using PIFilmAutoDetachCleanMC.Factories;
using PIFilmAutoDetachCleanMC.MVVM.ViewModels.Teaching;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PIFilmAutoDetachCleanMC.Services.Factories
{
    public class TeachingViewModelFactory
    {
        public TeachingViewModelFactory(RecipeSelector recipeSelector, Devices devices)
        {
            _recipeSelector = recipeSelector;
            _devices = devices;

            _unitTeachingVMs = new Dictionary<string, UnitTeachingViewModel>();
        }

        ~TeachingViewModelFactory()
        {
            _unitTeachingVMs.Clear();
        }

        public UnitTeachingViewModel Create(string name)
        {
            if (_unitTeachingVMs.ContainsKey(name) == false)
            {
                _unitTeachingVMs.Add(name, CreateVM(name));
            }

            return _unitTeachingVMs[name];
        }

        #region Private Methods
        private UnitTeachingViewModel CreateVM(string name)
        {
            if (name == "CST Load") return new UnitTeachingViewModel("CST Load", _recipeSelector)
            {
                Cylinders = _devices.GetInWorkConveyorCylinders(),
                Motions = _devices.GetCSTLoadMotions(),
                Inputs = _devices.GetInWorkConveyorInputs(),
                Outputs = _devices.GetInWorkConveyorOutputs(),
                Recipe = _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("InputCassetteStageImage")
            };

            if (name == "CST Unload") return new CSTUnloadTeachingViewModel("CST Unload", _recipeSelector)
            {
                Cylinders = _devices.GetOutWorkConveyorCylinders(),
                Motions = _devices.GetCSTUnloadMotions(),
                Inputs = _devices.GetOutWorkConveyorInputs(),
                Outputs = _devices.GetOutWorkConveyorOutputs(),
                Recipe = _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("InputCassetteStageImage")
            };

            if (name == "Vinyl Clean") return new VinylCleanTeachingViewModel("Vinyl Clean", _recipeSelector)
            {
                Cylinders = _devices.GetVinylCleanCylinders(),
                Inputs = _devices.GetVinylCleanInputs(),
                Outputs = _devices.GetVinylCleanOutputs(),
                Motions = new ObservableCollection<IMotion> { _devices.VinylCleanEncoder },
                UnWinder = _devices.TorqueControllers.VinylCleanUnWinder,
                Recipe = _recipeSelector.CurrentRecipe.VinylCleanRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("VinylCleanImage")
            };

            if (name == "Transfer Fixture") return new UnitTeachingViewModel("Transfer Fixture", _recipeSelector)
            {
                Cylinders = _devices.GetTransferFixtureCylinders(),
                Motions = _devices.GetTransferFixtureMotions(),
                Inputs = _devices.GetTransferFixtureInputs(),
                Outputs = _devices.GetTransferFixtureOutputs(),
                Recipe = _recipeSelector.CurrentRecipe.TransferFixtureRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferFixtureImage")
            };

            if (name == "Detach") return new UnitTeachingViewModel("Detach", _recipeSelector)
            {
                Cylinders = _devices.GetDetachCylinders(),
                Motions = _devices.GetDetachMotions(),
                Inputs = _devices.GetDetachInputs(),
                Outputs = _devices.GetDetachOutputs(),
                Recipe = _recipeSelector.CurrentRecipe.DetachRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("DetachImage")
            };

            if (name == "Glass Transfer") return new UnitTeachingViewModel("Glass Transfer", _recipeSelector)
            {
                Cylinders = _devices.GetGlassTransferCylinders(),
                Motions = _devices.GetGlassTransferMotions(),
                Inputs = _devices.GetGlassTransferInputs(),
                Outputs = _devices.GetGlassTransferOutputs(),
                Recipe = _recipeSelector.CurrentRecipe.GlassTransferRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("GlassTransferImage")
            };

            if (name == "Transfer In Shuttle Left") return new UnitTeachingViewModel("Transfer In Shuttle Left", _recipeSelector)
            {
                Cylinders = _devices.GetTransferInShuttleLeftCylinders(),
                Motions = _devices.GetTransferInShuttleLeftMotions(),
                Inputs = _devices.GetTransferInShuttleLeftInputs(),
                Outputs = _devices.GetTransferInShuttleLeftOutputs(),
                Recipe = _recipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage")
            };

            if (name == "Transfer In Shuttle Right") return new UnitTeachingViewModel("Transfer In Shuttle Right", _recipeSelector)
            {
                Cylinders = _devices.GetTransferInShuttleRightCylinders(),
                Motions = _devices.GetTransferInShuttleRightMotions(),
                Inputs = _devices.GetTransferInShuttleRightInputs(),
                Outputs = _devices.GetTransferInShuttleRightOutputs(),
                Recipe = _recipeSelector.CurrentRecipe.TransferInShuttleRightRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferShutterImage")
            };

            if (name == "WET Clean Left") return new CleanUnitTeachingViewModel("WET Clean Left", _recipeSelector)
            {
                Cylinders = _devices.GetWETCleanLeftCylinders(),
                Motions = _devices.GetWETCleanLeftMotions(),
                Inputs = _devices.GetWETCleanLeftInputs(),
                Outputs = _devices.GetWETCleanLeftOutputs(),
                SyringePump = _devices.SyringePumps.WetCleanLeftSyringePump,
                Recipe = _recipeSelector.CurrentRecipe.WetCleanLeftRecipe,
                Winder = _devices.TorqueControllers.WETCleanLeftWinder,
                UnWinder = _devices.TorqueControllers.WETCleanLeftUnWinder,
                Regulator = _devices.Regulators.WetCleanLRegulator,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage")
            };

            if (name == "WET Clean Right") return new CleanUnitTeachingViewModel("WET Clean Right", _recipeSelector)
            {
                Cylinders = _devices.GetWETCleanRightCylinders(),
                Motions = _devices.GetWETCleanRightMotions(),
                Inputs = _devices.GetWETCleanRightInputs(),
                Outputs = _devices.GetWETCleanRightOutputs(),
                SyringePump = _devices.SyringePumps.WetCleanRightSyringePump,
                Recipe = _recipeSelector.CurrentRecipe.WetCleanRightRecipe,
                Winder = _devices.TorqueControllers.WETCleanRightWinder,
                UnWinder = _devices.TorqueControllers.WETCleanRightUnWinder,
                Regulator = _devices.Regulators.WetCleanRRegulator,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage")
            };

            if (name == "Transfer Rotation Left") return new UnitTeachingViewModel("Transfer Rotation Left", _recipeSelector)
            {
                Cylinders = _devices.GetTransferRotationLeftCylinders(),
                Motions = _devices.GetTransferRotationLeftMotions(),
                Inputs = _devices.GetTransferRotationLeftInputs(),
                Outputs = _devices.GetTransferRotationLeftOutputs(),
                Recipe = _recipeSelector.CurrentRecipe.TransferRotationLeftRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage")
            };

            if (name == "Transfer Rotation Right") return new UnitTeachingViewModel("Transfer Rotation Right", _recipeSelector)
            {
                Cylinders = _devices.GetTransferRotationRightCylinders(),
                Motions = _devices.GetTransferRotationRightMotions(),
                Inputs = _devices.GetTransferRotationRightInputs(),
                Outputs = _devices.GetTransferRotationRightOutputs(),
                Recipe = _recipeSelector.CurrentRecipe.TransferRotationRightRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("TransferRotationImage")
            };

            if (name == "AF Clean Left") return new CleanUnitTeachingViewModel("AF Clean Left", _recipeSelector)
            {
                Cylinders = _devices.GetAFCleanLeftCylinders(),
                Motions = _devices.GetAFCleanLeftMotions(),
                Inputs = _devices.GetAFCleanLeftInputs(),
                Outputs = _devices.GetAFCleanLeftOutputs(),
                SyringePump = _devices.SyringePumps.AfCleanLeftSyringePump,
                Recipe = _recipeSelector.CurrentRecipe.AfCleanLeftRecipe,
                Winder = _devices.TorqueControllers.AFCleanLeftWinder,
                UnWinder = _devices.TorqueControllers.AFCleanLeftUnWinder,
                Regulator = _devices.Regulators.AfCleanLRegulator,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage")
            };

            if (name == "AF Clean Right") return new CleanUnitTeachingViewModel("AF Clean Right", _recipeSelector)
            {
                Cylinders = _devices.GetAFCleanRightCylinders(),
                Motions = _devices.GetAFCleanRightMotions(),
                Inputs = _devices.GetAFCleanRightInputs(),
                Outputs = _devices.GetAFCleanRightOutputs(),
                SyringePump = _devices.SyringePumps.AfCleanRightSyringePump,
                Recipe = _recipeSelector.CurrentRecipe.AfCleanRightRecipe,
                Winder = _devices.TorqueControllers.AFCleanRightWinder,
                UnWinder = _devices.TorqueControllers.AFCleanRightUnWinder,
                Regulator = _devices.Regulators.AfCleanRRegulator,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("AFCleanImage")
            };

            if (name == "Unload Transfer Left") return new UnitTeachingViewModel("Unload Transfer Left", _recipeSelector)
            {
                Cylinders = _devices.GetUnloadTransferLeftCylinders(),
                Motions = _devices.GetUnloadTransferLeftMotions(),
                Inputs = _devices.GetUnloadTransferLeftInputs(),
                Outputs = _devices.GetUnloadTransferLeftOutputs(),
                Recipe = _recipeSelector.CurrentRecipe.UnloadTransferLeftRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadTransferImage")
            };

            if (name == "Unload Transfer Right") return new UnitTeachingViewModel("Unload Transfer Right", _recipeSelector)
            {
                Cylinders = _devices.GetUnloadTransferRightCylinders(),
                Motions = _devices.GetUnloadTransferRightMotions(),
                Inputs = _devices.GetUnloadTransferRightInputs(),
                Outputs = _devices.GetUnloadTransferRightOutputs(),
                Recipe = _recipeSelector.CurrentRecipe.UnloadTransferRightRecipe,
                Image = (System.Windows.Media.ImageSource)Application.Current.FindResource("UnloadTransferImage")
            };

            return new UnitTeachingViewModel("INVALID", _recipeSelector);
        }
        #endregion

        #region Private Fields
        private readonly RecipeSelector _recipeSelector;
        private readonly Devices _devices;

        private Dictionary<string, UnitTeachingViewModel> _unitTeachingVMs;
        #endregion
    }
}
