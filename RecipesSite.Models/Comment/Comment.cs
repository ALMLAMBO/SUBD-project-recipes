using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipesSite.Models.Comment {
	public class Comment {
		[Required]
		public int Id { get; set; }

		[Required]
		public string Content { get; set; }
	}
}
