using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Shop.Core.Domain;
using Shop.Models.Accounts;
using System.Net.Mail;
using System.Net;
using Shop.Core.ServiceInterface;
using Shop.ApplicationServices.Services;
using Shop.Core.Dto.Emails;
using Shop.Models;
using System.Diagnostics;

namespace Shop.Controllers
{
	public class AccountsController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailServices _emailServices;



		public AccountsController
			(
				UserManager<ApplicationUser> userManager,
				SignInManager<ApplicationUser> signInManager,
				IEmailServices emailServices

			)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailServices = emailServices;
		}


		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public async Task<IActionResult> Register(RegisterViewModel vm)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					UserName = vm.Email,
					Name = vm.Name,
					Email = vm.Email,
					City = vm.City,
				};

				var result = await _userManager.CreateAsync(user, vm.Password);
				
				if (result.Succeeded)
				{
					var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

					var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, token = token }, Request.Scheme);

					if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
					{
						return RedirectToAction("ListUser", "Administrations");
					}

					EmailTokenDto newsignup = new();
					newsignup.Token = token;
					newsignup.Body = $"Thank you for registering: <a href=\"{confirmationLink}\" >click here</a>";
					newsignup.Subject = "CRUD REgistration";
					newsignup.To = user.Email;

					//ViewBag.ErrorTitle = "Registration successful";
					//ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
					//	"email, by clicking on the confirmation link we have emailed you";

					//return View("EmailError");

					_emailServices.SendEmailToken(newsignup, token);
					List<string> errordatas = [];
					ViewBag.ErrorDatas = errordatas;
					ViewBag.ErrorTitle = "You have successfully registered";
					ViewBag.ErrorMessage = "Before you can log in, please confirm email fom the link";
					return View("ConfirmEmail");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}

			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Login(string? returnUrl)
		{
			LoginViewModel vm = new()
			{
				ReturnUrl = returnUrl,
				ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
			};

			return View(vm);
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
		{
			model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);

				if (user != null && !user.EmailConfirmed && (await _userManager.CheckPasswordAsync(user, model.Password)))
				{
					ModelState.AddModelError(string.Empty, "Email not confirmed yet");
					return View(model);
				}

				var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

				if (result.Succeeded)
				{
					if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
					{
						return Redirect(returnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}

				if (result.IsLockedOut)
				{
					return View("AccountLocked");
				}
				ModelState.AddModelError("", "Invalid Login Attempt");
			}

			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public async Task<IActionResult> ConfirmEmail(string userId, string token)
		{
			if (userId == null || token == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				ViewBag.ErrorMessage = $"The user ID {userId} is not valid";
				return View("NotFound");
			}

			var result = await _userManager.ConfirmEmailAsync(user, token);
			List<string> errordatas =
				[
				"Area", "Accounts",
				"Issue", "Failure",
				"StatusMessage", "Confirmation Failure",
				"ActedOn", $"{user.Email}",
				"CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]\n[password hidden]"
				];


			if (result.Succeeded)
			{
				errordatas =
					[
					"Area", "Accounts",
					"Issue", "Failure",
					"StatusMessage", "Confirmation Failure",
					"ActedOn", $"{user.Email}",
					"CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]\n[password hidden]"
					];
				ViewBag.ErrorDatas = errordatas;
				return View();
			}

			ViewBag.ErrorDatas = errordatas;
			ViewBag.ErrorTitle = "Email cannot be confirmed";
			ViewBag.ErrorMessage = $"The users email, with userid of {userId}, cannot be confirmed";
			return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
		}



		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
        }

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if(ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user != null && await _userManager.IsEmailConfirmedAsync(user))
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var passwordResetLink = Url.Action("ResetPassword", "Accounts", new { email = model.Email, token = token }, Request.Scheme);

					return View("ForgotPasswordConfirmation");
				}
                return View("ForgotPasswordConfirmation");

            }
			return View(model);
        }

		//[HttpGet]
		//[AllowAnonymous]
		//public IActionResult SendEmailVerification()
		//{
		//	return View();
		//}

		//[HttpPost]
		//[AllowAnonymous]
		//public async Task<IActionResult> SendEmailVerification(SendEmailVerificationModel model)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return View(model);
		//	}

		//	var user = await _userManager.FindByEmailAsync(model.Email);
		//	if (user != null && await _userManager.IsEmailConfirmedAsync(user))
		//	{
		//		ModelState.AddModelError(string.Empty, "Email is already confirmed.");
		//		return View(model);
		//	}

		//	// Generate email confirmation token
		//	var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

		//	// Generate confirmation link
		//	var emailVerificationLink = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, token = token }, Request.Scheme);

		//	// Create the EmailDto with the necessary details
		//	var emailDto = new EmailDto
		//	{
		//		To = user.Email,
		//		Subject = "Email Verification",
		//		Body = $"Click the link to verify your email: <a href='{emailVerificationLink}'>Verify Email</a>",
		//	};

		//	// Log the email details
		//	_logger.LogInformation("Sending email to {To} with subject {Subject} and body {Body}",
		//		emailDto.To, emailDto.Subject, emailDto.Body);

		//	try
		//	{
		//		// Send the email using the service
		//		_emailServices.SendEmail(emailDto);

		//		ViewBag.Message = "A verification link has been sent to your email address.";
		//		return View("SendVerificationEmailConfirmation");
		//	}
		//	catch (Exception ex)
		//	{
		//		ModelState.AddModelError(string.Empty, $"Error sending email: {ex.Message}");
		//		return View(model);
		//	}
		//}


		[HttpGet]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.GetUserAsync(User);

				if (user == null)
				{
					return RedirectToAction("Login");
				}

				var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

				if (!result.Succeeded)
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}

					return View();
				}

				await _signInManager.RefreshSignInAsync(user);
				return View("ChangePasswordConfirmation");
			}

			return View(model);
		}
    }
}
