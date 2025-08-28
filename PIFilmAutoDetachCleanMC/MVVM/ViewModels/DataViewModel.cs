using CommunityToolkit.Mvvm.Input;
using EQX.Core.Common;
using EQX.Core.Recipe;
using EQX.UI.Controls;
using PIFilmAutoDetachCleanMC.Recipe;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace PIFilmAutoDetachCleanMC.MVVM.ViewModels
{
    public class DataViewModel : ViewModelBase
    {
        private string selectedRecipe;

        public DataViewModel(RecipeSelector recipeSelector)
        {
            RecipeSelector = recipeSelector;
        }

        public RecipeSelector RecipeSelector { get; }

        public string SelectedRecipe
        {
            get { return selectedRecipe; }
            set { selectedRecipe = value; OnPropertyChanged(); }
        }

        public event Action LoadRecipeEvent;

        public ObservableCollection<RecipeBase> Recipes
        {
            get
            {
                var recipeProps = typeof(RecipeList)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => typeof(RecipeBase).IsAssignableFrom(p.PropertyType))  // chỉ lấy property kế thừa RecipeBase
                .ToList();

                var recipeObjects = recipeProps
                .Select(p => p.GetValue(RecipeSelector.CurrentRecipe) as RecipeBase)
                .Where(r => r != null)
                .ToList();

                return new ObservableCollection<RecipeBase>(recipeObjects);
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
                    if (MessageBoxEx.ShowDialog($"{CopyRecipe} {SelectedRecipe} ? ") == true)
                    {
                        RecipeSelector.Copy(SelectedRecipe);
                        RecipeSelector.ValidRecipes = RecipeSelector.UpdateValidRecipes();
                        LoadRecipeEvent?.Invoke();
                    }
                });
            }
        }
    }
}
