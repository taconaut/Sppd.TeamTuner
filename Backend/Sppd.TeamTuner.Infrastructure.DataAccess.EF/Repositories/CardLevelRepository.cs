using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;
using Sppd.TeamTuner.Core.Exceptions;
using Sppd.TeamTuner.Core.Repositories;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class CardLevelRepository : Repository<CardLevel>, ICardLevelRepository
    {
        public CardLevelRepository(TeamTunerContext context)
            : base(context)
        {
        }

        public async Task<CardLevel> GetAsync(Guid userId, Guid cardId)
        {
            CardLevel entity;
            try
            {
                entity = await GetQueryableWithIncludes()
                    .SingleAsync(e => e.UserId == userId && e.CardId == cardId);
            }
            catch (InvalidOperationException)
            {
                throw new EntityNotFoundException(typeof(CardLevel), $"{nameof(userId)}, {nameof(cardId)}");
            }

            return entity;
        }

        public async Task<IEnumerable<CardLevel>> GetAllForUserAsync(Guid userId)
        {
            return await GetQueryableWithIncludes()
                         .Where(e => e.UserId == userId)
                         .ToListAsync();
        }
    }
}