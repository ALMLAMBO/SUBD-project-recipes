using MySql.Data.MySqlClient;
using RecipesSite.Models.Ingredient;
using RecipesSite.Models.Recipe;
using RecipesSite.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipesSite.Services {
	public class RecipeService : IRecipeInterface {
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

		public List<Ingredient> GetAllIngredientsForRecipe(int id) {
			throw new NotImplementedException();
		}

		public Dictionary<int, List<Recipe>> GetAllRecipes() {
			throw new NotImplementedException();
		}

		public Recipe GetRecipeById(int id) {
			throw new NotImplementedException();
		}

		public List<Recipe> GetRecipesByCategory(string catName) {
			throw new NotImplementedException();
		}

		public void UpdateRecipe(int id) {
			throw new NotImplementedException();
		}
	}
}
