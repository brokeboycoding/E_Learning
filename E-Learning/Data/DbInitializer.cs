using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace E_Learning.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAdminUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin@123!"; // Nên đổi trong môi trường thật
            string adminRole = "Admin";

            // Kiểm tra nếu vai trò Admin chưa có thì tạo
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // Kiểm tra nếu Admin chưa tồn tại thì tạo
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }
}
