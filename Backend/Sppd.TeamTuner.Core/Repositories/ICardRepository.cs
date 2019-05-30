using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;

namespace Sppd.TeamTuner.Core.Repositories
{
    public interface ICardRepository : IRepository<Card>
    {
        /// <summary>
        /// Gets the card having the external identifier.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <returns>The card if it could be found; otherwise a <see cref="EntityNotFoundException" /> will be thrown.</returns>
        Task<Card> GetByExternalIdAsync(string externalId);
    }
}