using System.Linq;
using System.Threading.Tasks;
using Eventures.Data;
using Eventures.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Eventures.Web.Middlewares
{
    public class SeedDataMiddleware
    {
        private readonly RequestDelegate next;

        public SeedDataMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(
            HttpContext httpContext,
            EventuresDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            await this.SeedRoles(dbContext, roleManager);
            await this.SeedFirstUser(dbContext, userManager);

            await this.next(httpContext);
        }

        private async Task SeedFirstUser(EventuresDbContext dbContext, UserManager<User> userManager)
        {
            if (!dbContext.Users.Any())
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Admin",
                    LastName = "Adminov",
                    UniqueCitizenNumber = "123456789"
                };

                IdentityResult result = await userManager.CreateAsync(user, "123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "ADMIN");
                }
            }
        }

        private async Task SeedRoles(EventuresDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            if (!dbContext.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("ADMIN"));
                await roleManager.CreateAsync(new IdentityRole("USER"));
            }
        }
    }
}
