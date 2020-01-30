using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipesSite.Models.Ingredient {
	public class RecipeIngredient {
		[Required]
		public int Id { get; set; }

		[Required]
		public string IngredientName { get; set; }

		[Required]
		public int Kcal { get; set; }
	}
}
