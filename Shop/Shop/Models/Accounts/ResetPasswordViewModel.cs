﻿using System.ComponentModel.DataAnnotations;

namespace Shop.Models.Accounts
{
	public class ResetPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(100, MinimumLength = 6)]
		public string Password { get; set; }

		[Required]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }

		public string Token { get; set; }
	}
}
