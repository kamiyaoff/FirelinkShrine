using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FirelinkShrine.Models {
	public class User {
		public int Id { get; set; }
		public string Name { get; set; } = null!;

		[DataType(DataType.Password), MinLength(6)]
		public string Password { get; set; } = null!;

		[NotMapped]
		[Required(ErrorMessage = "Confirmation Password is required.")]
		[Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
		public string? ConfirmPassword { get; set; }

		public List<Objective>? Objectives { get; set; }
		public List<Note>? Notes { get; set; }
	}
}