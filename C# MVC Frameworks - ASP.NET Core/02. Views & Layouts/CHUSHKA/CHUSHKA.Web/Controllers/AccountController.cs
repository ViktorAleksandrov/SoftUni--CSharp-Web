using System.Linq;
using CHUSHKA.Models;
using CHUSHKA.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CHUSHKA.Web.Controllers
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
        public IActionResult Login(LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                SignInResult signInResult = this.signInManager
                    .PasswordSignInAsync(model.Username, model.Password, false, false)
                    .Result;

                if (signInResult.Succeeded)
                {
                    return this.Redirect("/");
                }
            }

            return this.View();
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FullName = model.FullName
                };

                IdentityResult result = this.signInManager
                    .UserManager
                    .CreateAsync(user, model.Password)
                    .Result;

                if (result.Succeeded)
                {
                    if (this.signInManager.UserManager.Users.Count() == 1)
                    {
                        this.signInManager.UserManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                    else
                    {
                        this.signInManager.UserManager.AddToRoleAsync(user, "User").Wait();
                    }

                    this.signInManager.SignInAsync(user, false).Wait();

                    return this.Redirect("/");
                }
            }

            return this.View();
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.signInManager.SignOutAsync().Wait();

            return this.Redirect("/");
        }
    }
}