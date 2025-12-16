using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.Gymunity.FP.Infrastructure._Data.Migrations
{
    /// <inheritdoc />
    public partial class updateWorkoutLogEntityWithClientProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutLogs_AspNetUsers_ClientId",
                table: "WorkoutLogs");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutLogs_ClientId",
                table: "WorkoutLogs");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutLogs_ClientId_CompletedAt",
                table: "WorkoutLogs");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "WorkoutLogs");

            migrationBuilder.AddColumn<int>(
                name: "ClientProfileId",
                table: "WorkoutLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_ClientProfileId",
                table: "WorkoutLogs",
                column: "ClientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_ClientProfileId_CompletedAt",
                table: "WorkoutLogs",
                columns: new[] { "ClientProfileId", "CompletedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_ClientProfileId_ProgramDayId",
                table: "WorkoutLogs",
                columns: new[] { "ClientProfileId", "ProgramDayId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutLogs_ClientProfiles_ClientProfileId",
                table: "WorkoutLogs",
                column: "ClientProfileId",
                principalTable: "ClientProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutLogs_ClientProfiles_ClientProfileId",
                table: "WorkoutLogs");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutLogs_ClientProfileId",
                table: "WorkoutLogs");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutLogs_ClientProfileId_CompletedAt",
                table: "WorkoutLogs");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutLogs_ClientProfileId_ProgramDayId",
                table: "WorkoutLogs");

            migrationBuilder.DropColumn(
                name: "ClientProfileId",
                table: "WorkoutLogs");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "WorkoutLogs",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_ClientId",
                table: "WorkoutLogs",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_ClientId_CompletedAt",
                table: "WorkoutLogs",
                columns: new[] { "ClientId", "CompletedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutLogs_AspNetUsers_ClientId",
                table: "WorkoutLogs",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
