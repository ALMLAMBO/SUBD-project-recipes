using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RecipesSite.Models.Ingredient;
using RecipesSite.Models.Comment;

namespace RecipesSite.Models.Recipe {
	public class Recipe {
		[Required]
		public int Id { get; set; }

		[Required]
		public string RecipeName { get; set; }

		[Required]
		public string RecipeCategory { get; set; }

		[Required]
		public string RecipeImageLink { get; set; }

		public string WayOfCooking { get; set; }

		public List<RecipeIngredient> RecipeIngrediets { get; set; }

		public List<RecipeComment> RecipeComments { get; set; }
	}
}
