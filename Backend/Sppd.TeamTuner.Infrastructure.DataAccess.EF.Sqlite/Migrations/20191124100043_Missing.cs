using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Sqlite.Migrations
{
    public partial class Missing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamMembershipRequest_TeamId",
                table: "TeamMembershipRequest");

            migrationBuilder.CreateTable(
                name: "TeamTunerUserRegistrationRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false),
                    ModifiedOnUtc = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<Guid>(nullable: true),
                    Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    RegistrationCode = table.Column<Guid>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTunerUserRegistrationRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamTunerUserRegistrationRequest_TeamTunerUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TeamTunerUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUserRegistrationRequest_RegistrationCode",
                table: "TeamTunerUserRegistrationRequest",
                column: "RegistrationCode");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUserRegistrationRequest_UserId",
                table: "TeamTunerUserRegistrationRequest",
                column: "UserId",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamTunerUserRegistrationRequest");

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
