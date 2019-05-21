using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.MsSql.Migrations
{
    public partial class CharacterType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Card",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CharacterTypeId",
                table: "Card",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CharacterType",
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
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Card_CharacterTypeId",
                table: "Card",
                column: "CharacterTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_CharacterType_CharacterTypeId",
                table: "Card",
                column: "CharacterTypeId",
                principalTable: "CharacterType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_CharacterType_CharacterTypeId",
                table: "Card");

            migrationBuilder.DropTable(
                name: "CharacterType");

            migrationBuilder.DropIndex(
                name: "IX_Card_CharacterTypeId",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "CharacterTypeId",
                table: "Card");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Card",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
