﻿using Microsoft.AspNetCore.Authentication.Cookies;
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

					EmailTokenDto newsignup = new();
					newsignup.Token = token;
					newsignup.Body = $"Thank you for registering: <a href=\"{confirmationLink}\" >click here</a>";
					newsignup.Subject = "CRUD REgistration";
					newsignup.To = user.Email;

					if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
					{
						return RedirectToAction("ListUser", "Administrations");
					}

					_emailServices.SendEmailToken(newsignup, token);
					List<string> errordatas = 
						[
						"Area", "Accounts",
						"Issue", "Success",
						"StatusMessage", "Registration Success",
						"ActedOn", $"{vm.Email}",
						"CreatedAccountData", $"{vm.Email}\n{vm.City}\n[password hidden]\n[password hidden]"
						];
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
					var resetPasswordLink = Url.Action("ResetPassword", "Accounts", new { email = model.Email, token = token }, Request.Scheme);

					EmailTokenDto resetPasswordDto = new()
					{
						Token = token,
						Body = $"To reset your password, click <a href=\"{resetPasswordLink}\">here</a>.",
						Subject = "Password Reset",
						To = model.Email
					};

					_emailServices.SendEmailToken(resetPasswordDto, token);

					return View("ForgotPasswordConfirmation");
				}
                return View("ForgotPasswordConfirmation");

            }
			return View(model);
        }

		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPassword(string email, string token)
		{
			if (email == null || token == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var model = new ResetPasswordViewModel { Email = email, Token = token };
			return View(model);
		}

		// POST: /Accounts/ResetPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					return RedirectToAction("ResetPasswordConfirmation");
				}

				var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

				if (result.Succeeded)
				{
					return RedirectToAction("ResetPasswordConfirmation");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return View(model);
		}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }



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
