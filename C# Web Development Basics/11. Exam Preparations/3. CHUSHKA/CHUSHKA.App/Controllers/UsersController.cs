using CHUSHKA.App.Controllers.Base;
using CHUSHKA.Services.Contracts;
using CHUSHKA.ViewModels.Users;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;
using SIS.Framework.Security;

namespace CHUSHKA.App.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersService userService;

        public UsersController(IUsersService userService)
        {
            this.userService = userService;
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
            bool userExists = this.userService.UserExists(model);

            if (!userExists)
            {
                return this.View();
            }

            string role = this.userService.GetRole(model.Username);

            this.SignIn(new IdentityUser
            {
                Username = model.Username,
                Roles = new[] { role }
            });

            return this.RedirectToAction("/");
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
            if (model.Password != model.ConfirmPassword)
            {
                return this.View();
            }

            this.userService.RegisterUser(model);

            string role = this.userService.GetRole(model.Username);

            this.SignIn(new IdentityUser
            {
                Username = model.Username,
                Roles = new[] { role }
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
