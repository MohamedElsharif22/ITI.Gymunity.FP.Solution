using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.Gymunity.FP.Infrastructure._Data.Migrations
{
    /// <inheritdoc />
    public partial class addsomeentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Programs");

            migrationBuilder.AddColumn<string>(
                name: "StatusDescription",
                table: "TrainerProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusImageUrl",
                table: "TrainerProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Programs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Programs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Exercises",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TrainerName",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusDescription",
                table: "TrainerProfiles");

            migrationBuilder.DropColumn(
                name: "StatusImageUrl",
                table: "TrainerProfiles");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "TrainerName",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Programs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
