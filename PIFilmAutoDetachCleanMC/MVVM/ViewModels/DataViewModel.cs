using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Motion;
using EQX.Core.Recipe;
using EQX.Motion;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.Defines;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class DataViewModel : ViewModelBase
    {
        private string selectedModel;
        private RecipeBase _selectedRecipe;
        private readonly MotionsInovance _motionsInovance;
        private readonly MotionsAjin _motionAjin;
        public DataViewModel(RecipeSelector recipeSelector,
            MotionsInovance motionsInovance,
            MotionsAjin motionAjin)
        {
            RecipeSelector = recipeSelector;
            _motionsInovance = motionsInovance;
            _motionAjin = motionAjin;
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

    }
}
