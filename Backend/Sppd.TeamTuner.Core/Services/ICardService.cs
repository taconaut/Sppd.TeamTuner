using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

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
    }
}