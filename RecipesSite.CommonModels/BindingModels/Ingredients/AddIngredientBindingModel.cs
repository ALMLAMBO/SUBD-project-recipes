using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipesSite.CommonModels.BindingModels.Ingredients {
	public class AddIngredientBindingModel {
		[Required]
		public string IngredientName { get; set; }

		[Required]
		public int Kcal { get; set; }
	}
}
