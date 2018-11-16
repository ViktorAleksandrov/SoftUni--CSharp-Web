using System.Linq;
using TORSHIA.Data;
using TORSHIA.Models;
using TORSHIA.Models.Enums;
using TORSHIA.Services.Base;
using TORSHIA.Services.Contracts;

namespace TORSHIA.Services
{
    public class UsersService : BaseService, IUsersService
    {
        public UsersService(TorshiaDbContext context)
            : base(context)
        {
        }

        public bool UserExists(string username, string password)
        {
            return this.context.Users
                .Any(u => u.Username == username && u.Password == password);
        }

        public string GetRole(string username)
        {
            return this.context.Users
                .FirstOrDefault(u => u.Username == username)
                .Role
                .ToString();
        }

        public void RegisterUser(string username, string password, string email)
        {
            Role role = this.context.Users.Any() ? Role.User : Role.Admin;

            var user = new User
            {
                Username = username,
                Password = password,
                Email = email,
                Role = role
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }
    }
}
