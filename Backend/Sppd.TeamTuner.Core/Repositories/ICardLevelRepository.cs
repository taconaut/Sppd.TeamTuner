using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;

namespace Sppd.TeamTuner.Core.Repositories
{
    /// <summary>
    ///     Repository to handle <see cref="CardLevel" />s
    /// </summary>
    /// <seealso cref="IRepository{CardLevel}" />
    public interface ICardLevelRepository : IRepository<CardLevel>
    {
        /// <summary>
        ///     Gets the card level for the given user and card.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cardId">The card identifier.</param>
        /// <returns>The card level if it exists; otherwise and <see cref="EntityNotFoundException" /> will be thrown.</returns>
        Task<CardLevel> GetAsync(Guid userId, Guid cardId);

        /// <summary>
        ///     Gets all card levels for the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A card levels having been set for the user</returns>
        Task<IEnumerable<CardLevel>> GetAllForUserAsync(Guid userId);
    }
}