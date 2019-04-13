using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF
{
    /// <inheritdoc />
    /// <summary>
    ///     Partial class specifying the entity configurations
    /// </summary>
    /// <seealso cref="T:Microsoft.EntityFrameworkCore.DbContext" />
    internal partial class TeamTunerContext
    {
        private static void ConfigureBaseEntity<TEntity>(EntityTypeBuilder<TEntity> builder)
            where TEntity : BaseEntity
        {
            // Do not load soft deleted entities
            builder.HasQueryFilter(m => Microsoft.EntityFrameworkCore.EF.Property<bool>(m, DataAccessConstants.IS_DELETED_PROPERTY_NAME) == false);
        }

        private static void ConfigureNamedEntity<TEntity>(EntityTypeBuilder<TEntity> builder)
            where TEntity : NamedEntity
        {
            ConfigureBaseEntity(builder);
        }

        private static void ConfigureDescriptiveEntity<TEntity>(EntityTypeBuilder<TEntity> builder)
            where TEntity : DescriptiveEntity
        {
            ConfigureNamedEntity(builder);
        }

        private static void ConfigureCard(EntityTypeBuilder<Card> builder)
        {
            ConfigureNamedEntity(builder);

            // Indexes and unique constraint
            builder.HasIndex(e => e.ExternalId)
                   .IsUnique()
                   .HasFilter(DataAccessConstants.IS_DELETED_FILTER);
        }

        private static void ConfigureCardLevel(EntityTypeBuilder<CardLevel> builder)
        {
            ConfigureBaseEntity(builder);
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

        private static void ConfigureTeamTunerUser(EntityTypeBuilder<TeamTunerUser> builder)
        {
            ConfigureDescriptiveEntity(builder);

            builder.HasMany(e => e.CardLevels)
                   .WithOne(e => e.User);

            // Indexes and unique constraint
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
    }
}