﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Migrations
{
    [DbContext(typeof(TeamTunerContext))]
    partial class TeamTunerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedById");

                    b.Property<DateTime>("CreatedOnUtc");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedOnUtc");

                    b.Property<int>("ExternalId");

                    b.Property<string>("FriendlyName")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("ModifiedById");

                    b.Property<DateTime>("ModifiedOnUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid>("RarityId");

                    b.Property<Guid>("ThemeId");

                    b.Property<Guid>("TypeId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("ExternalId")
                        .IsUnique()
                        .HasFilter("[IsDeleted] = 0");

                    b.HasIndex("RarityId");

                    b.HasIndex("ThemeId");

                    b.HasIndex("TypeId");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.CardLevel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CardId");

                    b.Property<Guid>("CreatedById");

                    b.Property<DateTime>("CreatedOnUtc");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedOnUtc");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("Level");

                    b.Property<Guid>("ModifiedById");

                    b.Property<DateTime>("ModifiedOnUtc");

                    b.Property<Guid>("UserId");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("UserId");

                    b.ToTable("CardLevel");
                });

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.CardType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedById");

                    b.Property<DateTime>("CreatedOnUtc");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedOnUtc");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("ModifiedById");

                    b.Property<DateTime>("ModifiedOnUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("CardType");
                });

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.Federation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Avatar");

                    b.Property<Guid>("CreatedById");

                    b.Property<DateTime>("CreatedOnUtc");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedOnUtc");

                    b.Property<string>("Description")
                        .HasMaxLength(2000);

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("ModifiedById");

                    b.Property<DateTime>("ModifiedOnUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Federation");
                });

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.Rarity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedById");

                    b.Property<DateTime>("CreatedOnUtc");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedOnUtc");

                    b.Property<int>("FriendlyLevel");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("ModifiedById");

                    b.Property<DateTime>("ModifiedOnUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Rarity");
                });

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Avatar");

                    b.Property<Guid>("CreatedById");

                    b.Property<DateTime>("CreatedOnUtc");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedOnUtc");

                    b.Property<string>("Description")
                        .HasMaxLength(2000);

                    b.Property<Guid?>("FederationId");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("ModifiedById");

                    b.Property<DateTime>("ModifiedOnUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("FederationId");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.TeamTunerUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationRole")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte[]>("Avatar");

                    b.Property<Guid>("CreatedById");

                    b.Property<DateTime>("CreatedOnUtc");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedOnUtc");

                    b.Property<string>("Description")
                        .HasMaxLength(2000);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<Guid?>("FederationId");

                    b.Property<string>("FederationRole")
                        .HasMaxLength(20);

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("ModifiedById");

                    b.Property<DateTime>("ModifiedOnUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("SppdName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("TeamId");

                    b.Property<string>("TeamRole")
                        .HasMaxLength(20);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[IsDeleted] = 0");

                    b.HasIndex("FederationId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[IsDeleted] = 0");

                    b.HasIndex("SppdName")
                        .IsUnique()
                        .HasFilter("[IsDeleted] = 0");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamTunerUser");
                });

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.Theme", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedById");

                    b.Property<DateTime>("CreatedOnUtc");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedOnUtc");

                    b.Property<bool>("IsDeleted");

                    b.Property<Guid>("ModifiedById");

                    b.Property<DateTime>("ModifiedOnUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Theme");
                });

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.Card", b =>
                {
                    b.HasOne("Sppd.TeamTuner.Core.Domain.Entities.Rarity", "Rarity")
                        .WithMany()
                        .HasForeignKey("RarityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sppd.TeamTuner.Core.Domain.Entities.Theme", "Theme")
                        .WithMany()
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sppd.TeamTuner.Core.Domain.Entities.CardType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.CardLevel", b =>
                {
                    b.HasOne("Sppd.TeamTuner.Core.Domain.Entities.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sppd.TeamTuner.Core.Domain.Entities.TeamTunerUser", "User")
                        .WithMany("CardLevels")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.Team", b =>
                {
                    b.HasOne("Sppd.TeamTuner.Core.Domain.Entities.Federation", "Federation")
                        .WithMany("Teams")
                        .HasForeignKey("FederationId");
                });

            modelBuilder.Entity("Sppd.TeamTuner.Core.Domain.Entities.TeamTunerUser", b =>
                {
                    b.HasOne("Sppd.TeamTuner.Core.Domain.Entities.Federation", "Federation")
                        .WithMany()
                        .HasForeignKey("FederationId");

                    b.HasOne("Sppd.TeamTuner.Core.Domain.Entities.Team", "Team")
                        .WithMany("Users")
                        .HasForeignKey("TeamId");
                });
#pragma warning restore 612, 618
        }
    }
}
