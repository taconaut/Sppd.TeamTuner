using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.MsSql.Migrations
{
    public partial class Missing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamMembershipRequest_TeamId",
                table: "TeamMembershipRequest");

            migrationBuilder.CreateIndex(
                name: "IX_Theme_Name",
                table: "Theme",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembershipRequest_TeamId",
                table: "TeamMembershipRequest",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Name",
                table: "Team",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Rarity_Name",
                table: "Rarity",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Federation_Name",
                table: "Federation",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterType_Name",
                table: "CharacterType",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CardType_Name",
                table: "CardType",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Card_Name",
                table: "Card",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Theme_Name",
                table: "Theme");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembershipRequest_TeamId",
                table: "TeamMembershipRequest");

            migrationBuilder.DropIndex(
                name: "IX_Team_Name",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Rarity_Name",
                table: "Rarity");

            migrationBuilder.DropIndex(
                name: "IX_Federation_Name",
                table: "Federation");

            migrationBuilder.DropIndex(
                name: "IX_CharacterType_Name",
                table: "CharacterType");

            migrationBuilder.DropIndex(
                name: "IX_CardType_Name",
                table: "CardType");

            migrationBuilder.DropIndex(
                name: "IX_Card_Name",
                table: "Card");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembershipRequest_TeamId",
                table: "TeamMembershipRequest",
                column: "TeamId",
                unique: true,
                filter: "[IsDeleted] = 0");
        }
    }
}
