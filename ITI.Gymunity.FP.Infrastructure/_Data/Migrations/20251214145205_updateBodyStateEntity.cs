using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.Gymunity.FP.Infrastructure._Data.Migrations
{
    /// <inheritdoc />
    public partial class updateBodyStateEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BodyStatLogs_AspNetUsers_ClientId",
                table: "BodyStatLogs");

            migrationBuilder.DropIndex(
                name: "IX_BodyStatLogs_ClientId",
                table: "BodyStatLogs");

            migrationBuilder.DropIndex(
                name: "IX_BodyStatLogs_ClientId_LoggedAt",
                table: "BodyStatLogs");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "BodyStatLogs");

            migrationBuilder.AddColumn<int>(
                name: "ClientProfileId",
                table: "BodyStatLogs",
                type: "int",
                maxLength: 450,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BodyStatLogs_ClientProfileId",
                table: "BodyStatLogs",
                column: "ClientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyStatLogs_ClientProfileId_LoggedAt",
                table: "BodyStatLogs",
                columns: new[] { "ClientProfileId", "LoggedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_BodyStatLogs_ClientProfiles_ClientProfileId",
                table: "BodyStatLogs",
                column: "ClientProfileId",
                principalTable: "ClientProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BodyStatLogs_ClientProfiles_ClientProfileId",
                table: "BodyStatLogs");

            migrationBuilder.DropIndex(
                name: "IX_BodyStatLogs_ClientProfileId",
                table: "BodyStatLogs");

            migrationBuilder.DropIndex(
                name: "IX_BodyStatLogs_ClientProfileId_LoggedAt",
                table: "BodyStatLogs");

            migrationBuilder.DropColumn(
                name: "ClientProfileId",
                table: "BodyStatLogs");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "BodyStatLogs",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BodyStatLogs_ClientId",
                table: "BodyStatLogs",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyStatLogs_ClientId_LoggedAt",
                table: "BodyStatLogs",
                columns: new[] { "ClientId", "LoggedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_BodyStatLogs_AspNetUsers_ClientId",
                table: "BodyStatLogs",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
