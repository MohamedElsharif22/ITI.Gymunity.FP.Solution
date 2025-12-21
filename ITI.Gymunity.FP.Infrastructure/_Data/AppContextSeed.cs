using ITI.Gymunity.FP.Domain.Models.Enums;
using ITI.Gymunity.FP.Domain.Models.Identity;
using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.Models.Trainer;
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
            var trainerId = "29302cc1-6bc6-4534-8269-de75ef3650a7";
            var fixedDateTime = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);

            //// Seed Exercises
            //modelBuilder.Entity<Exercise>().HasData(
            //    new Exercise
            //    {
            //        Id = 1,
            //        Name = "Bench Press",
            //        Category = "Strength",
            //        MuscleGroup = "Chest",
            //        Equipment = "Barbell",
            //        VideoDemoUrl = "https://example.com/videos/bench-press.mp4",
            //        ThumbnailUrl = "https://example.com/images/bench-press.jpg",
            //        IsCustom = false,
            //        TrainerId = null,
            //        CreatedAt = fixedDateTime,
            //        UpdatedAt = fixedDateTime,
            //        IsDeleted = false
            //    },
            //    new Exercise
            //    {
            //        Id = 2,
            //        Name = "Squat",
            //        Category = "Strength",
            //        MuscleGroup = "Legs",
            //        Equipment = "Barbell",
            //        VideoDemoUrl = "https://example.com/videos/squat.mp4",
            //        ThumbnailUrl = "https://example.com/images/squat.jpg",
            //        IsCustom = false,
            //        TrainerId = null,
            //        CreatedAt = fixedDateTime,
            //        UpdatedAt = fixedDateTime,
            //        IsDeleted = false
            //    },
            //    new Exercise
            //    {
            //        Id = 3,
            //        Name = "Custom Leg Press",
            //        Category = "Strength",
            //        MuscleGroup = "Legs",
            //        Equipment = "Machine",
            //        VideoDemoUrl = "https://example.com/videos/leg-press.mp4",
            //        ThumbnailUrl = "https://example.com/images/leg-press.jpg",
            //        IsCustom = true,
            //        TrainerId = trainerId,
            //        CreatedAt = fixedDateTime,
            //        UpdatedAt = fixedDateTime,
            //        IsDeleted = false
            //    }
            //);

            //// Seed Packages
            //modelBuilder.Entity<Package>().HasData(
            //    new Package
            //    {
            //        Id = 1,
            //        TrainerId = trainerId,
            //        Name = "Basic Package",
            //        Description = "Entry-level training package with essential features",
            //        PriceMonthly = 99.99m,
            //        PriceYearly = 999.90m,
            //        Currency = "EGP",
            //        FeaturesJson = "{\"formChecksPerWeek\": 2, \"priorityMessaging\": false, \"monthlyVideoCall\": false}",
            //        IsActive = true,
            //        ThumbnailUrl = "https://example.com/images/basic-package.jpg",
            //        CreatedAt = fixedDateTime,
            //        IsAnnual = false,
            //        PromoCode = null,
            //        UpdatedAt = fixedDateTime,
            //        IsDeleted = false
            //    },
            //    new Package
            //    {
            //        Id = 2,
            //        TrainerId = trainerId,
            //        Name = "Premium Package",
            //        Description = "Advanced training package with all features",
            //        PriceMonthly = 299.99m,
            //        PriceYearly = 2999.90m,
            //        Currency = "EGP",
            //        FeaturesJson = "{\"formChecksPerWeek\": 4, \"priorityMessaging\": true, \"monthlyVideoCall\": true, \"customProgramEveryWeeks\": 4, \"earlyAccess\": true}",
            //        IsActive = true,
            //        ThumbnailUrl = "https://example.com/images/premium-package.jpg",
            //        CreatedAt = fixedDateTime,
            //        IsAnnual = false,
            //        PromoCode = "PREMIUM20",
            //        UpdatedAt = fixedDateTime,
            //        IsDeleted = false
            //    }
            //);

            //// Seed Programs
            //modelBuilder.Entity<Program>().HasData(
            //    new Program
            //    {
            //        Id = 1,
            //        TrainerId = trainerId,
            //        Title = "12-Week Muscle Building Program",
            //        Description = "A comprehensive 12-week program designed to build muscle mass and strength",
            //        Type = ProgramType.Workout,
            //        DurationWeeks = 12,
            //        Price = 499.99m,
            //        IsPublic = true,
            //        MaxClients = 50,
            //        ThumbnailUrl = "https://example.com/images/muscle-building.jpg",
            //        CreatedAt = fixedDateTime,
            //        UpdatedAt = fixedDateTime,
            //        TrainerProfileId = null,
            //        IsDeleted = false
            //    },
            //    new Program
            //    {
            //        Id = 2,
            //        TrainerId = trainerId,
            //        Title = "8-Week Fat Loss Program",
            //        Description = "An intense 8-week program focused on fat loss and cardio conditioning",
            //        Type = ProgramType.Workout,
            //        DurationWeeks = 8,
            //        Price = 299.99m,
            //        IsPublic = true,
            //        MaxClients = 100,
            //        ThumbnailUrl = "https://example.com/images/fat-loss.jpg",
            //        CreatedAt = fixedDateTime,
            //        UpdatedAt = fixedDateTime,
            //        TrainerProfileId = null,
            //        IsDeleted = false
            //    },
            //    new Program
            //    {
            //        Id = 3,
            //        TrainerId = trainerId,
            //        Title = "Complete Nutrition Guide",
            //        Description = "A detailed nutrition plan for optimal health and performance",
            //        Type = ProgramType.Nutrition,
            //        DurationWeeks = 4,
            //        Price = null,
            //        IsPublic = true,
            //        MaxClients = null,
            //        ThumbnailUrl = "https://example.com/images/nutrition.jpg",
            //        CreatedAt = fixedDateTime,
            //        UpdatedAt = fixedDateTime,
            //        TrainerProfileId = null,
            //        IsDeleted = false
            //    }
            //);
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

                var admin = new AppUser()
                {
                    FullName = "Admin User",
                    UserName = "admin",
                    Email = "admin@Gymunity.com",
                    Role = UserRole.Admin,
                };
                var trainer = new AppUser()
                {
                    FullName = "trainer test",
                    UserName = "trainerfit",
                    Email = "trainer@Gymunity.com",
                    Role = UserRole.Trainer,
                };
                var client = new AppUser()
                {
                    FullName = "Refaat",
                    UserName = "client1",
                    Email = "client@Gymunity.com",
                    Role = UserRole.Client,
                };

                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.CreateAsync(trainer, "P@ssw0rd");
                await userManager.CreateAsync(client, "P@ssw0rd");
                await userManager.AddToRoleAsync(admin, UserRole.Admin.ToString());
                await userManager.AddToRoleAsync(trainer, UserRole.Trainer.ToString());
                await userManager.AddToRoleAsync(client, UserRole.Client.ToString());
            }

        }
    }
}
