using ITI.Gymunity.FP.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure._Data
{
    public class AppContextSeed
    {
        public static void SeedDatabase(ModelBuilder modelBuilder)
        {
            
        }
        public static async Task SeedIdentityDataAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                if (!roleManager.Roles.Any())
                {
                    var adminRole = new IdentityRole("Admin");
                    await roleManager.CreateAsync(adminRole);

                    var userRole = new IdentityRole("User");
                    await roleManager.CreateAsync(userRole);
                }
                else
                {
                    Console.WriteLine($"\n{string.Join(", ", roleManager.Roles.Select(r => r.Name))}\n");
                }

                var user = new ApplicationUser()
                {
                    FirstName = "Admin",
                    LastName = "01",
                    UserName = "admin",
                    Email = "admin@AppName.com",


                };

                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, "Admin");
            }

        }
    }
}
