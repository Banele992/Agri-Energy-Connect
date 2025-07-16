using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeRolesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            string[] roles = { "Admin", "Employee", "Farmer" };
            
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
