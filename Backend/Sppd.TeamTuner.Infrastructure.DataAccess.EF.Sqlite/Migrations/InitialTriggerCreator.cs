using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Sqlite.Migrations
{
    internal static class InitialTriggerCreator
    {
        public static void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        public static void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}