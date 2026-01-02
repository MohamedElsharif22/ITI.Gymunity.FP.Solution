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
            var trainerProfileId = 6;
            var fixedDateTime = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
            var fixedDateTimeOffset = new DateTimeOffset(fixedDateTime);

            // Exercise IDs
            var exercises = Enumerable.Range(1001, 25).ToArray();

            // Seed 25 Exercises
            modelBuilder.Entity<Exercise>().HasData(
                new Exercise { Id = exercises[0], Name = "Barbell Back Squat", Category = "Strength", MuscleGroup = "Quadriceps", Equipment = "Barbell", VideoDemoUrl = "https://picsum.photos/seed/squat-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/squat/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[1], Name = "Goblet Squat", Category = "Strength", MuscleGroup = "Quadriceps", Equipment = "Dumbbell", VideoDemoUrl = "https://picsum.photos/seed/goblet-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/goblet/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[2], Name = "Romanian Deadlift", Category = "Strength", MuscleGroup = "Hamstrings", Equipment = "Barbell", VideoDemoUrl = "https://picsum.photos/seed/rdl-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/rdl/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[3], Name = "Bulgarian Split Squat", Category = "Strength", MuscleGroup = "Quadriceps", Equipment = "Dumbbell", VideoDemoUrl = "https://picsum.photos/seed/bulgarian-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/bulgarian/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[4], Name = "Leg Press", Category = "Strength", MuscleGroup = "Quadriceps", Equipment = "Machine", VideoDemoUrl = "https://picsum.photos/seed/legpress-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/legpress/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[5], Name = "Leg Curl", Category = "Strength", MuscleGroup = "Hamstrings", Equipment = "Machine", VideoDemoUrl = "https://picsum.photos/seed/legcurl-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/legcurl/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[6], Name = "Barbell Bench Press", Category = "Strength", MuscleGroup = "Chest", Equipment = "Barbell", VideoDemoUrl = "https://picsum.photos/seed/bench-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/bench/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[7], Name = "Dumbbell Shoulder Press", Category = "Strength", MuscleGroup = "Shoulders", Equipment = "Dumbbell", VideoDemoUrl = "https://picsum.photos/seed/shoulderpress-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/shoulderpress/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[8], Name = "Incline Dumbbell Press", Category = "Strength", MuscleGroup = "Chest", Equipment = "Dumbbell", VideoDemoUrl = "https://picsum.photos/seed/incline-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/incline/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[9], Name = "Lateral Raise", Category = "Strength", MuscleGroup = "Shoulders", Equipment = "Dumbbell", VideoDemoUrl = "https://picsum.photos/seed/lateral-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/lateral/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[10], Name = "Tricep Dips", Category = "Strength", MuscleGroup = "Triceps", Equipment = "Bodyweight", VideoDemoUrl = "https://picsum.photos/seed/dips-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/dips/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[11], Name = "Pull-ups", Category = "Strength", MuscleGroup = "Back", Equipment = "Bodyweight", VideoDemoUrl = "https://picsum.photos/seed/pullups-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/pullups/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[12], Name = "Barbell Row", Category = "Strength", MuscleGroup = "Back", Equipment = "Barbell", VideoDemoUrl = "https://picsum.photos/seed/row-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/row/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[13], Name = "Lat Pulldown", Category = "Strength", MuscleGroup = "Back", Equipment = "Cable", VideoDemoUrl = "https://picsum.photos/seed/latpull-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/latpull/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[14], Name = "Face Pulls", Category = "Strength", MuscleGroup = "Shoulders", Equipment = "Cable", VideoDemoUrl = "https://picsum.photos/seed/facepulls-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/facepulls/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[15], Name = "Bicep Curl", Category = "Strength", MuscleGroup = "Biceps", Equipment = "Dumbbell", VideoDemoUrl = "https://picsum.photos/seed/bicep-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/bicep/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[16], Name = "Plank", Category = "Core", MuscleGroup = "Abs", Equipment = "Bodyweight", VideoDemoUrl = "https://picsum.photos/seed/plank-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/plank/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[17], Name = "Cable Crunch", Category = "Core", MuscleGroup = "Abs", Equipment = "Cable", VideoDemoUrl = "https://picsum.photos/seed/crunch-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/crunch/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[18], Name = "Walking Lunges", Category = "Strength", MuscleGroup = "Quadriceps", Equipment = "Dumbbell", VideoDemoUrl = "https://picsum.photos/seed/lunges-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/lunges/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[19], Name = "Calf Raise", Category = "Strength", MuscleGroup = "Calves", Equipment = "Machine", VideoDemoUrl = "https://picsum.photos/seed/calf-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/calf/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[20], Name = "Chest Fly", Category = "Strength", MuscleGroup = "Chest", Equipment = "Dumbbell", VideoDemoUrl = "https://picsum.photos/seed/fly-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/fly/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[21], Name = "Hammer Curl", Category = "Strength", MuscleGroup = "Biceps", Equipment = "Dumbbell", VideoDemoUrl = "https://picsum.photos/seed/hammer-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/hammer/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[22], Name = "Tricep Pushdown", Category = "Strength", MuscleGroup = "Triceps", Equipment = "Cable", VideoDemoUrl = "https://picsum.photos/seed/pushdown-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/pushdown/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[23], Name = "Seated Cable Row", Category = "Strength", MuscleGroup = "Back", Equipment = "Cable", VideoDemoUrl = "https://picsum.photos/seed/cablerow-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/cablerow/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false },
                new Exercise { Id = exercises[24], Name = "Leg Extension", Category = "Strength", MuscleGroup = "Quadriceps", Equipment = "Machine", VideoDemoUrl = "https://picsum.photos/seed/legext-video/1920/1080", ThumbnailUrl = "https://picsum.photos/seed/legext/400/300", IsCustom = false, TrainerId = null, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false }
            );

            // Program
            var programId = 2001;
            modelBuilder.Entity<Program>().HasData(
                new Program { Id = programId, TrainerId = string.Empty, TrainerProfileId = trainerProfileId, Title = "Beginner Strength - 8 Weeks", Description = "8-week foundational strength program", Type = ProgramType.Workout, DurationWeeks = 8, Price = 49.99m, IsPublic = true, MaxClients = 500, ThumbnailUrl = "https://www.example.com/images/programs/beginner-strength-8w.jpg", CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false }
            );

            // 4 Weeks
            var weeks = Enumerable.Range(3001, 4).ToArray();
            for (int i = 0; i < 4; i++)
            {
                modelBuilder.Entity<ProgramWeek>().HasData(
                    new ProgramWeek { Id = weeks[i], ProgramId = programId, WeekNumber = i + 1, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false }
                );
            }

            // 16 Days (4 weeks × 4 days)
            var days = Enumerable.Range(4001, 16).ToArray();
            var dayTitles = new[] { "Lower Body A", "Upper Body Push", "Lower Body B", "Upper Body Pull" };
            var dayNotes = new[] { "Squat pattern + accessories", "Chest, shoulders, triceps", "Hinge pattern + unilateral", "Back, biceps, rear delts" };

            for (int week = 0; week < 4; week++)
            {
                for (int day = 0; day < 4; day++)
                {
                    int dayIndex = week * 4 + day;
                    modelBuilder.Entity<ProgramDay>().HasData(
                        new ProgramDay { Id = days[dayIndex], ProgramWeekId = weeks[week], DayNumber = day + 1, Title = dayTitles[day], Notes = dayNotes[day], CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false }
                    );
                }
            }

            // Program Day Exercises (6-8 per day)
            int pdeId = 5001;

            // Define exercise templates for each day type
            var lowerAExercises = new[] {
        (exercises[0], "4", "6-8", 180, "Main squat movement"),
        (exercises[2], "3", "8-10", 120, "Hamstring focus"),
        (exercises[4], "3", "10-12", 90, "Quad volume"),
        (exercises[5], "3", "12-15", 60, "Hamstring isolation"),
        (exercises[24], "3", "12-15", 60, "Quad isolation"),
        (exercises[19], "4", "15-20", 45, "Calf development"),
        (exercises[16], "3", "45-60s", 60, "Core stability")
    };

            var upperPushExercises = new[] {
        (exercises[6], "4", "6-8", 180, "Main pressing"),
        (exercises[7], "3", "8-10", 120, "Overhead press"),
        (exercises[8], "3", "8-12", 90, "Incline work"),
        (exercises[20], "3", "12-15", 60, "Chest isolation"),
        (exercises[9], "3", "12-15", 60, "Lateral delts"),
        (exercises[10], "3", "8-12", 90, "Tricep compound"),
        (exercises[22], "3", "12-15", 45, "Tricep isolation"),
        (exercises[17], "3", "30-45s", 60, "Core work")
    };

            var lowerBExercises = new[] {
        (exercises[2], "4", "6-8", 180, "Main hinge"),
        (exercises[3], "3", "8-10", 120, "Single leg"),
        (exercises[18], "3", "10-12", 90, "Unilateral"),
        (exercises[4], "3", "10-12", 90, "Quad volume"),
        (exercises[5], "3", "12-15", 60, "Hamstring isolation"),
        (exercises[19], "4", "15-20", 45, "Calves"),
        (exercises[17], "3", "30-45s", 60, "Core")
    };

            var upperPullExercises = new[] {
        (exercises[11], "4", "6-10", 180, "Main pull"),
        (exercises[12], "3", "8-10", 120, "Horizontal pull"),
        (exercises[13], "3", "10-12", 90, "Vertical pull"),
        (exercises[23], "3", "10-12", 90, "Cable row"),
        (exercises[14], "3", "15-20", 60, "Rear delts"),
        (exercises[15], "3", "10-12", 60, "Bicep work"),
        (exercises[21], "3", "10-12", 60, "Hammer curls"),
        (exercises[16], "3", "45-60s", 60, "Core")
    };

            // Apply exercises to all 16 days
            for (int week = 0; week < 4; week++)
            {
                for (int day = 0; day < 4; day++)
                {
                    int dayIndex = week * 4 + day;
                    var exerciseTemplate = day switch
                    {
                        0 => lowerAExercises,
                        1 => upperPushExercises,
                        2 => lowerBExercises,
                        3 => upperPullExercises,
                        _ => lowerAExercises
                    };

                    for (int i = 0; i < exerciseTemplate.Length; i++)
                    {
                        var ex = exerciseTemplate[i];
                        modelBuilder.Entity<ProgramDayExercise>().HasData(
                            new ProgramDayExercise
                            {
                                Id = pdeId++,
                                ProgramDayId = days[dayIndex],
                                ExerciseId = ex.Item1,
                                OrderIndex = i + 1,
                                Sets = ex.Item2,
                                Reps = ex.Item3,
                                RestSeconds = ex.Item4,
                                Notes = ex.Item5,
                                CreatedAt = fixedDateTimeOffset,
                                UpdatedAt = fixedDateTimeOffset,
                                IsDeleted = false
                            }
                        );
                    }
                }
            }

            // Package
            var packageId = 6001;
            modelBuilder.Entity<Package>().HasData(
                new Package { Id = packageId, TrainerId = trainerProfileId, Name = "Starter Pack", Description = "8-week beginner program + monthly check-in", PriceMonthly = 29.99m, Currency = "EGP", FeaturesJson = "{\"allPrograms\":false}", IsActive = true, ThumbnailUrl = "https://www.example.com/images/packages/starter-pack.jpg", CreatedAt = fixedDateTime, UpdatedAt = fixedDateTimeOffset, PromoCode = "STARTER6", IsDeleted = false }
            );

            // PackageProgram
            var packageProgramId = 7001;
            modelBuilder.Entity<PackageProgram>().HasData(
                new PackageProgram { Id = packageProgramId, PackageId = packageId, ProgramId = programId, CreatedAt = fixedDateTimeOffset, UpdatedAt = fixedDateTimeOffset, IsDeleted = false }
            );
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
