using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF
{
    /// <summary>
    ///     Partial class specifying the entity configurations
    /// </summary>
    /// <seealso cref="T:Microsoft.EntityFrameworkCore.DbContext" />
    public partial class TeamTunerContext
    {
        private static void ConfigureBaseEntity<TEntity>(EntityTypeBuilder<TEntity> builder)
            where TEntity : BaseEntity
        {
            // Do not load soft deleted entities
            builder.HasQueryFilter(m => !m.IsDeleted);

            builder.Property(e => e.Version)
                   .IsConcurrencyToken();

            // Specify UTC kind as this is not supported out of the box
            // See: https://github.com/aspnet/EntityFrameworkCore/issues/4711
            builder.Property(e => e.ModifiedOnUtc)
                   .HasConversion(dateTime => dateTime, dateTime => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
            builder.Property(e => e.CreatedOnUtc)
                   .HasConversion(dateTime => dateTime, dateTime => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
            builder.Property(e => e.DeletedOnUtc)
                   .HasConversion(dateTime => dateTime, dateTime => dateTime.HasValue ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) : (DateTime?) null);
        }

        private static void ConfigureNamedEntity<TEntity>(EntityTypeBuilder<TEntity> builder)
            where TEntity : NamedEntity
        {
            ConfigureBaseEntity(builder);

            builder.HasIndex(e => e.Name)
                   .IsUnique()
                   .HasFilter(DataAccessConstants.IS_DELETED_FILTER);
        }

        private static void ConfigureDescriptiveEntity<TEntity>(EntityTypeBuilder<TEntity> builder)
            where TEntity : DescriptiveEntity
        {
            ConfigureNamedEntity(builder);
        }

        private static void ConfigureCard(EntityTypeBuilder<Card> builder)
        {
            ConfigureNamedEntity(builder);

            // Indexes and unique constraints
            builder.HasIndex(e => e.ExternalId)
                   .IsUnique()
                   .HasFilter(DataAccessConstants.IS_DELETED_FILTER);
        }

        private static void ConfigureCardLevel(EntityTypeBuilder<CardLevel> builder)
        {
            ConfigureBaseEntity(builder);

            builder.HasIndex(nameof(CardLevel.UserId), nameof(CardLevel.CardId))
                   .IsUnique()
                   .HasFilter(DataAccessConstants.IS_DELETED_FILTER);
        }

        private static void ConfigureCardTheme(EntityTypeBuilder<Theme> builder)
        {
            ConfigureNamedEntity(builder);
        }

        private static void ConfigureRarity(EntityTypeBuilder<Rarity> builder)
        {
            ConfigureNamedEntity(builder);
        }

        private static void ConfigureCardType(EntityTypeBuilder<CardType> builder)
        {
            ConfigureNamedEntity(builder);
        }

        private static void ConfigureCharacterType(EntityTypeBuilder<CharacterType> builder)
        {
            ConfigureNamedEntity(builder);
        }

        private static void ConfigureTeamTunerUser(EntityTypeBuilder<TeamTunerUser> builder)
        {
            ConfigureDescriptiveEntity(builder);

            builder.HasMany(e => e.CardLevels)
                   .WithOne(e => e.User)
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexes and unique constraints
            builder.HasIndex(e => e.Name)
                   .IsUnique()
                   .HasFilter(DataAccessConstants.IS_DELETED_FILTER);
            builder.HasIndex(e => e.SppdName)
                   .IsUnique()
                   .HasFilter(DataAccessConstants.IS_DELETED_FILTER);
            builder.HasIndex(e => e.Email)
                   .IsUnique()
                   .HasFilter(DataAccessConstants.IS_DELETED_FILTER);
        }

        private static void ConfigureTeamTunerUserRegistrationValidation(EntityTypeBuilder<TeamTunerUserRegistrationRequest> builder)
        {
            ConfigureBaseEntity(builder);

            builder.Property(e => e.RegistrationDate)
                   .HasConversion(dateTime => dateTime, dateTime => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));

            builder.HasIndex(e => e.RegistrationCode);
            builder.HasIndex(e => e.UserId)
                   .IsUnique()
                   .HasFilter(DataAccessConstants.IS_DELETED_FILTER);
        }

        private static void ConfigureTeam(EntityTypeBuilder<Team> builder)
        {
            ConfigureDescriptiveEntity(builder);

            builder.HasMany(e => e.Users)
                   .WithOne(e => e.Team);

            // Ignore calculated properties
            builder.Ignore(e => e.Members)
                   .Ignore(e => e.Leader)
                   .Ignore(e => e.CoLeaders);
        }

        private static void ConfigureFederation(EntityTypeBuilder<Federation> builder)
        {
            ConfigureDescriptiveEntity(builder);

            builder.HasMany(e => e.Teams)
                   .WithOne(e => e.Federation);

            // Ignore calculated properties
            builder.Ignore(e => e.Users)
                   .Ignore(e => e.Members)
                   .Ignore(e => e.Leader)
                   .Ignore(e => e.CoLeaders);
        }

        private static void ConfigureTeamMembershipRequest(EntityTypeBuilder<TeamMembershipRequest> builder)
        {
            ConfigureBaseEntity(builder);

            builder.HasIndex(e => e.UserId)
                   .IsUnique()
                   .HasFilter(DataAccessConstants.IS_DELETED_FILTER);
            builder.HasIndex(e => e.TeamId);
        }
    }
}