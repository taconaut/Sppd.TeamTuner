using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.MsSql.Migrations
{
    public partial class AdditionalCardProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "Description",
                "Card",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                "ManaCost",
                "Card",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Description",
                "Card");

            migrationBuilder.DropColumn(
                "ManaCost",
                "Card");
        }
    }
}