using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;

namespace Sppd.TeamTuner.Core.Services
{
    /// <summary>
    ///     Services for <see cref="Card" /> handling.
    /// </summary>
    /// <seealso cref="IServiceBase{Card}" />
    public interface ICardService : IServiceBase<Card>
    {
        /// <summary>
        ///     Gets all the cards including the level for the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        ///     A dictionary containing all existing cards as key; the value will either be a level (1-7) or NULL if the user
        ///     hasn't set a level yet.
        /// </returns>
        Task<IDictionary<Card, int?>> GetForUserAsync(Guid userId);

        /// <summary>
        ///     Gets the card having the specified external identifier
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <returns>The card if it exists; otherwise throws <see cref="EntityNotFoundException" /></returns>
        Task<Card> GetByExternalIdAsync(string externalId);
    }
}