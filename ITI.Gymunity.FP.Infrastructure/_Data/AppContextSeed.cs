using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
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
        public static async Task SeedIdentityDataAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                if (!roleManager.Roles.Any())
                {
                    var adminRole = new IdentityRole(UserRole.Admin.ToString());
                    await roleManager.CreateAsync(adminRole);

                    var trainerRole = new IdentityRole(UserRole.Trainer.ToString());
                    await roleManager.CreateAsync(trainerRole);
                    
                    var clientRole = new IdentityRole(UserRole.Client.ToString());
                    await roleManager.CreateAsync(clientRole);
                }
                else
                {
                    Console.WriteLine($"\n{string.Join(", ", roleManager.Roles.Select(r => r.Name))}\n");
                }

                var user = new AppUser()
                {
                    FullName = "Admin User",
                    UserName = "admin",
                    Email = "admin@Gymunity.com",
                    Role = UserRole.Admin,
                };

                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
            }

        }
    }
}
