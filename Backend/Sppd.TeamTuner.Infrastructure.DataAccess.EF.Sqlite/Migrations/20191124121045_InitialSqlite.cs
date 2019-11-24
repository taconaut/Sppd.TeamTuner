using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Sqlite.Migrations
{
    public partial class InitialSqlite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "CardType",
                table => new
                         {
                             Id = table.Column<Guid>(),
                             CreatedOnUtc = table.Column<DateTime>(),
                             CreatedById = table.Column<Guid>(),
                             ModifiedOnUtc = table.Column<DateTime>(),
                             ModifiedById = table.Column<Guid>(),
                             IsDeleted = table.Column<bool>(),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             Name = table.Column<string>(maxLength: 50)
                         },
                constraints: table => { table.PrimaryKey("PK_CardType", x => x.Id); });

            migrationBuilder.CreateTable(
                "CharacterType",
                table => new
                         {
                             Id = table.Column<Guid>(),
                             CreatedOnUtc = table.Column<DateTime>(),
                             CreatedById = table.Column<Guid>(),
                             ModifiedOnUtc = table.Column<DateTime>(),
                             ModifiedById = table.Column<Guid>(),
                             IsDeleted = table.Column<bool>(),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             Name = table.Column<string>(maxLength: 50)
                         },
                constraints: table => { table.PrimaryKey("PK_CharacterType", x => x.Id); });

            migrationBuilder.CreateTable(
                "Federation",
                table => new
                         {
                             Id = table.Column<Guid>(),
                             CreatedOnUtc = table.Column<DateTime>(),
                             CreatedById = table.Column<Guid>(),
                             ModifiedOnUtc = table.Column<DateTime>(),
                             ModifiedById = table.Column<Guid>(),
                             IsDeleted = table.Column<bool>(),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             Name = table.Column<string>(maxLength: 50),
                             Avatar = table.Column<byte[]>(nullable: true),
                             Description = table.Column<string>(maxLength: 2000, nullable: true)
                         },
                constraints: table => { table.PrimaryKey("PK_Federation", x => x.Id); });

            migrationBuilder.CreateTable(
                "Rarity",
                table => new
                         {
                             Id = table.Column<Guid>(),
                             CreatedOnUtc = table.Column<DateTime>(),
                             CreatedById = table.Column<Guid>(),
                             ModifiedOnUtc = table.Column<DateTime>(),
                             ModifiedById = table.Column<Guid>(),
                             IsDeleted = table.Column<bool>(),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             Name = table.Column<string>(maxLength: 50),
                             FriendlyLevel = table.Column<int>()
                         },
                constraints: table => { table.PrimaryKey("PK_Rarity", x => x.Id); });

            migrationBuilder.CreateTable(
                "Theme",
                table => new
                         {
                             Id = table.Column<Guid>(),
                             CreatedOnUtc = table.Column<DateTime>(),
                             CreatedById = table.Column<Guid>(),
                             ModifiedOnUtc = table.Column<DateTime>(),
                             ModifiedById = table.Column<Guid>(),
                             IsDeleted = table.Column<bool>(),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             Name = table.Column<string>(maxLength: 50)
                         },
                constraints: table => { table.PrimaryKey("PK_Theme", x => x.Id); });

            migrationBuilder.CreateTable(
                "Team",
                table => new
                         {
                             Id = table.Column<Guid>(),
                             CreatedOnUtc = table.Column<DateTime>(),
                             CreatedById = table.Column<Guid>(),
                             ModifiedOnUtc = table.Column<DateTime>(),
                             ModifiedById = table.Column<Guid>(),
                             IsDeleted = table.Column<bool>(),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             Name = table.Column<string>(maxLength: 50),
                             Avatar = table.Column<byte[]>(nullable: true),
                             Description = table.Column<string>(maxLength: 2000, nullable: true),
                             FederationId = table.Column<Guid>(nullable: true)
                         },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        "FK_Team_Federation_FederationId",
                        x => x.FederationId,
                        "Federation",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Card",
                table => new
                         {
                             Id = table.Column<Guid>(),
                             CreatedOnUtc = table.Column<DateTime>(),
                             CreatedById = table.Column<Guid>(),
                             ModifiedOnUtc = table.Column<DateTime>(),
                             ModifiedById = table.Column<Guid>(),
                             IsDeleted = table.Column<bool>(),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             Name = table.Column<string>(maxLength: 50),
                             ExternalId = table.Column<string>(maxLength: 24, nullable: true),
                             Description = table.Column<string>(maxLength: 500, nullable: true),
                             ManaCost = table.Column<int>(),
                             ThemeId = table.Column<Guid>(),
                             RarityId = table.Column<Guid>(),
                             TypeId = table.Column<Guid>(),
                             CharacterTypeId = table.Column<Guid>(nullable: true)
                         },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.Id);
                    table.ForeignKey(
                        "FK_Card_CharacterType_CharacterTypeId",
                        x => x.CharacterTypeId,
                        "CharacterType",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Card_Rarity_RarityId",
                        x => x.RarityId,
                        "Rarity",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Card_Theme_ThemeId",
                        x => x.ThemeId,
                        "Theme",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Card_CardType_TypeId",
                        x => x.TypeId,
                        "CardType",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "TeamTunerUser",
                table => new
                         {
                             Id = table.Column<Guid>(),
                             CreatedOnUtc = table.Column<DateTime>(),
                             CreatedById = table.Column<Guid>(),
                             ModifiedOnUtc = table.Column<DateTime>(),
                             ModifiedById = table.Column<Guid>(),
                             IsDeleted = table.Column<bool>(),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             Name = table.Column<string>(maxLength: 50),
                             Avatar = table.Column<byte[]>(nullable: true),
                             Description = table.Column<string>(maxLength: 2000, nullable: true),
                             IsEmailVerified = table.Column<bool>(),
                             ProfileVisibility = table.Column<int>(),
                             SppdName = table.Column<string>(maxLength: 50),
                             PasswordHash = table.Column<byte[]>(maxLength: 64),
                             PasswordSalt = table.Column<byte[]>(maxLength: 128),
                             Email = table.Column<string>(maxLength: 200),
                             ApplicationRole = table.Column<string>(maxLength: 20),
                             TeamId = table.Column<Guid>(nullable: true),
                             TeamRole = table.Column<string>(maxLength: 20, nullable: true),
                             FederationId = table.Column<Guid>(nullable: true),
                             FederationRole = table.Column<string>(maxLength: 20, nullable: true)
                         },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTunerUser", x => x.Id);
                    table.ForeignKey(
                        "FK_TeamTunerUser_Federation_FederationId",
                        x => x.FederationId,
                        "Federation",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_TeamTunerUser_Team_TeamId",
                        x => x.TeamId,
                        "Team",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "CardLevel",
                table => new
                         {
                             Id = table.Column<Guid>(),
                             CreatedOnUtc = table.Column<DateTime>(),
                             CreatedById = table.Column<Guid>(),
                             ModifiedOnUtc = table.Column<DateTime>(),
                             ModifiedById = table.Column<Guid>(),
                             IsDeleted = table.Column<bool>(),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             CardId = table.Column<Guid>(),
                             UserId = table.Column<Guid>(),
                             Level = table.Column<int>()
                         },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardLevel", x => x.Id);
                    table.ForeignKey(
                        "FK_CardLevel_Card_CardId",
                        x => x.CardId,
                        "Card",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_CardLevel_TeamTunerUser_UserId",
                        x => x.UserId,
                        "TeamTunerUser",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "TeamMembershipRequest",
                table => new
                         {
                             Id = table.Column<Guid>(),
                             CreatedOnUtc = table.Column<DateTime>(),
                             CreatedById = table.Column<Guid>(),
                             ModifiedOnUtc = table.Column<DateTime>(),
                             ModifiedById = table.Column<Guid>(),
                             IsDeleted = table.Column<bool>(),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             TeamId = table.Column<Guid>(),
                             UserId = table.Column<Guid>(),
                             Comment = table.Column<string>(maxLength: 500, nullable: true)
                         },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembershipRequest", x => x.Id);
                    table.ForeignKey(
                        "FK_TeamMembershipRequest_Team_TeamId",
                        x => x.TeamId,
                        "Team",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_TeamMembershipRequest_TeamTunerUser_UserId",
                        x => x.UserId,
                        "TeamTunerUser",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "TeamTunerUserRegistrationRequest",
                table => new
                         {
                             Id = table.Column<Guid>(),
                             CreatedOnUtc = table.Column<DateTime>(),
                             CreatedById = table.Column<Guid>(),
                             ModifiedOnUtc = table.Column<DateTime>(),
                             ModifiedById = table.Column<Guid>(),
                             IsDeleted = table.Column<bool>(),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             UserId = table.Column<Guid>(),
                             RegistrationCode = table.Column<Guid>(),
                             RegistrationDate = table.Column<DateTime>()
                         },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTunerUserRegistrationRequest", x => x.Id);
                    table.ForeignKey(
                        "FK_TeamTunerUserRegistrationRequest_TeamTunerUser_UserId",
                        x => x.UserId,
                        "TeamTunerUser",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Card_CharacterTypeId",
                "Card",
                "CharacterTypeId");

            migrationBuilder.CreateIndex(
                "IX_Card_ExternalId",
                "Card",
                "ExternalId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_Card_Name",
                "Card",
                "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_Card_RarityId",
                "Card",
                "RarityId");

            migrationBuilder.CreateIndex(
                "IX_Card_ThemeId",
                "Card",
                "ThemeId");

            migrationBuilder.CreateIndex(
                "IX_Card_TypeId",
                "Card",
                "TypeId");

            migrationBuilder.CreateIndex(
                "IX_CardLevel_CardId",
                "CardLevel",
                "CardId");

            migrationBuilder.CreateIndex(
                "IX_CardLevel_UserId_CardId",
                "CardLevel",
                new[] {"UserId", "CardId"},
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_CardType_Name",
                "CardType",
                "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_CharacterType_Name",
                "CharacterType",
                "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_Federation_Name",
                "Federation",
                "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_Rarity_Name",
                "Rarity",
                "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_Team_FederationId",
                "Team",
                "FederationId");

            migrationBuilder.CreateIndex(
                "IX_Team_Name",
                "Team",
                "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_TeamMembershipRequest_TeamId",
                "TeamMembershipRequest",
                "TeamId");

            migrationBuilder.CreateIndex(
                "IX_TeamMembershipRequest_UserId",
                "TeamMembershipRequest",
                "UserId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_TeamTunerUser_Email",
                "TeamTunerUser",
                "Email",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_TeamTunerUser_FederationId",
                "TeamTunerUser",
                "FederationId");

            migrationBuilder.CreateIndex(
                "IX_TeamTunerUser_Name",
                "TeamTunerUser",
                "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_TeamTunerUser_SppdName",
                "TeamTunerUser",
                "SppdName",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_TeamTunerUser_TeamId",
                "TeamTunerUser",
                "TeamId");

            migrationBuilder.CreateIndex(
                "IX_TeamTunerUserRegistrationRequest_RegistrationCode",
                "TeamTunerUserRegistrationRequest",
                "RegistrationCode");

            migrationBuilder.CreateIndex(
                "IX_TeamTunerUserRegistrationRequest_UserId",
                "TeamTunerUserRegistrationRequest",
                "UserId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                "IX_Theme_Name",
                "Theme",
                "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            InitialTriggerCreator.Up(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            InitialTriggerCreator.Down(migrationBuilder);

            migrationBuilder.DropTable(
                "CardLevel");

            migrationBuilder.DropTable(
                "TeamMembershipRequest");

            migrationBuilder.DropTable(
                "TeamTunerUserRegistrationRequest");

            migrationBuilder.DropTable(
                "Card");

            migrationBuilder.DropTable(
                "TeamTunerUser");

            migrationBuilder.DropTable(
                "CharacterType");

            migrationBuilder.DropTable(
                "Rarity");

            migrationBuilder.DropTable(
                "Theme");

            migrationBuilder.DropTable(
                "CardType");

            migrationBuilder.DropTable(
                "Team");

            migrationBuilder.DropTable(
                "Federation");
        }
    }
}