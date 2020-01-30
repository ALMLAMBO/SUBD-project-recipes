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

	class IngredientController : ControllerBase
	{
		[HttpGet]
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
	}
}
