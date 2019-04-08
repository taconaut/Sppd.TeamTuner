using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Domain.Interfaces;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Providers;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Providers
{
    internal class BaseEntityMetadataProvider : IEntityMetadataProvider
    {
        private readonly Lazy<ITeamTunerUserProvider> _userProvider;
        private readonly Lazy<ChangeTracker> _changeTracker;

        public BaseEntityMetadataProvider(Lazy<ITeamTunerUserProvider> userProvider, IServiceProvider serviceProvider)
        {
            _userProvider = userProvider;
            _changeTracker = new Lazy<ChangeTracker>(() => serviceProvider.GetService<TeamTunerContext>().ChangeTracker);
        }

        public void SetModifierMetadataOnChangedEntities()
        {
            var entriesToSetModifier = _changeTracker.Value.Entries<BaseEntity>().Where(e => HasToSetModifierMetadata(e.State)).ToList();

            if (entriesToSetModifier.Count > 0)
            {
                var saveDate = DateTime.UtcNow;
                foreach (var entryToSetModifier in entriesToSetModifier)
                {
                    SetModifierMetadataProperties(entryToSetModifier, saveDate);
                }
            }
        }

        private static bool HasToSetModifierMetadata(EntityState state)
        {
            return state == EntityState.Modified || state == EntityState.Added;
        }

        private void SetModifierMetadataProperties(EntityEntry<BaseEntity> entry, DateTime saveDate)
        {
            var entity = entry.Entity;
            var currentUserId = GetCurrentUser(entry).Id;

            if (entity.IsDeleted)
            {
                entity.DeletedById = currentUserId;
                entity.DeletedOnUtc = saveDate;
                return;
            }

            if (entry.State == EntityState.Added)
            {
                entity.CreatedById = currentUserId;
                entity.CreatedOnUtc = saveDate;
            }

            entity.ModifiedById = currentUserId;
            entity.ModifiedOnUtc = saveDate;
        }

        private ITeamTunerUser GetCurrentUser(EntityEntry entry)
        {
            var userProvider = _userProvider.Value;
            if (userProvider.CurrentUser != null)
            {
                return userProvider.CurrentUser;
            }

            if (entry.Entity is ITeamTunerUser user)
            {
                // Special case for user creation: The user creates himself and thus doesn't exist yet. Use him as the current user.
                return user;
            }

            throw new BusinessException("CurrentUser not defined");
        }
    }
}