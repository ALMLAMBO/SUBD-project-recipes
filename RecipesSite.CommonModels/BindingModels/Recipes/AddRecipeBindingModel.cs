using BlazorInputFile;
using RecipesSite.Models.Ingredient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipesSite.CommonModels.BindingModels.Recipes {
	public class AddRecipeBindingModel {
		[Required]
		public string RecipeName { get; set; }

		[Required]
		public string RecipeCategory { get; set; }

		[Required]
		public IFileListEntry RecipeImage { get; set; }

		[Required]
		public string WayOfCooking { get; set; }

		public List<RecipeIngredient> Ingredients { get; set; }
	}
}
