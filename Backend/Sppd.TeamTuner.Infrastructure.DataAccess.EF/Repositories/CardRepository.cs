using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class CardRepository : Repository<Card>, ICardRepository
    {
        public CardRepository(TeamTunerContext context)
            : base(context)
        {
        }

        public async Task<bool> ExternalIdExistsAsync(string externalId)
        {
            return await Set.AnyAsync(card => Equals(externalId, card.ExternalId));
        }
    }
}