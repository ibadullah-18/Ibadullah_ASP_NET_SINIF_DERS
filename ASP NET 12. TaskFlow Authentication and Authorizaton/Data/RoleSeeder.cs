using ASP_NET_12._TaskFlow_Authentication_and_Authorizaton.Models;
using Microsoft.AspNetCore.Identity;

namespace ASP_NET_12._TaskFlow_Authentication_and_Authorizaton.Data;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManger = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[] { "Admin", "Manager", "User" };

        foreach (var role in roles)
        {
            if (!await roleManger.RoleExistsAsync(role))
                await roleManger.CreateAsync(new IdentityRole(role));
        }

        var adminEmail = "admin@taskflow.com";
        var adminPassword = "Admin123";

        if(await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Nadir",
                LastName = "Zamanov",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded) await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
