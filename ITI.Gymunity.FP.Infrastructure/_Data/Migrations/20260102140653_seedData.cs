using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ITI.Gymunity.FP.Infrastructure._Data.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TrainerId",
                table: "Programs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Category", "CreatedAt", "Equipment", "MuscleGroup", "Name", "ThumbnailUrl", "TrainerId", "UpdatedAt", "VideoDemoUrl" },
                values: new object[,]
                {
                    { 1001, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Barbell", "Quadriceps", "Barbell Back Squat", "https://picsum.photos/seed/squat/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/squat-video/1920/1080" },
                    { 1002, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Dumbbell", "Quadriceps", "Goblet Squat", "https://picsum.photos/seed/goblet/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/goblet-video/1920/1080" },
                    { 1003, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Barbell", "Hamstrings", "Romanian Deadlift", "https://picsum.photos/seed/rdl/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/rdl-video/1920/1080" },
                    { 1004, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Dumbbell", "Quadriceps", "Bulgarian Split Squat", "https://picsum.photos/seed/bulgarian/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/bulgarian-video/1920/1080" },
                    { 1005, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Machine", "Quadriceps", "Leg Press", "https://picsum.photos/seed/legpress/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/legpress-video/1920/1080" },
                    { 1006, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Machine", "Hamstrings", "Leg Curl", "https://picsum.photos/seed/legcurl/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/legcurl-video/1920/1080" },
                    { 1007, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Barbell", "Chest", "Barbell Bench Press", "https://picsum.photos/seed/bench/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/bench-video/1920/1080" },
                    { 1008, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Dumbbell", "Shoulders", "Dumbbell Shoulder Press", "https://picsum.photos/seed/shoulderpress/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/shoulderpress-video/1920/1080" },
                    { 1009, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Dumbbell", "Chest", "Incline Dumbbell Press", "https://picsum.photos/seed/incline/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/incline-video/1920/1080" },
                    { 1010, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Dumbbell", "Shoulders", "Lateral Raise", "https://picsum.photos/seed/lateral/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/lateral-video/1920/1080" },
                    { 1011, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Bodyweight", "Triceps", "Tricep Dips", "https://picsum.photos/seed/dips/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/dips-video/1920/1080" },
                    { 1012, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Bodyweight", "Back", "Pull-ups", "https://picsum.photos/seed/pullups/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/pullups-video/1920/1080" },
                    { 1013, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Barbell", "Back", "Barbell Row", "https://picsum.photos/seed/row/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/row-video/1920/1080" },
                    { 1014, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Cable", "Back", "Lat Pulldown", "https://picsum.photos/seed/latpull/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/latpull-video/1920/1080" },
                    { 1015, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Cable", "Shoulders", "Face Pulls", "https://picsum.photos/seed/facepulls/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/facepulls-video/1920/1080" },
                    { 1016, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Dumbbell", "Biceps", "Bicep Curl", "https://picsum.photos/seed/bicep/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/bicep-video/1920/1080" },
                    { 1017, "Core", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Bodyweight", "Abs", "Plank", "https://picsum.photos/seed/plank/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/plank-video/1920/1080" },
                    { 1018, "Core", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Cable", "Abs", "Cable Crunch", "https://picsum.photos/seed/crunch/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/crunch-video/1920/1080" },
                    { 1019, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Dumbbell", "Quadriceps", "Walking Lunges", "https://picsum.photos/seed/lunges/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/lunges-video/1920/1080" },
                    { 1020, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Machine", "Calves", "Calf Raise", "https://picsum.photos/seed/calf/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/calf-video/1920/1080" },
                    { 1021, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Dumbbell", "Chest", "Chest Fly", "https://picsum.photos/seed/fly/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/fly-video/1920/1080" },
                    { 1022, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Dumbbell", "Biceps", "Hammer Curl", "https://picsum.photos/seed/hammer/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/hammer-video/1920/1080" },
                    { 1023, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Cable", "Triceps", "Tricep Pushdown", "https://picsum.photos/seed/pushdown/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/pushdown-video/1920/1080" },
                    { 1024, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Cable", "Back", "Seated Cable Row", "https://picsum.photos/seed/cablerow/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/cablerow-video/1920/1080" },
                    { 1025, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Machine", "Quadriceps", "Leg Extension", "https://picsum.photos/seed/legext/400/300", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://picsum.photos/seed/legext-video/1920/1080" }
                });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "CreatedAt", "Currency", "Description", "FeaturesJson", "IsActive", "Name", "PriceMonthly", "PriceYearly", "PromoCode", "ThumbnailUrl", "TrainerId", "TrainerProfileId", "UpdatedAt" },
                values: new object[] { 6001, new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc), "EGP", "8-week beginner program + monthly check-in", "{\"allPrograms\":false}", true, "Starter Pack", 29.99m, null, "STARTER6", "https://www.example.com/images/packages/starter-pack.jpg", 6, null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Programs",
                columns: new[] { "Id", "CreatedAt", "Description", "DurationWeeks", "IsPublic", "MaxClients", "Price", "ThumbnailUrl", "Title", "TrainerId", "TrainerProfileId", "Type", "UpdatedAt" },
                values: new object[] { 2001, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "8-week foundational strength program", 8, true, 500, 49.99m, "https://www.example.com/images/programs/beginner-strength-8w.jpg", "Beginner Strength - 8 Weeks", "", 6, 1, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "PackagePrograms",
                columns: new[] { "Id", "CreatedAt", "PackageId", "ProgramId", "UpdatedAt" },
                values: new object[] { 7001, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 6001, 2001, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "ProgramWeeks",
                columns: new[] { "Id", "CreatedAt", "ProgramId", "UpdatedAt", "WeekNumber" },
                values: new object[,]
                {
                    { 3001, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2001, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 },
                    { 3002, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2001, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2 },
                    { 3003, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2001, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3 },
                    { 3004, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2001, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4 }
                });

            migrationBuilder.InsertData(
                table: "ProgramDays",
                columns: new[] { "Id", "CreatedAt", "DayNumber", "Notes", "ProgramWeekId", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 4001, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "Squat pattern + accessories", 3001, "Lower Body A", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4002, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "Chest, shoulders, triceps", 3001, "Upper Body Push", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4003, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, "Hinge pattern + unilateral", 3001, "Lower Body B", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4004, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, "Back, biceps, rear delts", 3001, "Upper Body Pull", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4005, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "Squat pattern + accessories", 3002, "Lower Body A", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4006, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "Chest, shoulders, triceps", 3002, "Upper Body Push", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4007, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, "Hinge pattern + unilateral", 3002, "Lower Body B", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4008, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, "Back, biceps, rear delts", 3002, "Upper Body Pull", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4009, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "Squat pattern + accessories", 3003, "Lower Body A", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4010, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "Chest, shoulders, triceps", 3003, "Upper Body Push", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4011, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, "Hinge pattern + unilateral", 3003, "Lower Body B", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4012, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, "Back, biceps, rear delts", 3003, "Upper Body Pull", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4013, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "Squat pattern + accessories", 3004, "Lower Body A", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4014, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "Chest, shoulders, triceps", 3004, "Upper Body Push", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4015, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, "Hinge pattern + unilateral", 3004, "Lower Body B", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { 4016, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, "Back, biceps, rear delts", 3004, "Upper Body Pull", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "ProgramDayExercises",
                columns: new[] { "Id", "CreatedAt", "ExerciseDataJson", "ExerciseId", "Notes", "OrderIndex", "Percent1RM", "ProgramDayId", "RPE", "Reps", "RestSeconds", "Sets", "Tempo", "UpdatedAt", "VideoUrl" },
                values: new object[,]
                {
                    { 5001, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1001, "Main squat movement", 1, null, 4001, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5002, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1003, "Hamstring focus", 2, null, 4001, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5003, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1005, "Quad volume", 3, null, 4001, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5004, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1006, "Hamstring isolation", 4, null, 4001, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5005, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1025, "Quad isolation", 5, null, 4001, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5006, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1020, "Calf development", 6, null, 4001, null, "15-20", 45, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5007, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1017, "Core stability", 7, null, 4001, null, "45-60s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5008, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1007, "Main pressing", 1, null, 4002, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5009, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1008, "Overhead press", 2, null, 4002, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5010, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1009, "Incline work", 3, null, 4002, null, "8-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5011, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1021, "Chest isolation", 4, null, 4002, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5012, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1010, "Lateral delts", 5, null, 4002, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5013, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1011, "Tricep compound", 6, null, 4002, null, "8-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5014, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1023, "Tricep isolation", 7, null, 4002, null, "12-15", 45, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5015, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1018, "Core work", 8, null, 4002, null, "30-45s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5016, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1003, "Main hinge", 1, null, 4003, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5017, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1004, "Single leg", 2, null, 4003, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5018, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1019, "Unilateral", 3, null, 4003, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5019, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1005, "Quad volume", 4, null, 4003, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5020, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1006, "Hamstring isolation", 5, null, 4003, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5021, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1020, "Calves", 6, null, 4003, null, "15-20", 45, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5022, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1018, "Core", 7, null, 4003, null, "30-45s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5023, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1012, "Main pull", 1, null, 4004, null, "6-10", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5024, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1013, "Horizontal pull", 2, null, 4004, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5025, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1014, "Vertical pull", 3, null, 4004, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5026, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1024, "Cable row", 4, null, 4004, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5027, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1015, "Rear delts", 5, null, 4004, null, "15-20", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5028, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1016, "Bicep work", 6, null, 4004, null, "10-12", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5029, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1022, "Hammer curls", 7, null, 4004, null, "10-12", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5030, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1017, "Core", 8, null, 4004, null, "45-60s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5031, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1001, "Main squat movement", 1, null, 4005, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5032, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1003, "Hamstring focus", 2, null, 4005, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5033, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1005, "Quad volume", 3, null, 4005, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5034, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1006, "Hamstring isolation", 4, null, 4005, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5035, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1025, "Quad isolation", 5, null, 4005, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5036, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1020, "Calf development", 6, null, 4005, null, "15-20", 45, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5037, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1017, "Core stability", 7, null, 4005, null, "45-60s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5038, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1007, "Main pressing", 1, null, 4006, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5039, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1008, "Overhead press", 2, null, 4006, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5040, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1009, "Incline work", 3, null, 4006, null, "8-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5041, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1021, "Chest isolation", 4, null, 4006, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5042, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1010, "Lateral delts", 5, null, 4006, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5043, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1011, "Tricep compound", 6, null, 4006, null, "8-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5044, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1023, "Tricep isolation", 7, null, 4006, null, "12-15", 45, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5045, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1018, "Core work", 8, null, 4006, null, "30-45s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5046, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1003, "Main hinge", 1, null, 4007, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5047, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1004, "Single leg", 2, null, 4007, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5048, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1019, "Unilateral", 3, null, 4007, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5049, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1005, "Quad volume", 4, null, 4007, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5050, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1006, "Hamstring isolation", 5, null, 4007, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5051, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1020, "Calves", 6, null, 4007, null, "15-20", 45, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5052, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1018, "Core", 7, null, 4007, null, "30-45s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5053, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1012, "Main pull", 1, null, 4008, null, "6-10", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5054, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1013, "Horizontal pull", 2, null, 4008, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5055, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1014, "Vertical pull", 3, null, 4008, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5056, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1024, "Cable row", 4, null, 4008, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5057, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1015, "Rear delts", 5, null, 4008, null, "15-20", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5058, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1016, "Bicep work", 6, null, 4008, null, "10-12", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5059, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1022, "Hammer curls", 7, null, 4008, null, "10-12", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5060, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1017, "Core", 8, null, 4008, null, "45-60s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5061, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1001, "Main squat movement", 1, null, 4009, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5062, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1003, "Hamstring focus", 2, null, 4009, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5063, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1005, "Quad volume", 3, null, 4009, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5064, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1006, "Hamstring isolation", 4, null, 4009, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5065, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1025, "Quad isolation", 5, null, 4009, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5066, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1020, "Calf development", 6, null, 4009, null, "15-20", 45, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5067, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1017, "Core stability", 7, null, 4009, null, "45-60s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5068, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1007, "Main pressing", 1, null, 4010, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5069, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1008, "Overhead press", 2, null, 4010, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5070, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1009, "Incline work", 3, null, 4010, null, "8-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5071, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1021, "Chest isolation", 4, null, 4010, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5072, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1010, "Lateral delts", 5, null, 4010, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5073, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1011, "Tricep compound", 6, null, 4010, null, "8-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5074, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1023, "Tricep isolation", 7, null, 4010, null, "12-15", 45, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5075, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1018, "Core work", 8, null, 4010, null, "30-45s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5076, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1003, "Main hinge", 1, null, 4011, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5077, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1004, "Single leg", 2, null, 4011, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5078, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1019, "Unilateral", 3, null, 4011, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5079, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1005, "Quad volume", 4, null, 4011, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5080, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1006, "Hamstring isolation", 5, null, 4011, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5081, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1020, "Calves", 6, null, 4011, null, "15-20", 45, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5082, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1018, "Core", 7, null, 4011, null, "30-45s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5083, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1012, "Main pull", 1, null, 4012, null, "6-10", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5084, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1013, "Horizontal pull", 2, null, 4012, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5085, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1014, "Vertical pull", 3, null, 4012, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5086, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1024, "Cable row", 4, null, 4012, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5087, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1015, "Rear delts", 5, null, 4012, null, "15-20", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5088, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1016, "Bicep work", 6, null, 4012, null, "10-12", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5089, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1022, "Hammer curls", 7, null, 4012, null, "10-12", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5090, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1017, "Core", 8, null, 4012, null, "45-60s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5091, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1001, "Main squat movement", 1, null, 4013, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5092, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1003, "Hamstring focus", 2, null, 4013, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5093, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1005, "Quad volume", 3, null, 4013, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5094, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1006, "Hamstring isolation", 4, null, 4013, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5095, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1025, "Quad isolation", 5, null, 4013, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5096, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1020, "Calf development", 6, null, 4013, null, "15-20", 45, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5097, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1017, "Core stability", 7, null, 4013, null, "45-60s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5098, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1007, "Main pressing", 1, null, 4014, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5099, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1008, "Overhead press", 2, null, 4014, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5100, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1009, "Incline work", 3, null, 4014, null, "8-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5101, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1021, "Chest isolation", 4, null, 4014, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5102, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1010, "Lateral delts", 5, null, 4014, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5103, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1011, "Tricep compound", 6, null, 4014, null, "8-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5104, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1023, "Tricep isolation", 7, null, 4014, null, "12-15", 45, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5105, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1018, "Core work", 8, null, 4014, null, "30-45s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5106, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1003, "Main hinge", 1, null, 4015, null, "6-8", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5107, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1004, "Single leg", 2, null, 4015, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5108, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1019, "Unilateral", 3, null, 4015, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5109, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1005, "Quad volume", 4, null, 4015, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5110, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1006, "Hamstring isolation", 5, null, 4015, null, "12-15", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5111, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1020, "Calves", 6, null, 4015, null, "15-20", 45, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5112, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1018, "Core", 7, null, 4015, null, "30-45s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5113, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1012, "Main pull", 1, null, 4016, null, "6-10", 180, "4", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5114, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1013, "Horizontal pull", 2, null, 4016, null, "8-10", 120, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5115, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1014, "Vertical pull", 3, null, 4016, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5116, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1024, "Cable row", 4, null, 4016, null, "10-12", 90, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5117, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1015, "Rear delts", 5, null, 4016, null, "15-20", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5118, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1016, "Bicep work", 6, null, 4016, null, "10-12", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5119, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1022, "Hammer curls", 7, null, 4016, null, "10-12", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { 5120, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1017, "Core", 8, null, 4016, null, "45-60s", 60, "3", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "PackagePrograms",
                keyColumn: "Id",
                keyValue: 7001);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5001);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5002);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5003);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5004);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5005);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5006);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5007);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5008);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5009);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5010);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5011);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5012);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5013);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5014);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5015);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5016);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5017);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5018);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5019);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5020);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5021);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5022);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5023);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5024);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5025);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5026);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5027);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5028);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5029);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5030);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5031);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5032);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5033);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5034);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5035);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5036);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5037);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5038);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5039);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5040);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5041);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5042);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5043);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5044);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5045);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5046);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5047);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5048);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5049);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5050);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5051);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5052);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5053);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5054);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5055);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5056);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5057);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5058);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5059);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5060);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5061);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5062);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5063);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5064);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5065);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5066);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5067);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5068);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5069);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5070);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5071);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5072);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5073);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5074);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5075);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5076);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5077);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5078);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5079);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5080);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5081);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5082);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5083);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5084);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5085);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5086);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5087);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5088);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5089);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5090);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5091);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5092);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5093);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5094);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5095);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5096);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5097);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5098);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5099);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5100);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5101);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5102);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5103);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5104);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5105);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5106);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5107);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5108);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5109);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5110);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5111);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5112);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5113);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5114);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5115);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5116);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5117);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5118);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5119);

            migrationBuilder.DeleteData(
                table: "ProgramDayExercises",
                keyColumn: "Id",
                keyValue: 5120);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1009);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1010);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1011);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1012);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1013);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1014);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1015);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1016);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1017);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1018);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1019);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1020);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1021);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1022);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1023);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1024);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1025);

            migrationBuilder.DeleteData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 6001);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4001);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4002);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4003);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4004);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4005);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4006);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4007);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4008);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4009);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4010);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4011);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4012);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4013);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4014);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4015);

            migrationBuilder.DeleteData(
                table: "ProgramDays",
                keyColumn: "Id",
                keyValue: 4016);

            migrationBuilder.DeleteData(
                table: "ProgramWeeks",
                keyColumn: "Id",
                keyValue: 3001);

            migrationBuilder.DeleteData(
                table: "ProgramWeeks",
                keyColumn: "Id",
                keyValue: 3002);

            migrationBuilder.DeleteData(
                table: "ProgramWeeks",
                keyColumn: "Id",
                keyValue: 3003);

            migrationBuilder.DeleteData(
                table: "ProgramWeeks",
                keyColumn: "Id",
                keyValue: 3004);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 2001);

            migrationBuilder.AlterColumn<string>(
                name: "TrainerId",
                table: "Programs",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
