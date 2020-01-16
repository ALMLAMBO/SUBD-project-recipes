using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
	}
}
