using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipesSite.CommonModels.BindingModels.Menus {
	public class AddMenuBindingModel {
		[Required]
		public string MenuName { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }
	}
}
