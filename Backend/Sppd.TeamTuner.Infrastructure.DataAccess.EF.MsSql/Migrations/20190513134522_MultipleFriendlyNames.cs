using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.MsSql.Migrations
{
    public partial class MultipleFriendlyNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendlyName",
                table: "Card");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                table: "Card",
                maxLength: 24,
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "FriendlyNames",
                table: "Card",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendlyNames",
                table: "Card");

            migrationBuilder.AlterColumn<int>(
                name: "ExternalId",
                table: "Card",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 24,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FriendlyName",
                table: "Card",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
