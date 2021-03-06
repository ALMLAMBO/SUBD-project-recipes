﻿using System;
using System.Text;
using System.Collections.Generic;
using RecipesSite.Models.Recipe;
using RecipesSite.Models.Comment;
using RecipesSite.Models.Ingredient;
using RecipesSite.CommonModels.BindingModels.Recipes;

namespace RecipesSite.Services.Interfaces {
	public interface IRecipeService {
		public void AddRecipe(AddRecipeBindingModel recipe);

		public Dictionary<int, List<Recipe>> GetAllRecipes();
		
		public void UpdateRecipe(int id, EditRecipeBindingModel recipe);

		public void DeleteRecipe(int id);

		public Recipe GetRecipeById(int id);

		public List<Recipe> GetRecipesByCategory(string catName);

		public List<RecipeComment> GetAllCommentsForRecipe(int id);

		public List<RecipeIngredient> GetAllIngredientsForRecipe(int id);
	}
}
