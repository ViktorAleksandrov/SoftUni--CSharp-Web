using IRunes.Models.Entities;
using IRunes.Models.ViewModels.Users;
using IRunes.Services.Contracts;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;
using SIS.Framework.Security;

namespace IRunes.App.Controllers
{
    public class UsersController : Controller
    {
        private const string Display = "display";
        private const string None = "none";
        private const string ErrorMessage = "errorMessage";
        private const string Inline = "inline";

        private readonly IHashService hashService;
        private readonly IUserService userService;

        public UsersController(IHashService hashService, IUserService userService)
        {
            this.hashService = hashService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            this.SetViewByUserAuthentication();

            this.Model[Display] = None;

            if (this.Identity != null)
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
                string.IsNullOrWhiteSpace(model.Email))
            {
                this.Model[Display] = Inline;
                this.Model[ErrorMessage] = "All fields must be filled!";

                return this.View();
            }

            if (model.Password != model.ConfirmPassword)
            {
                this.Model[Display] = Inline;
                this.Model[ErrorMessage] = "Passwords don't match!";

                return this.View();
            }

            if (this.userService.CheckIfUserExists(model.Username))
            {
                this.Model[Display] = Inline;
                this.Model[ErrorMessage] = "Username with this name already exists!";

                return this.View();
            }

            model.Password = this.hashService.Hash(model.Password);

            this.userService.AddUser(model);

            this.SignIn(new IdentityUser { Username = model.Username });

            return this.RedirectToAction("/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            this.SetViewByUserAuthentication();

            this.Model[Display] = None;

            if (this.Identity != null)
            {
                return this.RedirectToAction("/");
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            model.Password = this.hashService.Hash(model.Password);

            User user = this.userService.GetUser(model);

            if (user == null)
            {
                this.Model[Display] = Inline;
                this.Model[ErrorMessage] = "Invalid username or password!";

                return this.View();
            }

            this.SignIn(new IdentityUser { Username = model.Username });

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
