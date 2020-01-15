using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipesSite.CommonModels.BindingModels.Users {
	public class EditUserBindingModel {
		public string Username { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
