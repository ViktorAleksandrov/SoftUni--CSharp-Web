using System.Threading.Tasks;
using AutoMapper;
using Eventures.Models;
using Eventures.Services.Contracts;
using Eventures.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Eventures.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;

        public AccountController(IAccountService accountService, SignInManager<User> signInManager, IMapper mapper)
        {
            this.accountService = accountService;
            this.signInManager = signInManager;
            this.mapper = mapper;
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
                    .PasswordSignInAsync(model.Username, model.Password, false, false);

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
                User user = this.mapper.Map<User>(model);

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

        public async Task<IActionResult> ExternalLogin()
        {
            ExternalLoginInfo info = await this.signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            bool result = await this.accountService.ExternalLoginUser(info);

            if (result)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider)
        {
            string redirectUrl = "/Account/ExternalLogin";

            AuthenticationProperties properties = this.signInManager
                    .ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return this.RedirectToAction("Index", "Home");
        }
    }
}