using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.MsSql.Migrations
{
    public partial class RemoveCardFriendlyNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendlyNames",
                table: "Card");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FriendlyNames",
                table: "Card",
                nullable: true);
        }
    }
}
