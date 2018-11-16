using System.Linq;
using CHUSHKA.Data;
using CHUSHKA.Models;
using CHUSHKA.Models.Enums;
using CHUSHKA.Services.Base;
using CHUSHKA.Services.Contracts;
using CHUSHKA.ViewModels.Users;

namespace CHUSHKA.Services
{
    public class UsersService : BaseService, IUsersService
    {
        public UsersService(ChushkaDbContext context)
            : base(context)
        {
        }

        public bool UserExists(LoginViewModel model)
        {
            return this.context.Users
                .Any(u => u.Username == model.Username && u.Password == model.Password);
        }

        public string GetRole(string username)
        {
            return this.context.Users
                .FirstOrDefault(u => u.Username == username)
                .Role
                .ToString();
        }

        public void RegisterUser(RegisterViewModel model)
        {
            Role role = this.context.Users.Any() ? Role.User : Role.Admin;

            var user = new User
            {
                Username = model.Username,
                Password = model.Password,
                FullName = model.FullName,
                Email = model.Email,
                Role = role
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }
    }
}
