using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipesSite.CommonModels.BindingModels.Comments {
	public class AddCommentBindingModel {
		[Required]
		public string Content { get; set; }
	}
}
