using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.InMemory
{
    /// <summary>
    ///     Extends <see cref="TeamTunerContext" /> to allow managing migrations in the project.
    /// </summary>
    /// <seealso cref="TeamTunerContext" />
    internal class TeamTunerContextInMemory : TeamTunerContext
    {
        public TeamTunerContextInMemory(DbContextOptions options, Lazy<IValidationService> validationService, IEnumerable<Lazy<IEntityMetadataProvider>> entityMetadataProviders)
            : base(options, validationService, entityMetadataProviders)
        {
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(true, cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            PrepareSaveChanges();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void PrepareSaveChanges()
        {
            ChangeTracker.DetectChanges();

            // The in memory DB doesn't automatically set the version; do it manually.
            SetVersionOnChangedEntities();
        }

        private void SetVersionOnChangedEntities()
        {
            var entriesToSetVersionOn = ChangeTracker.Entries<BaseEntity>()
                                                     .Where(e => HasToSetModifierMetadata(e.State))
                                                     .ToList();

            if (!entriesToSetVersionOn.Any())
            {
                return;
            }

            var random = new Random();
            var entityVersion = new byte[8];
            random.NextBytes(entityVersion);
            foreach (var entry in entriesToSetVersionOn)
            {
                random.NextBytes(entityVersion);
                entry.Entity.Version = entityVersion;
            }
        }

        private static bool HasToSetModifierMetadata(EntityState state)
        {
            return state == EntityState.Modified || state == EntityState.Added || state == EntityState.Deleted;
        }
    }
}