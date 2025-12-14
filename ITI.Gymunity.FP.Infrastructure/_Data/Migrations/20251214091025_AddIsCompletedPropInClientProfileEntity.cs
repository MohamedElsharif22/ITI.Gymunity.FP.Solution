using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.Gymunity.FP.Infrastructure._Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCompletedPropInClientProfileEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnboardingCompleted",
                table: "ClientProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnboardingCompleted",
                table: "ClientProfiles");
        }
    }
}
