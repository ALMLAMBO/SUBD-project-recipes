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
			ConnectionConfig connection = new ConnectionConfig();
			MySqlConnection mySqlConnection = connection
				.GetMySqlConnection();

			MySqlTransaction mySqlTransaction = mySqlConnection
				.BeginTransaction();

			try {
				MySqlCommand mySqlCommand = new MySqlCommand();
				mySqlCommand.Connection = mySqlConnection;
				mySqlCommand.CommandText = "select max(Id) as LastId from Recipes;";
				MySqlDataReader reader = mySqlCommand.ExecuteReader();
				int lastId = 0;

				while(reader.Read()) {
					lastId = reader.GetInt32("LastId");
				}

				Recipe recipeToAdd = new Recipe() {
					Id = lastId,
					RecipeName = recipe.RecipeName,
					RecipeCategory = recipe.RecipeCategory,
					RecipeIngrediets = recipe.Ingredients,
					WayOfCooking = recipe.WayOfCooking
				};

				mySqlCommand.CommandText = "insert into Recipes value " +
					"(@recipeCategory, @recipeName, @wayOfCooking);";

				mySqlCommand.Parameters.AddWithValue(
					"@recipeCategory", recipeToAdd.RecipeCategory);

				mySqlCommand.Parameters.AddWithValue(
					"@recipeName", recipeToAdd.RecipeName);

				mySqlCommand.Parameters.AddWithValue(
					"@wayOfCooking", recipeToAdd.WayOfCooking);

				mySqlCommand.ExecuteNonQuery();

				mySqlCommand.CommandText = "insert into Ingredients(IngredientName, Kcal)" +
					" value (@ingredientName, @kcal);";

				string sql = "select * from Ingredients where id = @id;";
				MySqlCommand command = new MySqlCommand
					(sql, mySqlConnection, mySqlTransaction);


				foreach (var item in recipeToAdd.RecipeIngrediets) {
					command.Parameters.AddWithValue("@id", item.Id);
					MySqlDataReader r = command.ExecuteReader();
					if(r.HasRows) {
						while(r.Read()) {
							int ingredientId = r.GetInt32("Id");

							command.CommandText = "insert into RecipeIngredients" +
								"value (@recipeId, @ingredientId);";

							command.Parameters.AddWithValue("@recipeId", recipeToAdd.Id);
							command.Parameters.AddWithValue("@ingredientId", item.Id);

							command.ExecuteNonQuery();
							command.Parameters.Clear();
						}
					}
					else {
						mySqlCommand.CommandText = "insert into Ingredients" +
							"(IngredientName, Kcal) value (@ingredientName, @kcal);";

						mySqlCommand.Parameters.AddWithValue(
							"@ingredientName", item.IngredientName);

						mySqlCommand.Parameters.AddWithValue(
							"@kcal", item.Kcal);

						mySqlCommand.ExecuteNonQuery();
						mySqlCommand.Parameters.Clear();
					}
				}

				mySqlTransaction.Commit();
			} 
			catch {
				mySqlTransaction.Rollback();
			}
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
			Dictionary<int, List<Recipe>> recipes = 
				new Dictionary<int, List<Recipe>>();

			ConnectionConfig connCof = new ConnectionConfig();
			MySqlConnection conn = connCof.GetMySqlConnection();
			MySqlCommand command = new MySqlCommand();

			command.Connection = conn;
			command.CommandText = "select r.Id, r.RecipeName, r.RecipeCategory, " +
				"r.WayOfCooking, ri.IngredientId from RecipeIngredients ri " +
				"left join Recipes r on ri.RecipeId = r.Id";

			MySqlDataReader reader = command.ExecuteReader();
			List<Recipe> list = new List<Recipe>();
			int line = 0;

			while(reader.Read()) {
				Recipe recipe = new Recipe() {
					Id = reader.GetInt32("Id"),
					RecipeName = reader.GetString("RecipeName"),
					RecipeCategory = reader.GetString("RecipeCategory"),
					WayOfCooking = reader.GetString("WayOfCooking"),
					RecipeIngrediets = new List<RecipeIngredient>()
				};

				command.CommandText = "select i.IngredientName, i.Kcal" +
					"from RecipeIngredients ri " +
					"left join Ingredients i on ri.IngredientId = i.Id" +
					"where ri.RecipeId = @id";

				command.Parameters.AddWithValue("@id", recipe.Id);
				MySqlDataReader r = command.ExecuteReader();

				while(r.Read()) {
					RecipeIngredient ingredient = new RecipeIngredient() {
						IngredientName = r.GetString("IngredientName"),
						Kcal = r.GetInt32("Kcal")
					};

					recipe.RecipeIngrediets.Add(ingredient);
				}

				list.Add(recipe);

				if(list.Count == 3) {
					recipes.Add(line, list);
					line++;
				}
			}

			return recipes;
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
			List<Recipe> recipes = new List<Recipe>();
			ConnectionConfig connCof = new ConnectionConfig();
			MySqlConnection conn = connCof.GetMySqlConnection();
			MySqlCommand command = new MySqlCommand();

			command.Connection = conn;
			command.CommandText = "select * from Recipes r " +
				"where r.RecipeCategory = @category";

			command.Parameters.AddWithValue("@category", catName);
			MySqlDataReader reader = command.ExecuteReader();
			while(reader.Read()) {
				Recipe recipe = new Recipe() {
					Id = reader.GetInt32("Id"),
					RecipeName = reader.GetString("RecipeName"),
					RecipeCategory = reader.GetString("RecipeCategory"),
					WayOfCooking = reader.GetString("WayOfCooking")
				};

				recipe.RecipeIngrediets = GetAllIngredientsForRecipe(recipe.Id);

				recipes.Add(recipe);
			}

			return recipes;
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
