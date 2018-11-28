using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CHUSHKA.Web.Utilities
{
    public static class RolesSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            bool adminRoleExists = roleManager.RoleExistsAsync("Admin").Result;

            if (!adminRoleExists)
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
            }

            bool userRoleExists = roleManager.RoleExistsAsync("User").Result;

            if (!userRoleExists)
            {
                roleManager.CreateAsync(new IdentityRole("User")).Wait();
            }
        }
    }
}
