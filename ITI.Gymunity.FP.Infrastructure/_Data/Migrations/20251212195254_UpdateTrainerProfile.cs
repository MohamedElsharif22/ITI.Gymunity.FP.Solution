using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.Gymunity.FP.Infrastructure._Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainerProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
