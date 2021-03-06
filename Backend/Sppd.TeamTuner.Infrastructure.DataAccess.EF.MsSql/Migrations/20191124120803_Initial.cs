﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.MsSql.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardType",
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
                    table.PrimaryKey("PK_CardType", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Federation",
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
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Avatar = table.Column<byte[]>(nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Federation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rarity",
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
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    FriendlyLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rarity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Theme",
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
                    table.PrimaryKey("PK_Theme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
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
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Avatar = table.Column<byte[]>(nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    FederationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Federation_FederationId",
                        column: x => x.FederationId,
                        principalTable: "Federation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Card",
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
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    ExternalId = table.Column<string>(maxLength: 24, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    ManaCost = table.Column<int>(nullable: false),
                    ThemeId = table.Column<Guid>(nullable: false),
                    RarityId = table.Column<Guid>(nullable: false),
                    TypeId = table.Column<Guid>(nullable: false),
                    CharacterTypeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Card_CharacterType_CharacterTypeId",
                        column: x => x.CharacterTypeId,
                        principalTable: "CharacterType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Card_Rarity_RarityId",
                        column: x => x.RarityId,
                        principalTable: "Rarity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Card_Theme_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Theme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Card_CardType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CardType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamTunerUser",
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
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Avatar = table.Column<byte[]>(nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    IsEmailVerified = table.Column<bool>(nullable: false),
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
                        name: "FK_TeamTunerUser_Federation_FederationId",
                        column: x => x.FederationId,
                        principalTable: "Federation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamTunerUser_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardLevel",
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
                    CardId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardLevel_Card_CardId",
                        column: x => x.CardId,
                        principalTable: "Card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardLevel_TeamTunerUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TeamTunerUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembershipRequest",
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
                    TeamId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Comment = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembershipRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMembershipRequest_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMembershipRequest_TeamTunerUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TeamTunerUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Card_CharacterTypeId",
                table: "Card",
                column: "CharacterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Card_ExternalId",
                table: "Card",
                column: "ExternalId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Card_Name",
                table: "Card",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Card_RarityId",
                table: "Card",
                column: "RarityId");

            migrationBuilder.CreateIndex(
                name: "IX_Card_ThemeId",
                table: "Card",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Card_TypeId",
                table: "Card",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLevel_CardId",
                table: "CardLevel",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLevel_UserId_CardId",
                table: "CardLevel",
                columns: new[] { "UserId", "CardId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CardType_Name",
                table: "CardType",
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
                name: "IX_Federation_Name",
                table: "Federation",
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
                name: "IX_Team_FederationId",
                table: "Team",
                column: "FederationId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_Name",
                table: "Team",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembershipRequest_TeamId",
                table: "TeamMembershipRequest",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembershipRequest_UserId",
                table: "TeamMembershipRequest",
                column: "UserId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUser_Email",
                table: "TeamTunerUser",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUser_FederationId",
                table: "TeamTunerUser",
                column: "FederationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUser_Name",
                table: "TeamTunerUser",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUser_SppdName",
                table: "TeamTunerUser",
                column: "SppdName",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TeamTunerUser_TeamId",
                table: "TeamTunerUser",
                column: "TeamId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Theme_Name",
                table: "Theme",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardLevel");

            migrationBuilder.DropTable(
                name: "TeamMembershipRequest");

            migrationBuilder.DropTable(
                name: "TeamTunerUserRegistrationRequest");

            migrationBuilder.DropTable(
                name: "Card");

            migrationBuilder.DropTable(
                name: "TeamTunerUser");

            migrationBuilder.DropTable(
                name: "CharacterType");

            migrationBuilder.DropTable(
                name: "Rarity");

            migrationBuilder.DropTable(
                name: "Theme");

            migrationBuilder.DropTable(
                name: "CardType");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Federation");
        }
    }
}
