using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Migrations
{
    public partial class UniqueCardLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CardLevel_UserId",
                table: "CardLevel");

            migrationBuilder.CreateIndex(
                name: "IX_CardLevel_UserId_CardId",
                table: "CardLevel",
                columns: new[] { "UserId", "CardId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CardLevel_UserId_CardId",
                table: "CardLevel");

            migrationBuilder.CreateIndex(
                name: "IX_CardLevel_UserId",
                table: "CardLevel",
                column: "UserId");
        }
    }
}
