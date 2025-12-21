using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ITI.Gymunity.FP.Infrastructure._Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Category", "CreatedAt", "Equipment", "MuscleGroup", "Name", "ThumbnailUrl", "TrainerId", "UpdatedAt", "VideoDemoUrl" },
                values: new object[,]
                {
                    { 1, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Barbell", "Chest", "Bench Press", "https://example.com/images/bench-press.jpg", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://example.com/videos/bench-press.mp4" },
                    { 2, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Barbell", "Legs", "Squat", "https://example.com/images/squat.jpg", null, new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://example.com/videos/squat.mp4" }
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Category", "CreatedAt", "Equipment", "IsCustom", "MuscleGroup", "Name", "ThumbnailUrl", "TrainerId", "UpdatedAt", "VideoDemoUrl" },
                values: new object[] { 3, "Strength", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Machine", true, "Legs", "Custom Leg Press", "https://example.com/images/leg-press.jpg", "29302cc1-6bc6-4534-8269-de75ef3650a7", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://example.com/videos/leg-press.mp4" });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "CreatedAt", "Currency", "Description", "FeaturesJson", "IsActive", "Name", "PriceMonthly", "PriceYearly", "ThumbnailUrl", "TrainerId", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc), "EGP", "Entry-level training package with essential features", "{\"formChecksPerWeek\": 2, \"priorityMessaging\": false, \"monthlyVideoCall\": false}", true, "Basic Package", 99.99m, 999.90m, "https://example.com/images/basic-package.jpg", "29302cc1-6bc6-4534-8269-de75ef3650a7", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "CreatedAt", "Currency", "Description", "FeaturesJson", "IsActive", "Name", "PriceMonthly", "PriceYearly", "PromoCode", "ThumbnailUrl", "TrainerId", "UpdatedAt" },
                values: new object[] { 2, new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc), "EGP", "Advanced training package with all features", "{\"formChecksPerWeek\": 4, \"priorityMessaging\": true, \"monthlyVideoCall\": true, \"customProgramEveryWeeks\": 4, \"earlyAccess\": true}", true, "Premium Package", 299.99m, 2999.90m, "PREMIUM20", "https://example.com/images/premium-package.jpg", "29302cc1-6bc6-4534-8269-de75ef3650a7", new DateTimeOffset(new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Programs",
                columns: new[] { "Id", "CreatedAt", "Description", "DurationWeeks", "IsPublic", "MaxClients", "Price", "ThumbnailUrl", "Title", "TrainerId", "TrainerProfileId", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc), "A comprehensive 12-week program designed to build muscle mass and strength", 12, true, 50, 499.99m, "https://example.com/images/muscle-building.jpg", "12-Week Muscle Building Program", "29302cc1-6bc6-4534-8269-de75ef3650a7", null, 1, new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc), "An intense 8-week program focused on fat loss and cardio conditioning", 8, true, 100, 299.99m, "https://example.com/images/fat-loss.jpg", "8-Week Fat Loss Program", "29302cc1-6bc6-4534-8269-de75ef3650a7", null, 1, new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc), "A detailed nutrition plan for optimal health and performance", 4, true, null, null, "https://example.com/images/nutrition.jpg", "Complete Nutrition Guide", "29302cc1-6bc6-4534-8269-de75ef3650a7", null, 2, new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
