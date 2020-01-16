using RecipesSite.Models.Ingredient;
using RecipesSite.Models.Recipe;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipesSite.Services.Interfaces {
	public interface IRecipeInterface {
		public Dictionary<int, List<Recipe>> GetAllRecipes();

		public Recipe GetRecipeById(int id);

		public List<Recipe> GetRecipesByCategory(string catName);

		public void UpdateRecipe(int id);

		public void DeleteRecipe(int id);

		public List<Ingredient> GetAllIngredientsForRecipe(int id);
	}
}
