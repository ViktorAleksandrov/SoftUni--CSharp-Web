using CHUSHKA.ViewModels.Users;

namespace CHUSHKA.Services.Contracts
{
    public interface IUsersService
    {
        bool UserExists(LoginViewModel model);

        string GetRole(string username);

        void RegisterUser(RegisterViewModel model);
    }
}
