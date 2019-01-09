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

namespace Eventures.Web.Controllers
{
    public class AccountController : Controller
    {
        private const string HomeControllerName = "Home";
        private const string AdminRole = "ADMIN";
        private const string UserRole = "USER";

        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            this.accountService = accountService;
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
                bool isLoginSuccessful = await this.accountService.LoginAsync(model.Username, model.Password);

                if (isLoginSuccessful)
                {
                    return this.RedirectToAction(nameof(HomeController.Index), HomeControllerName);
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

                bool isUserCreatedSuccessfully = await this.accountService.CreateUserAsync(user, model.Password);

                if (isUserCreatedSuccessfully)
                {
                    await this.accountService.AddToRoleAsync(user, UserRole);
                    await this.accountService.SignInAfterRegistrationAsync(user);

                    return this.RedirectToAction(nameof(HomeController.Index), HomeControllerName);
                }
            }

            return this.View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.accountService.LogoutAsync();

            return this.RedirectToAction(nameof(HomeController.Index), HomeControllerName);
        }

        public async Task<IActionResult> ExternalLogin()
        {
            ExternalLoginInfo info = await this.accountService.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return this.RedirectToAction(nameof(HomeController.Index), HomeControllerName);
            }

            bool result = await this.accountService.ExternalLoginUserAsync(info);

            if (result)
            {
                return this.RedirectToAction(nameof(HomeController.Index), HomeControllerName);
            }

            return this.RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider)
        {
            string redirectUrl = "/Account/ExternalLogin";

            AuthenticationProperties properties = this.accountService
                    .ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> AllUsers()
        {
            IQueryable<User> users = this.accountService.GetAllUsersExceptCurrentUser(this.User.Identity.Name);

            var usersModels = new List<UserViewModel>();

            foreach (User user in users)
            {
                UserViewModel userModel = this.mapper.Map<UserViewModel>(user);

                bool isInRoleAdmin = await this.accountService.CheckUserRoleAsync(user, AdminRole);

                if (isInRoleAdmin)
                {
                    userModel.Role = AdminRole;
                }
                else
                {
                    userModel.Role = UserRole;
                }

                usersModels.Add(userModel);
            }

            return this.View(usersModels);
        }

        [HttpPost]
        [Authorize(Roles = AdminRole)]
        public async Task<IActionResult> ChangeUserRole(string userId)
        {
            User user = this.accountService.FindUserById(userId);

            if (user == null)
            {
                return this.RedirectToAction(nameof(AllUsers));
            }

            await this.accountService.ChangeUserRoleAsync(user);

            return this.RedirectToAction(nameof(AllUsers));
        }
    }
}