using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

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
    }
}