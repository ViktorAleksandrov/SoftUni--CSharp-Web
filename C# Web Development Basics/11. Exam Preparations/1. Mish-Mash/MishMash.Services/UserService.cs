using System.Linq;
using MishMash.Data;
using MishMash.Models;
using MishMash.Models.Enums;
using MishMash.Services.Contracts;
using MishMash.ViewModels.Users;

namespace MishMash.Services
{
    public class UserService : IUserService
    {
        public void AddUser(RegisterViewModel model)
        {
            using (var db = new MishMashDbContext())
            {
                Role role = db.Users.Any() ? Role.User : Role.Admin;

                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    Email = model.Email,
                    Role = role
                };

                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public string GetUserRole(string username)
        {
            using (var db = new MishMashDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Username == username).Role.ToString();
            }
        }

        public bool HasUser(LoginViewModel model)
        {
            using (var db = new MishMashDbContext())
            {
                return db.Users.Any(u => u.Username == model.Username && u.Password == model.Password);
            }
        }

        public bool IsUsernameAlreadyTaken(string username)
        {
            using (var db = new MishMashDbContext())
            {
                return db.Users.Any(u => u.Username == username);
            }
        }
    }
}
