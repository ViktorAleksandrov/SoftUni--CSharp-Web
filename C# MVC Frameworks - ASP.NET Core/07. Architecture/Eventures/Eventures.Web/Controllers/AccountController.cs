using System.Collections.Generic;
using System.Linq;
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

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return this.RedirectToAction("Index", "Home");
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

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AllUsers()
        {
            IQueryable<User> users = this.signInManager.UserManager.Users
                .Where(u => u.UserName != this.User.Identity.Name);

            var usersModels = new List<UserViewModel>();

            foreach (User user in users)
            {
                var userModel = new UserViewModel
                {
                    Id = user.Id,
                    Username = user.UserName
                };

                if (await this.signInManager.UserManager.IsInRoleAsync(user, "ADMIN"))
                {
                    userModel.Role = "ADMIN";
                }
                else
                {
                    userModel.Role = "USER";
                }

                usersModels.Add(userModel);
            }

            return this.View(usersModels);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ChangeUserRole(string userId)
        {
            User user = await this.signInManager.UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return this.RedirectToAction(nameof(AllUsers));
            }

            if (await this.signInManager.UserManager.IsInRoleAsync(user, "ADMIN"))
            {
                await this.signInManager.UserManager.RemoveFromRoleAsync(user, "ADMIN");
                await this.signInManager.UserManager.AddToRoleAsync(user, "USER");
            }
            else
            {
                await this.signInManager.UserManager.RemoveFromRoleAsync(user, "USER");
                await this.signInManager.UserManager.AddToRoleAsync(user, "ADMIN");
            }

            return this.RedirectToAction(nameof(AllUsers));
        }
    }
}