using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.Gymunity.FP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTrainerIdFromProgramAndFixProgramConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_AspNetUsers_TrainerId",
                table: "Programs");

            migrationBuilder.DropForeignKey(
                name: "FK_Programs_TrainerProfiles_TrainerProfileId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Programs_TrainerId_IsPublic",
                table: "Programs");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_TrainerProfileId_IsPublic",
                table: "Programs",
                columns: new[] { "TrainerProfileId", "IsPublic" });

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_TrainerProfiles_TrainerProfileId",
                table: "Programs",
                column: "TrainerProfileId",
                principalTable: "TrainerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_TrainerProfiles_TrainerProfileId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Programs_TrainerProfileId_IsPublic",
                table: "Programs");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_TrainerId_IsPublic",
                table: "Programs",
                columns: new[] { "TrainerId", "IsPublic" });

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_AspNetUsers_TrainerId",
                table: "Programs",
                column: "TrainerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_TrainerProfiles_TrainerProfileId",
                table: "Programs",
                column: "TrainerProfileId",
                principalTable: "TrainerProfiles",
                principalColumn: "Id");
        }
    }
}
