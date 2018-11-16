using System.Collections.Generic;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;
using SIS.Framework.Security;
using TORSHIA.App.Controllers.Base;
using TORSHIA.Services.Contracts;
using TORSHIA.ViewModels.Users;

namespace TORSHIA.App.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            bool userExists = this.usersService.UserExists(model.Username, model.Password);

            if (!userExists)
            {
                return this.View();
            }

            string role = this.usersService.GetRole(model.Username);

            this.SignIn(new IdentityUser
            {
                Username = model.Username,
                Roles = new List<string> { role }
            });

            return this.RedirectToAction("/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return this.View();
            }

            this.usersService.RegisterUser(model.Username, model.Password, model.Email);

            string role = this.usersService.GetRole(model.Username);

            this.SignIn(new IdentityUser
            {
                Username = model.Username,
                Roles = new List<string> { role }
            });

            return this.RedirectToAction("/");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            this.SignOut();

            return this.RedirectToAction("/");
        }
    }
}
