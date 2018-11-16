using System.Collections.Generic;
using MishMash.Services.Contracts;
using MishMash.ViewModels.Users;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Security;

namespace MishMash.App.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (this.IsLoggedIn)
            {
                return this.RedirectToAction("/");
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) ||
                string.IsNullOrWhiteSpace(model.Password) ||
                string.IsNullOrWhiteSpace(model.ConfirmPassword) ||
                string.IsNullOrWhiteSpace(model.Email) ||
                model.Password != model.ConfirmPassword ||
                this.userService.IsUsernameAlreadyTaken(model.Username))
            {
                return this.View();
            }

            this.userService.AddUser(model);

            string role = this.userService.GetUserRole(model.Username);

            this.SignIn(new IdentityUser
            {
                Username = model.Username,
                Roles = new List<string> { role }
            });

            return this.RedirectToAction("/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (this.IsLoggedIn)
            {
                return this.RedirectToAction("/");
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!this.userService.HasUser(model))
            {
                return this.View();
            }

            string role = this.userService.GetUserRole(model.Username);

            this.SignIn(new IdentityUser
            {
                Username = model.Username,
                Roles = new List<string> { role }
            });

            return this.RedirectToAction("/");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            this.SignOut();

            return this.RedirectToAction("/");
        }
    }
}
