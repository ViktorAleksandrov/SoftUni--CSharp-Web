using System.Threading.Tasks;
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
        public async Task<IActionResult> Login(LoginUserBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                SignInResult signInResult = await this.signInManager
                    .PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

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
        public async Task<IActionResult> Register(RegisterUserBindingModel model)
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

                IdentityResult result = await this.signInManager
                    .UserManager
                    .CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await this.signInManager.UserManager.AddToRoleAsync(user, "USER");
                    await this.signInManager.SignInAsync(user, false);

                    return this.RedirectToAction("Index", "Home");
                }
            }

            return this.View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return this.RedirectToAction("Index", "Home");
        }
    }
}