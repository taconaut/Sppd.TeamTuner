using System.Threading.Tasks;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Core.Repositories
{
    public interface ICardRepository : IRepository<Card>
    {
        /// <summary>
        ///     Checks if a card with the specified <see cref="externalId" /> exists.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <returns>True if a card having this externalId has been found; otherwise false</returns>
        Task<bool> ExternalIdExistsAsync(string externalId);
    }
}