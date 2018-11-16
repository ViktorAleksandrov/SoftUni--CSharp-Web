using MishMash.ViewModels.Users;

namespace MishMash.Services.Contracts
{
    public interface IUserService
    {
        bool IsUsernameAlreadyTaken(string username);

        bool HasUser(LoginViewModel model);

        void AddUser(RegisterViewModel model);

        string GetUserRole(string username);
    }
}
