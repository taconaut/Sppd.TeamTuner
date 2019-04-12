using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Config;
using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Services;
using Sppd.TeamTuner.Infrastructure.DataAccess.EF.Config;

// Allows testing elements marked as internal in the specified namespace
[assembly: InternalsVisibleTo("Sppd.TeamTuner.Tests.Integration.Common")]

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF
{
    /// <summary>
    ///     <see cref="DbContext" /> for the application.
    /// </summary>
    /// <seealso cref="DbContext" />
    internal partial class TeamTunerContext : DbContext
    {
        private readonly Lazy<DatabaseConfig> _databaseConfig;
        private readonly Lazy<IValidationService> _validationService;
        private readonly IEnumerable<Lazy<IEntityMetadataProvider>> _entityMetadataProviders;

        public TeamTunerContext(DbContextOptions options, Lazy<IValidationService> validationService, IConfigProvider<DatabaseConfig> databaseConfigProvider,
            IEnumerable<Lazy<IEntityMetadataProvider>> entityMetadataProviders)
            : base(options)
        {
            _validationService = validationService;
            _databaseConfig = new Lazy<DatabaseConfig>(() => databaseConfigProvider.Config);
            _entityMetadataProviders = entityMetadataProviders;
        }

        public override int SaveChanges()
        {
            PrepareSaveChanges();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            PrepareSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            PrepareSaveChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            PrepareSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Rarity>(ConfigureRarity);
            builder.Entity<Theme>(ConfigureCardTheme);
            builder.Entity<Card>(ConfigureCard);
            builder.Entity<CardLevel>(ConfigureCardLevel);
            builder.Entity<CardType>(ConfigureCardType);

            builder.Entity<TeamTunerUser>(ConfigureTeamTunerUser);
            builder.Entity<Team>(ConfigureTeam);
            builder.Entity<Federation>(ConfigureFederation);
        }

        private void PrepareSaveChanges()
        {
            ChangeTracker.DetectChanges();
            ValidateAllChangedEntities();
            SetModifierMetadataOnChangedEntities();
        }

        private void ValidateAllChangedEntities()
        {
            var validationResults = _validationService.Value.ValidateAllChangedEntities();
            validationResults.ThrowIfHasInvalid();
        }

        private void SetModifierMetadataOnChangedEntities()
        {
            foreach (var entityMetadataProvider in _entityMetadataProviders)
            {
                entityMetadataProvider.Value.SetModifierMetadataOnChangedEntities();
            }
        }
    }
}