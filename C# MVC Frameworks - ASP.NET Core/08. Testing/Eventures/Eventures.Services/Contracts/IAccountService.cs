using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Eventures.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Eventures.Services.Contracts
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(string username, string password);

        Task<bool> CreateUserAsync(User user, string password);

        Task<IdentityResult> AddToRoleAsync(User user, string roleName);

        Task SignInAfterRegistrationAsync(User user);

        Task LogoutAsync();

        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();

        Task<bool> ExternalLoginUserAsync(ExternalLoginInfo info);

        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);

        IQueryable<User> GetAllUsersExceptCurrentUser(string username);

        Task<bool> CheckUserRoleAsync(User user, string roleName);

        User FindUserById(string userId);

        Task ChangeUserRoleAsync(User user);

        string GetUserId(ClaimsPrincipal claimsPrincipal);
    }
}
