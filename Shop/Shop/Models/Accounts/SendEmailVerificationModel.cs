using System.ComponentModel.DataAnnotations;

namespace Shop.Models.Accounts
{
	public class SendEmailVerificationModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
