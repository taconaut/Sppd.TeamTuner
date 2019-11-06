using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Services;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF
{
    /// <summary>
    ///     <see cref="DbContext" /> for the application.
    /// </summary>
    /// <seealso cref="DbContext" />
    public partial class TeamTunerContext : DbContext, IDatabaseService
    {
        private readonly Lazy<IValidationService> _validationService;
        private readonly IEnumerable<Lazy<IEntityMetadataProvider>> _entityMetadataProviders;

        public TeamTunerContext(DbContextOptions options, Lazy<IValidationService> validationService, IEnumerable<Lazy<IEntityMetadataProvider>> entityMetadataProviders)
            : base(options)
        {
            _validationService = validationService;
            _entityMetadataProviders = entityMetadataProviders;
        }

        public void DeleteDatabase()
        {
            Database.EnsureDeleted();
        }

        public override int SaveChanges()
        {
            throw new NotSupportedException("Use SaveChangesAsync");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotSupportedException("Use SaveChangesAsync");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(true, cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            PrepareSaveChanges();
            return await ExecuteAndHandleExceptions(base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Rarity>(ConfigureRarity);
            builder.Entity<Theme>(ConfigureCardTheme);
            builder.Entity<Card>(ConfigureCard);
            builder.Entity<CardLevel>(ConfigureCardLevel);
            builder.Entity<CardType>(ConfigureCardType);
            builder.Entity<CharacterType>(ConfigureCharacterType);

            builder.Entity<TeamTunerUser>(ConfigureTeamTunerUser);
            builder.Entity<TeamTunerUserRegistrationRequest>(ConfigureTeamTunerUserRegistrationValidation);
            builder.Entity<Team>(ConfigureTeam);
            builder.Entity<Federation>(ConfigureFederation);
            builder.Entity<TeamMembershipRequest>(ConfigureTeamMembershipRequest);
        }

        private static async Task<TResult> ExecuteAndHandleExceptions<TResult>(Task<TResult> task)
        {
            try
            {
                await task;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrentEntityUpdateException("The entity has been modified since you last retrieved it");
            }
            catch (DbUpdateException ex)
            {
                throw new EntityUpdateException("Failed to update entities", ex);
            }

            return task.Result;
        }

        private void PrepareSaveChanges()
        {
            ChangeTracker.DetectChanges();
            ValidateAllChangedEntities();
            SetModifierMetadataOnChangedEntities();
        }

        private void ValidateAllChangedEntities()
        {
            _validationService.Value
                              .ValidateAllChangedEntities()
                              .ThrowIfHasInvalid();
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