using System;
using System.Text;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RecipesSite.Models.Recipe;
using RecipesSite.Models.Comment;
using RecipesSite.Models.Ingredient;
using RecipesSite.Services.Interfaces;
using RecipesSite.CommonModels.BindingModels.Recipes;

namespace RecipesSite.Services {
	public class RecipeService : IRecipeService {

		/// <summary>
		/// Adds a recipe to the database
		/// </summary>
		/// <param name="recipe">Recipe to add</param>
		public void AddRecipe(AddRecipeBindingModel recipe) {
			
		}

		/// <summary>
		/// Delete a recipe from database and all comments and ingredients
		/// </summary>
		/// <param name="id">Id of the recipe</param>
		public void DeleteRecipe(int id) {
			ConnectionConfig connCof = new ConnectionConfig();
			MySqlConnection conn = connCof.GetMySqlConnection();
			MySqlTransaction transaction = conn.BeginTransaction();
			
			try {

				MySqlCommand command = new MySqlCommand();
				command.Connection = conn;
				command.Transaction = transaction;

				command.CommandText = "delete from Recipes where Id = @id";
				command.Parameters.AddWithValue("@id", id);
				command.ExecuteNonQuery();
				
				command.CommandText = "delete from RecipeIngredients where RecipeId = @id";
				command.Parameters.AddWithValue("@id", id);
				command.ExecuteNonQuery();

				command.CommandText = "delete from RecipeComments where RecipeId = @id";
				command.Parameters.AddWithValue("@id", id);
				command.ExecuteNonQuery();

				transaction.Commit();
			} 
			catch {
				transaction.Rollback();
			}
		}

		/// <summary>
		/// Gets all comments for a recipe
		/// </summary>
		/// <param name="id">Id of the recipe</param>
		/// <returns>List of all comments</returns>
		public List<RecipeComment> GetAllCommentsForRecipe(int id) {
			List<RecipeComment> comments = new List<RecipeComment>();

			ConnectionConfig connCof = new ConnectionConfig();
			MySqlConnection conn = connCof.GetMySqlConnection();
			MySqlCommand command = new MySqlCommand();
			command.Connection = conn;
			command.CommandText = "select c.Content from RecipeComments rc " +
				"left join Comments c on c.Id = rc.CommentId where rc.RecipeId = @id";

			command.Parameters.AddWithValue("@id", id);
			MySqlDataReader reader = command.ExecuteReader();

			while(reader.Read()) {
				RecipeComment comment = new RecipeComment() {
					Content = reader.GetString("Content")
				};

				comments.Add(comment);
			}

			return comments;
		}

		/// <summary>
		/// Get all ingredients for recipe
		/// </summary>
		/// <param name="id">Id of recipe</param>
		/// <returns>List of all ingredients</returns>
		public List<RecipeIngredient> GetAllIngredientsForRecipe(int id) {
			List<RecipeIngredient> ingredients = new List<RecipeIngredient>();
			ConnectionConfig connCof = new ConnectionConfig();
			MySqlConnection conn = connCof.GetMySqlConnection();
			MySqlCommand command = new MySqlCommand();

			command.Connection = conn;
			command.CommandText = "select i.IngredientName, i.Kcal from RecipeIngredients ri" +
				"left join Ingredients i on i.Id = ri.IngredientId where ri.RecipeId = @id";

			command.Parameters.AddWithValue("@id", id);
			MySqlDataReader reader = command.ExecuteReader();

			while(reader.Read()) {
				RecipeIngredient ingredient = new RecipeIngredient() {
					IngredientName = reader.GetString("IngredientName"),
					Kcal = reader.GetInt32("Kcal")
				};

				ingredients.Add(ingredient);
			}

			return ingredients;
		}

		/// <summary>
		/// gets all recipes in groups of three
		/// </summary>
		/// <returns>dictionary which contains number of row and list of three recipes for each row</returns>
		public Dictionary<int, List<Recipe>> GetAllRecipes() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get recipe by id
		/// </summary>
		/// <param name="id">Id of the recipe</param>
		/// <returns></returns>
		public Recipe GetRecipeById(int id) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get all recipes by category
		/// </summary>
		/// <param name="catName">Name of the category</param>
		/// <returns></returns>
		public List<Recipe> GetRecipesByCategory(string catName) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Updates a recipe
		/// </summary>
		/// <param name="id">Id of the recipe</param>
		/// <param name="recipe">New data for the recipe</param>
		public void UpdateRecipe(int id, EditRecipeBindingModel recipe) {
			//TODO add properties of EditRecipeBindingModel class
		}
	}
}
