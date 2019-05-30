using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class CardRepository : Repository<Card>, ICardRepository
    {
        public CardRepository(TeamTunerContext context)
            : base(context)
        {
        }

        public async Task<Card> GetByExternalIdAsync(string externalId)
        {
            Card entity;
            try
            {
                entity = await GetQueryableWithIncludes().SingleAsync(e => e.ExternalId == externalId);
            }
            catch (InvalidOperationException)
            {
                throw new EntityNotFoundException(typeof(Card), externalId);
            }

            return entity;
        }
    }
}