﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Shop.Core.Domain;
using Shop.Models.Accounts;

namespace Shop.Controllers
{
	public class AccountsController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountsController
			(
				UserManager<ApplicationUser> userManager,
				SignInManager<ApplicationUser> signInManager
			)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}


		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
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

					ViewBag.ErrorTitle = "Registration successful";
					ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
						"email, by clicking on the confirmation link we have emailed you";

					return View("EmailError");
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
    }
}
