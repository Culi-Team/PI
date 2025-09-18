using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Motion;
using EQX.Core.Recipe;
using EQX.Motion;
using EQX.Motion.ByVendor.Inovance;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.Defines;
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
            IConfiguration configuration)
        {
            RecipeSelector = recipeSelector;
            _motionsInovance = motionsInovance;
            _motionAjin = motionAjin;
            _vinylCleanEncoder = vinylCleanEncoder;
            _configuration = configuration;
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
            // Lưu cấu hình Motion Inovance
            var inovanceConfigPath = _configuration["Files:MotionInovanceParaConfigFile"];
            if (!string.IsNullOrEmpty(inovanceConfigPath))
            {
                // Đọc file config hiện tại để giữ nguyên các giá trị khác
                var existingInovanceParams = JsonConvert.DeserializeObject<List<MotionInovanceParameter>>(
                    File.ReadAllText(inovanceConfigPath)) ?? new List<MotionInovanceParameter>();

                // Cập nhật chỉ 3 giá trị: Velocity, Acceleration, Deceleration
                for (int i = 0; i < _motionsInovance.All.Count && i < existingInovanceParams.Count; i++)
                {
                    existingInovanceParams[i].Velocity = _motionsInovance.All[i].Parameter.Velocity;
                    existingInovanceParams[i].Acceleration = _motionsInovance.All[i].Parameter.Acceleration;
                    existingInovanceParams[i].Deceleration = _motionsInovance.All[i].Parameter.Deceleration;
                }

                var inovanceJson = JsonConvert.SerializeObject(existingInovanceParams, Formatting.Indented);
                File.WriteAllText(inovanceConfigPath, inovanceJson);
            }

            // Lưu cấu hình Motion Ajin
            var ajinConfigPath = _configuration["Files:MotionAjinParaConfigFile"];
            if (!string.IsNullOrEmpty(ajinConfigPath))
            {
                // Đọc file config hiện tại để giữ nguyên các giá trị khác
                var existingAjinParams = JsonConvert.DeserializeObject<List<MotionAjinParameter>>(
                    File.ReadAllText(ajinConfigPath)) ?? new List<MotionAjinParameter>();

                // Cập nhật chỉ 3 giá trị: Velocity, Acceleration, Deceleration
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
    }
}
