using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Migrations
{
    public partial class SoftDeleteFilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamTunerUser_Email",
                table: "TeamTunerUser");

            migrationBuilder.DropIndex(
                name: "IX_TeamTunerUser_Name",
                table: "TeamTunerUser");

            migrationBuilder.DropIndex(
                name: "IX_TeamTunerUser_SppdName",
                table: "TeamTunerUser");

            migrationBuilder.DropIndex(
                name: "IX_Card_ExternalId",
                table: "Card");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUser_Email",
                table: "TeamTunerUser",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUser_Name",
                table: "TeamTunerUser",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUser_SppdName",
                table: "TeamTunerUser",
                column: "SppdName",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Card_ExternalId",
                table: "Card",
                column: "ExternalId",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamTunerUser_Email",
                table: "TeamTunerUser");

            migrationBuilder.DropIndex(
                name: "IX_TeamTunerUser_Name",
                table: "TeamTunerUser");

            migrationBuilder.DropIndex(
                name: "IX_TeamTunerUser_SppdName",
                table: "TeamTunerUser");

            migrationBuilder.DropIndex(
                name: "IX_Card_ExternalId",
                table: "Card");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUser_Email",
                table: "TeamTunerUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUser_Name",
                table: "TeamTunerUser",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUser_SppdName",
                table: "TeamTunerUser",
                column: "SppdName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Card_ExternalId",
                table: "Card",
                column: "ExternalId",
                unique: true);
        }
    }
}
