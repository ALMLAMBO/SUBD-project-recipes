using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using RecipesSite.Models.Ingredient;
using RecipesSite.Services;

namespace RecipesSite.API.Controllers
{
	[ApiController]
	[Route("api/ingredients/")]
	class IngredientController : ControllerBase
	{
		[HttpGet("get-ingredients")]
		public List<RecipeIngredient> GetIng(int id) {
			var connectionConfig = new ConnectionConfig();
			MySqlConnection mySqlConnection = connectionConfig.GetMySqlConnection();

			var ingList = new List<RecipeIngredient>();
			MySqlCommand mySqlCommand = new MySqlCommand();
			mySqlCommand.Connection = mySqlConnection;
			mySqlCommand.CommandText = "select i.IngredientName, i.Kcal from RecipeIngredients ri" +
				"left join Ingredients i on i.Id = ri.IngredientId where ri.RecipeId = @id";
			mySqlCommand.Parameters.AddWithValue("@id", id);
			MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

			while (mySqlDataReader.Read())
			{
				RecipeIngredient ingredient = new RecipeIngredient()
				{
					IngredientName = mySqlDataReader.GetString("IngredientName"),
					Kcal = mySqlDataReader.GetInt32("Kcal")
				};

				ingList.Add(ingredient);
			}
			return ingList;
		}

		[HttpGet("all-ingredients")]
		public List<RecipeIngredient> GetRecipeIngredients() {
			List<RecipeIngredient> ingredients = new List<RecipeIngredient>();

			ConnectionConfig config = new ConnectionConfig();
			MySqlConnection connection = config.GetMySqlConnection();
			MySqlCommand command = new MySqlCommand(
				"select * from Ingredients", connection);

			MySqlDataReader reader = command.ExecuteReader();
			while(reader.Read()) {
				RecipeIngredient ingredient = new RecipeIngredient() {
					Id = reader.GetInt32("Id"),
					IngredientName = reader.GetString("IngredientName"),
					Kcal = reader.GetInt32("Kcal")
				};

				ingredients.Add(ingredient);
			}

			return ingredients;
		}
	}
}
