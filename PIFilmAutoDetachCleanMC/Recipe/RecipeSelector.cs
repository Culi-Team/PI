using CommunityToolkit.Mvvm.ComponentModel;
using EQX.Core.Recipe;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class RecipeSelector : ObservableObject
    {
        private RecipeSetting recipeSetting;
        private readonly IConfiguration _configuration;

        public RecipeSetting RecipeSetting
        {
            get { return recipeSetting; }
            set { recipeSetting = value; }
        }

        public RecipeList CurrentRecipe { get; private set; }

        public RecipeSelector(IConfiguration configuration, RecipeList currentRecipe)
        {
            _configuration = configuration;
            CurrentRecipe = currentRecipe;
        }

        public bool Load()
        {
            string recipeFolder = _configuration.GetValue<string>("Folders:RecipeFolder");
            // 1. Get Current Recipe
            string recipeSettingFile = Path.Combine(recipeFolder, "RecipeSetting.json");
            if (File.Exists(recipeSettingFile) == false)
            {
                MessageBox.Show($"{recipeSettingFile} file not found");
                return false;
            }

            string currentRecipeFileContain = File.ReadAllText(recipeSettingFile);

            try
            {
                RecipeSetting = JsonConvert.DeserializeObject<RecipeSetting>(currentRecipeFileContain);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            // 2. Get Current Recipe
            string currentRecipeFolder = Path.Combine(recipeFolder, RecipeSetting.CurrentRecipe);
            string currentRecipeFile = Path.Combine(currentRecipeFolder, "Recipe.json");
            if (Directory.Exists(currentRecipeFolder) == false)
                MessageBox.Show($" Recipe folder \"{currentRecipeFolder}\" not found");

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            try
            {
                RecipeList backupRecipe = JsonConvert.DeserializeObject<RecipeList>(File.ReadAllText(currentRecipeFile), settings);
                PropertyInfo[] properties = CurrentRecipe.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        ((IRecipe)property.GetValue(CurrentRecipe, null)).Clone((IRecipe)property.GetValue(backupRecipe, null));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public void Copy(string selectedRecipe)
        {
            string recipeFolder = _configuration.GetValue<string>("Folders:RecipeFolder");

            string currentRecipeFolder = Path.Combine(recipeFolder, selectedRecipe);
            string currentRecipeFile = Path.Combine(currentRecipeFolder, "Recipe.json");
            if (Directory.Exists(currentRecipeFolder) == false)
                MessageBox.Show($" Recipe folder \"{currentRecipeFolder}\" not found");
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            RecipeList backupRecipe = JsonConvert.DeserializeObject<RecipeList>(File.ReadAllText(currentRecipeFile), settings);
            if (Directory.Exists($"{currentRecipeFolder}_Copy") == false)
            {
                Directory.CreateDirectory($"{currentRecipeFolder}_Copy");

                foreach (var file in Directory.GetFiles(currentRecipeFolder))
                {
                    string fileSource = Path.Combine(currentRecipeFolder, Path.GetFileName(file));
                    string fileBackup = Path.Combine($"{currentRecipeFolder}_Copy", Path.GetFileName(file));

                    File.Copy(fileSource, fileBackup, true);
                }
            }
            else
            {
                MessageBox.Show($" Recipe folder \"$\"{currentRecipeFolder}_copy\"\" exists");
            }
        }

        public void Save()
        {
            string recipeFolder = _configuration.GetValue<string>("Folders:RecipeFolder");

            string currentRecipeFolder = Path.Combine(recipeFolder, RecipeSetting.CurrentRecipe);
            string currentRecipeFile = Path.Combine(currentRecipeFolder, "Recipe.json");
            if (Directory.Exists(currentRecipeFolder) == false)
                MessageBox.Show($" Recipe folder \"{currentRecipeFolder}\" not found");

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            string serializeStr = JsonConvert.SerializeObject(CurrentRecipe, Formatting.Indented, settings);
            File.WriteAllText(currentRecipeFile, serializeStr);
        }

    }
}
