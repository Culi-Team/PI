using EQX.Core.Common;
using PIFilmAutoDetachCleanMC.Recipe;

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
    }
}
