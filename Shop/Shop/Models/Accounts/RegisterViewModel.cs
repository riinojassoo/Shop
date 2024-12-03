using System.ComponentModel.DataAnnotations;
using Shop.Utilities;

namespace Shop.Models.Accounts
{
	public class RegisterViewModel
	{
		[Required]
		[EmailAddress]
		[ValidEmailDomain(allowedDomain: ".com", ErrorMessage = "Email domain must be .com")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "Password and conformation password do not match.")]
		public string ConfirmPassword { get; set; }

		public string City { get; set; }

	}
}
