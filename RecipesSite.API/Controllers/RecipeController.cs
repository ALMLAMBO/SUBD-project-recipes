using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RecipesSite.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using RecipesSite.Models.Ingredient;
using RecipesSite.Models.Recipe;

namespace RecipesSite.API.Controllers
{
	[ApiController]
	class RecipeController : ControllerBase
	{
		[HttpPost]
		
		public void AddIng([FromBody]Recipe recipe) {
			var connectionConfig = new ConnectionConfig();
			MySqlConnection mySqlConnection = connectionConfig.GetMySqlConnection();
			MySqlTransaction mySqlTransaction = mySqlConnection.BeginTransaction();

			try
			{
				MySqlCommand mySqlCommand = new MySqlCommand();
				mySqlCommand.Connection = mySqlConnection;
				mySqlCommand.CommandText = "insert into Recipes(RecipeName, RecipeCategory, RecipeImageLink) value(@recipeName, @recipeCategory, @recipeImageLink)";
				mySqlCommand.Parameters.AddWithValue("@recipeName", recipe.RecipeName);
				mySqlCommand.Parameters.AddWithValue("@recipeCategory", recipe.RecipeCategory);
				mySqlCommand.Parameters.AddWithValue("@recipeImageLink", recipe.RecipeImageLink);
				mySqlCommand.ExecuteNonQuery();
				mySqlTransaction.Commit();
			}
			catch
			{
				mySqlTransaction.Rollback();
			}
		}

		[HttpDelete]
		public void RemoveIng(string IngredientName) {
			var connectionConfig = new ConnectionConfig();
			MySqlConnection mySqlConnection = connectionConfig.GetMySqlConnection();
			MySqlTransaction mySqlTransaction = mySqlConnection.BeginTransaction();
			try
			{
				MySqlCommand mySqlCommand = new MySqlCommand();
				mySqlCommand.Connection = mySqlConnection;
				mySqlCommand.CommandText = "delete from Ingredients i where i.IngredientName = @name;";
				mySqlCommand.Parameters.AddWithValue("@name", IngredientName);
				mySqlCommand.ExecuteNonQuery();
				mySqlTransaction.Commit();
			}
			catch
			{
				mySqlTransaction.Rollback();
			}
		}

		[HttpGet]
		public List<RecipeIngredient> GetList(int id) {
			var connectionConfig = new ConnectionConfig();
			MySqlConnection mySqlConnection = connectionConfig.GetMySqlConnection();

			var ingList = new List<RecipeIngredient>();
			MySqlCommand mySqlCommand = new MySqlCommand();
			mySqlCommand.Connection = mySqlConnection;
			mySqlCommand.CommandText = "select i.IngredientName, i.Kcal from RecipeIngredients ri" +
				"left join Ingredients i on i.Id = ri.IngredientId where ri.RecipeId = @id";
			mySqlCommand.Parameters.AddWithValue("@id", id);
			MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

			while(mySqlDataReader.Read()) {
				RecipeIngredient ingredient = new RecipeIngredient() {
					IngredientName = mySqlDataReader.GetString("IngredientName"),
					Kcal = mySqlDataReader.GetInt32("Kcal")
				};

				ingList.Add(ingredient);
			}
			return ingList;
		}
	}
}
