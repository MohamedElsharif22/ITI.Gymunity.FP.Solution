using ITI.Gymunity.FP.Domain.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure._Data
{
    public static class DbContextMaigrationAndSeeding
    {
        public static async Task<WebApplication> MiagrateAndSeedDatabasesAsync(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _appDbContext = services.GetRequiredService<AppDbContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _appDbContext.Database.MigrateAsync();

                var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var _userManeger = services.GetRequiredService<UserManager<AppUser>>();
                var _context = services.GetRequiredService<AppDbContext>();

                await AppContextSeed.SeedIdentityDataAsync(_userManeger, _roleManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AppDbContext>();

                logger.LogError(ex, "an error has been occured while running migration!");
            }

            return webApplication;
        }
    }
}
