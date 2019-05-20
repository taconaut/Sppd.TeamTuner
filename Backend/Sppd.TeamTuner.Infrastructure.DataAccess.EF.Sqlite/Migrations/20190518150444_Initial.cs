using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Sqlite.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "CardType",
                table => new
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
                             Name = table.Column<string>(maxLength: 50, nullable: false)
                         },
                constraints: table => { table.PrimaryKey("PK_CardType", x => x.Id); });

            migrationBuilder.CreateTable(
                "Federation",
                table => new
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
                             Name = table.Column<string>(maxLength: 50, nullable: false),
                             Avatar = table.Column<byte[]>(nullable: true),
                             Description = table.Column<string>(maxLength: 2000, nullable: true)
                         },
                constraints: table => { table.PrimaryKey("PK_Federation", x => x.Id); });

            migrationBuilder.CreateTable(
                "Rarity",
                table => new
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
                             Name = table.Column<string>(maxLength: 50, nullable: false),
                             FriendlyLevel = table.Column<int>(nullable: false)
                         },
                constraints: table => { table.PrimaryKey("PK_Rarity", x => x.Id); });

            migrationBuilder.CreateTable(
                "Theme",
                table => new
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
                             Name = table.Column<string>(maxLength: 50, nullable: false)
                         },
                constraints: table => { table.PrimaryKey("PK_Theme", x => x.Id); });

            migrationBuilder.CreateTable(
                "Team",
                table => new
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
                             Name = table.Column<string>(maxLength: 50, nullable: false),
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
                             Id = table.Column<Guid>(nullable: false),
                             CreatedOnUtc = table.Column<DateTime>(nullable: false),
                             CreatedById = table.Column<Guid>(nullable: false),
                             ModifiedOnUtc = table.Column<DateTime>(nullable: false),
                             ModifiedById = table.Column<Guid>(nullable: false),
                             IsDeleted = table.Column<bool>(nullable: false),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             Name = table.Column<string>(maxLength: 50, nullable: false),
                             ExternalId = table.Column<string>(maxLength: 24, nullable: true),
                             Description = table.Column<string>(maxLength: 500, nullable: true),
                             ManaCost = table.Column<int>(nullable: false),
                             ThemeId = table.Column<Guid>(nullable: false),
                             RarityId = table.Column<Guid>(nullable: false),
                             TypeId = table.Column<Guid>(nullable: false)
                         },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.Id);
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
                             Id = table.Column<Guid>(nullable: false),
                             CreatedOnUtc = table.Column<DateTime>(nullable: false),
                             CreatedById = table.Column<Guid>(nullable: false),
                             ModifiedOnUtc = table.Column<DateTime>(nullable: false),
                             ModifiedById = table.Column<Guid>(nullable: false),
                             IsDeleted = table.Column<bool>(nullable: false),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             Name = table.Column<string>(maxLength: 50, nullable: false),
                             Avatar = table.Column<byte[]>(nullable: true),
                             Description = table.Column<string>(maxLength: 2000, nullable: true),
                             ProfileVisibility = table.Column<int>(nullable: false),
                             SppdName = table.Column<string>(maxLength: 50, nullable: false),
                             PasswordHash = table.Column<byte[]>(maxLength: 64, nullable: false),
                             PasswordSalt = table.Column<byte[]>(maxLength: 128, nullable: false),
                             Email = table.Column<string>(maxLength: 200, nullable: false),
                             ApplicationRole = table.Column<string>(maxLength: 20, nullable: false),
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
                             Id = table.Column<Guid>(nullable: false),
                             CreatedOnUtc = table.Column<DateTime>(nullable: false),
                             CreatedById = table.Column<Guid>(nullable: false),
                             ModifiedOnUtc = table.Column<DateTime>(nullable: false),
                             ModifiedById = table.Column<Guid>(nullable: false),
                             IsDeleted = table.Column<bool>(nullable: false),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             CardId = table.Column<Guid>(nullable: false),
                             UserId = table.Column<Guid>(nullable: false),
                             Level = table.Column<int>(nullable: false)
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
                             Id = table.Column<Guid>(nullable: false),
                             CreatedOnUtc = table.Column<DateTime>(nullable: false),
                             CreatedById = table.Column<Guid>(nullable: false),
                             ModifiedOnUtc = table.Column<DateTime>(nullable: false),
                             ModifiedById = table.Column<Guid>(nullable: false),
                             IsDeleted = table.Column<bool>(nullable: false),
                             DeletedOnUtc = table.Column<DateTime>(nullable: true),
                             DeletedById = table.Column<Guid>(nullable: true),
                             Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                             TeamId = table.Column<Guid>(nullable: false),
                             UserId = table.Column<Guid>(nullable: false),
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

            migrationBuilder.CreateIndex(
                "IX_Card_ExternalId",
                "Card",
                "ExternalId",
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
                "IX_Team_FederationId",
                "Team",
                "FederationId");

            migrationBuilder.CreateIndex(
                "IX_TeamMembershipRequest_TeamId",
                "TeamMembershipRequest",
                "TeamId",
                unique: true,
                filter: "[IsDeleted] = 0");

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

            #region Manual additions

            migrationBuilder.Sql(
                @"CREATE TRIGGER SetCardVersionOnUpdate
                AFTER UPDATE ON Card
                BEGIN
                    UPDATE Card
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");
            migrationBuilder.Sql(
                @"CREATE TRIGGER SetCardVersionOnInsert
                AFTER INSERT ON Card
                BEGIN
                    UPDATE Card
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");

            migrationBuilder.Sql(
                @"CREATE TRIGGER SetCardLevelVersionOnUpdate
                AFTER UPDATE ON CardLevel
                BEGIN
                    UPDATE CardLevel
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");
            migrationBuilder.Sql(
                @"CREATE TRIGGER SetCardLevelVersionOnInsert
                AFTER INSERT ON CardLevel
                BEGIN
                    UPDATE CardLevel
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");

            migrationBuilder.Sql(
                @"CREATE TRIGGER SetCardTypeVersionOnUpdate
                AFTER UPDATE ON CardType
                BEGIN
                    UPDATE CardType
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");
            migrationBuilder.Sql(
                @"CREATE TRIGGER SetCardTypeVersionOnInsert
                AFTER INSERT ON CardType
                BEGIN
                    UPDATE CardType
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");

            migrationBuilder.Sql(
                @"CREATE TRIGGER SetFederationVersionOnUpdate
                AFTER UPDATE ON Federation
                BEGIN
                    UPDATE Federation
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");
            migrationBuilder.Sql(
                @"CREATE TRIGGER SetFederationVersionOnInsert
                AFTER INSERT ON Federation
                BEGIN
                    UPDATE Federation
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");

            migrationBuilder.Sql(
                @"CREATE TRIGGER SetRarityVersionOnUpdate
                AFTER UPDATE ON Rarity
                BEGIN
                    UPDATE Rarity
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");
            migrationBuilder.Sql(
                @"CREATE TRIGGER SetRarityVersionOnInsert
                AFTER INSERT ON Rarity
                BEGIN
                    UPDATE Rarity
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");

            migrationBuilder.Sql(
                @"CREATE TRIGGER SetTeamVersionOnUpdate
                AFTER UPDATE ON Team
                BEGIN
                    UPDATE Team
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");
            migrationBuilder.Sql(
                @"CREATE TRIGGER SetTeamVersionOnInsert
                AFTER INSERT ON Team
                BEGIN
                    UPDATE Team
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");

            migrationBuilder.Sql(
                @"CREATE TRIGGER SetTeamMembershipRequestVersionOnUpdate
                AFTER UPDATE ON TeamMembershipRequest
                BEGIN
                    UPDATE TeamMembershipRequest
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");
            migrationBuilder.Sql(
                @"CREATE TRIGGER SetTeamMembershipRequestVersionOnInsert
                AFTER INSERT ON TeamMembershipRequest
                BEGIN
                    UPDATE TeamMembershipRequest
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");

            migrationBuilder.Sql(
                @"CREATE TRIGGER SetTeamTunerUserVersionOnUpdate
                AFTER UPDATE ON TeamTunerUser
                BEGIN
                    UPDATE TeamTunerUser
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");
            migrationBuilder.Sql(
                @"CREATE TRIGGER SetTeamTunerUserVersionOnInsert
                AFTER INSERT ON TeamTunerUser
                BEGIN
                    UPDATE TeamTunerUser
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");

            migrationBuilder.Sql(
                @"CREATE TRIGGER SetThemeVersionOnUpdate
                AFTER UPDATE ON Theme
                BEGIN
                    UPDATE Theme
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");
            migrationBuilder.Sql(
                @"CREATE TRIGGER SetThemeVersionOnInsert
                AFTER INSERT ON Theme
                BEGIN
                    UPDATE Theme
                    SET Version = randomblob(8)
                    WHERE rowid = NEW.rowid;
                END");

            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            #region Manual additions

            migrationBuilder.Sql("DROP TRIGGER SetCardVersionOnUpdate");
            migrationBuilder.Sql("DROP TRIGGER SetCardVersionOnInsert");
            migrationBuilder.Sql("DROP TRIGGER SetCardLevelVersionOnUpdate");
            migrationBuilder.Sql("DROP TRIGGER SetCardLevelVersionOnInsert");
            migrationBuilder.Sql("DROP TRIGGER SetCardTypeVersionOnUpdate");
            migrationBuilder.Sql("DROP TRIGGER SetCardTypeVersionOnInsert");
            migrationBuilder.Sql("DROP TRIGGER SetFederationVersionOnUpdate");
            migrationBuilder.Sql("DROP TRIGGER SetFederationVersionOnInsert");
            migrationBuilder.Sql("DROP TRIGGER SetRarityVersionOnUpdate");
            migrationBuilder.Sql("DROP TRIGGER SetRarityVersionOnInsert");
            migrationBuilder.Sql("DROP TRIGGER SetTeamVersionOnUpdate");
            migrationBuilder.Sql("DROP TRIGGER SetTeamVersionOnInsert");
            migrationBuilder.Sql("DROP TRIGGER SetTeamMembershipRequestVersionOnUpdate");
            migrationBuilder.Sql("DROP TRIGGER SetTeamMembershipRequestVersionOnInsert");
            migrationBuilder.Sql("DROP TRIGGER SetTeamTunerUserVersionOnUpdate");
            migrationBuilder.Sql("DROP TRIGGER SetTeamTunerUserVersionOnInsert");
            migrationBuilder.Sql("DROP TRIGGER SetThemeVersionOnUpdate");
            migrationBuilder.Sql("DROP TRIGGER SetThemeVersionOnInsert");

            #endregion

            migrationBuilder.DropTable(
                "CardLevel");

            migrationBuilder.DropTable(
                "TeamMembershipRequest");

            migrationBuilder.DropTable(
                "Card");

            migrationBuilder.DropTable(
                "TeamTunerUser");

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