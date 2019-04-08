using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Sppd.TeamTuner.Core.Domain.Entities;

namespace Sppd.TeamTuner.Infrastructure.DataAccess.EF.Repositories
{
    internal class CardRepository : Repository<Card>
    {
        public override Func<IQueryable<Card>, IQueryable<Card>> Includes
        {
            get
            {
                return cards => cards.Include(card => card.Theme)
                                     .Include(card => card.Rarity);
            }
        }

        public CardRepository(TeamTunerContext context)
            : base(context)
        {
        }
    }
}