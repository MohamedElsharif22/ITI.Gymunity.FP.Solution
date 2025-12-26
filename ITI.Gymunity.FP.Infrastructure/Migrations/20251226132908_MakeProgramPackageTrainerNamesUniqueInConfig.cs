using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.Gymunity.FP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeProgramPackageTrainerNamesUniqueInConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Programs_Title",
                table: "Programs",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_Name",
                table: "Packages",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Programs_Title",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Packages_Name",
                table: "Packages");
        }
    }
}
