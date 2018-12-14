using Eventures.Models;
using Eventures.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Eventures.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginUserBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                SignInResult signInResult = this.signInManager
                    .PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false)
                    .Result;

                if (signInResult.Succeeded)
                {
                    return this.RedirectToAction("Index", "Home");
                }
            }

            return this.View();
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UniqueCitizenNumber = model.UCN
                };

                IdentityResult result = this.signInManager
                    .UserManager
                    .CreateAsync(user, model.Password)
                    .Result;

                if (result.Succeeded)
                {
                    this.signInManager.UserManager.AddToRoleAsync(user, "USER").Wait();
                    this.signInManager.SignInAsync(user, false).Wait();

                    return this.RedirectToAction("Index", "Home");
                }
            }

            return this.View();
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.signInManager.SignOutAsync().Wait();

            return this.RedirectToAction("Index", "Home");
        }
    }
}