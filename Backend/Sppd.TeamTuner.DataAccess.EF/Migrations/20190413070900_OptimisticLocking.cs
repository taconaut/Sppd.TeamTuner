using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Migrations
{
    public partial class OptimisticLocking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Theme",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "TeamTunerUser",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Team",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Rarity",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Federation",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "CardType",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "CardLevel",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Card",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Theme");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "TeamTunerUser");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Rarity");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Federation");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "CardType");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "CardLevel");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Card");
        }
    }
}
