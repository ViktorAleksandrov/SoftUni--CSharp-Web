namespace TORSHIA.Services.Contracts
{
    public interface IUsersService
    {
        bool UserExists(string username, string password);

        string GetRole(string username);

        void RegisterUser(string username, string password, string email);
    }
}
