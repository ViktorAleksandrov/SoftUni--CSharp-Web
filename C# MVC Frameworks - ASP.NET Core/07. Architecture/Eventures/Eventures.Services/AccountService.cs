using System.Security.Claims;
using System.Threading.Tasks;
using Eventures.Data;
using Eventures.Models;
using Eventures.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Eventures.Services
{
    public class AccountService : IAccountService
    {
        private readonly EventuresDbContext dbContext;
        private readonly SignInManager<User> signInManager;

        public AccountService(EventuresDbContext dbContext, SignInManager<User> signInManager)
        {
            this.dbContext = dbContext;
            this.signInManager = signInManager;
        }

        public async Task<bool> ExternalLoginUser(ExternalLoginInfo info)
        {
            string email = info.Principal.FindFirstValue(ClaimTypes.Email);

            User user = await this.signInManager.UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email
                };

                IdentityResult result = await this.signInManager.UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    IdentityResult externalLoginResult = await this.signInManager.UserManager.AddLoginAsync(user, info);

                    if (externalLoginResult.Succeeded)
                    {
                        await this.signInManager.UserManager.AddToRoleAsync(user, "USER");
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
    }
}
