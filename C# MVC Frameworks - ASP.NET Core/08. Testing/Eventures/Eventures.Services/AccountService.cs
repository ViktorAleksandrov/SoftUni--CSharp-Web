using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Eventures.Data;
using Eventures.Models;
using Eventures.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Eventures.Services
{
    public class AccountService : IAccountService
    {
        private const string AdminRole = "ADMIN";
        private const string UserRole = "USER";

        private readonly EventuresDbContext dbContext;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public AccountService(
            EventuresDbContext dbContext, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
        {
            IdentityResult result = await this.userManager.AddToRoleAsync(user, roleName);

            return result;
        }

        private async Task<IdentityResult> RemoveFromRoleAsync(User user, string roleName)
        {
            IdentityResult result = await this.userManager.RemoveFromRoleAsync(user, roleName);

            return result;
        }

        public async Task<bool> CheckUserRoleAsync(User user, string roleName)
        {
            bool isInRole = await this.userManager.IsInRoleAsync(user, roleName);

            return isInRole;
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            AuthenticationProperties properties = this.signInManager
                    .ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return properties;
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            IdentityResult result = await this.userManager.CreateAsync(user, password);

            return result.Succeeded;
        }

        public async Task<bool> ExternalLoginUserAsync(ExternalLoginInfo info)
        {
            string email = info.Principal.FindFirstValue(ClaimTypes.Email);

            User user = await this.userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email
                };

                IdentityResult result = await this.userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    IdentityResult externalLoginResult = await this.userManager.AddLoginAsync(user, info);

                    if (externalLoginResult.Succeeded)
                    {
                        await this.AddToRoleAsync(user, UserRole);
                        await this.signInManager.SignInAsync(user, false);

                        return true;
                    }
                }

                return false;
            }
            else
            {
                SignInResult result = await this.signInManager
                    .ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

                if (result.Succeeded)
                {
                    return true;
                }

                return false;
            }
        }

        public User FindUserById(string userId)
        {
            User user = this.dbContext.Users.Find(userId);

            return user;
        }

        public IQueryable<User> GetAllUsersExceptCurrentUser(string username)
        {
            return this.dbContext
                .Users
                .Where(u => u.UserName != username);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            ExternalLoginInfo info = await this.signInManager.GetExternalLoginInfoAsync();

            return info;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            SignInResult signInResult = await this.signInManager
                .PasswordSignInAsync(username, password, false, false);

            return signInResult.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        public async Task SignInAfterRegistrationAsync(User user)
        {
            await this.signInManager.SignInAsync(user, false);
        }

        public async Task ChangeUserRoleAsync(User user)
        {
            bool isInRoleAdmin = await this.CheckUserRoleAsync(user, AdminRole);

            if (isInRoleAdmin)
            {
                await this.RemoveFromRoleAsync(user, AdminRole);
                await this.AddToRoleAsync(user, UserRole);
            }
            else
            {
                await this.RemoveFromRoleAsync(user, UserRole);
                await this.AddToRoleAsync(user, AdminRole);
            }
        }

        public string GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            return this.userManager.GetUserId(claimsPrincipal);
        }
    }
}
