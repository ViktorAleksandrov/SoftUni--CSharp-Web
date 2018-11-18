using System.Linq;
using PANDA.App.Data;
using PANDA.App.Models;
using PANDA.App.Models.Enums;
using PANDA.App.ViewModels.Users;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;
using SIS.Framework.Security;

namespace PANDA.App.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(PandaDbContext context)
            : base(context)
        {
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
            User user = this.context.Users.
                FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
            {
                return this.View();
            }

            this.SignIn(new IdentityUser
            {
                Username = user.Username,
                Roles = new[] { user.Role.ToString() }
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

            Role role = this.context.Users.Any() ? Role.User : Role.Admin;

            var user = new User
            {
                Username = model.Username,
                Password = model.Password,
                Email = model.Email,
                Role = role
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();

            this.SignIn(new IdentityUser
            {
                Username = user.Username,
                Roles = new[] { user.Role.ToString() }
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
