using CakeBake.Constants;
using CakeBake.Models;
using Microsoft.AspNetCore.Identity;

namespace CakeBake.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            if (!await roleManager.RoleExistsAsync(Roles.Customer))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Customer));
            }
        }

        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var admins = await userManager.GetUsersInRoleAsync(Roles.Admin);

            if (!admins.Any())
            {
                var admin = new ApplicationUser
                {
                    FullName = "System Admin",
                    UserName = "admin@cakebake.com",
                    Email = "admin@cakebake.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
                }
            }
        }
    }
}