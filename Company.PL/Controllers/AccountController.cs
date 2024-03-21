using Company.DAL.Entities;
using Company.PL.Helper;
using Company.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using NuGet.Common;

namespace Company.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult SignUp()
		{
			//return View(new SignUpViewModel());
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
		{
			if (ModelState.IsValid) //server side validation
			{
				var user = new ApplicationUser
				{
					UserName = signUpViewModel.Email.Split('@')[0], //ayawaheed@gamil.com  
					Email = signUpViewModel.Email,
					IsActive = true
				};

				var result = await _userManager.CreateAsync(user, signUpViewModel.Password);
				if (result.Succeeded)
					return RedirectToAction("Login");

				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);

			}

			return View(signUpViewModel);

		}


		public IActionResult Login()
		{
			//return View(new SignUpViewModel());
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(SignInViewModel signInViewModel)
		{
			if (ModelState.IsValid) //server side validation
			{
				var user = await _userManager.FindByEmailAsync(signInViewModel.Email);

				if (user is null)
					ModelState.AddModelError("", "Invalid Login");


				var flag = await _userManager.CheckPasswordAsync(user, signInViewModel.Password);
				if (user != null && flag)
				{
					var result = await _signInManager.PasswordSignInAsync(user, signInViewModel.Password, signInViewModel.RememberMe, false);

					if (result.Succeeded)
						return RedirectToAction("Index", "Home");
				}
			}


			return View(signInViewModel);
		}

		public async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}

		
		public IActionResult ForgetPassword()
		{
			return View();
		}


		[HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel input)
        {
            if(ModelState.IsValid)
			{
                var user = await _userManager.FindByEmailAsync(input.Email);

                if (user is null)
                    ModelState.AddModelError("", "Invalid Login");

				if(user != null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);

					var resetPasswordLink = Url.Action("ResetPassword", "Account", new { Email = input.Email, Token = token }, "https"/*Request.Scheme*/);

					var email = new Email
					{
						Title = "Reset Password",
						Body = resetPasswordLink,
						To = input.Email
                    };


                    //send Email
                    EmailSettings.SendEmail(email);

                    return RedirectToAction("CompleteForgetPassword");

                }
            }

            return View(input);
        }

		public IActionResult CompleteForgetPassword()
		{
			return View();
		}

		public IActionResult ResetPassword(string email, string token)
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel input)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(input.Email);

				if (user is null)
					ModelState.AddModelError("", "Email Does Not Exist");


				if (user != null)
				{
					var result = await _userManager.ResetPasswordAsync(user, input.Token, input.Password);

					if (result.Succeeded)
						return RedirectToAction("Login");

					foreach (var error in result.Errors)
						ModelState.AddModelError("", error.Description);
				}
			}

			return View(input);
		}

		
		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
