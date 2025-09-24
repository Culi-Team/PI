using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Motion;
using EQX.Core.Recipe;
using EQX.Motion;
using EQX.Motion.ByVendor.Inovance;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Defines.Devices.Cassette;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using log4net;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class DataViewModel : ViewModelBase
    {
        private string selectedModel;
        private RecipeBase _selectedRecipe;
        private readonly MotionsInovance _motionsInovance;
        private readonly MotionsAjin _motionAjin;
        private readonly IMotion _vinylCleanEncoder;
        private readonly IConfiguration _configuration;
        public DataViewModel(RecipeSelector recipeSelector,
            MotionsInovance motionsInovance,
            MotionsAjin motionAjin,
            [FromKeyedServices("VinylCleanEncoder")] IMotion vinylCleanEncoder,
            IConfiguration configuration,
            CassetteList cassetteList)
        {
            RecipeSelector = recipeSelector;
            _motionsInovance = motionsInovance;
            _motionAjin = motionAjin;
            _vinylCleanEncoder = vinylCleanEncoder;
            _configuration = configuration;
            CassetteList = cassetteList;

            Log = LogManager.GetLogger("Data");

            RecipeSelector.CurrentRecipe.CommonRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.CstLoadUnloadRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.RobotLoadRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.VinylCleanRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.TransferFixtureRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.DetachRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.GlassTransferRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.TransferInShuttleLeftRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.TransferInShuttleRightRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.WetCleanLeftRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.WetCleanRightRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.TransferRotationLeftRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.TransferRotationRightRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.AfCleanLeftRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.AfCleanRightRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.UnloadTransferLeftRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.UnloadTransferRightRecipe.RecipeChanged += RecipeChanged_Handler;
            RecipeSelector.CurrentRecipe.RobotUnloadRecipe.RecipeChanged += RecipeChanged_Handler;
        }

        public ObservableCollection<IMotion> AllMotions
        {
            get
            {
                List<IMotion> motions = new List<IMotion>();
                motions.AddRange(_motionsInovance.All);
                motions.AddRange(_motionAjin.All);

                return new ObservableCollection<IMotion>(motions);
            }
        }

        public RecipeSelector RecipeSelector { get; }
        public CassetteList CassetteList { get; }

        public string SelectedModel
        {
            get { return selectedModel; }
            set 
            {
                selectedModel = value;
                OnPropertyChanged();
            }
        }

        public event Action LoadRecipeEvent;

        public ObservableCollection<RecipeBase> Recipes
        {
            get
            {
                var recipeProps = RecipeSelector.CurrentRecipe.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => typeof(RecipeBase).IsAssignableFrom(p.PropertyType))  
                .ToList();

                var recipeObjects = recipeProps
                .Select(p => p.GetValue(RecipeSelector.CurrentRecipe) as RecipeBase)
                .Where(r => r != null)
                .ToList();

                return new ObservableCollection<RecipeBase>(recipeObjects);
            }
        }

        public RecipeBase SelectedRecipe
        {
            get 
            {
                return _selectedRecipe; 
            }
            set 
            {
                _selectedRecipe = value;
                OnPropertyChanged(); 
            }
        }

        public RecipeBase CurrentRecipe
        {
            get
            {
                var recipeProps = RecipeSelector.CurrentRecipe.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => typeof(RecipeBase).IsAssignableFrom(p.PropertyType))
                .ToList();

                return recipeProps
                .Select(p => p.GetValue(RecipeSelector.CurrentRecipe) as RecipeBase)
                .ToList().First(r => r.Name == SelectedRecipe.Name);
            }
        }

        public ICommand SaveRecipeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (MessageBoxEx.ShowDialog((string)Application.Current.Resources["str_SaveAllData"]) == true)
                    {
                        RecipeSelector.Save();
                        CassetteList.RecipeUpdateHandle();
                        OnPropertyChanged(nameof(Recipes));
                    }
                });
            }
        }
        public ICommand RefreshRecipeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    RecipeSelector.ValidRecipes = RecipeSelector.UpdateValidRecipes();
                    LoadRecipeEvent?.Invoke();
                });
            }
        }
        public ICommand CopyRecipeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string CopyRecipe = (string)Application.Current.Resources["str_CopyRecipe"];
                    if (MessageBoxEx.ShowDialog($"{CopyRecipe} {SelectedModel} ? ") == true)
                    {
                        RecipeSelector.Copy(SelectedModel);
                        RecipeSelector.ValidRecipes = RecipeSelector.UpdateValidRecipes();
                        LoadRecipeEvent?.Invoke();
                    }
                });
            }
        }

        public ICommand SaveMotionConfigCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        var result = MessageBoxEx.ShowDialog("Do you want to save the motion configurations?",true,"Confirm Save");
                        
                        if (result == true) 
                        {
                            SaveMotionConfigurations();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxEx.ShowDialog($"Error saving motion configurations: {ex.Message}");
                    }
                });
            }
        }

        private void SaveMotionConfigurations()
        {
            var inovanceConfigPath = _configuration["Files:MotionInovanceParaConfigFile"];
            if (!string.IsNullOrEmpty(inovanceConfigPath))
            {
                var existingInovanceParams = JsonConvert.DeserializeObject<List<MotionInovanceParameter>>(
                    File.ReadAllText(inovanceConfigPath)) ?? new List<MotionInovanceParameter>();

                for (int i = 0; i < _motionsInovance.All.Count && i < existingInovanceParams.Count; i++)
                {
                    existingInovanceParams[i].Velocity = _motionsInovance.All[i].Parameter.Velocity;
                    existingInovanceParams[i].Acceleration = _motionsInovance.All[i].Parameter.Acceleration;
                    existingInovanceParams[i].Deceleration = _motionsInovance.All[i].Parameter.Deceleration;
                }

                var inovanceJson = JsonConvert.SerializeObject(existingInovanceParams, Formatting.Indented);
                File.WriteAllText(inovanceConfigPath, inovanceJson);
            }

            var ajinConfigPath = _configuration["Files:MotionAjinParaConfigFile"];
            if (!string.IsNullOrEmpty(ajinConfigPath))
            {
                var existingAjinParams = JsonConvert.DeserializeObject<List<MotionAjinParameter>>(
                    File.ReadAllText(ajinConfigPath)) ?? new List<MotionAjinParameter>();

                for (int i = 0; i < _motionAjin.All.Count && i < existingAjinParams.Count; i++)
                {
                    existingAjinParams[i].Velocity = _motionAjin.All[i].Parameter.Velocity;
                    existingAjinParams[i].Acceleration = _motionAjin.All[i].Parameter.Acceleration;
                    existingAjinParams[i].Deceleration = _motionAjin.All[i].Parameter.Deceleration;
                }

                var ajinJson = JsonConvert.SerializeObject(existingAjinParams, Formatting.Indented);
                File.WriteAllText(ajinConfigPath, ajinJson);
            }
        }

        private void RecipeChanged_Handler(object oldValue, object newValue, string? propertyName = null)
        {
            Log.Info($"{propertyName} value updated : {oldValue} -> {newValue}");
        }
    }
}
