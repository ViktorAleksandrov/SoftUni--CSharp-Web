using System.Linq;
using IRunes.Data;
using IRunes.Models.Entities;
using IRunes.Models.ViewModels.Users;
using IRunes.Services.Contracts;

namespace IRunes.Services
{
    public class UserService : IUserService
    {
        public void AddUser(RegisterViewModel model)
        {
            var user = new User
            {
                Username = model.Username,
                Password = model.Password,
                Email = model.Email
            };

            using (var db = new IRunesContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public bool CheckIfUserExists(string username)
        {
            using (var db = new IRunesContext())
            {
                return db.Users.Any(u => u.Username == username);
            }
        }

        public User GetUser(LoginViewModel model)
        {
            using (var db = new IRunesContext())
            {
                User user = db.Users.FirstOrDefault(u =>
                (u.Username == model.Username || u.Email == model.Username)
                && u.Password == model.Password);

                return user;
            }
        }
    }
}
