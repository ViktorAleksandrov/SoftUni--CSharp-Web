using IRunes.Models.Entities;
using IRunes.Models.ViewModels.Users;

namespace IRunes.Services.Contracts
{
    public interface IUserService
    {
        bool CheckIfUserExists(string username);

        void AddUser(RegisterViewModel model);

        User GetUser(LoginViewModel model);
    }
}